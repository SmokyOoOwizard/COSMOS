using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace COSMOS.UI
{
    public class MapZoomUI : MonoBehaviour
    {
        public Image ZoomGalaxy;
        public Image ZoomSystem;
        public RectTransform ZoomSlider;
        public Color EnableZoomColor;
        public Color DisableZoomColor;
        public bool EnableGalaxyZoom;
        bool enableGalaxyZoom;
        public bool EnableSystemZoom;
        bool enableSystemZoom;

        new RectTransform transform;

        public float GalaxyZoom;
        public float SystemZoom;
        [Range(0,1)]
        public float Zoom = 0.5f;

        private Vector2 prefMousePosisition;
        // Use this for initialization
        void Start()
        {
            transform = gameObject.transform as RectTransform;
        }

        // Update is called once per frame
        void Update()
        {
            if (EnableGalaxyZoom != enableGalaxyZoom)
            {
                ZoomGalaxy.color = EnableGalaxyZoom ? EnableZoomColor : DisableZoomColor;
                enableGalaxyZoom = EnableGalaxyZoom;
            }
            if (EnableSystemZoom != enableSystemZoom)
            {
                ZoomSystem.color = EnableSystemZoom ? EnableZoomColor : DisableZoomColor;
                enableSystemZoom = EnableSystemZoom;
            }
            Zoom = Mathf.Clamp(Zoom, enableGalaxyZoom ? 0 : 0.75f, enableSystemZoom ? 1 : 0.75f);

            ZoomSlider.anchoredPosition = new Vector2(transform.sizeDelta.x * Zoom, 0);

            GalaxyZoom = Mathf.Clamp01(ZoomSlider.anchoredPosition.x / (transform.sizeDelta.x * 0.75f));
            SystemZoom = Mathf.Clamp01((ZoomSlider.anchoredPosition.x - (transform.sizeDelta.x * 0.75f)) / (transform.sizeDelta.x * 0.25f));

            float scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta != 0)
            {
                Zoom += scrollDelta * Time.deltaTime;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 localMousePosition = transform.InverseTransformPoint(Input.mousePosition);
                if (transform.rect.Contains(localMousePosition))
                {
                    prefMousePosisition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
                else
                {
                    prefMousePosisition = Vector2.zero;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (prefMousePosisition != Vector2.zero)
                {
                    Vector2 mouseDelta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - prefMousePosisition;
                    if (mouseDelta.x != 0)
                    {
                        Zoom += mouseDelta.x * Time.deltaTime / 10;
                    }
                    prefMousePosisition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
            }
            else
            {
                prefMousePosisition = Vector2.zero;
            }
        }
    }
}