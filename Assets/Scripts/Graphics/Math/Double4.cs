using System.Collections;
using System.Collections.Generic;

namespace RayGraphics.Math
{
    public partial struct Double4
    {
        public double x, y, z, w;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public Double4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public Double4(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Set(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        /// <summary>
        /// 按索引访问
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get
            {
                if (index == 0)
                    return this.x;
                else if (index == 1)
                    return this.y;
                else if (index == 2)
                    return this.z;
                else if (index == 3)
                    return this.w;
                else return 0;
            }
            set
            {
                if (index == 0)
                    this.x = value;
                else if (index == 1)
                    this.y = value;
                else if (index == 2)
                    this.z = value;
                else if (index == 3)
                    this.w = value;
            }
        }
        /// <summary>
        /// 0
        /// </summary>
        private static readonly Double4 s_zero = new Double4(0, 0, 0, 0);
        public static Double4 zero
        {
            get { return s_zero; }
        }
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Double4 one
        {
            get { return s_one; }
        }
        private static readonly Double4 s_one = new Double4(1, 1, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static bool operator !=(Double4 vector1, Double4 vector2)
        {
            return vector1.x != vector2.x || vector1.y != vector2.y
                || vector1.z != vector2.z || vector1.w != vector2.w;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static bool operator ==(Double4 vector1, Double4 vector2)
        {
            return vector1.x == vector2.x && vector1.y == vector2.y
                && vector1.z == vector2.z && vector1.w == vector2.w;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Double4 operator +(Double4 vector1, Double4 vector2)
        {
            double xx = vector1.x + vector2.x;
            double yy = vector1.y + vector2.y;
            double zz = vector1.z + vector2.z;
            double ww = vector1.w + vector2.w;
            return new Double4(xx, yy, zz, ww);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Double4 operator -(Double4 vector1, Double4 vector2)
        {
            double xx = vector1.x - vector2.x;
            double yy = vector1.y - vector2.y;
            double zz = vector1.z - vector2.z;
            double ww = vector1.w - vector2.w;
            return new Double4(xx, yy, zz, ww);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Double4 operator *(double k, Double4 vector)
        {
            double xx = vector.x * k;
            double yy = vector.y * k;
            double zz = vector.z * k;
            double ww = vector.w * k;
            return new Double4(xx, yy, zz, ww);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Double4 operator *(Double4 vector, double k)
        {
            double xx = vector.x * k;
            double yy = vector.y * k;
            double zz = vector.z * k;
            double ww = vector.w * k;
            return new Double4(xx, yy, zz, ww);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Double4 p = (Double4)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) && (z == p.z) && (w == p.w);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Dot(Double4 a, Double4 b) //点积
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }
    }
}

