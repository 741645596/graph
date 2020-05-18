using RayGraphics.Math;

namespace RayGraphics.Geometric
{

    public class Triangle2D : AABB2D
    {
        /// <summary>
        /// 顶点1
        /// </summary>
        public Double2 p1;
        /// <summary>
        /// 顶点2
        /// </summary>
        public Double2 p2;
        /// <summary>
        /// 顶点3
        /// </summary>
        public Double2 p3;
        /// <summary>
        /// 顺时针顶点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Triangle2D(Double2 p1, Double2 p2, Double2 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            Double2 min = Double2.Min(p1, Double2.Min(p2, p3));
            Double2 max = Double2.Max(p1, Double2.Max(p2, p3));
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
        public override Double2[] GetPoints()
        {
            Double2[] points = new Double2[3];
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
        public override Double2 GetPoint(int index)
        {
            if (index == 0)
                return this.p1;
            else if (index == 1)
                return this.p2;
            else if (index == 2)
                return this.p3;
            else return Double2.zero;
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
            else return new LineSegment2D(Double2.zero, Double2.one);
        }
        /// <summary>
        /// 判断点三角形内。
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Double2 pt)
        {
            // 做射线， y = pt.y
            bool flag = false;
            int edgeNum = GetEdgeNum();
            for (int i = 0; i < edgeNum; i++)
            {
                LineSegment2D line = GetEdge(i);
                // 先判断点是否在边上。
                if (line.CheckIn(pt) == true)
                    return true;
                Double2 diff = line.endPoint - line.startPoint;

                if ((line.startPoint.y <= pt.y && line.endPoint.y >= pt.y) || (line.startPoint.y >= pt.y && line.endPoint.y <= pt.y))
                {
                    if (diff.y != 0)
                    {
                        double x = line.startPoint.x + (pt.y - line.startPoint.y) * diff.x / diff.y;
                        // 射线穿过多边形的边界
                        if (x > pt.x)
                        {
                            flag = !flag;
                        }
                    }
                    // 共线情况，肯定是点在线段2端。
                }
            }
            return flag;
        }
    }
}