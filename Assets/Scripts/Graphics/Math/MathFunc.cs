namespace RayGraphics.Math
{
    public class MathFunc 
    {
        /// <summary>
        /// Բ�ܻ�����
        /// </summary>
        private static readonly int kMaxCircleAngle = 512;
        /// <summary>
        /// Բ�ܻ��ְ���
        /// </summary>
        private static readonly int kHalfMaxCircleAngle = (kMaxCircleAngle / 2);
        /// <summary>
        /// Բ�ܻ����ķ���
        /// </summary>
        private static readonly int kQuarterMaxCircleAngle = (kHalfMaxCircleAngle / 4);
        /// <summary>
        /// ���������
        /// </summary>
        private static readonly int kMaskMaxCircleAngle = (kMaxCircleAngle - 1);
        /// <summary>
        /// ÿһ��������Ļ���
        /// </summary>
        private static readonly double kpAngle = MathUtil.kPI / kHalfMaxCircleAngle;
        /// <summary>
        /// ÿһ��������Ļ��ȵ���
        /// </summary>
        private static readonly double ktpAngle = kHalfMaxCircleAngle / MathUtil.kPI;
        /// <summary>
        /// Declare table of fast cosinus and sinus
        /// </summary>
        public static double[] fast_cossin_table = new double[kMaxCircleAngle];
        /// <summary>
        /// ��ȡCos ����
        /// </summary>
        /// <param name="angle">������</param>
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
        /// ��ȡsin ����
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
        /// ���ɱ�
        /// </summary>
        public static void MakeTable()
        {
            for (int i = 0; i < kMaxCircleAngle; i++)
            {
                fast_cossin_table[i] = System.Math.Sin(i * kpAngle);
            }
        }
        /// <summary>
        /// ����������
        /// </summary>
        public static void ClearTable()
        {
            fast_cossin_table = null;
        }
        /// <summary>
        /// ����cos angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double cosAngle(double angle)
        {
            return System.Math.Cos(angle);
        }
        /// <summary>
        /// ����sin angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double sinAngle(double angle)
        {
            return System.Math.Sin(angle);
        }
        /// <summary>
        /// ��angle��[-PI,  PI]
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

            if (sinValue == 0) // ����
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
