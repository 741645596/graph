using RayGraphics.Math;
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
        /// Center
        /// </summary>
        public Float2 Center
        {
            get { return (this.leftBottom + this.rightUp) * 0.5f; }
        }
        /// <summary>
        /// 2* Center
        /// </summary>
        public Float2 Center2
        {
            get { return this.leftBottom + this.rightUp; }
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
        /// <param name="isCounterclockwiseDir">路线在线段区域，是否逆时针方向</param>
        /// <param name="paths">返回路径</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public virtual  bool RayboundingNearestPath(LineSegment2D line, float offset, ref Float2 nearPoint, ref Float2 farPoint, ref bool isCounterclockwiseDir, ref List<Float2> paths)
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
        /// <summary>
        /// 与矩形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public virtual bool CheckIntersect(Rect2D ab)
        {
            return false;
        }
        /// <summary>
        /// 与圆的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public virtual bool CheckIntersect(Circle2D ab)
        {
            return false;
        }
        /// <summary>
        /// 与多边形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public virtual bool CheckIntersect(Polygon2D ab)
        {
            return false;
        }
        /// <summary>
        /// 判断2个对象是否相交
        /// </summary>
        /// <param name="ab1"></param>
        /// <param name="ab2"></param>
        /// <returns>暂处理不多边形的关系</returns>
        public static bool CheckIntersect(AABB2D ab1, AABB2D ab2)
        {
            if (ab1 == null || ab2 == null)
                return false;
            if (ab2 is Rect2D)
                return ab1.CheckIntersect(ab2 as Rect2D);
            else if (ab2 is Circle2D)
                return ab1.CheckIntersect(ab2 as Circle2D);
            else if(ab2 is Polygon2D)
                return ab1.CheckIntersect(ab2 as Polygon2D);
            return false;
        }
        /// <summary>
        /// 获取边
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public virtual LineSegment2D GetEdge(int edgeIndex)
        {
            if (edgeIndex == 0)
            {
                return new LineSegment2D(this.leftBottom, this.RightBottom);
            }
            else if (edgeIndex == 1)
            {
                return new LineSegment2D(this.RightBottom, this.rightUp);
            }
            else if (edgeIndex == 2)
            {
                return new LineSegment2D(this.rightUp, this.LeftUp);
            }
            else if (edgeIndex == 3)
            {
                return new LineSegment2D(this.LeftUp, this.leftBottom);
            }
            return new LineSegment2D(Float2.zero, Float2.zero);
        }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Float2 GetPoint(int index)
        {
            if (index == 0)
            {
                return this.leftBottom;
            }
            else if (index == 1)
            {
                return this.RightBottom;
            }
            else if (index == 2)
            {
                return this.rightUp;
            }
            else if (index == 3)
            {
                return this.LeftUp;
            }
            return Float2.zero;
        }

        /// <summary>
        /// 获取法线
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public virtual Float2 GetNormal(int edgeIndex)
        {
            if (edgeIndex == 0)
            {
                return Float2.down;
            }
            else if (edgeIndex == 1)
            {
                return Float2.right;
            }
            else if (edgeIndex == 2)
            {
                return Float2.up;
            }
            else if (edgeIndex == 3)
            {
                return Float2.left;
            }
            return Float2.zero;
        }

        /// <summary>
        /// 获取边数
        /// </summary>
        /// <returns></returns>
        public virtual int GetEdgeNum()
        {
            return 4;
        }
        /// <summary>
        /// 获取顶点数组
        /// </summary>
        /// <returns></returns>
        public virtual Float2[] GetPoints()
        {
            Float2[] points = new Float2[4];
            points[0] = this.leftBottom;
            points[1] = this.RightBottom;
            points[2] = this.rightUp;
            points[3] = this.LeftUp;
            return points;
        }
    }
}