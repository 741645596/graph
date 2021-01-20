using RayGraphics.Math;
/// <summary>
/// 2D极坐标系
/// </summary>
namespace RayGraphics.CoordinateSystem
{
    public struct Polar2D 
    {
        /// <summary>
        /// 长度
        /// </summary>
        public double radius;
        /// <summary>
        /// 角度
        /// </summary>
        public double angle;

        public Polar2D(float radius, float angle)
        {
            this.radius = radius;
            this.angle = angle;
        }
        public Polar2D(double radius, double angle)
        {
            this.radius = radius;
            this.angle = angle;
        }
    }
}
