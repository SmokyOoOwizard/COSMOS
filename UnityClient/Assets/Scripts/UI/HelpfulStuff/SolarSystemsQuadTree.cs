using COSMOS.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace COSMOS.UI.HelpfulStuff
{
    public sealed class SolarSystemsQuadTree
    {
        public const int QT_NODE_CAPACITY = 4;
        class Quad
        {
            List<SolarSystem> points = new List<SolarSystem>();

            public Rect boundary;

            public int deep = 0;

            public Quad parent;

            public Quad northWest;
            public Quad northEast;
            public Quad southWest;
            public Quad southEast;

            public Quad(Rect rect)
            {
                boundary = rect;
            }
            Quad(Rect rect, int deep)
            {
                boundary = rect;
                this.deep = deep;
            }

            public bool Insert(SolarSystem point)
            {
                if (!boundary.Contains(point.PosOnMap))
                {
                    return false;
                }

                if (points.Count < QT_NODE_CAPACITY)
                {
                    points.Add(point);
                    return true;
                }
                else
                {
                    subdivide();
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (northWest.Insert(points[i]) || northEast.Insert(points[i]) ||
                            southWest.Insert(points[i]) || southEast.Insert(points[i]))
                        {
                            points.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                    if (!northWest.Insert(point) && !northEast.Insert(point) &&
                        !southWest.Insert(point) && !southEast.Insert(point))
                    {
                        if(points.Count < QT_NODE_CAPACITY)
                        {
                            points.Add(point);
                            return true;
                        }
                    }
                }
                return false;
            }
            public SolarSystem[] QueryRange(Rect range, float queryDeep)
            {
                List<SolarSystem> tmp = new List<SolarSystem>();

                if (queryDeep > 0)
                {
                    if (boundary.Overlaps(range))
                    {
                        for (int i = 0; i < points.Count; i++)
                        {
                            if (range.Contains(points[i].PosOnMap))
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
                if (northWest == null)
                {
                    northWest = new Quad(new Rect(boundary.xMin, boundary.yMin + boundary.height / 2, boundary.width / 2, boundary.height / 2), deep + 1);
                    northWest.parent = this;
                }
                if (northEast == null)
                {
                    northEast = new Quad(new Rect(boundary.xMin + boundary.width / 2, boundary.yMin + boundary.height / 2, boundary.width / 2, boundary.height / 2), deep + 1);
                    northEast.parent = this;
                }
                if (southWest == null)
                {
                    southWest = new Quad(new Rect(boundary.xMin, boundary.yMin, boundary.width / 2, boundary.height / 2), deep + 1);
                    southWest.parent = this;
                }
                if (southEast == null)
                {
                    southEast = new Quad(new Rect(boundary.xMin + boundary.width / 2, boundary.yMin, boundary.width / 2, boundary.height / 2), deep + 1);
                    southEast.parent = this;
                }
            }
            public void debugDraw()
            {
                Debug.DrawLine(new Vector3(boundary.xMin, boundary.yMin), new Vector3(boundary.xMax, boundary.yMin));
                Debug.DrawLine(new Vector3(boundary.xMin, boundary.yMin), new Vector3(boundary.xMin, boundary.yMax));
                Debug.DrawLine(new Vector3(boundary.xMax, boundary.yMin), new Vector3(boundary.xMax, boundary.yMax));
                Debug.DrawLine(new Vector3(boundary.xMin, boundary.yMax), new Vector3(boundary.xMax, boundary.yMax));

                if (northWest != null)
                {
                    northWest.debugDraw();
                }
                if (northEast != null)
                {
                    northEast.debugDraw();
                }
                if (southWest != null)
                {
                    southWest.debugDraw();
                }
                if (southEast != null)
                {
                    southEast.debugDraw();
                }

                for (int i = 0; i < points.Count; i++)
                {
                    Debug.DrawRay(points[i].PosOnMap, Vector3.one);
                }
            }
        }
        public void DebugDraw()
        {
            if (child != null)
            {
                child.debugDraw();
            }
        }

        Quad child;

        public void Insert(SolarSystem value)
        {
            if (child == null)
            {
                if (value.PosOnMap != Vector2.zero)
                {
                    child = new Quad(new Rect(-value.PosOnMap / 2, value.PosOnMap));
                }
                else
                {
                    child = new Quad(new Rect(-5, -5, 10, 10));
                }
            }
            if (!child.boundary.Contains(value.PosOnMap))
            {
                while (true)
                {
                    if (child.boundary.Contains(value.PosOnMap))
                    {
                        break;
                    }
                    else
                    {
                        subdivideUp(value.PosOnMap);
                    }
                }
            }
            child.Insert(value);
        }
        public SolarSystem[] QueryRange(Rect range, float quaryDeep)
        {
            if (child != null)
            {
                return child.QueryRange(range, quaryDeep);
            }
            return null;
        }
        void subdivideUp(Vector2 pos)
        {
            Rect childRect = child.boundary;
            Vector2 dir = Vector2.zero;

            if (new Rect(new Vector2(childRect.position.x, childRect.position.y + childRect.size.y),
                childRect.size).Contains(pos)) dir = new Vector2(0, 1);
            else if (new Rect(new Vector2(childRect.position.x + childRect.size.x, childRect.position.y),
                childRect.size).Contains(pos)) dir = new Vector2(1, 0);
            else if (new Rect(new Vector2(childRect.position.x + childRect.size.x, childRect.position.y + childRect.size.y),
                childRect.size).Contains(pos)) dir = new Vector2(1, 1);
            else if (new Rect(new Vector2(childRect.position.x + childRect.size.x, childRect.position.y - childRect.size.y),
                childRect.size).Contains(pos)) dir = new Vector2(1, -1);
            else if (new Rect(new Vector2(childRect.position.x, childRect.position.y - childRect.size.y),
                childRect.size).Contains(pos)) dir = new Vector2(0, -1);
            else if (new Rect(new Vector2(childRect.position.x - childRect.size.x, childRect.position.y - childRect.size.y),
                childRect.size).Contains(pos)) dir = new Vector2(-1, -1);
            else if (new Rect(new Vector2(childRect.position.x - childRect.size.x, childRect.position.y),
                childRect.size).Contains(pos)) dir = new Vector2(-1, 0);
            else if (new Rect(new Vector2(childRect.position.x - childRect.size.x, childRect.position.y + childRect.size.y),
                childRect.size).Contains(pos)) dir = new Vector2(-1, 1);

            Vector2 newSize = child.boundary.size * 2;
            Quad newChild = null;
            if (dir.y == 1f)
            {
                if (dir.x == 1f || dir.x == 0f)
                {
                    newChild = new Quad(new Rect(childRect.position, newSize));
                    newChild.southWest = child;
                }
                else if (dir.x == -1)
                {
                    newChild = new Quad(new Rect(childRect.position - new Vector2(childRect.size.x, 0),
                        newSize));
                    newChild.southEast = child;
                }
            }
            else if (dir.y == 0f)
            {
                if (dir.x == 1f)
                {
                    newChild = new Quad(new Rect(childRect.position, newSize));
                    newChild.southWest = child;
                }
                else if (dir.x == -1f)
                {
                    newChild = new Quad(new Rect(childRect.position - new Vector2(childRect.size.x, 0),
                        newSize));
                    newChild.southEast = child;
                }
                else if (dir.x == 0f)
                {
                    throw new Exception("point out of bounds " + pos);
                }
            }
            else if (dir.y == -1f)
            {
                if (dir.x == 1f || dir.x == 0f)
                {
                    newChild = new Quad(new Rect(childRect.position - new Vector2(0, childRect.size.y),
                        newSize));
                    newChild.northWest = child;
                }
                else if (dir.x == -1)
                {
                    newChild = new Quad(new Rect(childRect.position - new Vector2(childRect.size.x, childRect.size.y),
                        newSize));
                    newChild.northEast = child;
                }
            }
            if (newChild != null)
            {
                child.parent = newChild;
                child = newChild;
            }
            else
            {
                throw new Exception("QuadTree: new parent is null");
            }
        }
    }
}
