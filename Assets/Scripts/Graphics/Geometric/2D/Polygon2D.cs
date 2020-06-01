using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{

    public class Polygon2D : AABB2D
    {
        /// <summary>
        /// 顶点列表
        /// </summary>
        private Double2[] pointArr = null;
        /// <summary>
        /// 法线列表。
        /// </summary>
        private Double2[] normalAttr = null;
        /// <summary>
        /// 顶点可同行数组
        /// </summary>
        private HashSet<int> listunCrossPoints = null;
        /// <summary>
        /// 逆时针
        /// </summary>
        private double[] distancArr = null;
        private double totalDistance = 0;

        public Polygon2D(Double2Bool[] points)
        {
            if (points == null || points.Length < 3)
                return;
            List<Double2> lpoints = new List<Double2>();
            List<int> unCrossPoints = new List<int>();
            for (int i = 0; i < points.Length; i++)
            {
                lpoints.Add(new Double2(points[i].x, points[i].y));
                if (points[i].isCross == false)
                {
                    unCrossPoints.Add(i);
                }
            }
            Init(lpoints.ToArray());
            if (unCrossPoints != null && unCrossPoints.Count > 0)
            {
                listunCrossPoints = new HashSet<int>();
                foreach (int index in unCrossPoints)
                {
                    listunCrossPoints.Add(index);
                }
            }
        }
        public Polygon2D(Double2[] points, List<int> unCrossPoints)
        {
            Init(points);
            if (unCrossPoints != null && unCrossPoints.Count > 0)
            {
                listunCrossPoints = new HashSet<int>();
                foreach (int index in unCrossPoints)
                {
                    listunCrossPoints.Add(index);
                }
            }
        }
        public Polygon2D(Double2[] points) 
        {
            Init(points);
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="points"></param>
        private void Init(Double2[] points)
        {
            if (points == null || points.Length < 3)
                return;
            this.pointArr = new Double2[points.Length];
            this.normalAttr = new Double2[points.Length];
            this.distancArr = new double[points.Length + 1];
            Double2 min = points[0];
            Double2 max = points[0];
            int i;
            for (i = 0; i < points.Length; i++)
            {
                this.pointArr[i] = points[i];
                if (i > 0)
                {
                    min = Double2.Min(min, points[i]);
                    max = Double2.Max(max, points[i]);
                    totalDistance += MathUtil.GetNearDistance(points[i - 1], points[i]);
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
                Double2 dir;
                if (i < points.Length - 1)
                {
                    dir = (points[i + 1] - points[i]).normalized;
                }
                else
                {
                    dir = (points[0] - points[i]).normalized;
                }
                this.normalAttr[i] = Double2.Perpendicular(dir);
            }
            totalDistance += MathUtil.GetNearDistance(points[points.Length - 1], points[0]);
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
        public override RBIResultType RayboundingNearestPath(LineSegment2D line, double offset, ref RayboundingInfo rbi)
        {
            if (rbi == null)
            {
                rbi = new RayboundingInfo();
            }
            List<Double3> lineArray = new List<Double3>();
            Double2 intersectionPoint = Double2.zero;
            Double2 pos1 = Double2.zero;
            for (int i = 0; i < this.pointArr.Length ; i++)
            {
                if (line.GetIntersectPoint(GetEdge(i), ref intersectionPoint, ref pos1) == true)
                {
                    lineArray.Add(new Double3(intersectionPoint.x, intersectionPoint.y, i));
                }
            }
            int count = lineArray.Count;

            if (count == 0)
            {
                return RBIResultType.Fail;
            }
            else if (count % 2 == 1)
            {
                lineArray.Sort((x, y) => MathUtil.GetCompareDis(new Double2(x.x, x.y), line.startPoint).CompareTo(MathUtil.GetCompareDis(new Double2(y.x, y.y), line.startPoint)));
                if (CheckIn(line.startPoint) == true)
                {
                    rbi.SetNear(line, offset, new Double2(lineArray[0].x, lineArray[0].y));
                }
                else 
                {
                    rbi.SetNear(line, offset, new Double2(lineArray[count -1].x, lineArray[count - 1].y));
                }
                return RBIResultType.UnCross;
            }
            else
            {
                // 先按距离进行排序。
                lineArray.Sort((x, y) => MathUtil.GetCompareDis(new Double2(x.x, x.y), line.startPoint).CompareTo(MathUtil.GetCompareDis(new Double2(y.x, y.y), line.startPoint)));
                //
                bool isCross = true;
                bool isPathDir = CheckPathDir(lineArray[0], lineArray[lineArray.Count - 1], ref isCross);
                if (isCross == false)
                {
                    rbi.SetNear(line, offset, new Double2(lineArray[0].x, lineArray[0].y));
                    return RBIResultType.UnCross;
                }
                List<Double2> temppaths = new List<Double2>();
                RayboundingNearestPath(lineArray[0], lineArray[lineArray.Count - 1], offset, isPathDir, ref temppaths);
                if (rbi.listpath == null)
                {
                    rbi.listpath = temppaths;
                }
                else
                {
                    rbi.listpath.AddRange(temppaths);
                }
                // 排斥需要扣除的点。
                for (int i = 1; i < lineArray.Count - 1; i += 2)
                {
                    if (CheckisSubChild((int)lineArray[0].z, (int)lineArray[lineArray.Count - 1].z, isPathDir, (int)lineArray[i].z, (int)lineArray[i + 1].z) == false)
                        continue;
                    List<Double2> listTemp = new List<Double2>();
                    RayboundingNearestPath(lineArray[i], lineArray[i + 1], offset, isPathDir, ref listTemp);
                    if (listTemp.Count > 0)
                    {
                        foreach (Double2 pos in listTemp)
                        {
                            rbi.listpath.Remove(pos);
                        }
                    }
                }
                //
                if (rbi.listpath != null && rbi.listpath.Count > 0)
                {
                    rbi.CalcHelpData(line, offset, new Double2(lineArray[0].x, lineArray[0].y), new Double2(lineArray[lineArray.Count - 1].x, lineArray[lineArray.Count - 1].y));
                    return RBIResultType.Succ;
                }
                return RBIResultType.Fail;
            }
        }
        /// <summary>
        /// 确定是否为其子集, 先简单实现。
        /// </summary>
        /// <returns></returns>
        private bool CheckisSubChild(int start , int end, bool isPathDir, int substart,int subend)
        {
           List<int> listall = GetPathPoint(start, end, isPathDir);
           List<int> listsub = GetPathPoint(substart, subend, isPathDir);

            foreach (int index in listsub)
            {
                listall.Remove(index);
            }
            if (listall.Count == 0)
                return false;
            else return true;
        }
        /// <summary>
        /// 中间经过的顶点，不包含2端的点。
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="offset"></param>
        /// <param name="isPathDir"></param>
        /// <param name="paths"></param>
        private void RayboundingNearestPath(Double3 p1, Double3 p2, double offset, bool isPathDir, ref List<Double2> paths)
        {
            List<Double2> listpath = new List<Double2>();
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
        /// 中间经过的顶点，不包含2端的点。
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="offset"></param>
        /// <param name="isPathDir"></param>
        /// <param name="paths"></param>
        private List<int> GetPathPoint(int startIndex, int endIndex, bool isPathDir)
        {
            List<int> listpath = new List<int>();
            // 先计算逆时针距离。
            if (startIndex < endIndex)
            {
                if (isPathDir == true)
                {
                    for (int i = startIndex + 1; i <= endIndex; i++)
                    {
                        listpath.Add(i);
                    }
                }
                else
                {
                    for (int i = startIndex; i >= 0; i--)
                    {
                        listpath.Add(i);
                    }
                    //
                    for (int i = this.pointArr.Length - 1; i > endIndex; i--)
                    {
                        listpath.Add(i);
                    }
                }
            }
            else if (startIndex > endIndex)
            {
                if (isPathDir == false)
                {
                    for (int i = startIndex; i >= endIndex + 1; i--)
                    {
                        listpath.Add(i);
                    }
                }
                else
                {
                    for (int i = startIndex + 1; i < this.pointArr.Length; i++)
                    {
                        listpath.Add(i);
                    }
                    for (int i = 0; i <= endIndex; i++)
                    {
                        listpath.Add(i);
                    }
                }
            }
            return listpath;
        }
        /// <summary>
        /// 获取指定顶点的外部偏移点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Double2 GetOutPoint(int index, double offset)
        {
            if (index < 0 || index >= this.pointArr.Length)
                return Double2.zero;
            Double2 diff;
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
        private bool CheckPathDir(Double3 p1, Double3 p2, ref bool isCross)
        {
            isCross = true;
            int crossValue = GetCrossValue((int)p1.z, (int)p2.z);
            if (crossValue == 3)
            {
                isCross = false;
                return false;
            }
            // 先计算逆时针距离。
            double dis;
            // 对边行为
            if (p1.z < p2.z)
            {
                if (crossValue == 1)
                {
                    return false;
                }
                else if (crossValue == 2)
                {
                    return true;
                }
                int max = (int)p2.z + 1;
                max = max > this.pointArr.Length - 1 ? 0 : max;
                dis = distancArr[(int)p2.z + 1] - distancArr[(int)p1.z]
                    - MathUtil.GetNearDistance(new Double2(p1.x, p1.y), this.pointArr[(int)p1.z])
                    - MathUtil.GetNearDistance(new Double2(p2.x, p2.y), this.pointArr[max]);
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
                if (crossValue == 1)
                {
                    return true;
                }
                else if (crossValue == 2)
                {
                    return false;
                }
                int max = (int)p1.z + 1;
                max = max > this.pointArr.Length - 1 ? 0 : max;
                dis = distancArr[(int)p1.z + 1] - distancArr[(int)p2.z]
                - MathUtil.GetNearDistance(new Double2(p2.x, p2.y), this.pointArr[(int)p2.z])
                - MathUtil.GetNearDistance(new Double2(p1.x, p1.y), this.pointArr[max]);
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
        /// 
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns>0, 可以通。1. 升序不可通， 2 降序不可通, 3 都不可通</returns>
        private int GetCrossValue(int index1, int index2)
        {
            int value = 0;
            if (index1 > index2)
            {
                int temp = index2;
                index2 = index1;
                index1 = temp;
            }
            //  升序
            for (int i = index1 + 1; i <= index2; i++)
            {
                if (CheckCrossPoint(i) == false)
                {
                    value = 1;
                    break;
                }
            }
            //  降序
            for (int i = index2 + 1; i < this.pointArr.Length; i++)
            {
                if (CheckCrossPoint(i) == false)
                {
                    value += 2;
                    return value;
                }
            }
            for (int i = 0; i <= index1; i++)
            {
                if (CheckCrossPoint(i) == false)
                {
                    value += 2;
                    return value;
                }
            }
            return value;
        }
        /// <summary>
        /// 获取挡格附近出生点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <param name="bornPoint"></param>
        /// <returns></returns>
        public override bool GetBornPoint(LineSegment2D line, double offset, ref Double2 bornPoint)
        {
            Double2 pos1 = Double2.zero;
            for (int i = 0; i < this.pointArr.Length; i++)
            {
                if (line.GetIntersectPoint(GetEdge(i), ref bornPoint, ref pos1) == true)
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
            Double2 pos = Double2.zero;
            Double2 pos1 = Double2.zero;
            for (int i = 0; i < this.pointArr.Length; i++)
            {
                if (line.GetIntersectPoint(GetEdge(i), ref pos, ref pos1) == true)
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
            double prev = CalcRotateValue(this.pointArr.Length -1);
            double cur ;
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
        private double CalcRotateValue(int index)
        {
            if (index < 0 || index >= this.pointArr.Length)
                return 0;
            if (index > 0)
            {
                return Double2.Cross(GetEdge(index - 1).normalizedDir, GetEdge(index).normalizedDir);
            }
            else if (index == 0)
            {
                return Double2.Cross(GetEdge(this.pointArr.Length - 1).normalizedDir, GetEdge(0).normalizedDir);
            }
            return 0;
        }
        /// <summary>
        /// 获取线段与多边形的所有交点，并按从近到远排序。，float3 z记录与多边形相交的边。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="paths"></param>
        public void GetAllIntersectPoint(LineSegment2D line,ref List<Double3> paths)
        {
            List<Double3> listpath = new List<Double3>();
            Double2 point = Double2.zero;
            Double2 point1 = Double2.zero;
            for (int i = 0; i < this.pointArr.Length; i++)
            {
                if (line.GetIntersectPoint(GetEdge(i), ref point, ref point1) == true)
                {
                    if (point1 == point)
                    {
                        listpath.Add(new Double3(point.x, point.y, i));
                    }
                    else 
                    {
                        listpath.Add(new Double3(point.x, point.y, i));
                        listpath.Add(new Double3(point1.x, point1.y, i));
                    }
                }
            }
            // 从近到远排好队。
            if (listpath.Count > 1)
            {
                listpath.Sort((x, y) => MathUtil.GetCompareDis(new Double2(x.x, x.y), line.startPoint).CompareTo(MathUtil.GetCompareDis(new Double2(y.x, y.y), line.startPoint)));
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
                return new LineSegment2D(Double2.zero, Double2.zero);
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
        /// 获取边
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public override Point2D GetSimpleEdge(int edgeIndex)
        {
            if (edgeIndex < 0 || edgeIndex > this.pointArr.Length - 1)
            {
                return new Point2D(Double2.zero, Double2.zero);
            }
            else if (edgeIndex == this.pointArr.Length - 1)
            {
                return new Point2D(this.pointArr[edgeIndex], this.pointArr[0]);
            }
            else
            {
                return new Point2D(this.pointArr[edgeIndex], this.pointArr[edgeIndex + 1]);
            }
        }
        /// <summary>
        /// 获取法线
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public override Double2 GetNormal(int edgeIndex)
        {
            if (edgeIndex < 0 || edgeIndex > this.normalAttr.Length - 1)
            {
                return Double2.zero;
            }
            return this.normalAttr[edgeIndex];
        }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override Double2 GetPoint(int index)
        {
            if (index < 0 || index > this.pointArr.Length - 1)
            {
                return Double2.zero;
            }
            return this.pointArr[index];
        }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override Double2Bool GetPointPlus(int index)
        {
            if (index < 0 || index > this.pointArr.Length - 1)
            {
                new Double2Bool(Double2.zero, true);
            }
            return new Double2Bool(this.pointArr[index], CheckCrossPoint(index));
        }
        /// <summary>
        /// 获取顶点数组
        /// </summary>
        /// <returns></returns>
        public override Double2Bool[] GetPointsPlus()
        {
            Double2Bool[] points = new Double2Bool[this.pointArr.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = GetPointPlus(i);
            }
            return points;
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
        public override Double2[] GetPoints()
        {
            return this.pointArr;
        }
        /// <summary>
        /// 判断点多边形内。
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Double2 pt)
        {
            if (base.CheckIn(pt) == false)
                return false;
            // 做射线， y = pt.y
            // 经过顶点的。
            int CrossPointCount = 0;
            bool flag = false;
            int edgeNum = GetEdgeNum();
            for (int i = 0; i < edgeNum; i++)
            {
                Point2D line = GetSimpleEdge(i);
                // 先判断点是否在边上。
                if (line.CheckIn(pt) == true)
                    return true;
                Double2 diff = line.endPoint - line.startPoint;

                if ((line.startPoint.y <= pt.y && line.endPoint.y >= pt.y) || (line.startPoint.y >= pt.y && line.endPoint.y <= pt.y))
                {
                    if (diff.y != 0)
                    {
                        double x = line.startPoint.x + (pt.y - line.startPoint.y) * diff.x / diff.y;
                        // 射线穿过多边形的边界
                        if (x > pt.x)
                        {
                            // 统计经过顶点的次数
                            if (line.startPoint.y == pt.y || line.endPoint.y == pt.y)
                            {
                                CrossPointCount++;
                            }
                            flag = !flag;
                        }
                    }
                    // 共线情况，肯定是点在线段2端。
                }
            }
            CrossPointCount /=  2;
            CrossPointCount %=  2;
            if (CrossPointCount == 1)
            {
                flag = !flag;
            }
            return flag;
        }
        /// <summary>
        /// 判断某个顶点是否可通行
        /// </summary>
        /// <param name="indexPoint"></param>
        /// <returns></returns>
        public override bool CheckCrossPoint(int indexPoint)
        {
            if (listunCrossPoints == null || listunCrossPoints.Count == 0)
                return true;
            if (listunCrossPoints.Contains(indexPoint) == true)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取顶点可通过数据
        /// </summary>
        /// <returns></returns>
        public override HashSet<int> GetCrossPointData()
        {
            return listunCrossPoints;
        }

        /// <summary>
        /// 获取近的相交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectStart"></param>
        /// <returns></returns>
        public override bool GetNearIntersectPoint(LineSegment2D line, ref Double2 intersectStart)
        {
            bool ret = false;
            intersectStart = Double2.positiveInfinity;
            double dis = float.MaxValue;
            Double2 pos = Double2.zero;
            Double2 pos1 = Double2.zero;

            for (int i = 0; i < GetEdgeNum(); i++)
            {
                if (GetEdge(i).GetIntersectPoint(line, ref pos, ref pos1) == true)
                {
                    ret = true;
                    double dis1 = MathUtil.GetCompareDis(line.startPoint, pos);
                    double dis2 = MathUtil.GetCompareDis(line.startPoint, pos1);
                    if (dis1 < dis)
                    {
                        intersectStart = pos;
                        dis = dis1;
                    }

                    if (dis2 < dis)
                    {
                        intersectStart = pos1;
                        dis = dis2;
                    }
                }
            }
            return ret;
        }
    }
}