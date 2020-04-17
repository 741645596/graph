using RayGraphics.Math;

namespace RayGraphics.Geometric
{

    public class Triangle2D : AABB2D
    {
        /// <summary>
        /// 顶点1
        /// </summary>
        public Float2 p1;
        /// <summary>
        /// 顶点2
        /// </summary>
        public Float2 p2;
        /// <summary>
        /// 顶点3
        /// </summary>
        public Float2 p3;
        /// <summary>
        /// 顺时针顶点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Triangle2D(Float2 p1, Float2 p2, Float2 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            Float2 min = Float2.Min(p1, Float2.Min(p2, p3));
            Float2 max = Float2.Max(p1, Float2.Max(p2, p3));
            this.SetAABB(min, max);
        }
        /// <summary>
        /// 获取边数
        /// </summary>
        /// <returns></returns>
        public override int GetEdgeNum()
        {
            return 3;
        }
        /// <summary>
        /// 获取顶点数组
        /// </summary>
        /// <returns></returns>
        public override Float2[] GetPoints()
        {
            Float2[] points = new Float2[3];
            points[0] = this.p1;
            points[1] = this.p2;
            points[2] = this.p3;
            return points;
        }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override Float2 GetPoint(int index)
        {
            if (index == 0)
                return this.p1;
            else if (index == 1)
                return this.p2;
            else if (index == 2)
                return this.p3;
            else return Float2.zero;
        }
        /// <summary>
        /// 获取边
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public override LineSegment2D GetEdge(int edgeIndex)
        {
            if (edgeIndex == 0)
            {
                return new LineSegment2D(this.p1, this.p2);
            }
            else if (edgeIndex == 1)
            {
                return new LineSegment2D(this.p2, this.p3);
            }
            else if (edgeIndex == 2)
            {
                return new LineSegment2D(this.p3, this.p1);
            }
            else return new LineSegment2D(Float2.zero, Float2.one);
        }
    }
}