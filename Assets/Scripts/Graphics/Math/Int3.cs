
namespace RayGraphics.Math
{
    [System.Serializable]
    public partial struct Int3
    {
        public int x, y, z;

        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(int x, int y, int z)
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
        public int this[int index]
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
        public static Int3 invalid
        {
            get { return s_invalid; }
        }
        private static readonly Int3 s_invalid = new Int3(-1, -1, -1);

        /// <summary>
        /// 0 向量
        /// </summary>
        public static Int3 zero
        {
            get { return s_zero; }
        }
        private static readonly Int3 s_zero = new Int3(0, 0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Int3 one
        {
            get { return s_one; }
        }
        private static readonly Int3 s_one = new Int3(1, 1, 1);

        /// <summary>
        /// back 向量
        /// </summary>
        public static Int3 down
        {
            get { return s_down; }
        }
        private static readonly Int3 s_down = new Int3(0, -1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Int3 up
        {
            get { return s_up; }
        }
        private static readonly Int3 s_up = new Int3(0, 1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Int3 left
        {
            get { return s_left; }
        }
        private static readonly Int3 s_left = new Int3(-1, 0, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Int3 right
        {
            get { return s_right; }
        }
        private static readonly Int3 s_right = new Int3(1, 0, 0);

        /// <summary>
        /// back 向量
        /// </summary>
        public static Int3 back
        {
            get { return s_back; }
        }
        private static readonly Int3 s_back = new Int3(0, 0, -1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Int3 foward
        {
            get { return s_foward; }
        }
        private static readonly Int3 s_foward = new Int3(0, 0, 1);



        public static bool operator !=(Int3 v1, Int3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y|| v1.z != v2.z;
        }

        public static bool operator ==(Int3 v1, Int3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static Int3 operator +(Int3 v1, Int3 v2)
        {
            int xx = v1.x + v2.x;
            int yy = v1.y + v2.y;
            int zz = v1.z + v2.z;
            return new Int3(xx, yy, zz);
        }

        public static Int3 operator -(Int3 v1, Int3 v2)
        {
            int xx = v1.x - v2.x;
            int yy = v1.y - v2.y;
            int zz = v1.z - v2.z;
            return new Int3(xx, yy, zz);
        }
        /// <summary>
        /// 负号重载
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Int3 operator -(Int3 v1)
        {
            int xx = -v1.x;
            int yy = -v1.y;
            int zz = -v1.z;
            return new Int3(xx, yy, zz);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Int3 operator *(int k, Int3 v)
        {
            int xx = v.x * k;
            int yy = v.y * k;
            int zz = v.z * k;
            return new Int3(xx, yy, zz);
        }
        /// <summary>
        ///  * 运算
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Int3 operator *(Int3 v, int k)
        {
            int xx = v.x * k;
            int yy = v.y * k;
            int zz = v.z * k;
            return new Int3(xx, yy, zz);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Int3 p = (Int3)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) && (z == p.z);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
