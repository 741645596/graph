using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    /// <summary>
    /// 顶点组
    /// </summary>
    [System.Serializable]
    public struct Point2D
    {
        /// <summary>
        /// 直线上得点
        /// </summary>
        public Double2 startPoint;
        /// <summary>
        /// 直线上得点
        /// </summary>
        public Double2 endPoint;
        /// <summary>
        /// 构建线段
        /// </summary>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        public Point2D(Double2 startPt, Double2 endPt)
        {
            this.startPoint = startPt;
            this.endPoint = endPt;
        }
    }
}
