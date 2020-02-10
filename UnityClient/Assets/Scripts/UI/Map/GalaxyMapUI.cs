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
using UnityEngine.UI;
using COSMOS.Shaders;

namespace COSMOS.UI
{
	[Manager]
	public class GalaxyMapUI : SingletonMono<GalaxyMapUI>
	{
		public Vector2 Position;
		private Vector2 position;
		public GameObject SystemsParent;
		public RawImage fogImage;
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
		public const float ZOOM_COEF = 100f;

		public ObjectPool<MapSolarSystemUI> ObjectPool;
		public Dictionary<SolarSystem, MapSolarSystemUI> Systems = new Dictionary<SolarSystem, MapSolarSystemUI>();
		public SolarSystem SelectedSystem { get; private set; }

		// Map shader
		ComputeShader fogShader;
		Texture2D fogPatern;
		RenderTexture fogTexture;
		GaussianBlurStatic gaussianBlur;

		public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
		{
			source.filterMode = FilterMode.Point;
			RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
			rt.filterMode = FilterMode.Point;
			RenderTexture.active = rt;
			Graphics.Blit(source, rt);
			Texture2D nTex = new Texture2D(newWidth, newHeight);
			nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
			nTex.Apply();
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(rt);
			return nTex;
		}
		void InitShaderPart()
		{
			fogShader = Resources.Load<ComputeShader>("Shaders/Compute/GalaxyMapFog");
			fogPatern = Resources.Load<Texture2D>("Textures/GalaxyPatern");
			float scale = Screen.height / (float)fogPatern.height;
			fogPatern = Resize(fogPatern, (int)(scale * fogPatern.width), (int)(scale * fogPatern.height));
			fogPatern.Apply();
			fogTexture = new RenderTexture(Screen.width, Screen.height, 0);
			fogTexture.enableRandomWrite = true;
			fogTexture.Create();

			gaussianBlur = new GaussianBlurStatic();
			gaussianBlur.SetRadius(50);
		}
		private void Awake()
		{
			InitShaderPart();
			fogImage.texture = fogTexture;
			fogImage.color = new Color(1, 1, 1, 1);
			InitPatern();
			ObjectPool = new ObjectPool<MapSolarSystemUI>(20, true, () =>
			{
				GameObject t = Instantiate(StarPrefab);
				t.transform.SetParent(SystemsParent.transform);
				t.SetActive(false);
				var star = t.AddComponent<MapSolarSystemUI>();
				return star;
			});
		}
		// Start is called before the first frame update
		void Start()
		{
			UpdateMap();
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
			UpdateFog();
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

		void UpdateFog()
		{
			int kernelIndex = fogShader.FindKernel("MoveAndZoom");

			int blockWidth = 8;
			int blockHeight = 8;

			Vector2 p = Position;
			//p = p * (1 + Zoom.GalaxyZoom * ZOOM_COEF) + new Vector2(Screen.width, Screen.height) * 0.5f;
			fogShader.SetFloat("coef", ZOOM_COEF);
			fogShader.SetFloats("screen", Screen.width * 0.5f, Screen.height * 0.5f);
			fogShader.SetInts("size", fogPatern.width, fogPatern.height);
			fogShader.SetFloats("position", p.x, p.y);
			fogShader.SetFloat("zoom", Zoom.GalaxyZoom);
			fogShader.SetTexture(kernelIndex, "patern", fogPatern); 
			fogShader.SetTexture(kernelIndex, "output", fogTexture);

			fogShader.Dispatch(kernelIndex, fogTexture.width / blockWidth, fogTexture.height / blockHeight, 1);
			//gaussianBlur.Disptach(fogTexture, ref fogTexture);
			
		}

		public void UpdateDescription()
		{
			SystemName.text = SelectedSystem.Name;
			SystemPlanetCount.text = SelectedSystem.Planets.Count.ToString();
		}

		public void SelectSystem(SolarSystem system)
		{
			SelectedSystem = system;
			UpdateDescription();
			Log.Info("Selected " + system.Name.Key);
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