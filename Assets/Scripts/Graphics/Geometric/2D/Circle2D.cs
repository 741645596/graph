using Graphics.Math;

namespace Graphics.Geometric
{
    [System.Serializable]
    public class Circle2D : iGeo2DElement
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Float2 center;
        /// <summary>
        /// 半价
        /// </summary>
        public float radius;


        public Circle2D(Float2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool CheckIn(Float2 pt)
        {
            Float2 diff = pt - this.center;
            if (diff.sqrMagnitude <= radius * radius)
                return true;
            return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public float CalcDistance(Float2 pt)
        {
            Float2 diff = pt - this.center;
            float dis = diff.magnitude - radius;
            dis = dis < 0 ? 0 : dis;
            return dis;
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 ProjectPoint(Float2 pt)
        {
            return pt + AixsVector(pt);
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 AixsVector(Float2 pt)
        {
            Float2 diff = pt - this.center;
            float length = diff.magnitude;
            if (length >= radius)
            {
                return diff.normalized * (length - radius);
            }
            else if (length > 0)
            {
                return  -diff.normalized * (length - radius);
            }
            return Float2.zero;
        }
        /// <summary>
        /// 与直线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(Line2D line)
        {
            float distance = line.CalcDistance(this.center);
            if (distance > this.radius)
                return LineRelation.Detach; 
            return LineRelation.Intersect;
        }
        /// <summary>
        /// 直线与射线间的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(Rays2D line)
        {
            if (CheckIn(line.startPoint) == true)
                return LineRelation.Intersect;
            //
            float distance = line.CalcDistance(this.center);
            if (distance > this.radius)
                return LineRelation.Detach;
            else
            {
                Float2 projectPoint = line.ProjectPoint(this.center);
                Float2 diff = line.startPoint - projectPoint;
                if (Float2.Dot(diff, line.normalizedDir) < 0)
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
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(LineSegment2D line)
        {
            if (CheckIn(line.startPoint) == true || CheckIn(line.endPoint))
                return LineRelation.Intersect;
            //
            float distance = line.CalcDistance(this.center);
            if (distance > this.radius)
                return LineRelation.Detach;
            else
            {
                Float2 projectPoint = line.ProjectPoint(this.center);
                Float2 diff = line.startPoint - projectPoint;
                if (Float2.Dot(diff, line.normalizedDir) < 0)
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
#if Client
        /// <summary>
        /// draw, 逆时针绘制
        /// </summary>
        public void Draw()
        {

        }
        /// <summary>
        /// DrawGizmos
        /// </summary>
        public void DrawGizmos()
        {
        }
#endif
    }
}
