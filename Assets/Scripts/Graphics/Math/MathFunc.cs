namespace RayGraphics.Math
{
    public class MathFunc 
    {
        /// <summary>
        /// 圆周划分数
        /// </summary>
        private static readonly int kMaxCircleAngle = 512;
        /// <summary>
        /// 圆周划分半数
        /// </summary>
        private static readonly int kHalfMaxCircleAngle = (kMaxCircleAngle / 2);
        /// <summary>
        /// 圆周划分四分数
        /// </summary>
        private static readonly int kQuarterMaxCircleAngle = (kHalfMaxCircleAngle / 4);
        /// <summary>
        /// 最大索引数
        /// </summary>
        private static readonly int kMaskMaxCircleAngle = (kMaxCircleAngle - 1);
        /// <summary>
        /// 每一份所代表的弧度
        /// </summary>
        private static readonly double kpAngle = MathUtil.kPI / kHalfMaxCircleAngle;
        /// <summary>
        /// 每一份所代表的弧度倒数
        /// </summary>
        private static readonly double ktpAngle = kHalfMaxCircleAngle / MathUtil.kPI;
        /// <summary>
        /// Declare table of fast cosinus and sinus
        /// </summary>
        public static double[] fast_cossin_table = new double[kMaxCircleAngle];
        /// <summary>
        /// 获取Cos 函数
        /// </summary>
        /// <param name="angle">弧度制</param>
        /// <returns></returns>
        public static double Cos(float angle)
        {
            int index = (int)(angle * ktpAngle);
            if (index < 0)
            {
                return fast_cossin_table[((-index) + kQuarterMaxCircleAngle) & kMaskMaxCircleAngle];
            }
            else
            {
                return fast_cossin_table[(index + kQuarterMaxCircleAngle) & kMaskMaxCircleAngle];
            }
        }
        /// <summary>
        /// 获取sin 函数
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double fastsin(float angle)
        {
            int index = (int)(angle * ktpAngle);
            if (index < 0)
            {
                return fast_cossin_table[(-((-index) & kMaskMaxCircleAngle)) + kMaxCircleAngle];
            }
            else
            {
                return fast_cossin_table[index & kMaskMaxCircleAngle];
            }
        }
        /// <summary>
        /// 生成表
        /// </summary>
        public static void MakeTable()
        {
            for (int i = 0; i < kMaxCircleAngle; i++)
            {
                fast_cossin_table[i] = System.Math.Sin(i * kpAngle);
            }
        }
        /// <summary>
        /// 清理表格数据
        /// </summary>
        public static void ClearTable()
        {
            fast_cossin_table = null;
        }
        /// <summary>
        /// 计算cos angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double cosAngle(double angle)
        {
            return System.Math.Cos(angle);
        }
        /// <summary>
        /// 计算sin angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double sinAngle(double angle)
        {
            return System.Math.Sin(angle);
        }
        /// <summary>
        /// 求angle，[-PI,  PI]
        /// </summary>
        /// <param name="cosValue"></param>
        /// <param name="sinValue"></param>
        /// <returns></returns>
        public static double GetAngle(double cosValue, double sinValue)
        {
            // [-PI / 2,  PI /2]  Asin
            // [0,  PI]  Acos
            sinValue = System.Math.Min(sinValue, 1.0f);
            sinValue = System.Math.Max(sinValue, -1.0f);
            cosValue = System.Math.Min(cosValue, 1.0f);
            cosValue = System.Math.Max(cosValue, -1.0f);

            if (sinValue == 0) // 共线
            {
                return cosValue >= 0 ? 0 : MathUtil.kPI;
            }
            else if (sinValue > 0)
            {
                return System.Math.Acos(cosValue);
            }
            else
            {
                return -System.Math.Acos(cosValue);
            }
        }
    }
}
