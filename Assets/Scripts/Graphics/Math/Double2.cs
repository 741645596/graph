
/// <summary>
/// 平面向量
/// </summary>
namespace RayGraphics.Math
{
    [System.Serializable]
    public partial struct Double2
    {
        public double x, y;

        public Double2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Double2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(double x, double y)
        {
            this.x = x;
            this.y = y;
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
        /// 单位向量
        /// </summary>
        public Double2 normalized
        {
            get
            {
                double length = this.magnitude;
                if (length == 0)
                {
                    return new Double2(x, y);
                }
                else
                {
                    length = 1 / length;
                    return new Double2(x * length, y * length);
                }
            }
        }
        /// <summary>
        /// 0 向量
        /// </summary>
        public static Double2 zero
        {
            get { return s_zero; }
        }
        private static readonly Double2 s_zero = new Double2(0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Double2 one
        {
            get { return s_one; }
        }
        private static readonly Double2 s_one = new Double2(1, 1);

        /// <summary>
        /// back 向量
        /// </summary>
        public static Double2 down
        {
            get { return s_down; }
        }
        private static readonly Double2 s_down = new Double2(0, -1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Double2 up
        {
            get { return s_up; }
        }
        private static readonly Double2 s_up = new Double2(0, 1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Double2 left
        {
            get { return s_left; }
        }
        private static readonly Double2 s_left = new Double2(-1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Double2 right
        {
            get { return s_right; }
        }
        private static readonly Double2 s_right = new Double2(1, 0);
        /// <summary>
        /// 负无穷大
        /// </summary>
        public static Double2 negativeInfinity
        {
            get { return s_negativeInfinity; }
        }
        private static readonly Double2 s_negativeInfinity = new Double2(double.NegativeInfinity, double.NegativeInfinity);

        /// <summary>
        /// 无穷大
        /// </summary>
        public static Double2 positiveInfinity
        {
            get { return s_positiveInfinity; }
        }
        private static readonly Double2 s_positiveInfinity = new Double2(double.PositiveInfinity, double.PositiveInfinity);
        /// <summary>
        /// ！=
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Double2 v1, Double2 v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }
        /// <summary>
        /// ==
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Double2 v1, Double2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }
        /// <summary>
        /// <
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <(Double2 v1, Double2 v2)
        {
            return v1.x < v2.x && v1.y < v2.y;
        }
        /// <summary>
        /// <=
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <=(Double2 v1, Double2 v2)
        {
            return v1.x <= v2.x && v1.y <= v2.y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >(Double2 v1, Double2 v2)
        {
            return v1.x > v2.x && v1.y > v2.y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >=(Double2 v1, Double2 v2)
        {
            return v1.x >= v2.x && v1.y >= v2.y;
        }
        /// <summary>
        /// +
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Double2 operator +(Double2 v1, Double2 v2)
        {
            double xx = v1.x + v2.x;
            double yy = v1.y + v2.y;
            return new Double2(xx, yy);
        }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Double2 operator -(Double2 v1, Double2 v2)
        {
            double xx = v1.x - v2.x;
            double yy = v1.y - v2.y;
            return new Double2(xx, yy);
        }
        /// <summary>
        /// 负号重载
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Double2 operator -(Double2 v1)
        {
            double xx = -v1.x;
            double yy = -v1.y;
            return new Double2(xx, yy);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Double2 operator *(double k, Double2 v)
        {
            double xx = v.x * k;
            double yy = v.y * k;
            return new Double2(xx, yy);
        }
        /// <summary>
        ///  * 运算
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Double2 operator *(Double2 v, double k)
        {
            double xx = v.x * k;
            double yy = v.y * k;
            return new Double2(xx, yy);
        }

        /// <summary>
        /// /运算
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Double2 operator /(Double2 v, double k)
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

            Double2 p = (Double2)obj;
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
        /// <summary>
        /// 夹角[0, PI]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Angle(Double2 from, Double2 to)
        {
            double cosAngle = CosAngle(from, to);
            return System.Math.Acos(cosAngle);
        }
        /// <summary>
        /// 求cos Angle
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double CosAngle(Double2 from, Double2 to)
        {
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
            return dot;
        }
        /// <summary>
        /// 求sin Angle
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double SinAngle(Double2 from, Double2 to)
        {
            double cross = Cross(from, to);
            double sqrCross = (cross * cross) / (from.sqrMagnitude * to.sqrMagnitude);
            if (cross > 0)
            {
                cross = System.Math.Sqrt(sqrCross);
                cross = System.Math.Min(cross, 1.0f);
            }
            else
            {
                cross = -System.Math.Sqrt(sqrCross);
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
        public static double Area(Double2 from, Double2 to)
        {
            return System.Math.Abs(Cross(from, to));
        }
        /// <summary>
        /// 共线判断，考虑误差
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckInLine(Double2 a, Double2 b)
        {
            return System.Math.Abs(Cross(a, b)) < MathUtil.kEpsilon ? true : false;
        }
        /// <summary>
        /// 垂直判断，考虑误差
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckVertical(Double2 a, Double2 b)
        {
            return System.Math.Abs(Dot(a, b)) < MathUtil.kEpsilon ? true : false;
        }
        /// <summary>
        /// 保持向量方向，调整向量长度
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double2 ClampMagnitude(Double2 a, double maxLength)
        {
            return a.normalized * maxLength;
        }
        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Dot(Double2 a, Double2 b) //点积
        {
            return a.x * b.x + a.y * b.y;
        }
        /// <summary>
        /// 叉积 , 可通过float3 的cross 推出，结果实际上就是z分量的值，因为，x，y分量都为0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Cross(Double2 a, Double2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
        /// <summary>
        /// 求距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double Distance(Double2 start, Double2 end)
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
        public static Double2 Lerp(Double2 start, Double2 end, double t)
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
        public static Double2 LerpUnclamped(Double2 start, Double2 end, double t)
        {
            return start + (end - start) * t;
        }
        /// <summary>
        /// 获取max向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double2 Max(Double2 a, Double2 b)
        {
            return new Double2(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));
        }
        /// <summary>
        /// 获取Min向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double2 Min(Double2 a, Double2 b)
        {
            return new Double2(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));
        }
        /// <summary>
        /// 朝目标向量移动maxDistanceDelta距离,未考虑移出。
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Double2 MoveTowards(Double2 current, Double2 target, double maxDistanceDelta)
        {
            return current + (target - current).normalized * maxDistanceDelta;
        }
        /// <summary>
        /// vector 在 onNormal的投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static Double2 Project(Double2 vector, Double2 onNormal)
        {
            Double2 normal = onNormal.normalized;
            return Double2.Dot(vector, normal) * normal;
        }
        /// <summary>
        /// 返回逆时针旋转90度的向量
        /// </summary>
        /// <param name="inDirection"></param>
        /// <returns></returns>
        public static Double2 Perpendicular(Double2 inDirection)
        {
            return Matrix2x2.RotateMatrix(MathUtil.kPI / 2) * inDirection;
        }
        /// <summary>
        /// 求发射光线
        /// </summary>
        /// <param name="inDirection">入射光线</param>
        /// <param name="inNormal">法线</param>
        /// <returns></returns>
        public static Double2 Reflect(Double2 inDirection, Double2 inNormal)
        {
            return inDirection - Double2.Project(inDirection, inNormal) * 2;
        }
        /// <summary>
        /// 分量分别相乘，得到新的向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double2 Scale(Double2 a, Double2 b)
        {
            return new Double2(a.x * b.x, a.y * b.y);
        }
        /// <summary>
        /// 向量旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Double2 Rotate(Double2 a, double angle)
        {
            return Matrix2x2.RotateMatrix(angle) * a;
        }
        /// <summary>
        /// 求角度,逆时针为正， 瞬时值为负。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double SignedAngle(Double2 from, Double2 to)
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
                    return System.Math.Asin(sinValue);
                }
                else  // 2
                {
                    return System.Math.Acos(dot);
                }
            }
            else
            {
                if (dot >= 0) // 4
                {
                    return System.Math.Asin(sinValue);
                }
                else  // 3
                {
                    return -System.Math.Acos(dot);
                }
            }
        }
        public static Double2 SmoothDamp(Double2 current, Double2 target, ref Double2 currentVelocity, double smoothTime, double maxSpeed, double deltaTime)
        {
            return Double2.zero;
        }


        /// <summary>
        /// 判断目标点，在夹角内，
        /// </summary>
        /// <param name="target">目标点</param>
        /// <param name="startPoint">起点</param>
        /// <param name="indir">起点出发的2个边界向量</param>
        /// <param name="outdir">起点出发的2个边界向量</param>
        /// <returns></returns>
        public static bool CheckPointInCorns(Double2 target, Double2 startPoint, Double2 indir, Double2 outdir)
        {
            Double2 diff = target - startPoint;
            if (diff == Double2.zero)
                return false;

            double ret = Cross(outdir, diff) * Cross(indir.normalized, diff);
            if (ret < 0)
            {
                Double2 mid = indir.normalized + outdir.normalized;
                // 添加异常处理,防止在反方向
                if (Dot(diff, mid) < 0)
                    return false;
                else return true;
            }
            else if (ret == 0)
            {
                if (Dot(diff, indir) <= 0)
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
        public static double LeftOf(Double2 p1, Double2 p2, Double2 target)
        {
            return Cross(target - p1, p2 - p1);
        }
#if Client
        /// <summary>
        /// 转vector2
        /// </summary>
        public UnityEngine.Vector3 V2
        {
            get { return new UnityEngine.Vector2((float)this.x, (float)this.y); }
        }
        /// <summary>
        /// 转vector3
        /// </summary>
        public UnityEngine.Vector3 V3
        {
            get { return new UnityEngine.Vector3((float)this.x, (float)this.y, 0); }
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