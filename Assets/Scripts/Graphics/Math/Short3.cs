namespace RayGraphics.Math
{
    [System.Serializable]
    public partial struct Short3
    {
        public short x, y, z;

        public Short3(short x, short y, short z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Short3(int x, int y, int z)
        {
            this.x = (short)x;
            this.y = (short)y;
            this.z = (short)z;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(short x, short y, short z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// 按索引访问
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public short this[int index]
        {
            get
            {
                if (index == 0)
                    return this.x;
                else if (index == 1)
                    return this.y;
                else if (index == 2)
                    return this.z;
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
            }
        }
        /// <summary>
        /// 模的平方
        /// </summary>
        public int sqrMagnitude
        {
            get { return this.x * this.x + this.y * this.y + this.z * this.z; }
        }
        /// <summary>
        /// 模
        /// </summary>
        public float magnitude
        {
            get { return (float)System.Math.Sqrt(this.sqrMagnitude); }
        }
        /// <summary>
        /// 无效
        /// </summary>
        public static Short3 invalid
        {
            get { return s_invalid; }
        }
        private static readonly Short3 s_invalid = new Short3(-1, -1, -1);
        /// <summary>
        /// 0 向量
        /// </summary>
        public static Short3 zero
        {
            get { return s_zero; }
        }
        private static readonly Short3 s_zero = new Short3(0, 0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Short3 one
        {
            get { return s_one; }
        }
        private static readonly Short3 s_one = new Short3(1, 1, 1);
        /// <summary>
        /// back 向量
        /// </summary>
        public static Short3 down
        {
            get { return s_down; }
        }
        private static readonly Short3 s_down = new Short3(0, -1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Short3 up
        {
            get { return s_up; }
        }
        private static readonly Short3 s_up = new Short3(0, 1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Short3 left
        {
            get { return s_left; }
        }
        private static readonly Short3 s_left = new Short3(-1, 0, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Short3 right
        {
            get { return s_right; }
        }
        private static readonly Short3 s_right = new Short3(1, 0, 0);
        /// <summary>
        /// back 向量
        /// </summary>
        public static Short3 back
        {
            get { return s_back; }
        }
        private static readonly Short3 s_back = new Short3(0, 0, -1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Short3 foward
        {
            get { return s_foward; }
        }
        private static readonly Short3 s_foward = new Short3(0, 0, 1);
        /// <summary>
        /// ！=
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Short3 v1, Short3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
        }
        /// <summary>
        /// ==
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Short3 v1, Short3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }
        /// <summary>
        /// > 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >(Short3 v1, Short3 v2)
        {
            return v1.x > v2.x && v1.y > v2.y && v1.z > v2.z;
        }
        /// <summary>
        /// < 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <(Short3 v1, Short3 v2)
        {
            return v1.x < v2.x && v1.y < v2.y && v1.z < v2.z;
        }
        /// <summary>
        /// >= 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >=(Short3 v1, Short3 v2)
        {
            return v1.x >= v2.x && v1.y >= v2.y && v1.z >= v2.z;
        }
        /// <summary>
        /// <= 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <=(Short3 v1, Short3 v2)
        {
            return v1.x <= v2.x && v1.y <= v2.y && v1.z <= v2.z;
        }
        /// <summary>
        /// +
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Short3 operator +(Short3 v1, Short3 v2)
        {
            int xx = v1.x + v2.x;
            int yy = v1.y + v2.y;
            int zz = v1.z + v2.z;
            return new Short3(xx, yy, zz);
        }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Short3 operator -(Short3 v1, Short3 v2)
        {
            int xx = v1.x - v2.x;
            int yy = v1.y - v2.y;
            int zz = v1.z - v2.z;
            return new Short3(xx, yy, zz);
        }
        /// <summary>
        /// 负号重载
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Short3 operator -(Short3 v1)
        {
            int xx = -v1.x;
            int yy = -v1.y;
            int zz = -v1.z;
            return new Short3(xx, yy, zz);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Short3 operator *(int k, Short3 v)
        {
            int xx = v.x * k;
            int yy = v.y * k;
            int zz = v.z * k;
            return new Short3(xx, yy, zz);
        }
        /// <summary>
        ///  * 运算
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Short3 operator *(Short3 v, int k)
        {
            int xx = v.x * k;
            int yy = v.y * k;
            int zz = v.z * k;
            return new Short3(xx, yy, zz);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Short3 p = (Short3)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) && (z == p.z);
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
