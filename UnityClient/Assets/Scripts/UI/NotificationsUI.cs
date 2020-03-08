using UnityEngine;
using COSMOS.Core;
using System.Collections.Generic;
using COSMOS.Core.HelpfulStuff;

namespace COSMOS.UI
{
    public class NotificationsUI : MonoBehaviour
    {
        readonly Dictionary<Notification, NotificationUI> notificationsWithUI
            = new Dictionary<Notification, NotificationUI>();
        readonly List<Notification> notificationsOrder = new List<Notification>();
        ObjectPool<NotificationUI> uiPool;

        [SerializeField]
        GameObject content;
        private void Awake()
        {
            GameData.PlayerNotifications.OnNewNotification += OnNewNotification;
            GameData.PlayerNotifications.OnRemoveNotification += OnRemoveNotification;
            uiPool = new ObjectPool<NotificationUI>(30, true, createNotificationUI, null);
        }

        private void OnRemoveNotification(Notification obj)
        {
            if (notificationsWithUI.ContainsKey(obj))
            {
                notificationsWithUI[obj].Remove();
                uiPool.Release(notificationsWithUI[obj]);
                notificationsOrder.Remove(obj);
                notificationsWithUI.Remove(obj);
            }
        }

        private void OnNewNotification(Notification notification, int id)
        {
            if (notificationsWithUI.ContainsKey(notification))
            {
                notificationsOrder.Remove(notification);
            }
            else
            {
                NotificationUI ui = uiPool.GetObject();
                notificationsWithUI.Add(notification, ui);
                ui.Init(notification);
            }
            notificationsOrder.Insert(id, notification);
            SortNotifications();
            notificationsWithUI[notification].Show();

        }
        NotificationUI createNotificationUI()
        {
            NotificationUI ui = NotificationUI.Spawn();
            ui.transform.SetParent(content.transform);
            return ui;
        }
        public void SortNotifications()
        {
            for (int i = 0; i < notificationsOrder.Count; i++)
            {
                Notification notification = notificationsOrder[i];
                NotificationUI uI = notificationsWithUI[notification];
                uI.transform.SetSiblingIndex(i);
            }
        }
    }
}
