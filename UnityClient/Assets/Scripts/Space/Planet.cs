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
            prefab.GetComponent<Planet>().Init(proto);
            return prefab.GetComponent<Planet>();
        }
        PlanetProto proto;
        public float time;
        public void Init(PlanetProto proto)
        {
            this.proto = proto;
        }

        private void Update()
        {
            time += proto.OrbitSpeed * Time.deltaTime;
            float x = Mathf.Sin(time);
            float y = Mathf.Cos(time);

            transform.localPosition = new Vector3(x, 0, y) * proto.OrbitSize;
        }
    }
}