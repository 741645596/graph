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
        public Float2 center;
        /// <summary>
        /// 半径
        /// </summary>
        public float radius;
        public Circle2D(Float2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
            this.SetAABB(center - Float2.one * radius, center + Float2.one * radius);
        }
        /// <returns></returns>
        /// <summary>
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="paths">返回路径</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public override bool RayboundingNearestPath(LineSegment2D line, float offset, ref Float2 nearPoint, ref Float2 farPoint, ref List<Float2> paths)
        {
            Float2 diff = this.center - line.startPoint;
            if (diff == Float2.zero)
                return false;
            //
            Float2 projectoint = line.ProjectPoint(this.center);
            diff = this.center - projectoint;
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

            float angle = Float2.SignedAngle(p1 - this.center, p2 - this.center);
            int count = (int)(System.Math.Abs(angle / 0.25f));
            float diffangle = angle / count;
            List<Float2> listpath = new List<Float2>();
            Float2 startVector = (p1 - this.center).normalized * (this.radius + offset);
            for (int i = 1; i <= count - 1; i++)
            {
                Float2 rorateVector = Float2.Rotate(startVector, -diffangle * i);
                listpath.Add(rorateVector + this.center);
            }
            nearPoint = p1;
            farPoint = p2;
            paths = listpath;
            return true;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Float2 pt)
        {
            Float2 diff = this.center - pt;
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
            Float2 diff = this.center - line.startPoint;
            if (diff == Float2.zero)
                return LineRelation.Intersect;
            //
            Float2 projectoint = line.ProjectPoint(this.center);
            diff = this.center - projectoint;

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
            float dis = line.AixsVector(this.center).sqrMagnitude - this.radius * this.radius;

            if (dis < 0)
            {
                Float2 diff = this.center - line.startPoint;
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
            float dis = line.CalcDistance(this.center);
            if (dis > this.radius)
                return LineRelation.Detach;
            else 
            {
                float sqrRadius = this.radius * this.radius;
                if ((this.center - line.startPoint).sqrMagnitude > sqrRadius)
                {
                    return LineRelation.Intersect;
                }
                if ((this.center - line.endPoint).sqrMagnitude > sqrRadius)
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
            Float2 diff = this.center - line.startPoint;
            if (diff == Float2.zero)
            {
                bornPoint = line.startPoint + line.normalizedDir * (this.radius + offset);
                return true;
            }
            else
            {
                Float2 projectoint = Float2.Project(diff, dirNormal) + line.startPoint;
                diff = this.center - projectoint;
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
    }
}
