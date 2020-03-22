using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    /// <summary>
    /// 射线
    /// </summary>
    public class Rays3D : Line3D
    {
        public Rays3D(Float3 startPt, Float3 dir)
        {
            this._startPoint = startPt;
            this._normalizedDir = dir.normalized;
        }
        /// <summary>
        /// 判断点是否在射线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Float3 pt)
        {
            bool ret = base.CheckIn(pt);
            if (ret == true)
            {
                Float3 diff = pt - this.StartPoint;
                if (Float3.Dot(diff, this.NormalizedDir) < 0)
                    return false;
                else return true;
            }
            return false;
        }
        /// <summary>
        /// 点导视线的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override float CalcDistance(Float3 pt)
        {
            Float3 diff = pt - this.StartPoint;
            if (diff == Float3.zero)
                return 0;
            if (Float3.Dot(diff, this.NormalizedDir) <= 0)
            {
                return diff.magnitude;
            }
            else
            {
                Float3 verticalAxis = this.AixsVector(pt);
                return verticalAxis.magnitude;
            }
        }
        /// <summary>
        /// 与线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public override LineRelation CheckLineRelation(Line3D line)
        {
            LineRelation lr = base.CheckLineRelation(line);
            if (lr == LineRelation.Intersect)
            {
                Float3 aixsVector = line.AixsVector(this.StartPoint);
                if (Float3.Dot(aixsVector, this.NormalizedDir) > 0)
                {
                    return LineRelation.Detach;
                }
                else
                {
                    return LineRelation.Intersect;
                }
            }
            else
            {
                return lr;
            }
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public override bool GetIntersectPoint(Line3D line, ref Float3 intersectPoint)
        {
            Float3 point = Float3.zero;
            if (base.GetIntersectPoint(line, ref point) == true)
            {
                if (Float3.Dot(line.NormalizedDir, point - line.StartPoint) >= 0)
                {
                    intersectPoint = point;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}