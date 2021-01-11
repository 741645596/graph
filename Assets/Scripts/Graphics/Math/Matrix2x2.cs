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
        public Double2 col1;
        /// <summary>
        /// 第二行
        /// </summary>
        public Double2 col2;
        /// <summary>
        /// 第一列
        /// </summary>
        public Double2 row1
        {
            get { return new Double2(col1.x, col2.x); }
        }
        /// <summary>
        /// 第二列
        /// </summary>
        public Double2 row2
        {
            get { return new Double2(col1.y, col2.y); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        public Matrix2x2(Double2 col1, Double2 col2)
        {
            this.col1 = col1;
            this.col2 = col2;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(Double2 col1, Double2 col2)
        {
            this.col1 = col1;
            this.col2 = col2;
        }

        /// <summary>
        /// 索引访问
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public double this[int col,int row]
        {
            get
            {
                if (col == 0)
                    return this.col1[row];
                else if (col == 1)
                    return this.col2[row];
                else return 0;
            }
            set
            {
                if (col == 0)
                    this.col1[row] = value;
                else if (col == 1)
                    this.col2[row] = value;
            }
        }
        /// <summary>
        /// 0 矩阵
        /// </summary>
        public static Matrix2x2 zero
        {
            get { return s_zero; }
        }
        private static readonly Matrix2x2 s_zero = new Matrix2x2(Double2.zero, Double2.zero);
        /// <summary>
        /// 单位矩阵
        /// </summary>
        public static Matrix2x2 one
        {
            get { return s_one; }
        }
        private static readonly Matrix2x2 s_one = new Matrix2x2(Double2.right, Double2.up);
        /// <summary>
        /// 旋转矩阵，逆时针旋转angle
        /// </summary>
        /// <param name="angle">旋转角度</param>
        /// <returns></returns>
        public static Matrix2x2 RotateMatrix(double angle)
        {
            double cosAngle = MathFunc.cosAngle(angle);
            double sinAngle = MathFunc.sinAngle(angle);
            Double2 col1 = new Double2(cosAngle, sinAngle);
            Double2 col2 = new Double2(-sinAngle, cosAngle);
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <returns></returns>
        public static Matrix2x2 ScaleMatrix(double scaleX, double scaleY)
        {
            Double2 col1 = new Double2(scaleX, 0);
            Double2 col2 = new Double2(0, scaleY);
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix2x2 ScaleMatrix(float scale)
        {
            Double2 col1 = new Double2(scale, 0);
            Double2 col2 = new Double2(0, scale);
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 沿着指定轴进行缩放
        /// </summary>
        /// <param name="axis">指定轴的法向量</param>
        /// <param name="scale">缩放比例</param>
        /// <returns></returns>
        public static Matrix2x2 ScaleMatrix(Double2 axis, float scale)
        {
            double dx = (scale - 1) * axis.x;
            double dy = (scale - 1) * axis.y;
            Double2 col1 = new Double2(1+ dx * axis.x, dx * axis.y);
            Double2 col2 = new Double2(dy * axis.x,    1+ dy * axis.y);
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 投影到X轴的正交投影矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix2x2 XOrthogonalProjectionMatrix()
        {
            Double2 col1 = Double2.right;
            Double2 col2 = Double2.zero;
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// 投影到X轴的正交投影矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix2x2 YOrthogonalProjectionMatrix()
        {
            Double2 col1 = Double2.zero;
            Double2 col2 = Double2.up;
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
            Double2 m1 = v1.col1 + v2.col1;
            Double2 m2 = v1.col2 + v2.col2;
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
            Double2 m1 = v1.col1 - v2.col1;
            Double2 m2 = v1.col2 - v2.col2;
            return new Matrix2x2(m1, m2);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix2x2 operator *(double k, Matrix2x2 v)
        {
            Double2 col1 = v.col1 * k;
            Double2 col2 = v.col2 * k;
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="k"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix2x2 operator *(Matrix2x2 v, double k)
        {
            Double2 col1 = v.col1 * k;
            Double2 col2 = v.col2 * k;
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
            Double2 col1 = new Double2(Double2.Dot(m1.col1, m2.row1), Double2.Dot(m1.col1, m2.row2));
            Double2 col2 = new Double2(Double2.Dot(m1.col2, m2.row1), Double2.Dot(m1.col2, m2.row2));
            return new Matrix2x2(col1, col2);
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="m"></param>
        /// <param name="v">作为列向量</param>
        /// <returns></returns>
        public static Double2 operator *(Matrix2x2 m, Double2 v)
        {
            return new Double2(Double2.Dot(m.col1, v), Double2.Dot(m.col2, v));
        }
        /// <summary>
        /// * 运算
        /// </summary>
        /// <param name="v">作为行向量</param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Double2 operator *(Double2 v, Matrix2x2 m)
        {
            return new Double2(Double2.Dot(v, m.row1), Double2.Dot(v, m.row2));
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
        /// <summary>
        /// 计算协方差举证
        /// https://zhuanlan.zhihu.com/p/37609917
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Matrix2x2 GetCovarianceMatrix(Double2[] points)
        {
            Matrix2x2 covarianceMatrix = Matrix2x2.zero;
            if (points == null || points.Length <= 0)
                return covarianceMatrix;
            int numPoints = points.Length;
            //Compute the covariance 
            for (int col = 0; col < 2; col++)
            {
                Double2 data;
                for (int row = col; row < 2; row++)
                {
                    covarianceMatrix[col, row] = 0.0f;
                    Double acc = 0.0f;
                    //cov(X,Y)=E[(X-x)(Y-y)]
                    for (int i = 0; i < numPoints; i++)
                    {
                        data = points[i];
                        acc += data[col] * data[row];
                    }
                    acc /= numPoints;
                    covarianceMatrix[col, row] = acc;
                    //symmetric
                    covarianceMatrix[row, col] = acc;
                }
            }
            return covarianceMatrix;
        }
        /// <summary>
        /// 求特征向量矩阵 通过使用Jacobi 迭代算法
        /// </summary>
        /// <param name="matrix"></param>
        public static Matrix2x2 GetFeatureVectorMatrix(Matrix2x2 matrix)
        {
            double p, q, spq;
            double cosa, sina;  //cos(alpha) and sin(alpha)
            double temp;
            double s1 = 0.0f;    //sums of squares of diagonal
            double s2;          //elements

            bool flag = true;  //determine whether to iterate again
            int iteration = 0;   //iteration counter

            double[] data = new double[2];
            Matrix2x2 t = Matrix2x2.one;//To store the product of the rotation matrices.
            do
            {
                iteration++;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = i + 1; j < 2; j++)
                    {
                        if (System.Math.Abs(matrix[j, i]) < MathUtil.kEpsilon)
                            matrix[j, i] = 0.0f;
                        else
                        {
                            q = System.Math.Abs(matrix[i, i] - matrix[j, j]);
                            if (q > MathUtil.kEpsilon)
                            {
                                p = (2.0f * matrix[j, i] * q) / (matrix[i, i] - matrix[j, j]);
                                spq = (float)System.Math.Sqrt(p * p + q * q);
                                cosa = (float)System.Math.Sqrt((1.0f + q / spq) / 2.0f);
                                sina = p / (2.0f * cosa * spq);
                            }
                            else
                                sina = cosa = (float)System.Math.Sqrt(2) * 0.5f;

                            for (int k = 0; k < 2; k++)
                            {
                                temp = t[i, k];
                                t[i, k] = (temp * cosa + t[j, k] * sina);
                                t[j, k] = (temp * sina - t[j, k] * cosa);
                            }
                            for (int k = i; k < 2; k++)
                            {
                                if (k > j)
                                {
                                    temp = matrix[k, i];
                                    matrix[k, i] = (cosa * temp + sina * matrix[k, j]);
                                    matrix[k, j] = (sina * temp - cosa * matrix[k, j]);
                                }
                                else
                                {
                                    data[k] = matrix[k, i];
                                    matrix[k, i] = (cosa * data[k] + sina * matrix[j, k]);

                                    if (k == j)
                                        matrix[k, j] = (sina * data[k] - cosa * matrix[k, j]);
                                }
                            }
                            data[j] = sina * data[i] - cosa * data[j];

                            for (int k = 0; k <= j; k++)
                            {
                                if (k <= i)
                                {
                                    temp = matrix[i, k];
                                    matrix[i, k] = (cosa * temp + sina * matrix[j, k]);
                                    matrix[j, k] = (sina * temp - cosa * matrix[j, k]);

                                }
                                else
                                    matrix[j, k] = (sina * data[k] - cosa * matrix[j, k]);

                            }
                        }
                    }
                }
                s2 = 0.0f;
                for (int i = 0; i < 2; i++)
                {
                    s2 += matrix[i, i] * matrix[i, i];
                }
                if (System.Math.Abs(s2) < MathUtil.kEpsilon || System.Math.Abs(1 - s1 / s2) < MathUtil.kEpsilon)
                    flag = false;
                else
                    s1 = s2;
            } while (flag);
            return t;
        }
    }
}