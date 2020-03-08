using COSMOS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace COSMOS.UI
{
    public class NotificationUI : MonoBehaviour
    {
        public const string PREFAB_ID = @"";
        public Notification Notification { get; protected set; }
        public float Speed = 4;

        [SerializeField]
        AnimationCurve AlphaCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField]
        AnimationCurve WidthCurve = AnimationCurve.Linear(0, 0, 1, 1);


        [SerializeField]
        Vector2 TargetSize = new Vector2(20, 20);
        [SerializeField]
        Image Image;

        public float amount = 0;

        public void Init(Notification notification)
        {
            Notification = notification;
            Sprite icon = AssetsDatabase.LoadSprite(Notification.IconID);
            if(icon != null)
            {
                Image.sprite = icon;
            }
        }
        public void Remove()
        {
            StopCoroutine(show());
            StopCoroutine(remove());
            StartCoroutine(remove());
        }
        public void Show()
        {
            StopCoroutine(remove());
            StopCoroutine(show());
            StartCoroutine(show());
        }

        IEnumerator remove()
        {
            while (amount >= 0)
            {
                amount -= Time.deltaTime * Speed;

                float alpha = AlphaCurve.Evaluate(Mathf.Clamp01(amount - 1));
                Color color = Image.color;
                color.a = alpha;
                Image.color = color;

                Vector2 size = (transform as RectTransform).sizeDelta;
                size.x = TargetSize.x * WidthCurve.Evaluate(Mathf.Clamp01(amount));
                (transform as RectTransform).sizeDelta = size;
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
        IEnumerator show()
        {
            while (amount < 2)
            {
                amount += Time.deltaTime * Speed;

                Vector2 size = (transform as RectTransform).sizeDelta;
                size.x = TargetSize.x * (WidthCurve.Evaluate(Mathf.Clamp01(amount)));
                (transform as RectTransform).sizeDelta = size;

                float alpha = AlphaCurve.Evaluate(Mathf.Clamp01(amount - 1));
                Color color = Image.color;
                color.a = alpha;
                Image.color = color;
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
        public static NotificationUI Spawn()
        {
            GameObject prefab = AssetsDatabase.LoadGameObject(PREFAB_ID);
            GameObject obj = Instantiate(prefab);
            return obj.GetComponent<NotificationUI>();
        }
    }
}
