using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace COSMOS.Space {
    public class PlanetScene : MonoBehaviour
    {
        public static PlanetScene CreatePlanet(Planet proto, GameObject parent)
        {
            GameObject prefab = AssetsDatabase.LoadGameObject(@"Prefabs\Planet");
            prefab = Instantiate(prefab);
            prefab.transform.SetParent(parent.transform);
            prefab.GetComponent<PlanetScene>().trail = prefab.transform.Find("Trail").gameObject;
            prefab.GetComponent<PlanetScene>().Init(proto);
            return prefab.GetComponent<PlanetScene>();
        }
        [SerializeField] Planet owner;
        GameObject trail;
        public int TrailDetail = 50;
        float time = 1;
        public void Init(Planet proto)
        {
            this.owner = proto;
            trail = transform.Find("Trail").gameObject;
            GenerateTrail();
        }
         
        private void Update()
        {
            time = (time + (owner.OrbitSpeed * Time.deltaTime) / owner.OrbitSize) % 360;
            time = float.IsNaN(time) ? 0 : time;
            float x = Mathf.Sin(time * Mathf.Deg2Rad);
            float y = Mathf.Cos(time * Mathf.Deg2Rad);

            transform.localPosition = new Vector3(x, 0, y) * owner.OrbitSize;
            trail.transform.position = transform.parent.position;
            trail.transform.localRotation = Quaternion.Euler(0, time, 0);
        }
        public void GenerateTrail()
        {
            LineRenderer lr = trail.GetComponent<LineRenderer>();
            Vector3[] points = new Vector3[TrailDetail];
            for (int i = 0; i < TrailDetail; i++)
            {
                float angle = ((360 / TrailDetail) * (i % TrailDetail)) * Mathf.Deg2Rad * Mathf.Sign(-owner.OrbitSpeed);
                points[i] = new Vector3(Mathf.Sin(angle) * owner.OrbitSize, 0, Mathf.Cos(angle) * owner.OrbitSize);
            }
            lr.positionCount = TrailDetail;
            lr.loop = false;
            lr.SetPositions(points);
        }
    }
}