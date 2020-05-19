using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{
    /// <summary>
    /// 矩形区域
    /// </summary>
    [System.Serializable]
    public class Rect2D : AABB2D
    {
        public Rect2D(Double2 lb, Double2 ru) 
        {
            this.SetAABB(lb, ru);
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
            for (int i = 0; i < GetEdgeNum(); i++)
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
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="rbi">包围盒信息</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public override bool RayboundingNearestPath(LineSegment2D line, double offset, ref RayboundingInfo rbi)
        {
            if (rbi == null)
            {
                rbi = new RayboundingInfo();
            }
            int index = 0;
            Double3[] lineArray = new Double3[2];
            Double2 intersectionPoint = Double2.zero;
            Double2 pos1 = Double2.zero;
            if (line.GetIntersectPoint(GetEdge(0), ref intersectionPoint, ref pos1) == true)
            {
                lineArray[index] = new Double3(intersectionPoint.x, intersectionPoint.y, 1);
                index++;
            }
            if (line.GetIntersectPoint(GetEdge(2), ref intersectionPoint, ref pos1) == true)
            {
                lineArray[index] = new Double3(intersectionPoint.x, intersectionPoint.y, 3);
                index++;
            }
            if (index < 2)
            {
                if (line.GetIntersectPoint(GetEdge(3), ref intersectionPoint, ref pos1) == true)
                {
                    lineArray[index] = new Double3(intersectionPoint.x, intersectionPoint.y, 4);
                    index++;
                }
            }
            if (index < 2)
            {
                if (line.GetIntersectPoint(GetEdge(1), ref intersectionPoint, ref pos1) == true)
                {
                    lineArray[index] = new Double3(intersectionPoint.x, intersectionPoint.y, 2);
                    index++;
                }
            }

            if (index == 2)
            {
                double v1 = (new Double2(lineArray[0].x, lineArray[0].y) - line.startPoint).sqrMagnitude;
                double v2 = (new Double2(lineArray[1].x, lineArray[1].y) - line.startPoint).sqrMagnitude;
                Double2 s ;
                Double2 e ;
                if (v1 < v2)
                {
                    s = new Double2(lineArray[0].x, lineArray[0].y);
                    e = new Double2(lineArray[1].x, lineArray[1].y);
                    RayboundingNearestPath(new Double3(s.x, s.y, lineArray[0].z), new Double3(e.x, e.y, lineArray[1].z), offset, ref rbi.listpath);
                }
                else
                {
                    e = new Double2(lineArray[0].x, lineArray[0].y);
                    s = new Double2(lineArray[1].x, lineArray[1].y);
                    RayboundingNearestPath(new Double3(s.x, s.y, lineArray[1].z), new Double3(e.x, e.y, lineArray[0].z), offset, ref rbi.listpath);
                }
                if (rbi.listpath != null && rbi.listpath.Count > 0)
                {
                    rbi.CalcHelpData(line, offset, s, e);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="sl"></param>
        private void RayboundingNearestPath(Double3 p1, Double3 p2, double offset, ref List<Double2> paths)
        {
            List<Double2> listpath = new List<Double2>();
            // 对边行为
            if ((p1.z == 1 && p2.z == 3) || (p1.z == 3 && p2.z == 1))
            {
                double v1 = p1.x - this.leftBottom.x + p2.x - this.leftBottom.x;
                double v2 = -p1.x + this.RightBottom.x - p2.x + this.RightBottom.x;
                if (v1 <= v2)
                {
                    listpath.Add(new Double2(this.leftBottom.x - offset, p1.y));
                    listpath.Add(new Double2(this.leftBottom.x - offset, p2.y));
                }
                else
                {
                    listpath.Add(new Double2(this.RightBottom.x + offset, p1.y));
                    listpath.Add(new Double2(this.RightBottom.x + offset, p2.y));
                }
            }
            else if ((p1.z == 2 && p2.z == 4) || (p1.z == 4 && p2.z == 2))
            {
                double v1 = p1.y - this.leftBottom.y + p2.y - this.leftBottom.y;
                double v2 = -p1.y + this.LeftUp.y - p2.y + this.LeftUp.y;
                if (v1 <= v2)
                {
                    listpath.Add(new Double2(p1.x, this.leftBottom.y - offset));
                    listpath.Add(new Double2(p2.x, this.leftBottom.y - offset));
                }
                else
                {
                    listpath.Add(new Double2(p1.x, this.LeftUp.y + offset));
                    listpath.Add(new Double2(p2.x, this.LeftUp.y + offset));
                }
            }
            else if ((p1.z == 1 && p2.z == 2) || (p1.z == 2 && p2.z == 1))
            {
                listpath.Add(this.RightBottom + new Double2(offset, -offset));
            }
            else if ((p1.z == 2 && p2.z == 3) || (p1.z == 3 && p2.z == 2))
            {
                listpath.Add(this.rightUp + new Double2(offset, offset));
            }
            else if ((p1.z == 3 && p2.z == 4) || (p1.z == 4 && p2.z == 3))
            {
                listpath.Add(this.LeftUp + new Double2(-offset, offset));
            }
            else if ((p1.z == 4 && p2.z == 1) || (p1.z == 1 && p2.z == 4))
            {
                listpath.Add(this.leftBottom + new Double2(-offset, -offset));
            }
            paths = listpath;
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
            for (int i = 0; i < GetEdgeNum(); i++)
            {
                if (line.GetIntersectPoint(GetEdge(i), ref pos, ref pos1) == true)
                {
                    return LineRelation.Intersect;
                }
            }
            return LineRelation.Detach;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectStart"></param>
        /// <param name="intersectEnd"></param>
        /// <returns></returns>
        public override bool GetIntersectPoint(LineSegment2D line, ref Double2 intersectStart, ref Double2 intersectEnd)
        {
            List<Double2> listPt = new List<Double2>();
            Double2 pos = Double2.zero;
            Double2 pos1 = Double2.zero;

            for (int i = 0; i < GetEdgeNum(); i++)
            {
                if (GetEdge(i).GetIntersectPoint(line, ref pos, ref pos1) == true)
                {
                    if (pos == pos1)
                    {
                        if (listPt.Contains(pos) == false)
                        {
                            listPt.Add(pos);
                        }
                    }
                    else 
                    {
                        if (listPt.Contains(pos) == false)
                        {
                            listPt.Add(pos);
                        }
                        if (listPt.Contains(pos1) == false)
                        {
                            listPt.Add(pos1);
                        }
                    }
                }
            }
            // 排序从近到远
            listPt.Sort((a, b) =>
            {
                double adis = (a - line.startPoint).sqrMagnitude;
                double bdis = (b - line.startPoint).sqrMagnitude;
                return adis.CompareTo(bdis);
            });
            //
            if (listPt.Count > 0)
            {
                intersectStart = listPt[0];
                intersectEnd = listPt[listPt.Count - 1];
                return true;
            }
            return false;
        }
        /// <summary>
        /// 与矩形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public override bool CheckIntersect(Triangle2D ab)
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
        /// 与矩形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public override bool CheckIntersect(Rect2D ab)
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
        /// 与圆的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public override bool CheckIntersect(Circle2D ab)
        {
            if (ab == null)
                return false;
            else return ab.CheckIntersect(this);
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
    }
}
