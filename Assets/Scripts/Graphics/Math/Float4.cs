using System.Collections;
using System.Collections.Generic;

namespace Graphics.Math
{
    public partial struct Float4
    {
        public float x, y, z, w;

        private static readonly Float4 s_zero = new Float4(0, 0, 0, 0);
        public static Float4 Zero
        {
            get { return s_zero; }
        }

        public Float4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public static bool operator !=(Float4 vector1, Float4 vector2)
        {
            return vector1.x != vector2.x || vector1.y != vector2.y 
                || vector1.z != vector2.z || vector1.w  != vector2.w;
        }

        public static bool operator ==(Float4 vector1, Float4 vector2)
        {
            return vector1.x == vector2.x && vector1.y == vector2.y 
                && vector1.z == vector2.z && vector1.w == vector2.w;
        }

        public static Float4 operator +(Float4 vector1, Float4 vector2)
        {
            float xx = vector1.x + vector2.x;
            float yy = vector1.y + vector2.y;
            float zz = vector1.z + vector2.z;
            float ww = vector1.w + vector2.w;
            return new Float4(xx, yy, zz, ww);
        }


        public static Float4 operator -(Float4 vector1, Float4 vector2)
        {
            float xx = vector1.x - vector2.x;
            float yy = vector1.y - vector2.y;
            float zz = vector1.z - vector2.z;
            float ww = vector1.w - vector2.w;
            return new Float4(xx, yy, zz, ww);
        }


        public static Float4 operator *(float k, Float4 vector)
        {
            float xx = vector.x * k;
            float yy = vector.y * k;
            float zz = vector.z * k;
            float ww = vector.w * k;
            return new Float4(xx, yy, zz, ww);
        }

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
    }
}

