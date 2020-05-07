using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    public class Plane: iGeo3DElement
    {
        /// <summary>
        /// 直线上得点
        /// </summary>
        protected Double3 _pt;
        public Double3 Pt
        {
            get { return _pt; }
        }
        /// <summary>
        /// 法线
        /// </summary>
        protected Double3 _normalizedNormal;
        public Double3 NormalizedNormal
        {
            get { return _normalizedNormal; }
        }
        public Plane()
        {
            this._pt = Double3.zero;
            this._normalizedNormal = Double3.up;
        }
        public Plane(Double3 pt, Double3 normal)
        {
            this._pt = pt;
            this._normalizedNormal = normal.normalized;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool CheckIn(Double3 pt)
        {
            Double3 diff = pt - this.Pt;
            if (Double3.Dot(diff, this.NormalizedNormal) == 0)
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
        public virtual double CalcDistance(Double3 pt)
        {
            Double3 diff = pt - this.Pt;
            return Double3.ProjectOnPlane(diff, this.NormalizedNormal).magnitude;
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual Double3 ProjectPoint(Double3 pt)
        {
            Double3 diff = pt - this.Pt;
            return pt - Double3.ProjectOnPlane(diff, this.NormalizedNormal);
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual Double3 AixsVector(Double3 pt)
        {
            Double3 diff = pt - this.Pt;
            return Double3.Project(diff, this.NormalizedNormal);
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
            if (Double3.Dot(line.NormalizedDir, this.NormalizedNormal) == 0)
            {
                Double3 diff = line.StartPoint - this.Pt;
                if (Double3.Dot(diff, this.NormalizedNormal) == 0)
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
                Double3 intersectPoint = Double3.zero;
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
        public virtual bool GetIntersectPoint(Line3D line, ref Double3 intersectPoint)
        {
            if (line == null)
                return false;
            Double3 aixsVector = AixsVector(line.StartPoint);
            double distance = aixsVector.magnitude;
            if (distance == 0)
            {
                intersectPoint = line.StartPoint;
                return true;
            }
            else
            {
                double dot = Double3.Dot(aixsVector.normalized, line.NormalizedDir);
                if (dot == 0)
                {
                    return false;
                }
                else
                {
                    Double3 point = line.StartPoint - distance / dot * line.NormalizedDir;
                    if (line is Rays3D)
                    {
                        if (Double3.Dot(line.NormalizedDir, point - line.StartPoint) >= 0)
                        {
                            intersectPoint = point;
                            return true;
                        }
                        else return false;
                    }
                    else if (line is LineSegment3D)
                    {
                        if (Double3.Dot(point - line.StartPoint, point - (line as LineSegment3D).EndPoint) <= 0)
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
