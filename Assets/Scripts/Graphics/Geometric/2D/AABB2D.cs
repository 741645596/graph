using Graphics.Math;


namespace Graphics.Geometric
{

    public class AABB2D: iGeo2DElement
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

        public AABB2D(Float2 lb, Float2 ru)
        {
            this.leftBottom = Float2.Min(lb, ru);
            this.rightUp = Float2.Max(lb, ru);
        }
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
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool CheckIn(Float2 pt)
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
        public LineRelation CheckLineRelation(Line2D line)
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
        public LineRelation CheckLineRelation(Rays2D line)
        {
            return LineRelation.Intersect;
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(LineSegment2D line)
        {
            return LineRelation.Intersect;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(Line2D line, ref Float2 intersectPoint)
        {
            return true;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(Rays2D line, ref Float2 intersectPoint)
        {
            return true;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(LineSegment2D line, ref Float2 intersectPoint)
        {
            return true;
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
    }
}