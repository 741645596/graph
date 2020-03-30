using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{

    public class Polygon2D : AABB2D
    {
        /// <summary>
        /// 顶点列表
        /// </summary>
        public Float2[] pointArr = null;
        /// <summary>
        /// 法线列表。
        /// </summary>
        public Float2[] normalAttr = null;
        /// <summary>
        /// 逆时针
        /// </summary>
        private float[] distancArr = null;
        private float totalDistance = 0;
        /// <summary>
        /// 获取边数
        /// </summary>
        /// <returns></returns>
        public int EdgeNum
        {
            get { return this.pointArr.Length; }
        }

        public Polygon2D(Float2 [] points) 
        {
            if (points == null || points.Length < 3)
                return;
            this.pointArr = new Float2[points.Length];
            this.normalAttr = new Float2[points.Length]; 


            this.distancArr = new float[points.Length + 1];
            Float2 min = points[0];
            Float2 max = points[0];
            int i ;
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
                //
                Float2 dir;
                if (i < points.Length - 1)
                {
                    dir = (points[i + 1] - points[i]).normalized;
                }
                else 
                {
                    dir = (points[0] - points[i]).normalized;
                }
                this.normalAttr[i] = Float2.Perpendicular(dir);
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
                            RayboundingNearestPath(line, lineArray[i], lineArray[i + 1], offset, isPathDir, ref temppaths);
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
                    RayboundingNearestPath(line, lineArray[0], lineArray[lineArray.Count - 1], offset, isPathDir, ref temppaths);
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
        private void RayboundingNearestPath(LineSegment2D line, Float3 p1, Float3 p2, float offset, bool isPathDir, ref List<Float2> paths)
        {
            List<Float2> listpath = new List<Float2>();
            Float2 first = new Float2(p1.x, p1.y);
            Float2 diff = offset * line.normalizedDir;
            // 先计算逆时针距离。
            if (p1.z < p2.z)
            {
                if (isPathDir == true)
                {
                    listpath.Add(first - diff);
                    for (int i = (int)p1.z + 1; i <= (int)p2.z; i++)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    listpath.Add(new Float2(p2.x, p2.y) + diff);
                }
                else 
                {
                    listpath.Add(first - diff);
                    //
                    for (int i = (int)p1.z; i >= 0; i--)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    //
                    for (int i = this.pointArr.Length -1; i > (int)p2.z; i--)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    listpath.Add(new Float2(p2.x, p2.y) + diff);
                }
            }
            else if (p1.z > p2.z)
            {
                if (isPathDir == false)
                {
                    listpath.Add(first - diff);
                    for (int i = (int)p1.z; i >= (int)p2.z + 1; i--)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    listpath.Add(new Float2(p2.x, p2.y) + diff);
                }
                else
                {
                    listpath.Add(first - diff);
                    for (int i = (int)p1.z + 1; i < this.pointArr.Length; i++)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    //
                    for (int i = 0; i <= (int)p2.z; i ++)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    //
                    listpath.Add(new Float2(p2.x, p2.y) + diff);
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
        /// 获取指定顶点的外部偏移点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Float2 GetOutPoint(int index, float offset)
        {
            if (index < 0 || index >= this.pointArr.Length)
                return Float2.zero;
            Float2 diff;
            if (index == 0)
            {
                diff = (this.pointArr[index] - this.pointArr[this.pointArr.Length - 1]).normalized + (this.pointArr[index] - this.pointArr[index + 1]).normalized;
            }
            else if (index == this.pointArr.Length - 1)
            {
                diff = (this.pointArr[index] - this.pointArr[index - 1]).normalized + (this.pointArr[index] - this.pointArr[0]).normalized;
            }
            else 
            {
                diff = (this.pointArr[index] - this.pointArr[index - 1]).normalized + (this.pointArr[index] - this.pointArr[index + 1]).normalized;
            }
            return this.pointArr[index] + offset * diff;
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

        /// <summary>
        /// 获取多边形的类型，凹的还是凸的。
        /// </summary>
        /// <returns></returns>
        public PolygonType GetPolygonType()
        {
            float prev = CalcRotateValue(this.pointArr.Length -1);
            float cur = 0.0f;
            for (int i = 0; i < this.pointArr.Length; i++)
            {
                cur = CalcRotateValue(i);
                if (cur * prev < 0)
                {
                    return PolygonType.Concave;
                }
                prev = cur;
            }
            return PolygonType.Convex;
        }
        /// <summary>
        /// 计算某个顶点叉乘。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private float CalcRotateValue(int index)
        {
            if (index < 0 || index >= this.pointArr.Length)
                return 0;
            if (index < this.pointArr.Length - 1 && index > 0)
            {
                return Float2.Cross(this.pointArr[index] - this.pointArr[index - 1], this.pointArr[index + 1] - this.pointArr[index]);
            }
            else if (index == this.pointArr.Length - 1)
            {
                return Float2.Cross(this.pointArr[index] - this.pointArr[index - 1], this.pointArr[0] - this.pointArr[index]);
            }
            else if (index == 0)
            {
                return Float2.Cross(this.pointArr[index] - this.pointArr[this.pointArr.Length - 1], this.pointArr[0] - this.pointArr[index]);
            }
            return 0;
        }
        /// <summary>
        /// 获取线段与多边形的所有交点，并按从近到远排序。，float3 z记录与多边形相交的边。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="paths"></param>
        public void GetAllIntersectPoint(LineSegment2D line,ref List<Float3> paths)
        {
            List<Float3> listpath = new List<Float3>();
            Float2 point = Float2.zero;
            for (int i = 0; i < this.pointArr.Length; i++)
            {
                if (i == this.pointArr.Length - 1)
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[0]), ref point) == true)
                    {
                        paths.Add(new Float3(point.x, point.y, i));
                    }
                }
                else
                {
                    if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[i + 1]), ref point) == true)
                    {
                        paths.Add(new Float3(point.x, point.y, i));
                    }
                }
            }
            // 从近到远排好队。
            if (listpath.Count > 1)
            {
                listpath.Sort((x, y) => Float2.Distance(new Float2(x.x, x.y), line.startPoint).CompareTo(Float2.Distance(new Float2(y.x, y.y), line.startPoint)));
            }
            
            paths = listpath;
        }
        /// <summary>
        /// 获取边
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public LineSegment2D GetEdge(int edgeIndex)
        {
            if (edgeIndex < 0 || edgeIndex > this.pointArr.Length - 1)
            {
                return new LineSegment2D(Float2.zero, Float2.zero);
            }
            else if (edgeIndex == this.pointArr.Length - 1)
            {
                return new LineSegment2D(this.pointArr[edgeIndex], this.pointArr[0]);
            }
            else 
            {
                return new LineSegment2D(this.pointArr[edgeIndex], this.pointArr[edgeIndex +　1]);
            }
        }
        /// <summary>
        /// 获取法线
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public Float2 GetNormal(int edgeIndex)
        {
            if (edgeIndex < 0 || edgeIndex > this.normalAttr.Length - 1)
            {
                return Float2.zero;
            }
            return this.normalAttr[edgeIndex];
        }

    }
}