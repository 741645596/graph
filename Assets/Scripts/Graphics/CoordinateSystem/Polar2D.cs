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
        public Polar2D(Complex v)
        {
            this.radius = v.magnitude;
            this.angle = v.GetAngle();
        }
        /// <summary>
        /// 0 ����
        /// </summary>
        public static Polar2D zero
        {
            get { return s_zero; }
        }
        private static readonly Polar2D s_zero = new Polar2D(0, 0);
        /// <summary>
        /// ��λ����
        /// </summary>
        public static Polar2D one
        {
            get { return s_one; }
        }
        private static readonly Polar2D s_one = new Polar2D(1, 0);
        /// <summary>
        /// ָ������
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
        /// * ����
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
