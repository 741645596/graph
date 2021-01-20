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
        public Double2 circleCenter;
        /// <summary>
        /// 半径
        /// </summary>
        public double radius;
        public Circle2D(Double2 center, double radius)
        {
            this.circleCenter = center;
            this.radius = radius;
            this.SetAABB(center - Double2.one * radius, center + Double2.one * radius);
        }
        /// <summary>
        /// 创建外接圆
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static Circle2D CreateCircumCircle(Double2 p1, Double2 p2, Double2 p3)
        {
            // 排除三点共线
            Double2 diff1 = p2 - p1;
            Double2 diff2 = p3 - p1;
            if (diff1 == Double2.zero || diff2 == Double2.zero)
                return null;

            double value = Double2.Cross(diff1, diff2);
            if (System.Math.Abs(value) < MathUtil.kEpsilon)
            {
                return null;
            }
            // 2垂直平分线就是圆心。
            Double2 mid1 = p1 + diff1 * 0.5f;
            Double2 mid2 = p1 + diff2 * 0.5f;
            Line2D line1 = new Line2D(mid1, Double2.Perpendicular(diff1));
            Line2D line2 = new Line2D(mid2, Double2.Perpendicular(diff2));

            Double2 center = Double2.zero;
            if (line1.GetIntersectPoint(line2, ref center) == true)
            {
                double radius = (center - p1).magnitude;
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
        public static Circle2D CreateInscribedCircle(Double2 p1, Double2 p2, Double2 p3)
        {
            // 排除三点共线
            Double2 diff1 = p2 - p1;
            Double2 diff2 = p3 - p1;
            if (diff1 == Double2.zero || diff2 == Double2.zero)
                return null;

            double value = Double2.Cross(diff1, diff2);
            if (System.Math.Abs(value) < MathUtil.kEpsilon)
            {
                return null;
            }
            // 2垂直平分线就是圆心。
            Double2 mid1dir = diff1.normalized + diff2.normalized;
            Double2 mid2dir = (p3 - p2).normalized - diff1.normalized;
            Line2D line1 = new Line2D(p1, mid1dir);
            Line2D line2 = new Line2D(p2, mid2dir);

            Double2 center = Double2.zero;
            if (line1.GetIntersectPoint(line2, ref center) == true)
            {
                double radius = line1.CalcDistance(center);
                return new Circle2D(center, radius);
            }
            else return null;
        }

        /// <summary>
        /// 获取近的相交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectStart"></param>
        /// <returns></returns>
        public override bool GetNearIntersectPoint(LineSegment2D line, ref Double2 intersectStart)
        {
            double radis2 = this.radius * this.radius;
            Double2 projectoint = line.ProjectPoint(this.circleCenter);
            Double2 diff = this.circleCenter - projectoint;
            // 跟直线相交奥
            double dis = diff.sqrMagnitude - radis2;
            if (dis < 0)
            {
                double dis1 = (this.circleCenter - line.startPoint).sqrMagnitude;
                double dis2 = (this.circleCenter - line.endPoint).sqrMagnitude;
                if (dis1 >= radis2 && dis2 <= radis2)
                {
                    dis = System.Math.Sqrt(dis);
                    intersectStart = projectoint - line.normalizedDir * dis;
                    return true;
                }
                else if (dis1 <= radis2 && dis2 >= radis2)
                {
                    dis = System.Math.Sqrt(dis);
                    intersectStart = projectoint + line.normalizedDir * dis;
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
        public override RBIResultType RayboundingNearestPath(LineSegment2D line, double offset, ref RayboundingInfo rbi)
        {
            if (rbi == null)
            {
                rbi = new RayboundingInfo();
            }
            Double2 diff = this.circleCenter - line.startPoint;
            if (diff == Double2.zero)
                return RBIResultType.Fail;
            //
            Double2 projectoint = line.ProjectPoint(this.circleCenter);
            diff = this.circleCenter - projectoint;
            // 跟直线相交奥
            double dis = diff.sqrMagnitude - this.radius * this.radius;
            if (dis >= 0)
                return RBIResultType.Fail;
            dis = -dis;
            // 在同侧不行。
            Double2 diff1 = line.startPoint - projectoint;
            Double2 diff2 = line.endPoint - projectoint;
            if (Double2.Dot(diff1, diff2) >= 0)
                return RBIResultType.Fail;
            //
            if (diff1.sqrMagnitude < dis || diff2.sqrMagnitude < dis)
                return RBIResultType.Fail;

            dis = System.Math.Sqrt(dis) + offset;

            Double2 p1 = projectoint - line.normalizedDir * dis;
            Double2 p2 = projectoint + line.normalizedDir * dis;

            double bigRadius = this.radius + offset;
            double angle = SignedAngleInCircle(p1 - this.circleCenter, p2 - this.circleCenter, bigRadius);
            int count = (int)(System.Math.Abs(angle / 0.25f));
            double diffangle = angle / count;
            List<Double2> listpath = new List<Double2>();
            Double2 startVector = (p1 - this.circleCenter).normalized * bigRadius;
            for (int i = 1; i <= count - 1; i++)
            {
                Double2 rorateVector = Double2.Rotate(startVector, -diffangle * i);
                listpath.Add(rorateVector + this.circleCenter);
            }
            rbi.listpath = listpath;
            if (rbi.listpath != null && rbi.listpath.Count > 0)
            {
                rbi.CalcHelpData(line, offset, p1, p2);
                return RBIResultType.Succ;
            }
            return RBIResultType.Fail;
        }
        /// <summary>
        /// 圆弧上的2个向量夹角
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private  double SignedAngleInCircle(Double2 from, Double2 to, double r)
        {
            // [-PI / 2,  PI /2]  Asin
            // [0,  PI]  acos
            double sqrRadius = r * r;
            double sinValue = Double2.Cross(from, to) / sqrRadius;
            sinValue = System.Math.Min(sinValue, 1.0f);
            sinValue = System.Math.Max(sinValue, -1.0f);
            double cosValue = Double2.Dot(from, to) / sqrRadius;
            cosValue = System.Math.Min(cosValue, 1.0f);
            cosValue = System.Math.Max(cosValue, -1.0f);
            return MathFunc.GetAngle(cosValue, sinValue);
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Double2 pt)
        {
            Double2 diff = this.circleCenter - pt;
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
            Double2 diff = this.circleCenter - line.startPoint;
            if (diff == Double2.zero)
                return LineRelation.Intersect;
            //
            Double2 projectoint = line.ProjectPoint(this.circleCenter);
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
            double dis = line.AixsVector(this.circleCenter).sqrMagnitude - this.radius * this.radius;

            if (dis < 0)
            {
                Double2 diff = this.circleCenter - line.startPoint;
                if (Double2.Dot(diff, line.normalizedDir) > 0)
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
            double dis = line.CalcDistance(this.circleCenter);
            if (dis > this.radius)
                return LineRelation.Detach;
            else 
            {
                double sqrRadius = this.radius * this.radius;
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
        public override bool GetBornPoint(LineSegment2D line, double offset, ref Double2 bornPoint)
        {
            Double2 dirNormal = line.normalizedDir;
            Double2 diff = this.circleCenter - line.startPoint;
            if (diff == Double2.zero)
            {
                bornPoint = line.startPoint + line.normalizedDir * (this.radius + offset);
                return true;
            }
            else
            {
                Double2 projectoint = Double2.Project(diff, dirNormal) + line.startPoint;
                diff = this.circleCenter - projectoint;
                double dis = this.radius * this.radius - diff.sqrMagnitude;
                if (dis <= 0)
                {
                    return false;
                }
                else
                {
                    dis = (float)System.Math.Sqrt(dis) + offset;
                    double value = Double2.Dot(dirNormal, line.endPoint - projectoint);
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
            Double2 diff = this.circleCenter - ab.circleCenter;
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
        /// <summary>
        /// 求外部点到圆的切线
        /// </summary>
        /// <param name="outPoint">在圆上/圆内不计算切线</param>
        /// <param name="leftTangent">逆时针切线</param>
        /// <param name="rightTangent">顺时针切线</param>
        /// <returns></returns>
        public bool GetTangent(Double2 outPoint,ref Double2 leftTangent, ref Double2 rightTangent)
        {
            Double2 diff = this.circleCenter - outPoint;
            double sqrDiff = diff.sqrMagnitude;
            double sqrRadius = this.radius * this.radius;
            // 先求切线的长度sqr
            double sqrtLen = sqrDiff - sqrRadius;
            if (sqrtLen <= 0)
            {
                return false;
            }
            else 
            {
                // 再求斜边的高度平方
                double sqrHeight = sqrtLen * sqrRadius / sqrDiff;
                // 求垂线。
                Double2 aix = -Double2.Perpendicular(diff);
                // 再求斜边的垂足。
                double value = (sqrtLen - sqrHeight) / sqrDiff;
                Double2 aixPos = System.Math.Sqrt((float)value) * diff + outPoint;
                // 再求2个切点。
                double height = System.Math.Sqrt((float)sqrHeight);
                leftTangent = aixPos - aix.normalized * height - outPoint;
                rightTangent = aixPos + aix.normalized * height - outPoint;
                return true;
            }
        }
    }
}
