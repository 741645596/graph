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
        public Rect2D(Float2 lb, Float2 ru) 
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
        public override bool GetBornPoint(LineSegment2D line, float offset, ref Float2 bornPoint)
        {
            if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.RightBottom), ref bornPoint) == true)
            {
                bornPoint = line.normalizedDir * ((bornPoint - line.startPoint).magnitude + offset) + line.startPoint;
                return true;
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.LeftUp, this.rightUp), ref bornPoint) == true)
            {
                bornPoint = line.normalizedDir * ((bornPoint - line.startPoint).magnitude + offset) + line.startPoint;
                return true;
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.LeftUp), ref bornPoint) == true)
            {
                bornPoint = line.normalizedDir * ((bornPoint - line.startPoint).magnitude + offset) + line.startPoint;
                return true;
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.RightBottom, this.rightUp), ref bornPoint) == true)
            {
                bornPoint = line.normalizedDir * ((bornPoint - line.startPoint).magnitude + offset) + line.startPoint;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="paths">返回路径</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public override bool RayboundingNearestPath(LineSegment2D line, float offset, ref Float2 nearPoint, ref Float2 farPoint, ref List<Float2> paths)
        {
            int index = 0;
            Float3[] lineArray = new Float3[2];
            Float2 intersectionPoint = Float2.zero;
            if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.RightBottom), ref intersectionPoint) == true)
            {
                lineArray[index] = new Float3(intersectionPoint.x, intersectionPoint.y, 1);
                index++;
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.LeftUp, this.rightUp), ref intersectionPoint) == true)
            {
                lineArray[index] = new Float3(intersectionPoint.x, intersectionPoint.y, 3);
                index++;
            }
            if (index < 2)
            {
                if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.LeftUp), ref intersectionPoint) == true)
                {
                    lineArray[index] = new Float3(intersectionPoint.x, intersectionPoint.y, 4);
                    index++;
                }
            }
            if (index < 2)
            {
                if (line.GetIntersectPoint(new LineSegment2D(this.RightBottom, this.rightUp), ref intersectionPoint) == true)
                {
                    lineArray[index] = new Float3(intersectionPoint.x, intersectionPoint.y, 2);
                    index++;
                }
            }

            if (index == 2)
            {
                float v1 = (new Float2(lineArray[0].x, lineArray[0].y) - line.startPoint).sqrMagnitude;
                float v2 = (new Float2(lineArray[1].x, lineArray[1].y) - line.startPoint).sqrMagnitude;
                float v = (new Float2((lineArray[0].x + lineArray[1].x) / 2, (lineArray[0].y + lineArray[1].y) / 2) - line.startPoint).sqrMagnitude;
                if (v1 < v2)
                {
                    Float2 s = new Float2(lineArray[0].x, lineArray[0].y);
                    Float2 e = new Float2(lineArray[1].x, lineArray[1].y);
                    Float2 fnormalized = (e - s).normalized * + offset;
                    s -= fnormalized;
                    e += fnormalized;
                    RayboundingNearestPath(new Float3(s.x, s.y, lineArray[0].z), new Float3(e.x, e.y, lineArray[1].z), offset, ref paths);
                    nearPoint = s;
                    farPoint = e;
                }
                else
                {
                    Float2 e = new Float2(lineArray[0].x, lineArray[0].y);
                    Float2 s = new Float2(lineArray[1].x, lineArray[1].y);
                    Float2 fnormalized = (e - s).normalized * offset;
                    s -= fnormalized;
                    e += fnormalized;
                    RayboundingNearestPath(new Float3(s.x, s.y, lineArray[1].z), new Float3(e.x, e.y, lineArray[0].z), offset, ref paths);
                    nearPoint = e;
                    farPoint = s;
                }
                return true;
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
        private void RayboundingNearestPath(Float3 p1, Float3 p2, float offset, ref List<Float2> paths)
        {
            List<Float2> listpath = new List<Float2>();
            // 对边行为
            if ((p1.z == 1 && p2.z == 3) || (p1.z == 3 && p2.z == 1))
            {
                float v1 = p1.x - this.leftBottom.x + p2.x - this.leftBottom.x;
                float v2 = -p1.x + this.RightBottom.x - p2.x + this.RightBottom.x;
                if (v1 <= v2)
                {
                    //listpath.Add(new Float2(p1.x, p1.y));
                    listpath.Add(new Float2(this.leftBottom.x - offset, p1.y));
                    listpath.Add(new Float2(this.leftBottom.x - offset, p2.y));
                    //listpath.Add(new Float2(p2.x, p2.y));
                }
                else
                {
                    //listpath.Add(new Float2(p1.x, p1.y));
                    listpath.Add(new Float2(this.RightBottom.x + offset, p1.y));
                    listpath.Add(new Float2(this.RightBottom.x + offset, p2.y));
                    //listpath.Add(new Float2(p2.x, p2.y));
                }
            }
            else if ((p1.z == 2 && p2.z == 4) || (p1.z == 4 && p2.z == 2))
            {
                float v1 = p1.y - this.leftBottom.y + p2.y - this.leftBottom.y;
                float v2 = -p1.y + this.LeftUp.y - p2.y + this.LeftUp.y;
                if (v1 <= v2)
                {
                    //listpath.Add(new Float2(p1.x, p1.y));
                    listpath.Add(new Float2(p1.x, this.leftBottom.y - offset));
                    listpath.Add(new Float2(p2.x, this.leftBottom.y - offset));
                    //listpath.Add(new Float2(p2.x, p2.y));
                }
                else
                {
                    //listpath.Add(new Float2(p1.x, p1.y));
                    listpath.Add(new Float2(p1.x, this.LeftUp.y + offset));
                    listpath.Add(new Float2(p2.x, this.LeftUp.y + offset));
                    //listpath.Add(new Float2(p2.x, p2.y));
                }
            }
            else if ((p1.z == 1 && p2.z == 2) || (p1.z == 2 && p2.z == 1))
            {
                //listpath.Add(new Float2(p1.x, p1.y));
                listpath.Add(this.RightBottom + new Float2(offset, -offset));
                //listpath.Add(new Float2(p2.x, p2.y));
            }
            else if ((p1.z == 2 && p2.z == 3) || (p1.z == 3 && p2.z == 2))
            {
               // listpath.Add(new Float2(p1.x, p1.y));
                listpath.Add(this.rightUp + new Float2(offset, offset));
               // listpath.Add(new Float2(p2.x, p2.y));
            }
            else if ((p1.z == 3 && p2.z == 4) || (p1.z == 4 && p2.z == 3))
            {
               // listpath.Add(new Float2(p1.x, p1.y));
                listpath.Add(this.LeftUp + new Float2(-offset, offset));
               // listpath.Add(new Float2(p2.x, p2.y));
            }
            else if ((p1.z == 4 && p2.z == 1) || (p1.z == 1 && p2.z == 4))
            {
               // listpath.Add(new Float2(p1.x, p1.y));
                listpath.Add(this.leftBottom + new Float2(-offset, -offset));
               // listpath.Add(new Float2(p2.x, p2.y));
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
            Float2 pos = Float2.zero;
            if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.RightBottom), ref pos) == true)
            {
                return LineRelation.Intersect;
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.LeftUp, this.rightUp), ref pos) == true)
            {
                return LineRelation.Intersect;
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.LeftUp), ref pos) == true)
            {
                return LineRelation.Intersect;
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.RightBottom, this.rightUp), ref pos) == true)
            {
                return LineRelation.Intersect;
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
        public override bool GetIntersectPoint(LineSegment2D line, ref Float2 intersectStart, ref Float2 intersectEnd)
        {
            List<Float2> listPt = new List<Float2>();
            Float2 pos = Float2.zero;
            if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.RightBottom), ref pos) == true)
            {
                if (listPt.Contains(pos) == false)
                {
                    listPt.Add(pos);
                }
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.LeftUp, this.rightUp), ref pos) == true)
            {
                if (listPt.Contains(pos) == false)
                {
                    listPt.Add(pos);
                }
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.leftBottom, this.LeftUp), ref pos) == true)
            {
                if (listPt.Contains(pos) == false)
                {
                    listPt.Add(pos);
                }
            }
            if (line.GetIntersectPoint(new LineSegment2D(this.RightBottom, this.rightUp), ref pos) == true)
            {
                if (listPt.Contains(pos) == false)
                {
                    listPt.Add(pos);
                }
            }
            // 排序从近到远
            listPt.Sort((a, b) =>
            {
                float adis = (a - line.startPoint).sqrMagnitude;
                float bdis = (b - line.startPoint).sqrMagnitude;
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
    }
}
