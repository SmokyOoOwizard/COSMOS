using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace COSMOS.HelpfullStuff.UI 
{
    [RequireComponent(typeof(Image))]
    public class RandomAlpha : MonoBehaviour
    {
        [Range(0, 1)]
        public float MinAlpha = 0;
        [Range(0, 1)]
        public float MaxAlpha = 1;
        public float CurrentAlpha;
        [Range(0, 1)]
        public float ChanceOfChange = 0.5f;


        Image image;

        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Random.Range(0f, 1f) > ChanceOfChange)
            {
                CurrentAlpha += Mathf.Sign(Random.Range(-1, 1)) * Time.deltaTime;
            }
            CurrentAlpha = Mathf.Clamp(CurrentAlpha, MinAlpha, MaxAlpha);

            Color tmp = image.color;
            tmp.a = CurrentAlpha;
            image.color = tmp;
        }
    }
}
