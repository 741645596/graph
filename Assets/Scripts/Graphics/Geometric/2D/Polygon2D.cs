using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{

    public class Polygon2D : AABB2D
    {
        /// <summary>
        /// 顶点列表
        /// </summary>
        private Float2[] pointArr = null;
        /// <summary>
        /// 法线列表。
        /// </summary>
        private Float2[] normalAttr = null;
        /// <summary>
        /// 逆时针
        /// </summary>
        private float[] distancArr = null;
        private float totalDistance = 0;

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
        /// <param name="rbi">包围盒信息</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public override bool RayboundingNearestPath(LineSegment2D line, float offset, ref RayboundingInfo rbi)
        {
            if (rbi == null)
            {
                rbi = new RayboundingInfo();
            }
            List<Float3> lineArray = new List<Float3>();
            Float2 intersectionPoint = Float2.zero;
            for (int i = 0; i < this.pointArr.Length ; i++)
            {
                if (line.GetIntersectPoint(GetEdge(i), ref intersectionPoint) == true)
                {
                    lineArray.Add(new Float3(intersectionPoint.x, intersectionPoint.y, i));
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
                            if (rbi.listpath == null)
                            {
                                rbi.listpath = temppaths;
                            }
                            else
                            {
                                rbi.listpath.AddRange(temppaths);
                            }
                        }
                        isIn = !isIn;
                    }
                }
                else  // 与整体方向不一致。直接暴力取2头的点。
                {
                    List<Float2> temppaths = new List<Float2>();
                    RayboundingNearestPath(lineArray[0], lineArray[lineArray.Count - 1], offset, isPathDir, ref temppaths);
                    if (rbi.listpath == null)
                    {
                        rbi.listpath = temppaths;
                    }
                    else
                    {
                        rbi.listpath.AddRange(temppaths);
                    }
                }
                if (rbi.listpath != null && rbi.listpath.Count > 0)
                {
                    rbi.CalcHelpData(line, offset, new Float2(lineArray[0].x, lineArray[0].y), new Float2(lineArray[lineArray.Count - 1].x, lineArray[lineArray.Count - 1].y));
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
        /// 中间经过的顶点，不包含2端的点。
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="offset"></param>
        /// <param name="isPathDir"></param>
        /// <param name="paths"></param>
        private void RayboundingNearestPath(Float3 p1, Float3 p2, float offset, bool isPathDir, ref List<Float2> paths)
        {
            List<Float2> listpath = new List<Float2>();
            // 先计算逆时针距离。
            if (p1.z < p2.z)
            {
                if (isPathDir == true)
                {
                    for (int i = (int)p1.z + 1; i <= (int)p2.z; i++)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                }
                else 
                {
                    for (int i = (int)p1.z; i >= 0; i--)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    //
                    for (int i = this.pointArr.Length -1; i > (int)p2.z; i--)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                }
            }
            else if (p1.z > p2.z)
            {
                if (isPathDir == false)
                {
                    for (int i = (int)p1.z; i >= (int)p2.z + 1; i--)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                }
                else
                {
                    for (int i = (int)p1.z + 1; i < this.pointArr.Length; i++)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                    for (int i = 0; i <= (int)p2.z; i ++)
                    {
                        listpath.Add(GetOutPoint(i, offset));
                    }
                }
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
                diff = GetNormal(this.pointArr.Length - 1) + GetNormal(0);
            }
            else
            {
                diff = GetNormal(index - 1) + GetNormal(index);
            }
            return this.pointArr[index] + offset * diff.normalized;
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
                if (line.GetIntersectPoint(GetEdge(i), ref bornPoint) == true)
                {
                    bornPoint = line.normalizedDir * ((bornPoint - line.startPoint).magnitude + offset) + line.startPoint;
                    return true;
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
                if (line.GetIntersectPoint(GetEdge(i), ref pos) == true)
                {
                    return LineRelation.Intersect;
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
            float cur ;
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
            if (index > 0)
            {
                return Float2.Cross(GetEdge(index - 1).normalizedDir, GetEdge(index).normalizedDir);
            }
            else if (index == 0)
            {
                return Float2.Cross(GetEdge(this.pointArr.Length - 1).normalizedDir, GetEdge(0).normalizedDir);
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
                if (line.GetIntersectPoint(GetEdge(i), ref point) == true)
                {
                    listpath.Add(new Float3(point.x, point.y, i));
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
        public override LineSegment2D GetEdge(int edgeIndex)
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
        public override Float2 GetNormal(int edgeIndex)
        {
            if (edgeIndex < 0 || edgeIndex > this.normalAttr.Length - 1)
            {
                return Float2.zero;
            }
            return this.normalAttr[edgeIndex];
        }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override Float2 GetPoint(int index)
        {
            if (index < 0 || index > this.pointArr.Length - 1)
            {
                return Float2.zero;
            }
            return this.pointArr[index];
        }
        /// <summary>
        /// 与矩形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public override bool CheckIntersect(Rect2D ab)
        {
            if (ab == null)
                return false;
            return ab.CheckIntersect(this);
        }
        /// <summary>
        /// 与圆的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public override bool CheckIntersect(Circle2D ab)
        {
            if (ab == null)
                return false;
            return ab.CheckIntersect(this);
        }
        /// <summary>
        /// 与多边形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public override bool CheckIntersect(Polygon2D ab)
        {
            if (ab == null)
                return false;
            for (int i = 0; i < ab.GetEdgeNum(); i++)
            {
                if (this.CheckIn(ab.GetPoint(i)) == true)
                {
                    return true;
                }
                if (this.CheckLineRelation(ab.GetEdge(i)) == LineRelation.Intersect)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取边数
        /// </summary>
        /// <returns></returns>
        public override int GetEdgeNum()
        {
            return this.pointArr.Length; 
        }
        /// <summary>
        /// 获取顶点数组
        /// </summary>
        /// <returns></returns>
        public override Float2[] GetPoints()
        {
            return this.pointArr;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Float2 pt)
        {
            // 做射线， y = pt.y
            bool flag = false;
            int edgeNum = GetEdgeNum();
            for (int i = 0; i < edgeNum; i++)
            {
                LineSegment2D line = GetEdge(i);
                // 先判断点是否在边上。
                if (line.CheckIn(pt) == true)
                    return true;
                Float2 diff = line.endPoint - line.startPoint;

                if ((line.startPoint.y <= pt.y && line.endPoint.y >= pt.y) || (line.startPoint.y >= pt.y && line.endPoint.y <= pt.y))
                {
                    if (diff.y != 0)
                    {
                        float x = line.startPoint.x + (pt.y - line.startPoint.y) * diff.x / diff.y;
                        // 射线穿过多边形的边界
                        if (x > pt.x)
                        {
                            flag = !flag;
                        }
                    }
                    // 共线情况，肯定是点在线段2端。
                }
            }
            return flag;
        }

    }
}