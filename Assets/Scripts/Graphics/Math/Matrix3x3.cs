using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 矩阵
/// </summary>
namespace Graphics.Math
{
    public  class Matrix3x3
    {
        /// <summary>
        /// 第一行
        /// </summary>
        public Float3 col1;
        /// <summary>
        /// 第二行
        /// </summary>
        public Float3 col2;
        /// <summary>
        /// 第三行
        /// </summary>
        public Float3 col3;
        /// <summary>
        /// 第一列
        /// </summary>
        public Float3 row1
        {
            get { return new Float3(col1.x, col2.x, col3.x); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Float3 row2
        {
            get { return new Float3(col1.y, col2.y, col3.y); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Float3 row3
        {
            get { return new Float3(col1.z, col2.z, col3.z); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        public Matrix3x3(Float3 col1, Float3 col2, Float3 col3)
        {
            this.col1 = col1;
            this.col2 = col2;
            this.col3 = col3;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <param name="col3"></param>
        public void Set(Float3 col1, Float3 col2, Float3 col3)
        {
            this.col1 = col1;
            this.col2 = col2;
            this.col3 = col3;
        }

        /// <summary>
        /// 0 矩阵
        /// </summary>
        public static Matrix3x3 zero
        {
            get { return s_zero; }
        }
        private static readonly Matrix3x3 s_zero = new Matrix3x3(Float3.zero, Float3.zero, Float3.zero);
        /// <summary>
        /// 单位矩阵
        /// </summary>
        public static Matrix3x3 one
        {
            get { return s_one; }
        }
        private static readonly Matrix3x3 s_one = new Matrix3x3(Float3.right, Float3.up, Float3.foward);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrix3x3 RotateZMatrix(double angle)
        {
            Float3 col1 = new Float3(System.Math.Cos(angle), System.Math.Sin(angle), 0);
            Float3 col2 = new Float3(-System.Math.Sin(angle), System.Math.Cos(angle), 0);
            Float3 col3 = new Float3(0, 0, 1);
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 矩阵判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix3x3 v1, Matrix3x3 v2)
        {
            return v1.col1 != v2.col1 || v1.col2 != v2.col2 || v1.col3 != v2.col3;
        }
        /// <summary>
        /// 矩阵判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix3x3 v1, Matrix3x3 v2)
        {
            return v1.col1 == v2.col1 && v1.col2 == v2.col2 && v1.col3 == v2.col3;
        }
        /// <summary>
        /// 矩阵+
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix3x3 operator +(Matrix3x3 v1, Matrix3x3 v2)
        {
            Float3 m1 = v1.col1 + v2.col1;
            Float3 m2 = v1.col2 + v2.col2;
            Float3 m3 = v1.col3 + v2.col3;
            return new Matrix3x3(m1, m2, m3);
        }
        /// <summary>
        /// 矩阵-
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix3x3 operator -(Matrix3x3 v1, Matrix3x3 v2)
        {
            Float3 m1 = v1.col1 - v2.col1;
            Float3 m2 = v1.col2 - v2.col2;
            Float3 m3 = v1.col3 - v2.col3;
            return new Matrix3x3(m1, m2, m3);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix3x3 operator *(float k, Matrix3x3 v)
        {
            Float3 col1 = v.col1 * k;
            Float3 col2 = v.col2 * k;
            Float3 col3 = v.col3 * k;
            return new Matrix3x3(col1, col2,col3);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix3x3 operator *(Matrix3x3 v, float k)
        {
            Float3 col1 = v.col1 * k;
            Float3 col2 = v.col2 * k;
            Float3 col3 = v.col3 * k;
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2)
        {
            Float3 col1 = new Float3(Float3.Dot(m1.col1, m2.row1), Float3.Dot(m1.col1, m2.row2), Float3.Dot(m1.col1, m2.row3));
            Float3 col2 = new Float3(Float3.Dot(m1.col2, m2.row1), Float3.Dot(m1.col2, m2.row2), Float3.Dot(m1.col2, m2.row3));
            Float3 col3 = new Float3(Float3.Dot(m1.col3, m2.row1), Float3.Dot(m1.col3, m2.row2), Float3.Dot(m1.col3, m2.row3));
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Float3 operator *(Matrix3x3 m, Float3 v)
        {
            return new Float3(Float3.Dot(m.col1, v), Float3.Dot(m.col2, v), Float3.Dot(m.col3, v));
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Float3 operator *(Float3 v, Matrix3x3 m)
        {
            return new Float3(Float3.Dot(v, m.row1), Float3.Dot(v, m.row2), Float3.Dot(v, m.row3));
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

            Matrix3x3 p = (Matrix3x3)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (col1 == p.col1) && (col2 == p.col2) && (col3 == p.col3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}