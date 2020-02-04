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
		public Dictionary<SolarSystem, MapSolarSystemUI> Systems = new Dictionary<SolarSystem, MapSolarSystemUI>();
		QuadTree<SolarSystem> qw = new QuadTree<SolarSystem>();
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
			qw.Insert(new Vector2(0, 0), new SolarSystem());
			qw.Insert(new Vector2(10, 10), new SolarSystem());
			qw.Insert(new Vector2(6, -6), new SolarSystem());
		}

		// Update is called once per frame
		void Update()
		{
			var mousePosition = Input.mousePosition;
			var mouseDelta = mousePosition - prefMousePosition;
			prefMousePosition = mousePosition;
			if (Input.GetKey(KeyCode.Q))
			{
				Position += new Vector2(mouseDelta.x, mouseDelta.y) / (1 + Zoom.GalaxyZoom * ZOOM_COEF);
			}
			if(position != Position)
			{
				UpdateMap();
			}
			//qw.DebugDraw();
			SolarSystemManager.systemsQuadTree.DebugDraw();
		}
		void UpdateMap()
		{
			var systems = SelectSystems();

			Dictionary<SolarSystem, MapSolarSystemUI> toDelete = new Dictionary<SolarSystem, MapSolarSystemUI>(Systems);
			Systems.Clear();
			foreach (var item in systems)
			{
				MapSolarSystemUI system;
				if (toDelete.ContainsKey(item))
				{
					system = toDelete[item];
					Systems.Add(item, system);
					toDelete.Remove(item);
				}
				else
				{
					system = ObjectPool.GetObject();
					system.Init(item);
					system.gameObject.SetActive(true);
					Systems.Add(item, system);
				}
				system.transform.position = (item.PosOnMap + Position) * (1 + Zoom.GalaxyZoom * ZOOM_COEF) + 
					new Vector2(Screen.width, Screen.height) * 0.5f;

			}
			foreach (var item in toDelete)
			{
				item.Value.gameObject.SetActive(false);
				if (!ObjectPool.Release(item.Value))
				{
					Destroy(item.Value.gameObject);
				}
				else
				{
					item.Value.name = "disable";
				}
			}
		}
		public void SelectSystem(SolarSystem system)
		{

		}
		public void SelectSpaceObject(SpaceObject obj)
		{

		}
		List<SolarSystem> SelectSystems()
		{
			List<SolarSystem> foundedSystems = new List<SolarSystem>();
			float zoom = (1 + Zoom.GalaxyZoom * ZOOM_COEF);

			Vector2 newSize = new Vector2(Screen.width, Screen.height) / zoom;
			
			Vector2 newPos = new Vector2(Position.x + (newSize.x) * 0.5f, Position.y + (newSize.y) * 0.5f);
			
			Rect view = new Rect(-newPos, newSize);

			foundedSystems.AddRange(SolarSystemManager.SystemsOnMap(view, 
				Mathf.RoundToInt(Mathf.Lerp(0, SolarSystemManager.MaxNotInpotanceSystem.ImportanceOnMap, Zoom.GalaxyZoom))));

			//Log.Info(foundedSystems.Count);

			return foundedSystems;
		}
	}
}