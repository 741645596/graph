﻿using RayGraphics.Math;


namespace RayGraphics.Geometric
{
    /// <summary>
    /// 直线
    /// </summary>
    [System.Serializable]
    public struct Rays2D : iGeo2DElement
    {
        /// <summary>
        /// 直线上得点
        /// </summary>
        public Double2 startPoint;
        /// <summary>
        /// 方向为单位向量
        /// </summary>
        public Double2 normalizedDir;

        public Rays2D(Double2 startPt, Double2 dir)
        {
            this.startPoint = startPt;
            this.normalizedDir = dir.normalized;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool CheckIn(Double2 pt)
        {
            Double2 diff = pt - this.startPoint;
            if (Double2.CheckInLine(diff, this.normalizedDir) == true && Double2.Dot(diff, this.normalizedDir) > 0)
            {
                 return true;
            }
            return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public double CalcDistance(Double2 pt)
        {
            Double2 aixsVector = this.AixsVector(pt);
            Double2 diff = pt - this.startPoint;
            if (Double2.Dot(diff, this.normalizedDir) >= 0)
            {
                return aixsVector.magnitude;
            }
            else 
            {
                return diff.magnitude;
            }    
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Double2 ProjectPoint(Double2 pt)
        {
            Double2 diff = pt - this.startPoint;
            if (diff == Double2.zero)
                return pt;
            return Double2.Project(diff, this.normalizedDir) + this.startPoint;
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Double2 AixsVector(Double2 pt)
        {
            Double2 diff = pt - this.startPoint;
            if (diff == Double2.zero)
                return Double2.zero;
            return diff - Double2.Project(diff, this.normalizedDir);
        }
        /// <summary>
        /// 与射线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(Line2D line)
        {
            // 共线判断
            if (Double2.CheckInLine(this.normalizedDir, line.normalizedDir) == true)
            {
                Double2 diff = line.startPoint - this.startPoint;
                // 贡献判断
                if (Double2.CheckInLine(this.normalizedDir, diff) == true)
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
                Double2 aixsVector = line.AixsVector(this.startPoint);
                if (Double2.Dot(aixsVector, this.normalizedDir) > 0)
                {
                    return LineRelation.Detach;
                }
                else
                {
                    return LineRelation.Intersect;
                }
            }
        }
        /// <summary>
        /// 射线与射线间的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(Rays2D line)
        {
            // 共线判断
            if (Double2.CheckInLine(this.normalizedDir, line.normalizedDir) == true)
            {
                Double2 diff = line.startPoint - this.startPoint;
                // 共线判断
                if (Double2.CheckInLine(this.normalizedDir, diff) == true)
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
                // 先判断this 是否与line 所在直线相交
                Double2 aixsVector = line.AixsVector(this.startPoint);
                if (Double2.Dot(aixsVector, this.normalizedDir) > 0)
                {
                    return LineRelation.Detach;
                }
                // 先判断line 是否与this 所在直线相交
                aixsVector = this.AixsVector(line.startPoint);
                if (Double2.Dot(aixsVector, line.normalizedDir) > 0)
                {
                    return LineRelation.Detach;
                }
                return LineRelation.Intersect;
            }
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(LineSegment2D line)
        {
            return line.CheckLineRelation(this);
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(Line2D line, ref Double2 intersectPoint)
        {
            Double2 aixsVector = line.AixsVector(this.startPoint);
            double distance = aixsVector.magnitude;
            if (distance == 0)
            {
                intersectPoint = this.startPoint;
                return true;
            }
            else
            {
                double dot = Double2.Dot(aixsVector.normalized, this.normalizedDir);
                if (dot < 0)
                {

                    intersectPoint = this.startPoint - distance / dot * this.normalizedDir;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(Rays2D line, ref Double2 intersectPoint)
        {
            Double2 aixsVector = line.AixsVector(this.startPoint);
            double distance = aixsVector.magnitude;
            if (distance == 0)
            {
                intersectPoint = this.startPoint;
                return true;
            }
            else
            {
                double dot = Double2.Dot(aixsVector.normalized, this.normalizedDir);
                if (dot < 0)
                {
                    Double2 point = this.startPoint - distance / dot * this.normalizedDir;
                    Double2 diff = point - line.startPoint;
                    if (Double2.Dot(diff, line.normalizedDir) > 0)
                    {
                        intersectPoint = point;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(LineSegment2D line, ref Double2 intersectStartPoint, ref Double2 intersectEndPoint)
        {
            return line.GetIntersectPoint(this, ref intersectStartPoint);
        }
        /// <summary>
        /// 镜面但
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Double2 GetMirrorPoint(Double2 point)
        {
            return point - 2 * AixsVector(point);
        }
#if Client
        /// <summary>
        /// start point
        /// </summary>
        public UnityEngine.Vector3 StartPoint
        {
            get { return startPoint.V3; }
        }
        /// <summary>
        /// End point
        /// </summary>
        public UnityEngine.Vector3 EndPoint
        {
            get { return (startPoint + 10 * normalizedDir).V3; }
        }
        /// <summary>
        /// draw
        /// </summary>
        public void Draw()
        {
            UnityEngine.GL.Vertex(this.StartPoint);
            UnityEngine.GL.Vertex(this.EndPoint);
        }
        /// <summary>
        /// DrawGizmos
        /// </summary>
        public void DrawGizmos()
        {
            startPoint.DrawGizmos();
            Double2 v1 = Double2.Perpendicular(normalizedDir).normalized * 0.2f;
            Double2 diff = 9.8f * normalizedDir;
            UnityEngine.Gizmos.DrawLine((startPoint + diff + v1).V3, EndPoint);
            UnityEngine.Gizmos.DrawLine((startPoint + diff - v1).V3, EndPoint);
        }
#endif
    }
}