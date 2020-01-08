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

		private void Awake()
		{
			InitPatern();
		}
		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if(position != Position)
			{
				//if (Old)
				//{
				//	SelectSystems();
				//}
				//else
				//{
				//	SelectSystems3();
				//}
			}
		}
		private void OnDrawGizmos()
		{
			var systems = SelectSystems3();
			for (int i = 0; i < systems.Count; i++)
			{
				Gizmos.DrawWireSphere(systems[i].Value.PosOnMap, 1);
			}
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
		List<QuadTree<SolarSystemProto>.Point<SolarSystemProto>> SelectSystems3()
		{
			List<QuadTree<SolarSystemProto>.Point<SolarSystemProto>> foundedSystems = new List<QuadTree<SolarSystemProto>.Point<SolarSystemProto>>();

			Rect view = new Rect(Position, new Vector2(100, 100) / (1 + Zoom.GalaxyZoom * ZOOM_COEF));

			foundedSystems.AddRange(tree.QueryRange(view, (int)Mathf.Lerp(3, QuadTree<SolarSystemProto>.QT_NODE_DEEP, Zoom.GalaxyZoom)));

			//Log.Info(foundedSystems.Count);

			return foundedSystems;
		}
	}
}