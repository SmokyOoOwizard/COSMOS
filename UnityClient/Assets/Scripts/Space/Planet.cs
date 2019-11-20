using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COSMOS.DataBase;
namespace COSMOS.Space {
    public class Planet : MonoBehaviour
    {
        public static Planet CreatePlanet(PlanetProto proto, GameObject parent)
        {
            GameObject prefab = AssetsDatabase.LoadGameObject(@"ProtoPrefabs\Planet");
            prefab = Instantiate(prefab);
            prefab.transform.SetParent(parent.transform);
            prefab.GetComponent<Planet>().trail = prefab.transform.Find("Trail").gameObject;
            prefab.GetComponent<Planet>().Init(proto);
            return prefab.GetComponent<Planet>();
        }
        PlanetProto proto;
        GameObject trail;
        public int TrailDetail = 50;
        float time;
        public void Init(PlanetProto proto)
        {
            this.proto = proto;
            GenerateTrail();
        }

        private void Update()
        {
            time = (time + proto.OrbitSpeed * Time.deltaTime) % 360;
            float x = Mathf.Sin(time * Mathf.Deg2Rad);
            float y = Mathf.Cos(time * Mathf.Deg2Rad);

            transform.localPosition = new Vector3(x, 0, y) * proto.OrbitSize;
            trail.transform.position = transform.parent.position;
            trail.transform.localRotation = Quaternion.Euler(0, time, 0);
        }
        public void GenerateTrail()
        {
            LineRenderer lr = trail.GetComponent<LineRenderer>();
            Vector3[] points = new Vector3[TrailDetail];
            for (int i = 0; i < TrailDetail; i++)
            {
                float angle = ((360 / TrailDetail) * (i % TrailDetail)) * Mathf.Deg2Rad;
                points[i] = new Vector3(Mathf.Sin(angle) * proto.OrbitSize, 0, Mathf.Cos(angle) * proto.OrbitSize);
            }
            lr.positionCount = TrailDetail;
            lr.loop = true;
            lr.SetPositions(points);
        }
    }
}