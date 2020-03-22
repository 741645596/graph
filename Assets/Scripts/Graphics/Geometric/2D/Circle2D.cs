using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    [System.Serializable]
    public class Circle2D : AABB2D
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Float2 center;
        /// <summary>
        /// 半径
        /// </summary>
        public float radius;


        public Circle2D(Float2 center, float radius) : base(center - Float2.one * radius, center + Float2.one * radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }
}
