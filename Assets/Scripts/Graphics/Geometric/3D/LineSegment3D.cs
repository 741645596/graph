using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Geometric
{
    /// <summary>
    /// 线段
    /// </summary>
    public class LineSegment3D : Line3D
    {
        protected Double3 _endPoint;
        public Double3 EndPoint
        {
            get { return _endPoint; }
        }
        public LineSegment3D(Double3 startPt, Double3 endPt)
        {
            this._startPoint = startPt;
            this._endPoint = endPt;
            this._normalizedDir = (endPt - startPt).normalized;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Double3 pt)
        {
            bool ret = base.CheckIn(pt);
            if (ret == true)
            {
                Double3 diff1 = pt - this.StartPoint;
                Double3 diff2 = pt - this.EndPoint;
                if (Double3.Dot(diff1, diff2) > 0)
                    return false;
                else return true;
            }
            return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override double CalcDistance(Double3 pt)
        {
            Double3 diff1 = pt - this.StartPoint;
            Double3 diff2 = pt - this.EndPoint;
            double ret1 = Double3.Dot(diff1, this.NormalizedDir);
            double ret2 = Double3.Dot(diff2, this.NormalizedDir);
            if (ret1 * ret2 <= 0)
            {
                Double3 aixsVector = this.AixsVector(pt);
                return aixsVector.magnitude;
            }
            else if (ret1 < 0)
            {
                return diff1.magnitude;
            }
            else
            {
                return diff2.magnitude;
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
                // 计算轴向量
                Double3 aixsVector1 = line.AixsVector(this.StartPoint);
                Double3 aixsVector2 = line.AixsVector(this.EndPoint);
                // 方向相反相交
                if (Double3.Dot(aixsVector1, aixsVector2) <= 0)
                {
                    return LineRelation.Intersect;
                }
                else
                {
                    return LineRelation.Detach;
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
                if (Double3.Dot(point - line.StartPoint, point - (line as LineSegment3D).EndPoint) <= 0)
                {
                    intersectPoint = point;
                    return true;
                }
                else return false;
            }
            return false;
        }
    }
}
