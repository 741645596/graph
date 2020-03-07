using System.Collections;
using System.Collections.Generic;
using Graphics.Math;

namespace Graphics.Geometric
{
    public class Plane: iGeo3DElement
    {
        /// <summary>
        /// 直线上得点
        /// </summary>
        protected Float3 _pt;
        public Float3 Pt
        {
            get { return _pt; }
        }
        /// <summary>
        /// 法线
        /// </summary>
        protected Float3 _normalizedNormal;
        public Float3 NormalizedNormal
        {
            get { return _normalizedNormal; }
        }
        public Plane()
        {
            this._pt = Float3.zero;
            this._normalizedNormal = Float3.up;
        }
        public Plane(Float3 pt, Float3 normal)
        {
            this._pt = pt;
            this._normalizedNormal = normal.normalized;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool CheckIn(Float3 pt)
        {
            Float3 diff = pt - this.Pt;
            if (Float3.Dot(diff, this.NormalizedNormal) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual float CalcDistance(Float3 pt)
        {
            Float3 diff = pt - this.Pt;
            return Float3.ProjectOnPlane(diff, this.NormalizedNormal).magnitude;
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual Float3 ProjectPoint(Float3 pt)
        {
            Float3 diff = pt - this.Pt;
            return pt - Float3.ProjectOnPlane(diff, this.NormalizedNormal);
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual Float3 AixsVector(Float3 pt)
        {
            Float3 diff = pt - this.Pt;
            return Float3.Project(diff, this.NormalizedNormal);
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
            if (Float3.Dot(line.NormalizedDir, this.NormalizedNormal) == 0)
            {
                Float3 diff = line.StartPoint - this.Pt;
                if (Float3.Dot(diff, this.NormalizedNormal) == 0)
                {
                    // 线在平面上
                    return LineRelation.Coincide;
                }
                else
                {
                    return LineRelation.Parallel;
                }
            }
            else
            {
                Float3 intersectPoint = Float3.zero;
                if (GetIntersectPoint(line, ref intersectPoint) == true)
                {
                    return LineRelation.Intersect;
                }
                else 
                {
                    return LineRelation.Detach;
                }
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
                    Float3 point = line.StartPoint - distance / dot * line.NormalizedDir;
                    if (line is Rays3D)
                    {
                        if (Float3.Dot(line.NormalizedDir, point - line.StartPoint) >= 0)
                        {
                            intersectPoint = point;
                            return true;
                        }
                        else return false;
                    }
                    else if (line is LineSegment3D)
                    {
                        if (Float3.Dot(point - line.StartPoint, point - (line as LineSegment3D).EndPoint) <= 0)
                        {
                            intersectPoint = point;
                            return true;
                        }
                        else return false;
                    }
                    else
                    {
                        intersectPoint = point;
                        return true;
                    }
                }
            }
        }
    }
}
