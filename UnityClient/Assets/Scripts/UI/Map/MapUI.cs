using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using COSMOS.Space;
using System.Threading.Tasks;
using System.Linq;
using COSMOS.Paterns;
using COSMOS.Core.HelpfulStuff;

namespace COSMOS.UI
{
	[Manager]
	public class MapUI : SingletonMono<MapUI>
	{
		public Vector2 Position;
		private Vector2 position;
		public GameObject Back;
		public GameObject StarPrefab;
		public Vector3 prefMousePosition;
		[Header("UI")]
		public TMP_InputField Search;
		public MapZoomUI Zoom;

		[Header("Solar system")]
		[Header("Description")]
		#region SolarSystem
		public TextMeshProUGUI SystemName;
		public TextMeshProUGUI SystemPosition;
		public TextMeshProUGUI SystemPlanetCount;
		public TextMeshProUGUI SystemSpaceStationCount;
		public RectTransform SystemEvents;
		public RectTransform SystemObjects;
		public TextMeshProUGUI SystemDescription;
		#endregion

		public const int MIN_SOLAR_SYSTEM_COUNT = 10;
		public const float CLIP_AREA_PERCENT = 1.10f;
		public const float ZOOM_COEF = 10f;

		public static QuadTree<SolarSystemProto> tree = new QuadTree<SolarSystemProto>(new Rect());
		public ObjectPool<GameObject> ObjectPool;
		public List<GameObject> Objects = new List<GameObject>();

		private void Awake()
		{
			InitPatern();
			ObjectPool = new ObjectPool<GameObject>(20, true, () =>
			{
				GameObject t = Instantiate(StarPrefab);
				t.transform.SetParent(Back.transform);
				return t;
			});
		}
		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			var mousePosition = Input.mousePosition;
			var mouseDelta = mousePosition - prefMousePosition;
			prefMousePosition = mousePosition;
			if (Input.GetMouseButton(0))
			{
				Position += new Vector2(mouseDelta.x, mouseDelta.y);
			}
			if(position != Position)
			{
				UpdateMap();
			}
		}
		void UpdateMap()
		{
			var systems = SelectSystems();
			if (systems.Count < Objects.Count)
			{
				while (systems.Count < Objects.Count)
				{
					int i = Objects.Count - 1;
					GameObject tmp = Objects[i];
					tmp.SetActive(false);
					if (!ObjectPool.Release(tmp))
					{
						Destroy(tmp);
					}
					Objects.RemoveAt(i);
				}
			}
			for (int i = 0; i < systems.Count; i++)
			{
				if (Objects.Count - 1 < i)
				{
					Objects.Add(ObjectPool.GetObject());
				}
				GameObject tmp = Objects[i];
				tmp.SetActive(true);
				tmp.transform.position = (systems[i].Value.PosOnMap + Position) * (1 + Zoom.GalaxyZoom * ZOOM_COEF);
			}
		}
		private void OnDrawGizmos()
		{
			//var systems = SelectSystems3();
			//for (int i = 0; i < systems.Count; i++)
			//{
			//	Gizmos.DrawWireSphere(systems[i].Value.PosOnMap, 1);
			//}
			tree.DebugDraw();
			Rect view = new Rect(Position, new Vector2(100, 100) / (1 + Zoom.GalaxyZoom * ZOOM_COEF));

			Debug.DrawLine(view.min, new Vector3(view.xMin, view.yMax), Color.green);
			Debug.DrawLine(view.min, new Vector3(view.xMax, view.yMin), Color.green);

			Debug.DrawLine(view.max, new Vector3(view.xMax, view.yMin), Color.green);
			Debug.DrawLine(view.max, new Vector3(view.xMin, view.yMax), Color.green);
		}

		[InitMethod(int.MinValue)]
		static void Init()
		{
			tree = new QuadTree<SolarSystemProto>(new Rect(Vector2.zero, SolarSystemManager.MaxFarSystem.PosOnMap));

			for (int i = 0; i < SolarSystemManager.SortedSolarSystemPrototypesByStarBrightness.Count; i++)
			{
				SolarSystemProto tmp = SolarSystemManager.SortedSolarSystemPrototypesByStarBrightness[i];
				tree.Insert(new QuadTree<SolarSystemProto>.Point<SolarSystemProto>() { Value = tmp, Position = tmp.PosOnMap });
			}
		}
		List<QuadTree<SolarSystemProto>.Point<SolarSystemProto>> SelectSystems()
		{
			List<QuadTree<SolarSystemProto>.Point<SolarSystemProto>> foundedSystems = new List<QuadTree<SolarSystemProto>.Point<SolarSystemProto>>();

			Rect view = new Rect(-Position, new Vector2(Screen.width, Screen.height) / (1 + Zoom.GalaxyZoom * ZOOM_COEF));

			foundedSystems.AddRange(tree.QueryRange(view, (int)Mathf.Lerp(3, QuadTree<SolarSystemProto>.QT_NODE_DEEP, Zoom.GalaxyZoom)));

			//Log.Info(foundedSystems.Count);

			return foundedSystems;
		}
	}
}