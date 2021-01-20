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
        public Polar2D(Complex v)
        {
            this.radius = v.magnitude;
            this.angle = v.GetAngle();
        }
        /// <summary>
        /// 0 向量
        /// </summary>
        public static Polar2D zero
        {
            get { return s_zero; }
        }
        private static readonly Polar2D s_zero = new Polar2D(0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Polar2D one
        {
            get { return s_one; }
        }
        private static readonly Polar2D s_one = new Polar2D(1, 0);
        /// <summary>
        /// 指数函数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Polar2D Exp(double n)
        {
            if (radius == 0)
            {
                return zero;
            }
            double len = System.Math.Pow(radius, n);
            angle *= n;
            return new Polar2D(len, angle);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Polar2D operator *(Polar2D v1, Polar2D v2)
        {
            double xx = v1.radius * v2.radius;
            double yy = v1.angle + v2.angle;
            return new Polar2D(xx, yy);
        }
    }
}
