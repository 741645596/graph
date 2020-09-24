/// <summary>
/// 平面向量
/// </summary>
namespace RayGraphics.Math
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
        private static readonly Float2 s_right = new Float2(1, 0);
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
        /// <summary>
        /// > 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >(Float2 v1, Float2 v2)
        {
            return v1.x > v2.x && v1.y > v2.y;
        }
        /// <summary>
        /// < 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <(Float2 v1, Float2 v2)
        {
            return v1.x < v2.x && v1.y < v2.y;
        }
        /// <summary>
        /// >= 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >=(Float2 v1, Float2 v2)
        {
            return v1.x >= v2.x && v1.y >= v2.y;
        }
        /// <summary>
        /// <= 运算符
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <=(Float2 v1, Float2 v2)
        {
            return v1.x <= v2.x && v1.y <= v2.y ;
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
            float dot = Dot(from, to);
            double sqrDot = (dot * dot) / (from.sqrMagnitude * to.sqrMagnitude);
            if (dot > 0)
            {
                dot = (float)System.Math.Sqrt(sqrDot);
                dot = System.Math.Min(dot, 1.0f);
            }
            else
            {
                dot = -(float)System.Math.Sqrt(sqrDot);
                dot = System.Math.Max(dot, -1.0f);
            }
            return dot;
        }
        /// <summary>
        /// 求sin Angle
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float SinAngle(Float2 from, Float2 to)
        {
            float cross = Cross(from, to);
            double sqrCross = (cross * cross) / (from.sqrMagnitude * to.sqrMagnitude);
            if (cross > 0)
            {
                cross = (float)System.Math.Sqrt(sqrCross);
                cross = System.Math.Min(cross, 1.0f);
            }
            else
            {
                cross = -(float)System.Math.Sqrt(sqrCross);
                cross = System.Math.Max(cross, -1.0f);
            }
            return cross;
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
            return System.Math.Abs(Float2.Cross(a, b)) < MathUtil.kEpsilon ? true : false;
        }
        /// <summary>
        /// 垂直判断，考虑误差
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckVertical(Float2 a, Float2 b)
        {
            return System.Math.Abs(Float2.Dot(a, b)) < MathUtil.kEpsilon ? true : false;
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
        public static float Cross(Float2 a, Float2 b)
        {
            return a.x * b.y - a.y * b.x;
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
            return Dot(vector, normal) * normal;
        }
        /// <summary>
        /// 返回逆时针旋转90度的向量
        /// </summary>
        /// <param name="inDirection"></param>
        /// <returns></returns>
        public static Float2 Perpendicular(Float2 inDirection)
        {
            Double2 ret = Matrix2x2.RotateMatrix(MathUtil.kPI / 2) * new Double2(inDirection.x, inDirection.y);
            return new Float2(ret.x, ret.y);
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
            Double2 ret = Matrix2x2.RotateMatrix(angle) * new Double2(a.x, a.y);
            return new Float2(ret.x, ret.y);
        }
        /// <summary>
        /// 求角度,逆时针为正， 瞬时值为负。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float SignedAngle(Float2 from, Float2 to)
        {
            // [-PI / 2,  PI /2]  Asin
            // [0,  PI]  acos

            double sinValue = SinAngle(from, to);
            double dot = Dot(from, to);
            double sqrDot = (dot * dot) / (from.sqrMagnitude * to.sqrMagnitude);
            if (dot > 0)
            {
                dot = System.Math.Sqrt(sqrDot);
                dot = System.Math.Min(dot, 1.0f);
            }
            else
            {
                dot = -System.Math.Sqrt(sqrDot);
                dot = System.Math.Max(dot, -1.0f);
            }

            if (sinValue == 0) // 共线
            {
                return dot >= 0 ? 0 : MathUtil.kPI;
            }
            else if (sinValue > 0)
            {
                if (dot >= 0) // 1
                {
                    return (float)(System.Math.Asin(sinValue));
                }
                else  // 2
                {
                    return (float)(System.Math.Acos(dot));
                }
            }
            else 
            {
                if ( dot >= 0) // 4
                {
                    return (float)(System.Math.Asin(sinValue));
                }
                else  // 3
                {
                    return -(float)(System.Math.Acos(dot));
                }
            }
        }
        public static Float2 SmoothDamp(Float2 current, Float2 target, ref Float2 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            return Float2.zero;
        }


        /// <summary>
        /// 判断目标点，在夹角内，
        /// </summary>
        /// <param name="target">目标点</param>
        /// <param name="startPoint">起点</param>
        /// <param name="indir">起点出发的2个边界向量</param>
        /// <param name="outdir">起点出发的2个边界向量</param>
        /// <returns></returns>
        public static bool CheckPointInCorns(Float2 target, Float2 startPoint, Float2 indir, Float2 outdir)
        {
            Float2 diff = target - startPoint;
            if (diff == Float2.zero)
                return false;

            float ret = Float2.Cross(outdir, diff) * Float2.Cross(indir.normalized, diff);
            if (ret < 0)
            {
                Float2 mid = indir.normalized + outdir.normalized;
                // 添加异常处理,防止在反方向
                if (Float2.Dot(diff, mid) < 0)
                    return false;
                else return true;
            }
            else if (ret == 0)
            {
                if (Float2.Dot(diff, indir) <= 0)
                    return false;

                if (indir.sqrMagnitude < diff.sqrMagnitude)
                    return true;
                else return false;
            }
            return false;
        }
        /// <summary>
        /// 当点target位于线p1p2的左侧时为负
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="target">目标点</param>
        /// <returns></returns>
        public static float LeftOf(Float2 p1, Float2 p2, Float2 target)
        {
            return Cross(target - p1, p2 - p1);
        }
#if Client
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
#endif
    }
}