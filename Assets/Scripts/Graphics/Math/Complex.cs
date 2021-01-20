using System;
/// <summary>
/// 复数：满足加法交换律，乘法交换律， 加法结合律，乘法结合律， 分配律
/// </summary>
namespace RayGraphics.Math
{
    public struct Complex 
    {
        public double x, y;

        public Complex(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Complex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// 0 向量
        /// </summary>
        public static Complex zero
        {
            get { return s_zero; }
        }
        private static readonly Complex s_zero = new Complex(0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Complex one
        {
            get { return s_one; }
        }
        private static readonly Complex s_one = new Complex(1, 0);
        /// <summary>
        /// 模的平方
        /// </summary>
        public double sqrMagnitude
        {
            get { return this.x * this.x + this.y * this.y; }
        }
        /// <summary>
        /// 模
        /// </summary>
        public double magnitude
        {
            get { return System.Math.Sqrt(this.sqrMagnitude); }
        }
        /// <summary>
        /// 共轭复数
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Complex GetConjugateComplex(Complex v)
        {
            return new Complex(v.x, -v.y);
        }
        /// <summary>
        /// ！=
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Complex v1, Complex v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }
        /// <summary>
        /// ==
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Complex v1, Complex v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        /// <summary>
        /// +
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Complex operator +(Complex v1, Complex v2)
        {
            double xx = v1.x + v2.x;
            double yy = v1.y + v2.y;
            return new Complex(xx, yy);
        }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Complex operator -(Complex v1, Complex v2)
        {
            double xx = v1.x - v2.x;
            double yy = v1.y - v2.y;
            return new Complex(xx, yy);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Complex operator *(double k, Complex v)
        {
            double xx = v.x * k;
            double yy = v.y * k;
            return new Complex(xx, yy);
        }
        /// <summary>
        ///  * 运算
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Complex operator *(Complex v, double k)
        {
            double xx = v.x * k;
            double yy = v.y * k;
            return new Complex(xx, yy);
        }
        /// <summary>
        /// /运算
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Complex operator /(Complex v, double k)
        {
            if (k != 0)
            {
                k = 1 / k;
            }
            else
            {
                k = 1;
            }
            return v * k;
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Complex operator *(Complex v1, Complex v2)
        {
            double xx = v1.x * v2.x - v1.y * v2.y;
            double yy = v1.x * v2.y + v1.y * v2.x;
            return new Complex(xx, yy);
        }
        /// <summary>
        /// / 运算,除法相当于都乘以一个共轭负数
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Complex operator /(Complex v1, Complex v2)
        {
            double k = v2.sqrMagnitude;
            if (k == 0)
            {
                return v1;
            }
            k = 1 / k;
            double xx = v1.x * v2.x + v1.y * v2.y;
            double yy = v1.y * v2.x - v1.x * v2.y;
            return new Complex(xx  * k, yy * k);
        }
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Complex p = (Complex)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
