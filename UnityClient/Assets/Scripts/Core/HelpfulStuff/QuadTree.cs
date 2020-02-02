using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.Core.HelpfulStuff
{
    public class QuadTree<T>
    {
        public struct Point<T>
        {
            public T Value;
            public Vector2 Position;
        }
        public const int QT_NODE_CAPACITY = 4;
        public const int QT_NODE_DEEP = 10;

        List<Point<T>> points = new List<Point<T>>();

        Rect boundary;

        int deep = 0;

        QuadTree<T> northWest;
        QuadTree<T> northEast;
        QuadTree<T> southWest;
        QuadTree<T> southEast;

        public QuadTree(Rect rect)
        {
            boundary = rect;
        }
        QuadTree(Rect rect, int deep)
        {
            boundary = rect;
            this.deep = deep;
        }

        public bool Insert(Point<T> point)
        {
            if (!boundary.Contains(point.Position))
            {
                return false;
            }

            if (points.Count < QT_NODE_CAPACITY || deep >= QT_NODE_DEEP)
            {
                points.Add(point);
                return true;
            }
            else
            {
                if (northWest == null)
                {
                    subdivide();
                }
                if (!northWest.Insert(point) && !northEast.Insert(point) && !southWest.Insert(point) && !southEast.Insert(point))
                {
                    return false;
                }
            }
            return false;
        }
        public Point<T>[] QueryRange(Rect range, int queryDeep = QT_NODE_DEEP)
        {
            List<Point<T>> tmp = new List<Point<T>>();

            if (queryDeep > 0)
            {
                if (boundary.Overlaps(range))
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (range.Contains(points[i].Position))
                        {
                            tmp.Add(points[i]);
                        }
                    }

                    if (northWest != null)
                    {
                        tmp.AddRange(northWest.QueryRange(range, queryDeep - 1));
                        tmp.AddRange(northEast.QueryRange(range, queryDeep - 1));
                        tmp.AddRange(southWest.QueryRange(range, queryDeep - 1));
                        tmp.AddRange(southEast.QueryRange(range, queryDeep - 1));
                    }
                }
            }
            return tmp.ToArray();
        }

        void subdivide()
        {
            northWest = new QuadTree<T>(new Rect(boundary.xMin, boundary.yMin + boundary.height / 2, boundary.width / 2, boundary.height / 2), deep + 1);
            northEast = new QuadTree<T>(new Rect(boundary.xMin + boundary.width / 2, boundary.yMin + boundary.height / 2, boundary.width / 2, boundary.height / 2), deep + 1);
            southWest = new QuadTree<T>(new Rect(boundary.xMin, boundary.yMin, boundary.width / 2, boundary.height / 2), deep + 1);
            southEast = new QuadTree<T>(new Rect(boundary.xMin + boundary.width / 2, boundary.yMin, boundary.width / 2, boundary.height / 2), deep + 1);
        } 

        #region UNITY_EDITOR
        public void DebugDraw()
        {
            Debug.DrawLine(boundary.min, new Vector3(boundary.xMin, boundary.yMax), Color.red);
            Debug.DrawLine(boundary.min, new Vector3(boundary.xMax, boundary.yMin), Color.red);

            Debug.DrawLine(boundary.max, new Vector3(boundary.xMax, boundary.yMin), Color.red);
            Debug.DrawLine(boundary.max, new Vector3(boundary.xMin, boundary.yMax), Color.red);

            if(northWest != null)
            {
                northWest.DebugDraw();
                northEast.DebugDraw();
                southWest.DebugDraw();
                southEast.DebugDraw();
            }
        }
        #endregion
    }
}
