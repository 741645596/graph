using System.Collections.Generic;
using RayGraphics.Math;
using System;

namespace RayGraphics.Bounding
{
	/// <summary>
	/// OBB方向包围盒(Oriented bounding box)
	/// </summary>
	public class OBB : IBounding
	{
		public void Generate() { }
		public void Generate(List<Float3> listPt) { }
		public void Generate(Float3[] ptArray) { }
#if Client
		public void Generate(UnityEngine.Mesh mesh) { }
#endif

		public bool CheckIN()
		{
			return true;
		}
		/// <summary>
		/// 计算协方差举证
		/// https://zhuanlan.zhihu.com/p/37609917
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public Matrix2x2 CalcCovarianceMatrix(Float2[] points)
		{
			Matrix2x2 covariance = Matrix2x2.zero;
			if (points == null || points.Length < 2)
				return covariance;
			int numPoints = points.Length;
			//
			Float2[] pVectors = new Float2[numPoints];
			//Compute the average x,y
			Float2 avg = Float2.zero;
			for (int i = 0; i < numPoints; i++)
			{
				pVectors[i] = points[i];
				avg += pVectors[i];
			}
			avg /= numPoints;
			for (int i = 0; i < numPoints; i++)
			{
				pVectors[i] = pVectors[i] - avg;
			}
			//Compute the covariance 
			for (int col = 0; col<2; col++)
			{
				Float2 data;
				for(int row = col; row < 2; row++)
				{
					covariance[col,row] = 0.0f;
					float acc = 0.0f;
					//cov(X,Y)=E[(X-x)(Y-y)]
					for(int i= 0;i < numPoints; i++)
					{
						data = pVectors[i];
						acc += data[col] * data[row];
					}
					acc/= numPoints;
					covariance[col, row] = acc;
					//symmetric
					covariance[row,col] =acc;
				}
			}
			return covariance;
		}
		/// <summary>
		/// 求出特征向量，也就是轴
		/// </summary>
		/// <param name="matrix"></param>
		/// <param name="eValue"></param>
		/// <param name="eVectors"></param>
		public void jacobiSolver(Matrix2x2 matrix, float[] eValue, Float2[] eVectors)
		{
			float p, q, spq;
			float cosa ,sina ;  //cos(alpha) and sin(alpha)
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
								t[i, k]  =(temp * cosa + t[j, k] * sina);
								t[j, k]  =(temp * sina - t[j, k] * cosa);
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
					eValue[i] = matrix[i, i];
					s2 += eValue[i] * eValue[i];
				}
				if (System.Math.Abs(s2) < MathUtil.kEpsilon || System.Math.Abs(1 - s1 / s2) < MathUtil.kEpsilon)
					flag = false;
				else
					s1 = s2;
			} while (flag);
			eVectors[0] = t.col1;
			eVectors[1] = t.col2;
		}
	}
}