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
        protected Float3 _endPoint;
        public Float3 EndPoint
        {
            get { return _endPoint; }
        }
        public LineSegment3D(Float3 startPt, Float3 endPt)
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
        public override bool CheckIn(Float3 pt)
        {
            bool ret = base.CheckIn(pt);
            if (ret == true)
            {
                Float3 diff1 = pt - this.StartPoint;
                Float3 diff2 = pt - this.EndPoint;
                if (Float3.Dot(diff1, diff2) > 0)
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
        public override float CalcDistance(Float3 pt)
        {
            Float3 diff1 = pt - this.StartPoint;
            Float3 diff2 = pt - this.EndPoint;
            float ret1 = Float3.Dot(diff1, this.NormalizedDir);
            float ret2 = Float3.Dot(diff2, this.NormalizedDir);
            if (ret1 * ret2 <= 0)
            {
                Float3 aixsVector = this.AixsVector(pt);
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
                Float3 aixsVector1 = line.AixsVector(this.StartPoint);
                Float3 aixsVector2 = line.AixsVector(this.EndPoint);
                // 方向相反相交
                if (Float3.Dot(aixsVector1, aixsVector2) <= 0)
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
        public override bool GetIntersectPoint(Line3D line, ref Float3 intersectPoint)
        {
            Float3 point = Float3.zero;
            if (base.GetIntersectPoint(line, ref point) == true)
            {
                if (Float3.Dot(point - line.StartPoint, point - (line as LineSegment3D).EndPoint) <= 0)
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
