﻿using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{

    public class Polygon2D : AABB2D
    {
        // 顶点列表多+ 1,回头了
        public Float2[] pointArr = null;
        /// <summary>
        /// 逆时针
        /// </summary>
        public float[] distancArr = null;
        public float totalDistance = 0;
        public Polygon2D(Float2 [] points) 
        {
            if (points == null || points.Length < 3)
                return;
            this.pointArr = new Float2[points.Length];
            this.distancArr = new float[points.Length + 1];
            Float2 min = points[0];
            Float2 max = points[0];
            int i = 0;
            for (i = 0; i < points.Length; i++)
            {
                this.pointArr[i] = points[i];
                if (i > 0)
                {
                    min = Float2.Min(min, points[i]);
                    max = Float2.Max(max, points[i]);
                    totalDistance += (float)Float2.Distance(points[i - 1], points[i]);
                    this.distancArr[i] = totalDistance;
                }
                else 
                {
                    min = points[i];
                    max = points[i];
                    this.distancArr[i] = 0;
                    totalDistance = 0;
                }
            }
            totalDistance += (float)Float2.Distance(points[points.Length - 1], points[0]);
            this.distancArr[i] = totalDistance;
            this.SetAABB(min, max);
        }
        /// <summary>
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="paths">返回路径</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public override bool RayboundingNearestPath(LineSegment2D line, float offset, ref List<Float2> paths)
        {
            List<Float3> lineArray = new List<Float3>();
            Float2 intersectionPoint = Float2.zero;
            for (int i = 0; i < this.pointArr.Length ; i++)
            {
                if (i == this.pointArr.Length - 1)
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[0]), ref intersectionPoint) == true)
                    {
                        lineArray.Add(new Float3(intersectionPoint.x, intersectionPoint.y, i));
                    }
                }
                else 
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[i + 1]), ref intersectionPoint) == true)
                    {
                        lineArray.Add(new Float3(intersectionPoint.x, intersectionPoint.y, i));
                    }
                }
            }
            if (lineArray.Count == 0)
            {
                return false;
            }
            else 
            {
                // 先按距离进行排序。
                lineArray.Sort((x, y) => Float2.Distance(new Float2(x.x, x.y), line.startPoint).CompareTo(Float2.Distance(new Float2(y.x, y.y), line.startPoint)));
                //
                bool isPathDir = CheckPathDir(lineArray[0], lineArray[lineArray.Count - 1]);
                if (CheckSamePathDir(lineArray, isPathDir) == true)
                {
                    bool isIn = true;
                    for (int i = 0; i < lineArray.Count - 1; i++)
                    {
                        if (isIn == true)
                        {
                            List<Float2> temppaths = new List<Float2>();
                            RayboundingNearestPath(lineArray[i], lineArray[i + 1], offset, isPathDir, ref temppaths);
                            if (paths == null)
                            {
                                paths = temppaths;
                            }
                            else
                            {
                                paths.AddRange(temppaths);
                            }
                        }
                        isIn = !isIn;
                    }
                }
                else  // 与整体方向不一致。直接暴力取2头的点。
                {
                    List<Float2> temppaths = new List<Float2>();
                    RayboundingNearestPath(lineArray[0], lineArray[lineArray.Count - 1], offset, isPathDir, ref temppaths);
                    if (paths == null)
                    {
                        paths = temppaths;
                    }
                    else
                    {
                        paths.AddRange(temppaths);
                    }
                }
                if (paths != null && paths.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 确认局部方向是否跟全局方向保持一致
        /// </summary>
        /// <param name="lineArray"></param>
        /// <param name="isPathDir"></param>
        /// <returns></returns>
        private bool CheckSamePathDir(List<Float3> lineArray, bool isPathDir)
        {
            if (lineArray == null || lineArray.Count < 3)
                return true;
            int p1 = (int)lineArray[0].z;
            int p2 = (int)lineArray[1].z;
            int p3 = (int)lineArray[2].z;
            if (p1 >= p2 && p1 >= p3)
            {
                if (p3 >= p2)
                {
                    // 顺时针方向
                    return isPathDir;
                }
                else
                {
                    return !isPathDir;
                }
            }
            else if (p2 >= p1 && p2 >= p3)
            {
                if (p1 >= p3)
                {
                    // 顺时针方向
                    return isPathDir;
                }
                else
                {
                    return !isPathDir;
                }
            }
            else 
            {
                if (p2 >= p1)
                {
                    // 顺时针方向
                    return isPathDir;
                }
                else
                {
                    return !isPathDir;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="sl"></param>
        private void RayboundingNearestPath(Float3 p1, Float3 p2, float offset, bool isPathDir, ref List<Float2> paths)
        {
            List<Float2> listpath = new List<Float2>();
            // 先计算逆时针距离。
            if (p1.z < p2.z)
            {
                if (isPathDir == true)
                {
                    listpath.Add(new Float2(p1.x, p1.y));
                    for (int i = (int)p1.z + 1; i <= (int)p2.z; i++)
                    {
                        listpath.Add(this.pointArr[i]);
                    }
                    listpath.Add(new Float2(p2.x, p2.y));
                }
                else 
                {
                    listpath.Add(new Float2(p1.x, p1.y));
                    //
                    for (int i = (int)p1.z; i >= 0; i--)
                    {
                        listpath.Add(this.pointArr[i]);
                    }
                    //
                    for (int i = this.pointArr.Length -1; i > (int)p2.z; i--)
                    {
                        listpath.Add(this.pointArr[i]);
                    }
                    listpath.Add(new Float2(p2.x, p2.y));
                }
            }
            else if (p1.z > p2.z)
            {
                if (isPathDir == false)
                {
                    listpath.Add(new Float2(p1.x, p1.y));
                    for (int i = (int)p1.z; i >= (int)p2.z + 1; i--)
                    {
                        listpath.Add(this.pointArr[i]);
                    }
                    listpath.Add(new Float2(p2.x, p2.y));
                }
                else
                {
                    listpath.Add(new Float2(p1.x, p1.y));
                    for (int i = (int)p1.z + 1; i < this.pointArr.Length; i++)
                    {
                        listpath.Add(this.pointArr[i]);
                    }
                    //
                    for (int i = 0; i <= (int)p2.z; i ++)
                    {
                        listpath.Add(this.pointArr[i]);
                    }
                    //
                    listpath.Add(new Float2(p2.x, p2.y));
                }
            }
            else 
            {
                listpath.Add(new Float2(p1.x, p1.y));
                listpath.Add(new Float2(p2.x, p2.y));
            }
            paths = listpath;
        }
        /// <summary>
        /// 确认方向（顺时针，逆时针方向）
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private bool CheckPathDir(Float3 p1, Float3 p2)
        {
            // 先计算逆时针距离。
            float dis;
            // 对边行为
            if (p1.z < p2.z)
            {
                int max = (int)p2.z + 1;
                max = max > this.pointArr.Length - 1 ? 0 : max;
                dis = distancArr[(int)p2.z + 1] - distancArr[(int)p1.z]
                    - (float)Float2.Distance(new Float2(p1.x, p1.y), this.pointArr[(int)p1.z])
                    - (float)Float2.Distance(new Float2(p2.x, p2.y), this.pointArr[max]);
                if (dis < totalDistance - dis)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (p1.z > p2.z)
            {
                int max = (int)p1.z + 1;
                max = max > this.pointArr.Length - 1 ? 0 : max;
                dis = distancArr[(int)p1.z + 1] - distancArr[(int)p2.z]
                - (float)Float2.Distance(new Float2(p2.x, p2.y), this.pointArr[(int)p2.z])
                - (float)Float2.Distance(new Float2(p1.x, p1.y), this.pointArr[max]);
                if (dis < totalDistance - dis)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 获取挡格附近出生点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <param name="bornPoint"></param>
        /// <returns></returns>
        public override bool GetBornPoint(LineSegment2D line, float offset, ref Float2 bornPoint)
        {
            for (int i = 0; i < this.pointArr.Length; i++)
            {
                if (i == this.pointArr.Length - 1)
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[0]), ref bornPoint) == true)
                    {
                        bornPoint = line.normalizedDir * ((bornPoint - line.startPoint).magnitude + offset) + line.startPoint;
                        return true;
                    }
                }
                else
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[i + 1]), ref bornPoint) == true)
                    {
                        bornPoint = line.normalizedDir * ((bornPoint - line.startPoint).magnitude + offset) + line.startPoint;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public override LineRelation CheckLineRelation(LineSegment2D line)
        {
            Float2 pos = Float2.zero;
            for (int i = 0; i < this.pointArr.Length; i++)
            {
                if (i == this.pointArr.Length - 1)
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[0]), ref pos) == true)
                    {
                        return LineRelation.Intersect;
                    }
                }
                else
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[i + 1]), ref pos) == true)
                    {
                        return LineRelation.Intersect;
                    }
                }
            }
            return LineRelation.Detach;
        }
    }
}