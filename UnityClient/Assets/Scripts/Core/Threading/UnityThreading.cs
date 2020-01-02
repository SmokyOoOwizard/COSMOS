using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Core.Threading
{
    [Manager]
    public static class UnityThreading
    {
        public enum Queue
        {
            Update,
            LateUpdate,
            FixedUpdate
        }
        class ExecuteObject : MonoBehaviour
        {
            object sync = new object();
            List<Action> updateActions = new List<Action>();
            List<Action> lateUpdateActions = new List<Action>();
            List<Action> fixedUpdateActions = new List<Action>();


            private void Update()
            {
                lock (sync)
                {
                    for (int i = 0; i < updateActions.Count; i++)
                    {
                        updateActions[i]?.Invoke();
                    }
                    updateActions.Clear();
                }
            }
            private void LateUpdate()
            {
                lock (sync)
                {
                    for (int i = 0; i < lateUpdateActions.Count; i++)
                    {
                        lateUpdateActions[i]?.Invoke();
                    }
                    lateUpdateActions.Clear();
                }
            }
            private void FixedUpdate()
            {
                lock (sync)
                {
                    for (int i = 0; i < fixedUpdateActions.Count; i++)
                    {
                        fixedUpdateActions[i]?.Invoke();
                    }
                    fixedUpdateActions.Clear();
                }
            }

            public void Execute(Action action, Queue queue = Queue.Update)
            {
                lock (sync)
                { //обеспечиваем потокобезопасность записи в лист
                    switch (queue)
                    {
                        case Queue.Update:
                            updateActions.Add(action);
                            break;
                        case Queue.LateUpdate:
                            lateUpdateActions.Add(action);
                            break;
                        case Queue.FixedUpdate:
                            fixedUpdateActions.Add(action);
                            break;
                    }
                }
            }
            public void WaitExecute(Action action, Queue queue = Queue.Update)
            {
                lock (sync)
                { //обеспечиваем потокобезопасность записи в лист
                    switch (queue)
                    {
                        case Queue.Update:
                            updateActions.Add(action);
                            break;
                        case Queue.LateUpdate:
                            lateUpdateActions.Add(action);
                            break;
                        case Queue.FixedUpdate:
                            fixedUpdateActions.Add(action);
                            break;
                    }
                }
                try
                {
                    if (!Task.CurrentId.HasValue)
                    {
                        Thread.Sleep(Timeout.Infinite);
                    }
                }
                catch (ThreadInterruptedException)
                {
                }
                finally { }
            }

        }
        static Dictionary<GameObject, ExecuteObject> ExecuteObjects = new Dictionary<GameObject, ExecuteObject>();
        static ExecuteObject MainObject;

        [InitMethod(int.MaxValue - 1)]
        public static void Init()
        {
            ExecuteObjects.Clear();
            MainObject = (new GameObject("Main threading object")).AddComponent<ExecuteObject>();
            GameObject.DontDestroyOnLoad(MainObject.gameObject);
        }

        public static void Execute(Action action, Queue queue = Queue.Update)
        {
            MainObject.Execute(action, queue);
        }
        public static void Execute(GameObject go, Action action, Queue queue = Queue.Update)
        {
            if (go == null)
            {
                Execute(action, queue);
                return;
            }
            if (!ExecuteObjects.ContainsKey(go))
            {
                Execute(() => { ExecuteObjects.Add(go, go.AddComponent<ExecuteObject>()); ExecuteObjects[go].Execute(action, queue); });
                return;
            }
            ExecuteObjects[go].Execute(action, queue);
        }
        public static void WaitExecute(Action action, Queue queue = Queue.Update)
        {
            Thread thread = Thread.CurrentThread;
            if (Task.CurrentId.HasValue)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                MainObject.WaitExecute(() => { action?.Invoke(); resetEvent.Set(); }, queue);

                resetEvent.WaitOne();
            }
            else
            {
                MainObject.WaitExecute(() => { action?.Invoke(); thread.Interrupt(); });
            }
        }
        public static void WaitExecute(GameObject go, Action action, Queue queue = Queue.Update)
        {
            if (go == null)
            {
                WaitExecute(action, queue);
                return;
            }
            if (!ExecuteObjects.ContainsKey(go))
            {
                WaitExecute(() => { ExecuteObjects.Add(go, go.AddComponent<ExecuteObject>()); });
            }
            Thread thread = Thread.CurrentThread;
            if (Task.CurrentId.HasValue)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                ExecuteObjects[go].WaitExecute(() => { action?.Invoke(); resetEvent.Set(); }, queue);
                resetEvent.WaitOne();
            }
            else
            {
                ExecuteObjects[go].WaitExecute(() => { action?.Invoke(); thread.Interrupt(); }, queue);
            }
        }
    }
}
