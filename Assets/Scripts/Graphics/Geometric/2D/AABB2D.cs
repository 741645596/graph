using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{

    public class AABB2D
    {
        /// <summary>
        /// 左下
        /// </summary>
        public Double2 leftBottom;
        /// <summary>
        /// 右上
        /// </summary>
        public Double2 rightUp;
        /// <summary>
        /// 左⬆️
        /// </summary>
        public Double2 LeftUp
        {
            get { return new Double2(leftBottom.x, rightUp.y); }
        }
        /// <summary>
        /// 右⬇️
        /// </summary>
        public Double2 RightBottom
        {
            get { return new Double2(rightUp.x, leftBottom.y); }
        }
        /// <summary>
        /// Center
        /// </summary>
        public Double2 Center
        {
            get { return (this.leftBottom + this.rightUp) * 0.5f; }
        }
        /// <summary>
        /// 2* Center
        /// </summary>
        public Double2 Center2
        {
            get { return this.leftBottom + this.rightUp; }
        }
        /// <summary>
        /// 设置aabb
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ru"></param>
        protected void SetAABB(Double2 lb, Double2 ru)
        {
            this.leftBottom = Double2.Min(lb, ru);
            this.rightUp = Double2.Max(lb, ru);
        }
#if Client
        /// <summary>
        /// draw, 逆时针绘制
        /// </summary>
        public void Draw()
        {
            UnityEngine.GL.Vertex(this.leftBottom.V3);
            UnityEngine.GL.Vertex(this.RightBottom.V3);
            //
            UnityEngine.GL.Vertex(this.RightBottom.V3);
            UnityEngine.GL.Vertex(this.rightUp.V3);
            //
            UnityEngine.GL.Vertex(this.rightUp.V3);
            UnityEngine.GL.Vertex(this.LeftUp.V3);
            //
            UnityEngine.GL.Vertex(this.LeftUp.V3);
            UnityEngine.GL.Vertex(this.leftBottom.V3);
        }
        /// <summary>
        /// DrawGizmos
        /// </summary>
        public void DrawGizmos()
        {
            leftBottom.DrawGizmos();
            rightUp.DrawGizmos();
            LeftUp.DrawGizmos();
            RightBottom.DrawGizmos();
        }
#endif
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool CheckIn(Double2 pt)
        {
            Double2 max = this.rightUp;
            if (pt.x > max.x || pt.y > max.y)
                return false;
            Double2 min = this.leftBottom;
            if (pt.x < min.x || pt.y < min.y)
                return false;
            return true;
        }
        /// <summary>
        /// 与直线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual LineRelation CheckLineRelation(Line2D line)
        {
            if (CheckIn(line.startPoint) == true)
                return LineRelation.Intersect;
            else // 4个点在同侧
            {
                Double2 p1 = this.leftBottom - line.startPoint;
                Double2 p2 = this.RightBottom - line.startPoint;
                
                
                double cross1 = Double2.Cross(line.normalizedDir, p1);
                if (cross1 * Double2.Cross(line.normalizedDir, p2) <= 0)
                {
                    return LineRelation.Intersect;
                }
                Double2 p3 = this.rightUp - line.startPoint;
                if (cross1 * Double2.Cross(line.normalizedDir, p3) <= 0)
                {
                    return LineRelation.Intersect;
                }
                Double2 p4 = this.LeftUp - line.startPoint;
                if (cross1 * Double2.Cross(line.normalizedDir, p4) <= 0)
                {
                    return LineRelation.Intersect;
                }
                return LineRelation.Detach;
            }
        }
        /// <summary>
        /// 射线间的关系
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
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="rbi">包围盒信息</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public virtual RBIResultType RayboundingNearestPath(LineSegment2D line, double offset, ref RayboundingInfo rbi)
        {
            return RBIResultType.Fail;
        }
        /// <summary>
        /// 获取挡格附近出生点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <param name="bornPoint"></param>
        /// <returns></returns>
        public virtual bool GetBornPoint(LineSegment2D line, double offset, ref Double2 bornPoint)
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
        public virtual bool GetIntersectPoint(LineSegment2D line, ref Double2 intersectStart, ref Double2 intersectEnd)
        {
            return false;
        }
        /// <summary>
        /// 与矩形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public virtual bool CheckIntersect(Triangle2D ab)
        {
            return false;
        }
        /// <summary>
        /// 与矩形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public virtual bool CheckIntersect(Rect2D ab)
        {
            return false;
        }
        /// <summary>
        /// 与圆的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public virtual bool CheckIntersect(Circle2D ab)
        {
            return false;
        }
        /// <summary>
        /// 与多边形的关系
        /// </summary>
        /// <param name="dbd1"></param>
        /// <returns>true 相交： false 不相交</returns>
        public virtual bool CheckIntersect(Polygon2D ab)
        {
            return false;
        }
        /// <summary>
        /// 判断2个对象是否相交
        /// </summary>
        /// <param name="ab1"></param>
        /// <param name="ab2"></param>
        /// <returns>暂处理不多边形的关系</returns>
        public static bool CheckIntersect(AABB2D ab1, AABB2D ab2)
        {
            if (ab1 == null || ab2 == null)
                return false;
            if (ab2 is Rect2D)
                return ab1.CheckIntersect(ab2 as Rect2D);
            else if (ab2 is Circle2D)
                return ab1.CheckIntersect(ab2 as Circle2D);
            else if(ab2 is Polygon2D)
                return ab1.CheckIntersect(ab2 as Polygon2D);
            return false;
        }
        /// <summary>
        /// 获取边
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public virtual LineSegment2D GetEdge(int edgeIndex)
        {
            if (edgeIndex == 0)
            {
                return new LineSegment2D(this.leftBottom, this.RightBottom);
            }
            else if (edgeIndex == 1)
            {
                return new LineSegment2D(this.RightBottom, this.rightUp);
            }
            else if (edgeIndex == 2)
            {
                return new LineSegment2D(this.rightUp, this.LeftUp);
            }
            else if (edgeIndex == 3)
            {
                return new LineSegment2D(this.LeftUp, this.leftBottom);
            }
            return new LineSegment2D(Double2.zero, Double2.zero);
        }

        /// <summary>
        /// 获取边
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public virtual Point2D GetSimpleEdge(int edgeIndex)
        {
            if (edgeIndex == 0)
            {
                return new Point2D(this.leftBottom, this.RightBottom);
            }
            else if (edgeIndex == 1)
            {
                return new Point2D(this.RightBottom, this.rightUp);
            }
            else if (edgeIndex == 2)
            {
                return new Point2D(this.rightUp, this.LeftUp);
            }
            else if (edgeIndex == 3)
            {
                return new Point2D(this.LeftUp, this.leftBottom);
            }
            return new Point2D(Double2.zero, Double2.zero);
        }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Double2 GetPoint(int index)
        {
            if (index == 0)
            {
                return this.leftBottom;
            }
            else if (index == 1)
            {
                return this.RightBottom;
            }
            else if (index == 2)
            {
                return this.rightUp;
            }
            else if (index == 3)
            {
                return this.LeftUp;
            }
            return Double2.zero;
        }
        /// <summary>
        /// 获取顶点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Double2Bool GetPointPlus(int index)
        {
            if (index == 0)
            {
                return  new Double2Bool(this.leftBottom.x, this.leftBottom.y, true);
            }
            else if (index == 1)
            {
                return new Double2Bool(this.RightBottom.x, this.RightBottom.y, true);
            }
            else if (index == 2)
            {
                return new Double2Bool(this.rightUp.x, this.rightUp.y, true);
            }
            else if (index == 3)
            {
                return new Double2Bool(this.LeftUp.x, this.LeftUp.y, true);
            }
            return new Double2Bool(0, 0, true);
        }
        /// <summary>
        /// 获取法线
        /// </summary>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public virtual Double2 GetNormal(int edgeIndex)
        {
            if (edgeIndex == 0)
            {
                return Double2.down;
            }
            else if (edgeIndex == 1)
            {
                return Double2.right;
            }
            else if (edgeIndex == 2)
            {
                return Double2.up;
            }
            else if (edgeIndex == 3)
            {
                return Double2.left;
            }
            return Double2.zero;
        }

        /// <summary>
        /// 获取边数
        /// </summary>
        /// <returns></returns>
        public virtual int GetEdgeNum()
        {
            return 4;
        }
        /// <summary>
        /// 获取顶点数组
        /// </summary>
        /// <returns></returns>
        public virtual Double2[] GetPoints()
        {
            Double2[] points = new Double2[4];
            points[0] = this.leftBottom;
            points[1] = this.RightBottom;
            points[2] = this.rightUp;
            points[3] = this.LeftUp;
            return points;
        }
        /// <summary>
        /// 判断某个顶点是否可通行
        /// </summary>
        /// <param name="indexPoint"></param>
        /// <returns></returns>
        public virtual bool CheckCrossPoint(int indexPoint)
        {
            return true;
        }
        /// <summary>
        /// 获取顶点可通过数据
        /// </summary>
        /// <returns></returns>
        public virtual HashSet<int>  GetCrossPointData()
        {
            return null;
        }
    }
}