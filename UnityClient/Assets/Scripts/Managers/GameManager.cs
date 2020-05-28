using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COSMOS.Core.Paterns;
namespace COSMOS {
    public class GameManager : SingletonMono<GameManager>
    {
        public float TimeScale = 3;
        // Start is called before the first frame update
        private void Awake()
        {
            InitPatern();
        }
        // Update is called once per frame
        void Update()
        {
            GameData.CurrentDate = GameData.CurrentDate.AddSeconds(Time.deltaTime * 10 * TimeScale);
        }
    }
}
