﻿using RayGraphics.Math;
using System.Collections.Generic;


namespace RayGraphics.Geometric
{

    public class AABB2D
    {
        /// <summary>
        /// 左下
        /// </summary>
        public Float2 leftBottom;
        /// <summary>
        /// 右上
        /// </summary>
        public Float2 rightUp;
        /// <summary>
        /// 左⬆️
        /// </summary>
        public Float2 LeftUp
        {
            get { return new Float2(leftBottom.x, rightUp.y); }
        }
        /// <summary>
        /// 右⬇️
        /// </summary>
        public Float2 RightBottom
        {
            get { return new Float2(rightUp.x, leftBottom.y); }
        }
        /// <summary>
        /// 左边法线
        /// </summary>
        public Float2 LeftEdgeNormal
        {
            get { return Float2.left; }
        }
        /// <summary>
        /// 右边法线
        /// </summary>
        public Float2 RightEdgeNormal
        {
            get { return Float2.right; }
        }
        /// <summary>
        /// 下边法线
        /// </summary>
        public Float2 BottomEdgeNormal
        {
            get { return Float2.down; }
        }
        /// <summary>
        /// 上边法线
        /// </summary>
        public Float2 UpEdgeNormal
        {
            get { return Float2.down; }
        }
        /// <summary>
        /// 设置aabb
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ru"></param>
        protected void SetAABB(Float2 lb, Float2 ru)
        {
            this.leftBottom = Float2.Min(lb, ru);
            this.rightUp = Float2.Max(lb, ru);
        }
#if Client
        /// <summary>
        /// draw, 逆时针绘制
        /// </summary>
        public void Draw()
        {
            UnityEngine.GL.Vertex(this.leftBottom.V3);
            UnityEngine.GL.Vertex(this.RightBottom.V3);
            //
            UnityEngine.GL.Vertex(this.RightBottom.V3);
            UnityEngine.GL.Vertex(this.rightUp.V3);
            //
            UnityEngine.GL.Vertex(this.rightUp.V3);
            UnityEngine.GL.Vertex(this.LeftUp.V3);
            //
            UnityEngine.GL.Vertex(this.LeftUp.V3);
            UnityEngine.GL.Vertex(this.leftBottom.V3);
        }
        /// <summary>
        /// DrawGizmos
        /// </summary>
        public void DrawGizmos()
        {
            leftBottom.DrawGizmos();
            rightUp.DrawGizmos();
            LeftUp.DrawGizmos();
            RightBottom.DrawGizmos();
        }
#endif
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool CheckIn(Float2 pt)
        {
            Float2 max = this.rightUp;
            if (pt.x > max.x || pt.y > max.y)
                return false;
            Float2 min = this.leftBottom;
            if (pt.x < min.x || pt.y < min.y)
                return false;
            return true;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public float CalcDistance(Float2 pt)
        {
            float distance = 0.0f;
            AligentStyle style = GetAligentStyle(pt);
            switch (style)
            {
                case AligentStyle.LeftBottom:
                    distance = (this.leftBottom - pt).magnitude;
                    break;
                case AligentStyle.MiddleBottom:
                    distance = this.leftBottom.y - pt.y;
                    break;
                case AligentStyle.RightBottom:
                    distance = (this.RightBottom - pt).magnitude;
                    break;
                case AligentStyle.LeftMiddle:
                    distance = this.leftBottom.x - pt.x;
                    break;
                case AligentStyle.MiddleMiddle:
                    distance = 0.0f;
                    break;
                case AligentStyle.RightMiddle:
                    distance = pt.x - this.RightBottom.x;
                    break;
                case AligentStyle.LeftUp:
                    distance = (this.LeftUp - pt).magnitude;
                    break;
                case AligentStyle.MiddleUp:
                    distance = pt.y - this.LeftUp.y;
                    break;
                case AligentStyle.RightUp:
                    distance = (pt - this.rightUp).magnitude;
                    break;
                default:
                    break;
            }
            return distance;
        }
        /// <summary>
        /// 点与矩形的关系
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public AligentStyle GetAligentStyle(Float2 pt)
        {
            Float2 max = this.rightUp;
            Float2 min = this.leftBottom;
            // 8 种情形
            if (pt.y < min.y)
            {
                if (pt.x < min.x)
                    return AligentStyle.LeftBottom;
                else if (pt.x >= min.x && pt.x <= max.x)
                    return AligentStyle.MiddleBottom;
                else
                    return AligentStyle.RightBottom;
            }
            else if (pt.y >= min.y && pt.y <= max.y)
            {
                if (pt.x < min.x)
                    return AligentStyle.LeftMiddle;
                else if (pt.x >= min.x && pt.x <= max.x)
                    return AligentStyle.MiddleMiddle;
                else
                    return AligentStyle.RightMiddle;
            }
            else
            {
                if (pt.x < min.x)
                    return AligentStyle.LeftUp;
                else if (pt.x >= min.x && pt.x <= max.x)
                    return AligentStyle.MiddleUp;
                else
                    return AligentStyle.RightUp;
            }
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 ProjectPoint(Float2 pt)
        {
            return Float2.zero;
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 AixsVector(Float2 pt)
        {
            return Float2.zero;
        }
        /// <summary>
        /// 与直线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(Line2D line)
        {
            if (CheckIn(line.startPoint) == true)
                return LineRelation.Intersect;
            else // 4个点在同侧
            {
                Float2 p1 = this.leftBottom - line.startPoint;
                Float2 p2 = this.RightBottom - line.startPoint;
                
                
                float cross1 = Float2.Cross(line.normalizedDir, p1);
                if (cross1 * Float2.Cross(line.normalizedDir, p2) <= 0)
                {
                    return LineRelation.Intersect;
                }
                Float2 p3 = this.rightUp - line.startPoint;
                if (cross1 * Float2.Cross(line.normalizedDir, p3) <= 0)
                {
                    return LineRelation.Intersect;
                }
                Float2 p4 = this.LeftUp - line.startPoint;
                if (cross1 * Float2.Cross(line.normalizedDir, p4) <= 0)
                {
                    return LineRelation.Intersect;
                }
                return LineRelation.Detach;
            }
        }
        /// <summary>
        /// 直线与射线间的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(Rays2D line)
        {
            return LineRelation.Intersect;
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(LineSegment2D line)
        {
            return LineRelation.Intersect;
        }
        /// <summary>
        /// 镜面但
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Float2 GetMirrorPoint(Float2 point)
        {
            return point - 2 * AixsVector(point);
        }
        /// <summary>
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="nearPoint">最近的一个交点</param>
        /// <param name="farPoint">最远的一个交点</param>
        /// <param name="paths">返回路径</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public virtual  bool RayboundingNearestPath(LineSegment2D line, float offset, ref Float2 nearPoint, ref Float2 farPoint, ref List<Float2> paths)
        {
            return false;
        }
        /// <summary>
        /// 获取挡格附近出生点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <param name="bornPoint"></param>
        /// <returns></returns>
        public virtual bool GetBornPoint(LineSegment2D line, float offset, ref Float2 bornPoint)
        {
            return false;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectStart"></param>
        /// <param name="intersectEnd"></param>
        /// <returns></returns>
        public virtual bool GetIntersectPoint(LineSegment2D line, ref Float2 intersectStart, ref Float2 intersectEnd)
        {
            return false;
        }
    }
}