/// <summary>
/// 平面向量
/// </summary>
namespace Graphics.Math
{
    [System.Serializable]
    public partial struct Float2
    {
        public float x, y;

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Float2(double x, double y)
        {
            this.x = (float)x;
            this.y = (float)y;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
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
        public float sqrMagnitude
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
        /// 单位向量
        /// </summary>
        public Float2 normalized
        {
            get
            {
                float length = this.magnitude;
                if (length == 0)
                {
                    return new Float2(x, y);
                }
                else
                {
                    length = 1 / length;
                    return new Float2(x * length, y * length);
                }
            }
        }
        /// <summary>
        /// 0 向量
        /// </summary>
        public static Float2 zero
        {
            get { return s_zero; }
        }
        private static readonly Float2 s_zero = new Float2(0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Float2 one
        {
            get { return s_one; }
        }
        private static readonly Float2 s_one = new Float2(1, 1);

        /// <summary>
        /// back 向量
        /// </summary>
        public static Float2 down
        {
            get { return s_down; }
        }
        private static readonly Float2 s_down = new Float2(0, -1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Float2 up
        {
            get { return s_up; }
        }
        private static readonly Float2 s_up = new Float2(0, 1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Float2 left
        {
            get { return s_left; }
        }
        private static readonly Float2 s_left = new Float2(-1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Float2 right
        {
            get { return s_right; }
        }
        private static readonly Float2 s_right = new Float2(0, 1);
        /// <summary>
        /// 负无穷大
        /// </summary>
        public static Float2 negativeInfinity
        {
            get { return s_negativeInfinity; }
        }
        private static readonly Float2 s_negativeInfinity = new Float2(float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// 无穷大
        /// </summary>
        public static Float2 positiveInfinity
        {
            get { return s_positiveInfinity; }
        }
        private static readonly Float2 s_positiveInfinity = new Float2(float.PositiveInfinity, float.PositiveInfinity);

        public static bool operator !=(Float2 v1, Float2 v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public static bool operator ==(Float2 v1, Float2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static Float2 operator +(Float2 v1, Float2 v2)
        {
            float xx = v1.x + v2.x;
            float yy = v1.y + v2.y;
            return new Float2(xx, yy);
        }

        public static Float2 operator -(Float2 v1, Float2 v2)
        {
            float xx = v1.x - v2.x;
            float yy = v1.y - v2.y;
            return new Float2(xx, yy);
        }
        /// <summary>
        /// 负号重载
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Float2 operator -(Float2 v1)
        {
            float xx = -v1.x;
            float yy = -v1.y;
            return new Float2(xx, yy);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Float2 operator *(float k, Float2 v)
        {
            float xx = v.x * k;
            float yy = v.y * k;
            return new Float2(xx, yy);
        }
        /// <summary>
        ///  * 运算
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Float2 operator *(Float2 v, float k)
        {
            float xx = v.x * k;
            float yy = v.y * k;
            return new Float2(xx, yy);
        }

        /// <summary>
        /// /运算
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Float2 operator /(Float2 v, float k)
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

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Float2 p = (Float2)obj;
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
        public static float Angle(Float2 from, Float2 to)
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
        public static float CosAngle(Float2 from, Float2 to)
        {
            return Float2.Dot(from, to) / (from.magnitude * to.magnitude);
        }
        /// <summary>
        /// 求cos Angle
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float SinAngle(Float2 from, Float2 to)
        {
            return Float2.Cross(from, to) / (from.magnitude * to.magnitude);
        }
        /// <summary>
        /// 向量组成的平行四边形的面积， 已经取绝对值。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Area(Float2 from, Float2 to)
        {
            return System.Math.Abs(Float2.Cross(from, to));
        }
        /// <summary>
        /// 共线判断，考虑误差
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckInLine(Float2 a, Float2 b)
        {
            return System.Math.Abs(Float2.SinAngle(a, b)) < MathUtil.kEpsilon ? true : false;
        }
        /// <summary>
        /// 垂直判断，考虑误差
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckVertical(Float2 a, Float2 b)
        {
            return System.Math.Abs(Float2.CosAngle(a, b)) < MathUtil.kEpsilon ? true : false;
        }
        /// <summary>
        /// 保持向量方向，调整向量长度
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Float2 ClampMagnitude(Float2 a, float maxLength)
        {
            return a.normalized * maxLength;
        }
        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(Float2 a, Float2 b) //点积
        {
            return a.x * b.x + a.y * b.y;
        }
        /// <summary>
        /// 叉积 , 可通过float3 的cross 推出，结果实际上就是z分量的值，因为，x，y分量都为0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Cross(Float2 lhs, Float2 rhs)
        {
            return lhs.x * rhs.y - lhs.y * rhs.x;
        }
        /// <summary>
        /// 求距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double Distance(Float2 start, Float2 end)
        {
            return (start - end).magnitude;
        }
        /// <summary>
        /// 限制性在点start，end 之间插值Linearly interpolates between two points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t">[0,1]</param>
        /// <returns></returns>
        public static Float2 Lerp(Float2 start, Float2 end, float t)
        {
            t = t < 0 ? 0 : t;
            t = t > 1 ? 1 : t;
            return start + (end - start) * t;
        }
        /// <summary>
        /// 非限制性在点start，end 之间插值Linearly interpolates between two points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Float2 LerpUnclamped(Float2 start, Float2 end, float t)
        {
            return start + (end - start) * t;
        }
        /// <summary>
        /// 获取max向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Float2 Max(Float2 a, Float2 b)
        {
            return new Float2(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));
        }
        /// <summary>
        /// 获取Min向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Float2 Min(Float2 a, Float2 b)
        {
            return new Float2(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));
        }
        /// <summary>
        /// 朝目标向量移动maxDistanceDelta距离,未考虑移出。
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Float2 MoveTowards(Float2 current, Float2 target, float maxDistanceDelta)
        {
            return current + (target - current).normalized * maxDistanceDelta;
        }
        /// <summary>
        /// vector 在 onNormal的投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static Float2 Project(Float2 vector, Float2 onNormal)
        {
            Float2 normal = onNormal.normalized;
            return Float2.Dot(vector, normal) * normal;
        }
        /// <summary>
        /// 返回逆时针旋转90度的向量
        /// </summary>
        /// <param name="inDirection"></param>
        /// <returns></returns>
        public static Float2 Perpendicular(Float2 inDirection)
        {
            return inDirection * Matrix2x2.RotateMatrix(MathUtil.kPI / 2);
        }
        /// <summary>
        /// 求发射光线
        /// </summary>
        /// <param name="inDirection">入射光线</param>
        /// <param name="inNormal">法线</param>
        /// <returns></returns>
        public static Float2 Reflect(Float2 inDirection, Float2 inNormal)
        {
            return inDirection - Float2.Project(inDirection, inNormal) * 2;
        }
        /// <summary>
        /// 分量分别相乘，得到新的向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Float2 Scale(Float2 a, Float2 b)
        {
            return new Float2(a.x * b.x, a.y * b.y);
        }
        /// <summary>
        /// 向量旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Float2 Rotate(Float2 a, double angle)
        {
            return a * Matrix2x2.RotateMatrix(angle);
        }
        /// <summary>
        /// 求角度
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="axis">from, to 的垂直轴</param>
        /// <returns></returns>
        public static float SignedAngle(Float2 from, Float2 to)
        {
            return 0;
        }
        public static Float2 SmoothDamp(Float2 current, Float2 target, ref Float2 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            return Float2.zero;
        }
        /// <summary>
        /// 转vector2
        /// </summary>
        public UnityEngine.Vector3 V2
        {
            get { return new UnityEngine.Vector2(this.x, this.y); }
        }
        /// <summary>
        /// 转vector3
        /// </summary>
        public UnityEngine.Vector3 V3
        {
            get { return new UnityEngine.Vector3(this.x, this.y, 0); }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return "x:" + x + "y:" + y;
        }
        /// <summary>
        /// DrawGizmos
        /// </summary>
        public void DrawGizmos()
        {
            UnityEngine.Gizmos.color = UnityEngine.Color.red;
            UnityEngine.Gizmos.DrawSphere(this.V3, 0.1f);
        }
    }
}