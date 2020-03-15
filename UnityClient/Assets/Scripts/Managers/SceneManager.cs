using COSMOS.Core.Paterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Managers
{
    public sealed class SceneManager : SingletonMono<SceneManager>
    {
        public static Level CurrentLevel { get; private set; }

        public static event Action OnLevelLoaded;
        private void Awake()
        {
            InitPatern();
        }

        public static void LoadLevel(Level level)
        {
            instance.StopCoroutine(instance.Loading());
            if (CurrentLevel != null)
            {
                UnLoadLevel();
            }
            CurrentLevel = level;
            instance.StartCoroutine(instance.Loading());
        }
        public static void UnLoadLevel()
        {

        }
        IEnumerator Loading()
        {
            if (CurrentLevel != null)
            {
                Dictionary<LevelObject, int> children = new Dictionary<LevelObject, int>();
                double delta = 0;
                Stopwatch timer = new Stopwatch();
                foreach (var obj in CurrentLevel.LevelObjects)
                {
                    LevelObject child = obj;
                    children.Clear();
                    do
                    {
                        timer.Start();
                        // spawn logic

                        timer.Stop();
                        timer.Reset();
                        delta += timer.ElapsedMilliseconds;
                        if(delta >= 16)
                        {
                            yield return new WaitForEndOfFrame();
                            delta = 0;
                        }
                        do
                        {
                            // found new child
                            if(child.Children.Length > 0)
                            {
                                if (children.ContainsKey(child))
                                {
                                    int i = children[child];
                                    if(i > child.Children.Length)
                                    {
                                        children.Remove(child);
                                        if(child.Parent != null)
                                        {
                                            child = child.Parent;
                                        }
                                    }
                                    else
                                    {
                                        children[child]++;
                                        child = child.Children[i];
                                        break;
                                    }
                                }
                                else
                                {
                                    children.Add(child, 1);
                                    child = child.Children[0];
                                    break;
                                }

                            }
                            else
                            {
                                if(child.Parent != null)
                                {
                                    child = child.Parent;
                                }
                            }
                        }
                        while (child.Parent != null);
                    }
                    while (child.Parent != null);
                }
            }
            OnLevelLoaded?.Invoke();
            yield return null;
        }

        IEnumerator UnLoading()
        {
            yield return null;
        }
    }
}
