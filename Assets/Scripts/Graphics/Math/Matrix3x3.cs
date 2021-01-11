/// <summary>
/// 矩阵
/// </summary>
namespace RayGraphics.Math
{
    public  class Matrix3x3
    {
        /// <summary>
        /// 第一行
        /// </summary>
        public Double3 col1;
        /// <summary>
        /// 第二行
        /// </summary>
        public Double3 col2;
        /// <summary>
        /// 第三行
        /// </summary>
        public Double3 col3;
        /// <summary>
        /// 第一列
        /// </summary>
        public Double3 row1
        {
            get { return new Double3(col1.x, col2.x, col3.x); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Double3 row2
        {
            get { return new Double3(col1.y, col2.y, col3.y); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Double3 row3
        {
            get { return new Double3(col1.z, col2.z, col3.z); }
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
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        public Matrix3x3(Double3 col1, Double3 col2, Double3 col3)
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
        public void Set(Double3 col1, Double3 col2, Double3 col3)
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
        private static readonly Matrix3x3 s_zero = new Matrix3x3(Double3.zero, Double3.zero, Double3.zero);
        /// <summary>
        /// 单位矩阵
        /// </summary>
        public static Matrix3x3 one
        {
            get { return s_one; }
        }
        private static readonly Matrix3x3 s_one = new Matrix3x3(Double3.right, Double3.up, Double3.foward);
        /// <summary>
        /// 绕Z旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrix3x3 RotateZMatrix(double angle)
        {
            double cosAngle = MathFunc.cosAngle(angle);
            double sinAngle = MathFunc.sinAngle(angle);
            Double3 col1 = new Double3(cosAngle, sinAngle, 0);
            Double3 col2 = new Double3(-sinAngle, cosAngle, 0);
            Double3 col3 = new Double3(0, 0, 1);
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 绕X旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrix3x3 RotateXMatrix(double angle)
        {
            double cosAngle = System.Math.Cos(angle);
            double sinAngle = System.Math.Sin(angle);
            Double3 col1 = new Double3(1, 0, 0);
            Double3 col2 = new Double3(0, cosAngle, sinAngle);
            Double3 col3 = new Double3(0, -sinAngle, cosAngle);
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 绕轴旋转
        /// </summary>
        /// <param name="axis">轴的法向量</param>
        /// <param name="angle">旋转角度</param>
        /// <returns></returns>
        public static Matrix3x3 RotateMatrix(Double3 axis,double angle)
        {
            double cosAngle = MathFunc.cosAngle(angle);
            double sinAngle = MathFunc.sinAngle(angle);
            double nxCosAngleDiff = axis.x * (1- cosAngle);
            double nyCosAngleDiff = axis.y * (1 - cosAngle);
            double nzCosAngleDiff = axis.z * (1 - cosAngle);
            double nxSinAngle = axis.x * sinAngle;
            double nySinAngle = axis.y * sinAngle;
            double nzSinAngle = axis.z * sinAngle;
            Double3 col1 = new Double3(axis.x * nxCosAngleDiff + cosAngle, axis.x * nyCosAngleDiff + nzSinAngle, axis.x * nzCosAngleDiff - nySinAngle);
            Double3 col2 = new Double3(axis.y * nxCosAngleDiff - nzSinAngle, axis.y * nyCosAngleDiff + cosAngle, axis.y * nzCosAngleDiff + nxSinAngle);
            Double3 col3 = new Double3(axis.z * nxCosAngleDiff + nySinAngle, axis.z * nyCosAngleDiff - nxSinAngle, axis.z * nzCosAngleDiff + cosAngle);
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 绕Y旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Matrix3x3 RotateYMatrix(double angle)
        {
            double cosAngle = MathFunc.cosAngle(angle);
            double sinAngle = MathFunc.sinAngle(angle);
            Double3 col1 = new Double3(cosAngle, 0, sinAngle);
            Double3 col2 = new Double3(0, 1, 0);
            Double3 col3 = new Double3(-sinAngle, 0, cosAngle);
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <returns></returns>
        public static Matrix3x3 ScaleMatrix(float scaleX, float scaleY, float scaleZ)
        {
            Double3 col1 = scaleX * Double3.right;
            Double3 col2 = scaleY * Double3.up;
            Double3 col3 = scaleZ * Double3.foward;
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix3x3 ScaleMatrix(float scale)
        {
            Double3 col1 = scale * Double3.right;
            Double3 col2 = scale * Double3.up;
            Double3 col3 = scale * Double3.foward;
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 沿着指定轴进行缩放
        /// </summary>
        /// <param name="axis">指定轴的法向量</param>
        /// <param name="scale">缩放比例</param>
        /// <returns></returns>
        public static Matrix3x3 ScaleMatrix(Double3 axis, float scale)
        {
            double dx = (scale - 1) * axis.x;
            double dy = (scale - 1) * axis.y;
            double dz = (scale - 1) * axis.z;
            Double3 col1 = new Double3(1+ dx * axis.x, dx * axis.y, dx * axis.y);
            Double3 col2 = new Double3(dy * axis.x, 1 + dy * axis.y, dy * axis.z);
            Double3 col3 = new Double3(dz * axis.x, dz * axis.y, 1 + dz * axis.z);
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 投影到XY平面的正交投影矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix3x3 XYOrthogonalProjectionMatrix()
        {
            Double3 col1 = Double3.right;
            Double3 col2 = Double3.up;
            Double3 col3 = Double3.zero;
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 投影到XZ平面的正交投影矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix3x3 XZOrthogonalProjectionMatrix()
        {
            Double3 col1 = Double3.right;
            Double3 col2 = Double3.zero;
            Double3 col3 = Double3.foward;
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// 投影到XZ平面的正交投影矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix3x3 YZOrthogonalProjectionMatrix()
        {
            Double3 col1 = Double3.zero;
            Double3 col2 = Double3.up;
            Double3 col3 = Double3.foward;
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
            Double3 m1 = v1.col1 + v2.col1;
            Double3 m2 = v1.col2 + v2.col2;
            Double3 m3 = v1.col3 + v2.col3;
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
            Double3 m1 = v1.col1 - v2.col1;
            Double3 m2 = v1.col2 - v2.col2;
            Double3 m3 = v1.col3 - v2.col3;
            return new Matrix3x3(m1, m2, m3);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix3x3 operator *(double k, Matrix3x3 v)
        {
            Double3 col1 = v.col1 * k;
            Double3 col2 = v.col2 * k;
            Double3 col3 = v.col3 * k;
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix3x3 operator *(Matrix3x3 v, double k)
        {
            Double3 col1 = v.col1 * k;
            Double3 col2 = v.col2 * k;
            Double3 col3 = v.col3 * k;
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
            Double3 col1 = new Double3(Double3.Dot(m1.col1, m2.row1), Double3.Dot(m1.col1, m2.row2), Double3.Dot(m1.col1, m2.row3));
            Double3 col2 = new Double3(Double3.Dot(m1.col2, m2.row1), Double3.Dot(m1.col2, m2.row2), Double3.Dot(m1.col2, m2.row3));
            Double3 col3 = new Double3(Double3.Dot(m1.col3, m2.row1), Double3.Dot(m1.col3, m2.row2), Double3.Dot(m1.col3, m2.row3));
            return new Matrix3x3(col1, col2, col3);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Double3 operator *(Matrix3x3 m, Double3 v)
        {
            return new Double3(Double3.Dot(m.col1, v), Double3.Dot(m.col2, v), Double3.Dot(m.col3, v));
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Double3 operator *(Double3 v, Matrix3x3 m)
        {
            return new Double3(Double3.Dot(v, m.row1), Double3.Dot(v, m.row2), Double3.Dot(v, m.row3));
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

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Matrix3x3 GetT()
        {
            return new Matrix3x3(this.row1, this.row2, this.row3);
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix3x3 T(Matrix3x3 v)
        {
            return new Matrix3x3(v.row1, v.row2, v.row3);
        }
    }
}