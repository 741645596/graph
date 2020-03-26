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
        /// 索引访问
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public float this[int col,int row]
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
        /// <summary>
        /// 计算协方差举证
        /// https://zhuanlan.zhihu.com/p/37609917
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Matrix2x2 GetCovarianceMatrix(Float2[] points)
        {
            Matrix2x2 covarianceMatrix = Matrix2x2.zero;
            if (points == null || points.Length <= 0)
                return covarianceMatrix;
            int numPoints = points.Length;
            //Compute the covariance 
            for (int col = 0; col < 2; col++)
            {
                Float2 data;
                for (int row = col; row < 2; row++)
                {
                    covarianceMatrix[col, row] = 0.0f;
                    float acc = 0.0f;
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
            float p, q, spq;
            float cosa, sina;  //cos(alpha) and sin(alpha)
            float temp;
            float s1 = 0.0f;    //sums of squares of diagonal
            float s2;          //elements

            bool flag = true;  //determine whether to iterate again
            int iteration = 0;   //iteration counter

            float[] data = new float[2];
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