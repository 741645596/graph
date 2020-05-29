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
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool CheckIn(Double2 pt)
        {
            Double2 diff1 = pt - this.startPoint;
            Double2 diff2 = pt - this.endPoint;
            if (Double2.CheckInLine(diff1, diff2) == true && Double2.Dot(diff1, diff2) <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
