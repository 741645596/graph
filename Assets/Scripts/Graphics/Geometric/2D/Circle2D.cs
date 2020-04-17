using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{
    [System.Serializable]
    public class Circle2D : AABB2D
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Float2 circleCenter;
        /// <summary>
        /// 半径
        /// </summary>
        public float radius;
        public Circle2D(Float2 center, float radius)
        {
            this.circleCenter = center;
            this.radius = radius;
            this.SetAABB(center - Float2.one * radius, center + Float2.one * radius);
        }
        /// <summary>
        /// 创建外接圆
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static Circle2D CreateCircumCircle(Float2 p1, Float2 p2, Float2 p3)
        {
            // 排除三点共线
            Float2 diff1 = p2 - p1;
            Float2 diff2 = p3 - p1;
            if (diff1 == Float2.zero || diff2 == Float2.zero)
                return null;

            float value = Float2.Cross(diff1, diff2);
            if (System.Math.Abs(value) < MathUtil.kEpsilon)
            {
                return null;
            }
            // 2垂直平分线就是圆心。
            Float2 mid1 = p1 + diff1 * 0.5f;
            Float2 mid2 = p1 + diff2 * 0.5f;
            Line2D line1 = new Line2D(mid1, Float2.Perpendicular(diff1));
            Line2D line2 = new Line2D(mid2, Float2.Perpendicular(diff2));

            Float2 center = Float2.zero;
            if (line1.GetIntersectPoint(line2, ref center) == true)
            {
                float radius = (center - p1).magnitude;
                return new Circle2D(center, radius);
            }
            else return null;
        }


        /// <summary>
        /// 创建内接圆
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static Circle2D CreateInscribedCircle(Float2 p1, Float2 p2, Float2 p3)
        {
            // 排除三点共线
            Float2 diff1 = p2 - p1;
            Float2 diff2 = p3 - p1;
            if (diff1 == Float2.zero || diff2 == Float2.zero)
                return null;

            float value = Float2.Cross(diff1, diff2);
            if (System.Math.Abs(value) < MathUtil.kEpsilon)
            {
                return null;
            }
            // 2垂直平分线就是圆心。
            Float2 mid1dir = diff1.normalized + diff2.normalized;
            Float2 mid2dir = (p3 - p2).normalized - diff1.normalized;
            Line2D line1 = new Line2D(p1, mid1dir);
            Line2D line2 = new Line2D(p2, mid2dir);

            Float2 center = Float2.zero;
            if (line1.GetIntersectPoint(line2, ref center) == true)
            {
                float radius = line1.CalcDistance(center);
                return new Circle2D(center, radius);
            }
            else return null;
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
            Float2 diff = this.circleCenter - line.startPoint;
            if (diff == Float2.zero)
                return false;
            //
            Float2 projectoint = line.ProjectPoint(this.circleCenter);
            diff = this.circleCenter - projectoint;
            // 跟直线相交奥
            float dis = diff.sqrMagnitude - this.radius * this.radius;
            if (dis >= 0)
                return false;
            dis = -dis;
            // 在同侧不行。
            Float2 diff1 = line.startPoint - projectoint;
            Float2 diff2 = line.endPoint - projectoint;
            if (Float2.Dot(diff1, diff2) >= 0)
                return false;
            //
            if (diff1.sqrMagnitude < dis || diff2.sqrMagnitude < dis)
                return false;

            dis = (float)System.Math.Sqrt(dis) + offset;

            Float2 p1 = projectoint - line.normalizedDir * dis;
            Float2 p2 = projectoint + line.normalizedDir * dis;

            float angle = Float2.SignedAngle(p1 - this.circleCenter, p2 - this.circleCenter);
            int count = (int)(System.Math.Abs(angle / 0.25f));
            float diffangle = angle / count;
            List<Float2> listpath = new List<Float2>();
            Float2 startVector = (p1 - this.circleCenter).normalized * (this.radius + offset);
            for (int i = 1; i <= count - 1; i++)
            {
                Float2 rorateVector = Float2.Rotate(startVector, -diffangle * i);
                listpath.Add(rorateVector + this.circleCenter);
            }
            rbi.listpath = listpath;
            if (rbi.listpath != null && rbi.listpath.Count > 0)
            {
                rbi.CalcHelpData(line, offset, p1, p2);
                return true;
            }

            return true;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Float2 pt)
        {
            Float2 diff = this.circleCenter - pt;
            if (diff.sqrMagnitude <= this.radius * this.radius)
                return true;
            else return false;
        }
        /// <summary>
        /// 与直线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public override LineRelation CheckLineRelation(Line2D line)
        {
            Float2 diff = this.circleCenter - line.startPoint;
            if (diff == Float2.zero)
                return LineRelation.Intersect;
            //
            Float2 projectoint = line.ProjectPoint(this.circleCenter);
            diff = this.circleCenter - projectoint;

            if (diff.sqrMagnitude <= this.radius * this.radius)
                return LineRelation.Intersect;
            else return LineRelation.Detach;
        }
        /// <summary>
        /// 直线与射线间的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public override LineRelation CheckLineRelation(Rays2D line)
        {
            if (CheckIn(line.startPoint) == true)
            {
                return LineRelation.Intersect;
            }
            float dis = line.AixsVector(this.circleCenter).sqrMagnitude - this.radius * this.radius;

            if (dis < 0)
            {
                Float2 diff = this.circleCenter - line.startPoint;
                if (Float2.Dot(diff, line.normalizedDir) > 0)
                    return LineRelation.Intersect;
                else return LineRelation.Detach;
            }
            else return LineRelation.Detach;
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public override LineRelation CheckLineRelation(LineSegment2D line)
        {
            float dis = line.CalcDistance(this.circleCenter);
            if (dis > this.radius)
                return LineRelation.Detach;
            else 
            {
                float sqrRadius = this.radius * this.radius;
                if ((this.circleCenter - line.startPoint).sqrMagnitude > sqrRadius)
                {
                    return LineRelation.Intersect;
                }
                if ((this.circleCenter - line.endPoint).sqrMagnitude > sqrRadius)
                {
                    return LineRelation.Intersect;
                }
                return LineRelation.Coincide;
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
            Float2 dirNormal = line.normalizedDir;
            Float2 diff = this.circleCenter - line.startPoint;
            if (diff == Float2.zero)
            {
                bornPoint = line.startPoint + line.normalizedDir * (this.radius + offset);
                return true;
            }
            else
            {
                Float2 projectoint = Float2.Project(diff, dirNormal) + line.startPoint;
                diff = this.circleCenter - projectoint;
                float dis = this.radius * this.radius - diff.sqrMagnitude;
                if (dis <= 0)
                {
                    return false;
                }
                else
                {
                    dis = (float)System.Math.Sqrt(dis) + offset;
                    float value = Float2.Dot(dirNormal, line.endPoint - projectoint);
                    if (value > 0)
                    {
                        bornPoint = projectoint + dirNormal * dis;
                    }
                    else
                    {
                        bornPoint = projectoint - dirNormal * dis;
                    }
                    return true;
                }
            }
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
            if (ab.CheckIn(this.circleCenter) == true)
                return true;

            for (int i = 0; i < ab.GetEdgeNum(); i++)
            {
                LineSegment2D ls = ab.GetEdge(i);
                if (this.CheckLineRelation(ls) != LineRelation.Detach)
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
            Float2 diff = this.circleCenter - ab.circleCenter;
            if (diff.magnitude > (this.radius + ab.radius))
            {
                return false;
            }
            else return true;
        }
        /// <summary>
        /// 与多边形的关系
        /// </summary>
        /// <param name="ab"></param>
        /// <returns>true 相交： false 不相交</returns>
        public override bool CheckIntersect(Polygon2D ab)
        {
            if (ab == null)
                return false;
            if (ab.CheckIn(this.circleCenter) == true)
                return true;

            for (int i = 0; i < ab.GetEdgeNum(); i++)
            {
                LineSegment2D ls = ab.GetEdge(i);
                if (this.CheckLineRelation(ls) != LineRelation.Detach)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
