using RayGraphics.Math;
/// <summary>
/// 2D������ϵ
/// </summary>
namespace RayGraphics.CoordinateSystem
{
    public struct Polar2D 
    {
        /// <summary>
        /// ����
        /// </summary>
        public double radius;
        /// <summary>
        /// �Ƕ�
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
