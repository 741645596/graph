﻿using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 矩阵
/// </summary>
namespace RayGraphics.Math
{
    public struct Matrix4x4
    {
        /// <summary>
        /// 第一行
        /// </summary>
        public Double4 col1;
        /// <summary>
        /// 第二行
        /// </summary>
        public Double4 col2;
        /// <summary>
        /// 第三行
        /// </summary>
        public Double4 col3;
        /// <summary>
        /// 第四行
        /// </summary>
        public Double4 col4;
        /// <summary>
        /// 
        /// </summary>
        public Double4 row1
        {
            get { return new Double4(col1.x, col2.x, col3.x, col4.x); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Double4 row2
        {
            get { return new Double4(col1.y, col2.y, col3.y, col4.y); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Double4 row3
        {
            get { return new Double4(col1.z, col2.z, col3.z, col4.z); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Double4 row4
        {
            get { return new Double4(col1.w, col2.w, col3.w, col4.w); }
        }
        /// <summary>
        /// 索引访问
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public double this[int col, int row]
        {
            get
            {
                if (col == 0)
                    return this.col1[row];
                else if (col == 1)
                    return this.col2[row];
                else if (col == 2)
                    return this.col3[row];
                else if (col == 3)
                    return this.col4[row];
                else return 0;
            }
            set
            {
                if (col == 0)
                    this.col1[row] = value;
                else if (col == 1)
                    this.col2[row] = value;
                else if (col == 2)
                    this.col3[row] = value;
                else if (col == 3)
                    this.col4[row] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        public Matrix4x4(Double4 col1, Double4 col2, Double4 col3, Double4 col4)
        {
            this.col1 = col1;
            this.col2 = col2;
            this.col3 = col3;
            this.col4 = col4;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <param name="col3"></param>
        public void Set(Double4 col1, Double4 col2, Double4 col3, Double4 col4)
        {
            this.col1 = col1;
            this.col2 = col2;
            this.col3 = col3;
            this.col4 = col4;
        }

        /// <summary>
        /// 0 矩阵
        /// </summary>
        public static Matrix4x4 zero
        {
            get { return s_zero; }
        }
        private static readonly Matrix4x4 s_zero = new Matrix4x4(Double4.zero, Double4.zero, Double4.zero, Double4.zero);
        /// <summary>
        /// 单位矩阵
        /// </summary>
        public static Matrix4x4 one
        {
            get { return s_one; }
        }
        private static readonly Matrix4x4 s_one = new Matrix4x4(new Double4(1,0,0,0), new Double4(0, 1, 0, 0), new Double4(0, 0, 1, 0), new Double4(0, 0, 0, 1));
        /// <summary>
        /// 矩阵判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix4x4 v1, Matrix4x4 v2)
        {
            return v1.col1 != v2.col1 || v1.col2 != v2.col2 || v1.col3 != v2.col3 || v1.col4 != v2.col4;
        }
        /// <summary>
        /// 矩阵判断相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix4x4 v1, Matrix4x4 v2)
        {
            return v1.col1 == v2.col1 && v1.col2 == v2.col2 && v1.col3 == v2.col3 && v1.col4 == v2.col4;
        }
        /// <summary>
        /// 矩阵+
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix4x4 operator +(Matrix4x4 v1, Matrix4x4 v2)
        {
            Double4 m1 = v1.col1 + v2.col1;
            Double4 m2 = v1.col2 + v2.col2;
            Double4 m3 = v1.col3 + v2.col3;
            Double4 m4 = v1.col4 + v2.col4;
            return new Matrix4x4(m1, m2, m3, m4);
        }
        /// <summary>
        /// 矩阵-
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix4x4 operator -(Matrix4x4 v1, Matrix4x4 v2)
        {
            Double4 m1 = v1.col1 - v2.col1;
            Double4 m2 = v1.col2 - v2.col2;
            Double4 m3 = v1.col3 - v2.col3;
            Double4 m4 = v1.col4 - v2.col4;
            return new Matrix4x4(m1, m2, m3, m4);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix4x4 operator *(double k, Matrix4x4 v)
        {
            Double4 m1 = v.col1 * k;
            Double4 m2 = v.col2 * k;
            Double4 m3 = v.col3 * k;
            Double4 m4 = v.col4 * k;
            return new Matrix4x4(m1, m2, m3, m4);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix4x4 operator *(Matrix4x4 v, double k)
        {
            Double4 m1 = v.col1 * k;
            Double4 m2 = v.col2 * k;
            Double4 m3 = v.col3 * k;
            Double4 m4 = v.col4 * k;
            return new Matrix4x4(m1, m2, m3, m4);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            Double4 col1 = new Double4(Double4.Dot(m1.col1, m2.row1), Double4.Dot(m1.col1, m2.row2), Double4.Dot(m1.col1, m2.row3), Double4.Dot(m1.col1, m2.row4));
            Double4 col2 = new Double4(Double4.Dot(m1.col2, m2.row1), Double4.Dot(m1.col2, m2.row2), Double4.Dot(m1.col2, m2.row3), Double4.Dot(m1.col2, m2.row4));
            Double4 col3 = new Double4(Double4.Dot(m1.col3, m2.row1), Double4.Dot(m1.col3, m2.row2), Double4.Dot(m1.col3, m2.row3), Double4.Dot(m1.col3, m2.row4));
            Double4 col4 = new Double4(Double4.Dot(m1.col4, m2.row1), Double4.Dot(m1.col4, m2.row2), Double4.Dot(m1.col4, m2.row3), Double4.Dot(m1.col4, m2.row4));
            return new Matrix4x4(col1, col2, col3, col4);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Double4 operator *(Matrix4x4 m, Double4 v)
        {
            return new Double4(Double4.Dot(m.col1, v), Double4.Dot(m.col2, v), Double4.Dot(m.col3, v), Double4.Dot(m.col4, v));
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Double4 operator *(Double4 v, Matrix4x4 m)
        {
            return new Double4(Double4.Dot(v, m.row1), Double4.Dot(v, m.row2), Double4.Dot(v, m.row3), Double4.Dot(v, m.row4));
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

            Matrix4x4 p = (Matrix4x4)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (col1 == p.col1) && (col2 == p.col2) && (col3 == p.col3) && (col4 == p.col4);
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
        public Matrix4x4 GetT()
        {
            return new Matrix4x4(this.row1, this.row2, this.row3, this.row4);
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix4x4 T(Matrix4x4 v)
        {
            return new Matrix4x4(v.row1, v.row2, v.row3, v.row4);
        }
    }
}