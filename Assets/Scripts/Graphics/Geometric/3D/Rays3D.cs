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
        public Rays3D(Double3 startPt, Double3 dir)
        {
            this._startPoint = startPt;
            this._normalizedDir = dir.normalized;
        }
        /// <summary>
        /// 判断点是否在射线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Double3 pt)
        {
            bool ret = base.CheckIn(pt);
            if (ret == true)
            {
                Double3 diff = pt - this.StartPoint;
                if (Double3.Dot(diff, this.NormalizedDir) < 0)
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
        public override double CalcDistance(Double3 pt)
        {
            Double3 diff = pt - this.StartPoint;
            if (diff == Double3.zero)
                return 0;
            if (Double3.Dot(diff, this.NormalizedDir) <= 0)
            {
                return diff.magnitude;
            }
            else
            {
                Double3 verticalAxis = this.AixsVector(pt);
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
                Double3 aixsVector = line.AixsVector(this.StartPoint);
                if (Double3.Dot(aixsVector, this.NormalizedDir) > 0)
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
        public override bool GetIntersectPoint(Line3D line, ref Double3 intersectPoint)
        {
            Double3 point = Double3.zero;
            if (base.GetIntersectPoint(line, ref point) == true)
            {
                if (Double3.Dot(line.NormalizedDir, point - line.StartPoint) >= 0)
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