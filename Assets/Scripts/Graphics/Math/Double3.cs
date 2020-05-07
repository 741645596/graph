using System.Collections;
using System.Collections.Generic;


namespace RayGraphics.Math
{
    public partial struct Double3
    {
        public double x, y, z;
        public Double3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Double3(double x, double y, double z)
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
        /// <param name="z"></param>
        public void Set(double x, double y, double z)
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
        public double sqrMagnitude
        {
            get { return this.x * this.x + this.y * this.y + this.z * this.z; }
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
        public Double3 normalized
        {
            get
            {
                double length = this.magnitude;
                if (length == 0)
                {
                    return new Double3(x, y, z);
                }
                else
                {
                    length = 1 / length;
                    return new Double3(x * length, y * length, z * length);
                }
            }
        }
        /// <summary>
        /// 0 向量
        /// </summary>
        public static Double3 zero
        {
            get { return s_zero; }
        }
        private static readonly Double3 s_zero = new Double3(0, 0, 0);
        /// <summary>
        /// 单位向量
        /// </summary>
        public static Double3 one
        {
            get { return s_one; }
        }
        private static readonly Double3 s_one = new Double3(1, 1, 1);

        /// <summary>
        /// back 向量
        /// </summary>
        public static Double3 back
        {
            get { return s_back; }
        }
        private static readonly Double3 s_back = new Double3(0, 0, -1);
        /// <summary>
        /// back 向量
        /// </summary>
        public static Double3 down
        {
            get { return s_down; }
        }
        private static readonly Double3 s_down = new Double3(0, -1, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Double3 foward
        {
            get { return s_foward; }
        }
        private static readonly Double3 s_foward = new Double3(0, 0, 1);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Double3 left
        {
            get { return s_left; }
        }
        private static readonly Double3 s_left = new Double3(-1, 0, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Double3 right
        {
            get { return s_right; }
        }
        private static readonly Double3 s_right = new Double3(1, 0, 0);
        /// <summary>
        /// foward 向量
        /// </summary>
        public static Double3 up
        {
            get { return s_up; }
        }
        private static readonly Double3 s_up = new Double3(0, 1, 0);
        /// <summary>
        /// 负无穷大
        /// </summary>
        public static Double3 negativeInfinity
        {
            get { return s_negativeInfinity; }
        }
        private static readonly Double3 s_negativeInfinity = new Double3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

        /// <summary>
        /// 无穷大
        /// </summary>
        public static Double3 positiveInfinity
        {
            get { return s_positiveInfinity; }
        }
        private static readonly Double3 s_positiveInfinity = new Double3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Double3 v1, Double3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
        }
        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Double3 v1, Double3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }
        /// <summary>
        /// + 运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Double3 operator +(Double3 v1, Double3 v2)
        {
            double xx = v1.x + v2.x;
            double yy = v1.y + v2.y;
            double zz = v1.z + v2.z;
            return new Double3(xx, yy, zz);
        }
        /// <summary>
        /// - 运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Double3 operator -(Double3 v1, Double3 v2)
        {
            double xx = v1.x - v2.x;
            double yy = v1.y - v2.y;
            double zz = v1.z - v2.z;
            return new Double3(xx, yy, zz);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Double3 operator *(double k, Double3 v)
        {
            double xx = v.x * k;
            double yy = v.y * k;
            double zz = v.z * k;
            return new Double3(xx, yy, zz);
        }
        /// <summary>
        /// ×运算
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Double3 operator *(Double3 v, double k)
        {
            double xx = v.x * k;
            double yy = v.y * k;
            double zz = v.z * k;
            return new Double3(xx, yy, zz);
        }
        /// <summary>
        /// ×运算
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Double3 operator /(Double3 v, double k)
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
        /// 相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Double3 p = (Double3)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) && (z == p.z);
        }
        /// <summary>
        /// hash code
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
        public static double Angle(Double3 from, Double3 to)
        {
            double cosAngle = Double3.Dot(from, to) / (from.magnitude * to.magnitude);
            return System.Math.Acos(cosAngle);
        }
        /// <summary>
        /// 保持向量方向，调整向量长度
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double3 ClampMagnitude(Double3 a, double maxLength)
        {
            return a.normalized * maxLength;
        }
        /// <summary>
        /// 叉积 Cross Product of two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double3 Cross(Double3 lhs, Double3 rhs)
        {
            return new Double3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }
        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Dot(Double3 a, Double3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        /// <summary>
        /// 求距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double Distance(Double3 start, Double3 end)
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
        public static Double3 Lerp(Double3 start, Double3 end, double t)
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
        public static Double3 LerpUnclamped(Double3 start, Double3 end, double t)
        {
            return start + (end - start) * t;
        }
        /// <summary>
        /// 获取max向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double3 Max(Double3 a, Double3 b)
        {
            return new Double3(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y), System.Math.Max(a.z, b.z));
        }
        /// <summary>
        /// 获取Min向量，计算包围盒有用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double3 Min(Double3 a, Double3 b)
        {
            return new Double3(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y), System.Math.Min(a.z, b.z));
        }
        /// <summary>
        /// 朝目标向量移动maxDistanceDelta距离,未考虑移出。
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Double3 MoveTowards(Double3 current, Double3 target, double maxDistanceDelta)
        {
            return current + (target - current).normalized * maxDistanceDelta;
        }
        /// <summary>
        /// vector 在 onNormal的投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static Double3 Project(Double3 vector, Double3 onNormal)
        {
            Double3 normal = onNormal.normalized;
            return Double3.Dot(vector, normal) * normal;
        }
        /// <summary>
        /// 求向量vector 在平面上的投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="planeNormal">平面法向量</param>
        /// <returns></returns>
        public static Double3 ProjectOnPlane(Double3 vector, Double3 planeNormal)
        {
            return vector - Double3.Project(vector, planeNormal);
        }
        /// <summary>
        /// 求发射光线
        /// </summary>
        /// <param name="inDirection">入射光线</param>
        /// <param name="inNormal">法线</param>
        /// <returns></returns>
        public static Double3 Reflect(Double3 inDirection, Double3 inNormal)
        {
            return inDirection - Double3.Project(inDirection, inNormal) * 2;
        }
        /// <summary>
        /// 分量分别相乘，得到新的向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Double3 Scale(Double3 a, Double3 b)
        {
            return new Double3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        /// <summary>
        /// 求角度
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="axis">from, to 的垂直轴</param>
        /// <returns></returns>
        public static double SignedAngle(Double3 from, Double3 to, Double3 axis)
        {
            return 0;
        }
        /// <summary>
        /// 绕目标轴旋转
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxRadiansDelta"></param>
        /// <param name="maxMagnitudeDelta"></param>
        /// <returns></returns>
        public static Double3 RotateTowards(Double3 current, Double3 target, double maxRadiansDelta, double maxMagnitudeDelta)
        {
            return Double3.zero;
        }
        /// <summary>
        /// Spherically interpolates between two vectors 向量球面插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Double3 Slerp(Double3 a, Double3 b, double t)
        {
            return Double3.zero;
        }
        /// <summary>
        /// Spherically interpolates between two vectors. 向量球面插值，非限制
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Double3 SlerpUnclamped(Double3 a, Double3 b, double t)
        {
            return Double3.zero;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Double3 SmoothDamp(Double3 current, Double3 target, ref Double3 currentVelocity, double smoothTime, double maxSpeed, double deltaTime)
        {
            return Double3.zero;
        }
#if Client
        /// <summary>
        /// 转vector3
        /// </summary>
        public UnityEngine.Vector3 V3
        {
            get { return new UnityEngine.Vector3((float)this.x, (float)this.y, (float)this.z); }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return "x:" + x + "y:" + y + "x:" + z;
        }
        /// <summary>
        /// DrawGizmos
        /// </summary>
        public void DrawGizmos()
        {
            UnityEngine.Gizmos.color = UnityEngine.Color.red;
            UnityEngine.Gizmos.DrawSphere(this.V3, 0.25f);
        }
#endif
    }
}