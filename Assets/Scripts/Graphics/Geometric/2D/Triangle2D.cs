using RayGraphics.Math;

namespace RayGraphics.Geometric
{

    public class Triangle2D : AABB2D
    {
        /// <summary>
        /// 顶点1
        /// </summary>
        public Float2 p1;
        /// <summary>
        /// 顶点2
        /// </summary>
        public Float2 p2;
        /// <summary>
        /// 顶点3
        /// </summary>
        public Float2 p3;
        /// <summary>
        /// 顺时针顶点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Triangle2D(Float2 p1, Float2 p2, Float2 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            Float2 min = Float2.Min(p1, Float2.Min(p2, p3));
            Float2 max = Float2.Max(p1, Float2.Max(p2, p3));
            this.SetAABB(min, max);
        }
    }
}