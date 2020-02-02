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
        List<SolarSystem> points = new List<SolarSystem>();

        Rect boundary;

        int deep = 0;

        SolarSystemsQuadTree northWest;
        SolarSystemsQuadTree northEast;
        SolarSystemsQuadTree southWest;
        SolarSystemsQuadTree southEast;

        public SolarSystemsQuadTree(Rect rect)
        {
            boundary = rect;
        }
        SolarSystemsQuadTree(Rect rect, int deep)
        {
            boundary = rect;
            this.deep = deep;
        }

        public bool Insert(SolarSystem system)
        {
            if (!boundary.Contains(system.PosOnMap))
            {
                return false;
            }

            if ((int)system.ImportanceOnMap <= deep)
            {
                points.Add(system);
                return true;
            }
            else
            {
                if (northWest == null)
                {
                    subdivide();
                }
                if (!northWest.Insert(system) && !northEast.Insert(system) 
                    && !southWest.Insert(system) && !southEast.Insert(system))
                {
                    return false;
                }
            }
            return false;
        }
        public SolarSystem[] QueryRange(Rect range, float queryDeep)
        {
            List<SolarSystem> tmp = new List<SolarSystem>();

            if (queryDeep >= 0)
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
            northWest = new SolarSystemsQuadTree(new Rect(boundary.xMin, boundary.yMin + boundary.height / 2, boundary.width / 2, boundary.height / 2), deep + 1);
            northEast = new SolarSystemsQuadTree(new Rect(boundary.xMin + boundary.width / 2, boundary.yMin + boundary.height / 2, boundary.width / 2, boundary.height / 2), deep + 1);
            southWest = new SolarSystemsQuadTree(new Rect(boundary.xMin, boundary.yMin, boundary.width / 2, boundary.height / 2), deep + 1);
            southEast = new SolarSystemsQuadTree(new Rect(boundary.xMin + boundary.width / 2, boundary.yMin, boundary.width / 2, boundary.height / 2), deep + 1);
        }

    }
}
