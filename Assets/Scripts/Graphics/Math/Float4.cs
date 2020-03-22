using System.Collections;
using System.Collections.Generic;

namespace RayGraphics.Math
{
    public partial struct Float4
    {
        public float x, y, z, w;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public Float4(float x, float y, float z, float w)
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
        public Float4(double x, double y, double z, double w)
        {
            this.x = (float)x;
            this.y = (float)y;
            this.z = (float)z;
            this.w = (float)w;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Set(float x, float y, float z, float w)
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
        public float this[int index]
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
        private static readonly Float4 s_zero = new Float4(0, 0, 0, 0);
        public static Float4 zero
        {
            get { return s_zero; }
        }
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Float4 one
        {
            get { return s_one; }
        }
        private static readonly Float4 s_one = new Float4(1, 1, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static bool operator !=(Float4 vector1, Float4 vector2)
        {
            return vector1.x != vector2.x || vector1.y != vector2.y 
                || vector1.z != vector2.z || vector1.w  != vector2.w;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static bool operator ==(Float4 vector1, Float4 vector2)
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
        public static Float4 operator +(Float4 vector1, Float4 vector2)
        {
            float xx = vector1.x + vector2.x;
            float yy = vector1.y + vector2.y;
            float zz = vector1.z + vector2.z;
            float ww = vector1.w + vector2.w;
            return new Float4(xx, yy, zz, ww);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Float4 operator -(Float4 vector1, Float4 vector2)
        {
            float xx = vector1.x - vector2.x;
            float yy = vector1.y - vector2.y;
            float zz = vector1.z - vector2.z;
            float ww = vector1.w - vector2.w;
            return new Float4(xx, yy, zz, ww);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Float4 operator *(float k, Float4 vector)
        {
            float xx = vector.x * k;
            float yy = vector.y * k;
            float zz = vector.z * k;
            float ww = vector.w * k;
            return new Float4(xx, yy, zz, ww);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Float4 operator *(Float4 vector, float k)
        {
            float xx = vector.x * k;
            float yy = vector.y * k;
            float zz = vector.z * k;
            float ww = vector.w * k;
            return new Float4(xx, yy, zz, ww);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Float4 p = (Float4)obj;
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
        public static float Dot(Float4 a, Float4 b) //点积
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }
    }
}

