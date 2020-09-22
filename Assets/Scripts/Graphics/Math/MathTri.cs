using System;
/// <summary>
/// 使用查表三角函数
/// </summary>
namespace RayGraphics.Math
{
    public class MathTri
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
        private static readonly int kQuarterMaxCircleAngle  = (kHalfMaxCircleAngle / 4);
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
        private static readonly double ktpAngle =  kHalfMaxCircleAngle/ MathUtil.kPI;
        /// <summary>
        /// Declare table of fast cosinus and sinus
        /// </summary>
        public static  double[] fast_cossin_table = new double[kMaxCircleAngle];
        /// <summary>
        /// 获取Cos 函数
        /// </summary>
        /// <param name="angle">弧度制</param>
        /// <returns></returns>
        public static double Cos(float angle)
        {
            int index  = (int)(angle * ktpAngle);
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
                fast_cossin_table[i] =  System.Math.Sin(i * kpAngle);
            }
        }
        /// <summary>
        /// 清理表格数据
        /// </summary>
        public static void ClearTable()
        {
            fast_cossin_table = null;
        }

    }
}
