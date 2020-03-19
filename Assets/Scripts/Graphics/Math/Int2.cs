

namespace RayGraphics.Math
{
    [System.Serializable]
    public partial struct Int2
    {
        public int x, y;

        public Int2(short x, short y)
        {
            this.x = x;
            this.y = y;
        }
        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
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
                else return 0;
            }
            set
            {
                if (index == 0)
                    this.x = value;
                else if (index == 1)
                    this.y = value;
            }
        }
        /// <summary>
        /// 模的平方
        /// </summary>
        public int sqrMagnitude
        {
            get { return this.x * this.x + this.y * this.y; }
        }
        /// <summary>
        /// 模
        /// </summary>
        public float magnitude
        {
            get { return (float)System.Math.Sqrt(this.sqrMagnitude); }
        }
        /// <summary>
        /// 0 向量
        /// </summary>
        public static Int2 zero
        {
            get { return s_zero; }
        }
        private static readonly Int2 s_zero = new Int2(0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Int2 one
        {
            get { return s_one; }
        }
        private static readonly Int2 s_one = new Int2(1, 1);

        /// <summary>
        /// back 向量
        /// </summary>
        public static Int2 down
        {
            get { return s_down; }
        }
        private static readonly Int2 s_down = new Int2(0, -1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Int2 up
        {
            get { return s_up; }
        }
        private static readonly Int2 s_up = new Int2(0, 1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Int2 left
        {
            get { return s_left; }
        }
        private static readonly Int2 s_left = new Int2(-1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Int2 right
        {
            get { return s_right; }
        }
        private static readonly Int2 s_right = new Int2(0, 1);

        public static bool operator !=(Int2 v1, Int2 v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public static bool operator ==(Int2 v1, Int2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static Int2 operator +(Int2 v1, Int2 v2)
        {
            int xx = v1.x + v2.x;
            int yy = v1.y + v2.y;
            return new Int2(xx, yy);
        }

        public static Int2 operator -(Int2 v1, Int2 v2)
        {
            int xx = v1.x - v2.x;
            int yy = v1.y - v2.y;
            return new Int2(xx, yy);
        }
        /// <summary>
        /// 负号重载
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Int2 operator -(Int2 v1)
        {
            int xx = -v1.x;
            int yy = -v1.y;
            return new Int2(xx, yy);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Int2 operator *(int k, Int2 v)
        {
            int xx = v.x * k;
            int yy = v.y * k;
            return new Int2(xx, yy);
        }
        /// <summary>
        ///  * 运算
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Int2 operator *(Int2 v, int k)
        {
            int xx = v.x * k;
            int yy = v.y * k;
            return new Int2(xx, yy);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Int2 p = (Int2)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// 夹角[0, PI]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Angle(Int2 from, Int2 to)
        {
            float cosAngle = CosAngle(from, to);
            return (float)System.Math.Acos(cosAngle);
        }
        /// <summary>
        /// 求cos Angle
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float CosAngle(Int2 from, Int2 to)
        {
            return Dot(from, to) / (from.magnitude * to.magnitude);
        }
        /// <summary>
        /// 求sin Angle
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float SinAngle(Int2 from, Int2 to)
        {
            return Cross(from, to) / (from.magnitude * to.magnitude);
        }

        /// <summary>
        /// 求距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double Distance(Short2 start, Short2 end)
        {
            return (start - end).magnitude;
        }
        /// <summary>
        /// 向量组成的平行四边形的面积， 已经取绝对值。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Area(Int2 from, Int2 to)
        {
            return System.Math.Abs(Cross(from, to));
        }
        /// <summary>
        /// 共线判断，考虑误差
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckInLine(Int2 a, Int2 b)
        {
            return System.Math.Abs(SinAngle(a, b)) < MathUtil.kEpsilon ? true : false;
        }
        /// <summary>
        /// 垂直判断，考虑误差
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckVertical(Int2 a, Int2 b)
        {
            return System.Math.Abs(CosAngle(a, b)) < MathUtil.kEpsilon ? true : false;
        }
        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Dot(Int2 a, Int2 b) //点积
        {
            return a.x * b.x + a.y * b.y;
        }
        /// <summary>
        /// 叉积 , 可通过float3 的cross 推出，结果实际上就是z分量的值，因为，x，y分量都为0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Cross(Int2 lhs, Int2 rhs)
        {
            return lhs.x * rhs.y - lhs.y * rhs.x;
        }
        /// <summary>
        /// 获取max向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Int2 Max(Int2 a, Int2 b)
        {
            return new Int2(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));
        }
        /// <summary>
        /// 获取Min向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Int2 Min(Int2 a, Int2 b)
        {
            return new Int2(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));
        }
        /// <summary>
        /// 分量分别相乘，得到新的向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Int2 Scale(Int2 a, Int2 b)
        {
            return new Int2(a.x * b.x, a.y * b.y);
        }
    }
}