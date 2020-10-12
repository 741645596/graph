using System.Collections.Generic;
using RayGraphics.Math;

/// <summary>
/// polygon bool 运算的基础函数
/// </summary>
namespace RayGraphics.Geometric
{
    public class PolygonBool 
    {
        /// <summary>
        /// 清理动作
        /// </summary>
        /// <param name="Poly1IntersectArray"></param>
        /// <param name="Poly2IntersectArray"></param>
        public static void ClearPolyIntersectArray(ref List<Double3>[] Poly1IntersectArray, ref List<Double3>[] Poly2IntersectArray)
        {
            if (Poly1IntersectArray != null)
            {
                foreach (List<Double3> v in Poly1IntersectArray)
                {
                    if (v != null && v.Count > 0)
                    {
                        v.Clear();
                    }
                }
                Poly1IntersectArray = null;
            }

            if (Poly2IntersectArray != null)
            {
                foreach (List<Double3> v in Poly2IntersectArray)
                {
                    if (v != null && v.Count > 0)
                    {
                        v.Clear();
                    }
                }
                Poly2IntersectArray = null;
            }
        }

        /// <summary>
        /// 获取另外一个多边形的边
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="poly1"></param>
        /// <param name="poly2"></param>
        /// <param name="edgeIndex"></param>
        /// <returns></returns>
        public static Point2D GetOtherEdge(Polygon2D poly, Polygon2D poly1, Polygon2D poly2, int edgeIndex)
        {
            if (poly == poly1)
            {
                return poly2.GetSimpleEdge(edgeIndex);
            }
            else return poly1.GetSimpleEdge(edgeIndex);
        }
        /// <summary>
        /// 加入顶点
        /// </summary>
        /// <param name="listPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool AddPoint(List<Double2> listPoint, Double2 point)
        {
            if (listPoint.Contains(point) == true)
                return false;
            else
            {
                listPoint.Add(point);
                return true;
            }
        }
        /// <summary>
        /// 主多边形上找到一个顶点，不在diff多边形内的。
        /// </summary>
        /// <param name="mainPoly"></param>
        /// <param name="diffpoly"></param>
        /// <param name="poly"></param>
        /// <param name="curedge"></param>
        public static bool FindOutDiffPointPoint(Polygon2D mainPoly, Polygon2D diffpoly, ref Double2 curPoint, ref int curedge)
        {
            if (mainPoly == null || diffpoly == null || mainPoly.GetEdgeNum() < 3 || diffpoly.GetEdgeNum() < 3)
                return false;

            for (int i = 0; i < mainPoly.GetEdgeNum(); i++)
            {
                Double2 point = mainPoly.GetPoint(i);
                if (diffpoly.CheckIn(point) == false)
                {
                    curPoint = point;
                    curedge = i;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 交换数据
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="poly1"></param>
        /// <param name="poly2"></param>
        public static void ExChangePoly(ref Polygon2D poly, Polygon2D poly1, Polygon2D poly2,
            ref List<Double3>[] curPolyIntersectArray, List<Double3>[] poly1IntersectArray, List<Double3>[] poly2IntersectArray)
        {
            if (poly == poly1)
            {
                poly = poly2;
            }
            else poly = poly1;


            if (curPolyIntersectArray == poly1IntersectArray)
            {
                curPolyIntersectArray = poly2IntersectArray;
            }
            else curPolyIntersectArray = poly1IntersectArray;
        }

        /// <summary>
        /// 获取多边形所有边的相交点
        /// </summary>
        /// <param name="mainPoly"></param>
        /// <param name="diffpoly"></param>
        /// <param name="mainPolyIntersectArray"></param>
        /// <param name="Poly2IntersectArray"></param>
        public static void GetAllEdgeInterSectPoint(Polygon2D mainPoly, Polygon2D diffpoly, ref List<Double3>[] mainPolyIntersectArray, ref List<Double3>[] Poly2IntersectArray)
        {
            if (mainPoly == null || diffpoly == null)
                return;

            for (int i = 0; i < mainPoly.GetEdgeNum(); i++)
            {
                diffpoly.GetAllIntersectPoint(mainPoly.GetEdge(i), ref mainPolyIntersectArray[i]);
            }
            //
            for (int i = 0; i < diffpoly.GetEdgeNum(); i++)
            {
                mainPoly.GetAllIntersectPoint(diffpoly.GetEdge(i), ref Poly2IntersectArray[i]);
            }
        }
    }
}
