using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{

    public class OBB2D
    {
        /// <summary>
        /// 起点
        /// </summary>
        public Double2 startPoint;
        /// <summary>
        /// 轴1
        /// </summary>
        public Double2 aix1;
        /// <summary>
        /// 轴2
        /// </summary>
        public Double2 aix2;
        /// <summary>
        /// obb的大小
        /// </summary>
        public Double2 size;
        /// <summary>
        /// 设置aabb
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ru"></param>
        public void SetObb(Double2 startPoint , Double2 aix1, Double2 aix2, Double2 size)
        {
            this.startPoint = startPoint;
            this.aix1 = aix1;
            this.aix2 = aix2;
            this.size = size;
            
        }
        /// <summary>
        /// 根据点阵创建obb
        /// </summary>
        /// <param name="points"></param>
        public void Init(Double2[] points)
        {
            Double2 centerPos = Double2.zero;
            Double2[] offPoints = CalcCenter(points, ref centerPos);
            if (offPoints != null)
            {
                Matrix2x2 matrix = Matrix2x2.GetCovarianceMatrix(offPoints);
                matrix = Matrix2x2.GetFeatureVectorMatrix(matrix);
                this.aix1 = matrix.col1;
                this.aix2 = matrix.col2;
                CalcSize(offPoints, centerPos, matrix);
            }
        }
        /// <summary>
        /// 计算点阵的中心及修正后的点阵。
        /// </summary>
        /// <param name="points"></param>
        /// <param name="centerPos"></param>
        /// <returns></returns>
        private Double2[] CalcCenter(Double2[] points, ref Double2 centerPos)
        {
            if (points == null || points.Length <= 0)
            {
                centerPos = Double2.zero;
                return null;
            }
            int numPoints = points.Length;
            Double2[] pVectors = new Double2[numPoints];
            centerPos = Double2.zero;
            for (int i = 0; i < numPoints; i++)
            {
                pVectors[i] = points[i];
                centerPos += pVectors[i];
            }
            centerPos /= numPoints;
            for (int i = 0; i < numPoints; i++)
            {
                pVectors[i] = pVectors[i] - centerPos;
            }
            return pVectors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offPoints"></param>
        /// <param name="centerPos"></param>
        /// <param name="featreVectorMatrix"></param>
        private void CalcSize(Double2[] offPoints, Double2 centerPos, Matrix2x2 featreVectorMatrix)
        {
            Double2 min = Double2.positiveInfinity;
            Double2 max = Double2.negativeInfinity;
            for (int index = 0; index < offPoints.Length; index++)
            {
                Double2 vec = offPoints[index];

                min.x = System.Math.Min(min.x, Double2.Dot(vec, featreVectorMatrix.row1));
                min.y = System.Math.Min(min.y, Double2.Dot(vec, featreVectorMatrix.row2));

                max.x = System.Math.Max(max.x, Double2.Dot(vec, featreVectorMatrix.row1));
                max.y = System.Math.Max(max.y, Double2.Dot(vec, featreVectorMatrix.row2));
            }
            this.startPoint = featreVectorMatrix.col1 * min.x + featreVectorMatrix.col2 * min.y + centerPos;
            this.size = max - min;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool CheckIn(Float2 pt)
        {
            return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public float CalcDistance(Float2 pt)
        {
            return 0;
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 ProjectPoint(Float2 pt)
        {
            return Float2.zero;
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 AixsVector(Float2 pt)
        {
            return Float2.zero;
        }
        /// <summary>
        /// 与直线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(Line2D line)
        {
            return LineRelation.Detach;
        }
        /// <summary>
        /// 直线与射线间的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(Rays2D line)
        {
            return LineRelation.Intersect;
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(LineSegment2D line)
        {
            return LineRelation.Intersect;
        }
        /// <summary>
        /// 镜面但
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Float2 GetMirrorPoint(Float2 point)
        {
            return point - 2 * AixsVector(point);
        }
        /// <summary>
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="paths">返回路径</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public virtual bool RayboundingNearestPath(LineSegment2D line, float offset, ref List<Float2> paths)
        {
            return false;
        }
        /// <summary>
        /// 获取挡格附近出生点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <param name="bornPoint"></param>
        /// <returns></returns>
        public virtual bool GetBornPoint(LineSegment2D line, float offset, ref Float2 bornPoint)
        {
            return false;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectStart"></param>
        /// <param name="intersectEnd"></param>
        /// <returns></returns>
        public virtual bool GetIntersectPoint(LineSegment2D line, ref Float2 intersectStart, ref Float2 intersectEnd)
        {
            return false;
        }
        /// <summary>
        /// 求obb的点
        /// </summary>
        /// <returns></returns>
        public Double2[] GetPoints()
        {
            Double2[] points = new Double2[4];
            points[0] = this.startPoint;
            points[1] = this.startPoint + this.aix1 * this.size.x;
            points[2] = this.startPoint + this.aix1 * this.size.x + this.aix2 * this.size.y;
            points[3] = this.startPoint + this.aix2 * this.size.y;
            return points;
        }
    }
}