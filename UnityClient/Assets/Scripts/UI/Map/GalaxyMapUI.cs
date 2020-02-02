using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using COSMOS.Space;
using System.Threading.Tasks;
using System.Linq;
using COSMOS.Paterns;
using COSMOS.Core.HelpfulStuff;
using COSMOS.UI.HelpfulStuff;
using COSMOS.UI.Map;

namespace COSMOS.UI
{
	[Manager]
	public class GalaxyMapUI : SingletonMono<GalaxyMapUI>
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

		public const float CLIP_AREA_PERCENT = 1.10f;
		public const float ZOOM_COEF = 10f;

		public ObjectPool<MapSolarSystemUI> ObjectPool;
		public List<MapSolarSystemUI> Systems = new List<MapSolarSystemUI>();

		private void Awake()
		{
			InitPatern();
			ObjectPool = new ObjectPool<MapSolarSystemUI>(20, true, () =>
			{
				GameObject t = Instantiate(StarPrefab);
				t.transform.SetParent(Back.transform);
				t.SetActive(false);
				var star = t.AddComponent<MapSolarSystemUI>();
				return star;
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
			if (Input.GetKey(KeyCode.Q))
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
			if (systems.Count > Systems.Count)
			{
				for (int i = Systems.Count; i < systems.Count; i++)
				{
					Systems.Add(ObjectPool.GetObject());
				}
			}
			else if (systems.Count < Systems.Count)
			{
				while (systems.Count < Systems.Count)
				{
					int i = Systems.Count - 1;
					MapSolarSystemUI tmp = Systems[i];
					tmp.gameObject.SetActive(false);
					if (!ObjectPool.Release(tmp))
					{
						Destroy(tmp);
					}
					else
					{
						tmp.name = "disable";
					}
					Systems.RemoveAt(i);
				}
			} 	
			for (int i = 0; i < systems.Count; i++)
			{
				MapSolarSystemUI tmp = Systems[i];
				tmp.gameObject.name = i.ToString();
				tmp.SolarSytem = systems[i];
				tmp.gameObject.SetActive(true);
				tmp.transform.position = (systems[i].PosOnMap + Position) * (1 + Zoom.GalaxyZoom * ZOOM_COEF) + new Vector2(Screen.width, Screen.height) * 0.5f;
			}
		}
		public void SelectSystem(SolarSystem system)
		{

		}
		List<SolarSystem> SelectSystems()
		{
			List<SolarSystem> foundedSystems = new List<SolarSystem>();

			Vector2 newSize = new Vector2(Screen.width, Screen.height) / (1 + Zoom.GalaxyZoom * ZOOM_COEF);
			
			Vector2 newPos = new Vector2(Position.x + (newSize.x) * 0.5f, Position.y + (newSize.y) * 0.5f);
			
			Rect view = new Rect(-newPos, newSize);

			foundedSystems.AddRange(SolarSystemManager.SystemsOnMap(view, Mathf.RoundToInt(Mathf.Lerp(3, QuadTree<SolarSystem>.QT_NODE_DEEP, Zoom.GalaxyZoom))));

			//Log.Info(foundedSystems.Count);

			return foundedSystems;
		}
	}
}