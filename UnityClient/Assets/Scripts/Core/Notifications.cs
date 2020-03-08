using COSMOS.Core.EventDispacher;
using COSMOS.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSMOS.Core
{
    public class Notification : DataWithEvents
    {
        public virtual string IconID { get; protected set; }
        public virtual string LkeyDescription { get; protected set; }
        public virtual int Preority { get; protected set; } = 0;
    }
    public class Notifications
    {
        public delegate void OnNotification(Notification notification, int id);

        readonly List<Notification> notifications = new List<Notification>();
        ReadOnlyCollection<Notification> notificationsReadOnly;
        bool needNewReadOnly = true;

        public event OnNotification OnNewNotification; 
        public event Action<Notification> OnRemoveNotification;

        public void AddNotification(Notification notification)
        {
            notifications.Add(notification);
            notifications.Sort((x, y) => x.Preority.CompareTo(y.Preority));
            int index = notifications.IndexOf(notification);
            needNewReadOnly = true;
            OnNewNotification?.Invoke(notification, index);
        }
        public void Remove(int i)
        {
            if(notifications.Count < i)
            {
                Notification notiForDelete = notifications[i];
                notifications.RemoveAt(i);
                needNewReadOnly = true;
                OnRemoveNotification?.Invoke(notiForDelete);
            }
        }
        public bool Remove(Notification notification)
        {
            needNewReadOnly = true;
            bool result = notifications.Remove(notification);
            if (result)
            {
                OnRemoveNotification?.Invoke(notification);
            }
            return result;
        }
        public Notification Get(int i)
        {
            if(notifications.Count < i)
            {
                return notifications[i];
            }
            return null;
        }
        public ReadOnlyCollection<Notification> GetAll()
        {
            if (needNewReadOnly)
            {
                notificationsReadOnly = notifications.AsReadOnly();
                needNewReadOnly = false;
            }
            return notificationsReadOnly;
        }
    }
}
