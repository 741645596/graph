using System.Collections;
using System.Collections.Generic;
using Graphics.Math;


namespace Graphics.Geometric
{
    /// <summary>
    /// 直线
    /// </summary>
    public class Line3D : iGeo3DElement
    {
        /// <summary>
        /// 直线上得点
        /// </summary>
        protected  Float3 _startPoint;
        public Float3 StartPoint
        {
            get { return _startPoint; }
        }
        /// <summary>
        /// 方向为单位向量
        /// </summary>
        protected Float3 _normalizedDir;
        public Float3 NormalizedDir
        {
            get { return _normalizedDir; }
        }

        public Line3D()
        {
            this._startPoint = Float3.zero;
            this._normalizedDir = Float3.up;
        }
        public Line3D(Float3 startPt, Float3 dir)
        {
            this._startPoint = startPt;
            this._normalizedDir = dir.normalized;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool CheckIn(Float3 pt)
        {
            Float3 aixsVector = this.AixsVector(pt);
            if (aixsVector == Float3.zero)
                return true;
            else return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual float CalcDistance(Float3 pt)
        {
            Float3 aixsVector = this.AixsVector(pt);
            return aixsVector.magnitude;
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual Float3 ProjectPoint(Float3 pt)
        {
            Float3 diff = pt - this.StartPoint;
            if (diff == Float3.zero)
                return pt;
            return Float3.Project(diff, this.NormalizedDir) + this.StartPoint;
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual Float3 AixsVector(Float3 pt)
        {
            Float3 diff = pt - this.StartPoint;
            if (diff == Float3.zero)
                return Float3.zero;
            return diff - Float3.Project(diff, this.NormalizedDir);
        }
        /// <summary>
        /// 与线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(Line3D line)
        {
            if (line == null)
                return LineRelation.None;
            if (CheckInSamePlane(this, line) == false)
            {
                if (Float3.Cross(this.NormalizedDir, line.NormalizedDir) == Float3.zero)
                {
                    Float3 diff = line.StartPoint - this.StartPoint;
                    if (Float3.Cross(this.NormalizedDir, diff) == Float3.zero)
                    {
                        return LineRelation.Coincide;
                    }
                    else
                    {
                        return LineRelation.Parallel;
                    }
                }
                else 
                {
                    return LineRelation.Intersect;
                }
            }
            else 
            {
                return LineRelation.Detach;
            }
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public virtual bool GetIntersectPoint(Line3D line, ref Float3 intersectPoint)
        {
            if (line == null)
                return false;
            Float3 aixsVector = AixsVector(line.StartPoint);
            float distance = aixsVector.magnitude;
            if (distance == 0)
            {
                intersectPoint = line.StartPoint;
                return true;
            }
            else 
            {
                float dot = Float3.Dot(aixsVector.normalized, line.NormalizedDir);
                if (dot == 0)
                {
                    return false;
                }
                else 
                {
                    intersectPoint = line.StartPoint - distance / dot * line.NormalizedDir;
                    return true;
                }
            }
        }
        /// <summary>
        /// 判断是否在相同的平面
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static bool CheckInSamePlane(Line3D line1, Line3D line2)
        {
            if (line1 == null||line2 == null)
                return false;
            // 判断是否共面
            Float3 diff = line2.StartPoint - line1.StartPoint;
            Float3 VerticalAxis = Line3D.GetVerticalAxis(line1, line2);
            return Float3.Dot(diff, VerticalAxis) == 0 ? true : false;
        }
        /// <summary>
        /// 获取垂直轴
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static Float3 GetVerticalAxis(Line3D line1, Line3D line2)
        {
            if (line1 == null || line2 == null)
                return Float3.zero;
            return Float3.Cross(line1.NormalizedDir, line2.NormalizedDir);
        }
    }
}