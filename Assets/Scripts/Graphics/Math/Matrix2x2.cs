using System;

/// <summary>
/// 矩阵
/// </summary>
namespace RayGraphics.Math
{
    public struct Matrix2x2
    {
        /// <summary>
        /// 第一行
        /// </summary>
        public Float2 col1;
        /// <summary>
        /// 第二行
        /// </summary>
        public Float2 col2;
        /// <summary>
        /// 第一列
        /// </summary>
        public Float2 row1
        {
            get { return new Float2(col1.x, col2.x); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Float2 row2
        {
            get { return new Float2(col1.y, col2.y); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        public Matrix2x2(Float2 col1, Float2 col2)
        {
            this.col1 = col1;
            this.col2 = col2;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(Float2 col1, Float2 col2)
        {
            this.col1 = col1;
            this.col2 = col2;
        }

        /// <summary>
        /// 0 矩阵
        /// </summary>
        public static Matrix2x2 zero
        {
            get { return s_zero; }
        }
        private static readonly Matrix2x2 s_zero = new Matrix2x2(Float2.zero, Float2.zero);
        /// <summary>
        /// 单位矩阵
        /// </summary>
        public static Matrix2x2 one
        {
            get { return s_one; }
        }
        private static readonly Matrix2x2 s_one = new Matrix2x2(Float2.right, Float2.up);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrix2x2 RotateMatrix(double angle)
        {
            Float2 col1 = new Float2(System.Math.Cos(angle), System.Math.Sin(angle));
            Float2 col2 = new Float2(-System.Math.Sin(angle), System.Math.Cos(angle));
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <returns></returns>
        public static Matrix2x2 ScaleMatrix(float scaleX, float scaleY)
        {
            Float2 col1 = new Float2(scaleX, 0);
            Float2 col2 = new Float2(0, scaleY);
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix2x2 ScaleMatrix(float scale)
        {
            Float2 col1 = new Float2(scale, 0);
            Float2 col2 = new Float2(0, scale);
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 矩阵判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix2x2 v1, Matrix2x2 v2)
        {
            return v1.col1 != v2.col1 || v1.col2 != v2.col2;
        }
        /// <summary>
        /// 矩阵判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix2x2 v1, Matrix2x2 v2)
        {
            return v1.col1 == v2.col1 && v1.col2 == v2.col2;
        }
        /// <summary>
        /// 矩阵+
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix2x2 operator +(Matrix2x2 v1, Matrix2x2 v2)
        {
            Float2 m1 = v1.col1 + v2.col1;
            Float2 m2 = v1.col2 + v2.col2;
            return new Matrix2x2(m1, m2);
        }
        /// <summary>
        /// 矩阵-
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix2x2 operator -(Matrix2x2 v1, Matrix2x2 v2)
        {
            Float2 m1 = v1.col1 - v2.col1;
            Float2 m2 = v1.col2 - v2.col2;
            return new Matrix2x2(m1, m2);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix2x2 operator *(float k, Matrix2x2 v)
        {
            Float2 col1 = v.col1 * k;
            Float2 col2 = v.col2 * k;
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix2x2 operator *(Matrix2x2 v, float k)
        {
            Float2 col1 = v.col1 * k;
            Float2 col2 = v.col2 * k;
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix2x2 operator *(Matrix2x2 m1, Matrix2x2 m2)
        {
            Float2 col1 = new Float2(Float2.Dot(m1.col1, m2.row1), Float2.Dot(m1.col1, m2.row2));
            Float2 col2 = new Float2(Float2.Dot(m1.col2, m2.row1), Float2.Dot(m1.col2, m2.row2));
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Float2 operator *(Matrix2x2 m, Float2 v)
        {
            return new Float2(Float2.Dot(m.col1, v), Float2.Dot(m.col2, v));
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Float2 operator *(Float2 v, Matrix2x2 m)
        {
            return new Float2(Float2.Dot(v, m.row1), Float2.Dot(v, m.row2));
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Matrix2x2 p = (Matrix2x2)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (col1 == p.col1) && (col2 == p.col2);
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
        /// 转置矩阵
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public  Matrix2x2 GetT()
        {
            return new Matrix2x2(this.row1, this.row2);
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix2x2 T(Matrix2x2 v)
        {
            return new Matrix2x2(v.row1, v.row2);
        }
    }
}