using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Geometric
{
    /// <summary>
    /// 多边形差
    /// </summary>
    public class Polygon2DSetDiff
    {
        /// <summary>
        /// 求多边形的差。
        /// 算法核心，主多边形走逆时针方向，diff多边形走顺时针方向。
        /// 保证diffpoly多边形不会把mainpoly分成多个子部分。
        /// mainpoly> diffpoly
        /// </summary>
        /// <param name="mainPoly">逆时针序列</param>
        /// <param name="diffpoly">顺时针序列，必须顺时针序列</param>
        /// <returns></returns>
        public static Double2[] CalcPoly(Double2[] mainPoly, Double2[] diffpoly)
        {
            if (mainPoly == null || mainPoly.Length < 3)
                return null;
            if (diffpoly == null || diffpoly.Length < 3)
                return mainPoly;
            //
            Polygon2D mainPoly_ = new Polygon2D(mainPoly);
            Polygon2D diffPoly_ = new Polygon2D(diffpoly);
            // 获取边上的交点
            List<Double3>[] mainPolyIntersectArray = new List<Double3>[mainPoly_.GetEdgeNum()];
            List<Double3>[] diffPolyIntersectArray = new List<Double3>[diffPoly_.GetEdgeNum()];
            PolygonBool.GetAllEdgeInterSectPoint(mainPoly_, diffPoly_, ref mainPolyIntersectArray, ref diffPolyIntersectArray);
            // 没有交点直接返回呗
            bool CheckIntersect = false;
            foreach (List<Double3> list in mainPolyIntersectArray)
            {
                if (list != null && list.Count > 0)
                {
                    CheckIntersect = true;
                    break;
                }
            }
            if (CheckIntersect == false)
            {
                PolygonBool.ClearPolyIntersectArray(ref mainPolyIntersectArray, ref diffPolyIntersectArray);
                return mainPoly;
            }
            // 有交点的处理
            List<Double2> listPoint = new List<Double2>();
            int curedge = 0;
            Double2 curPoint = Double2.zero;
            // 查找在diffpoly外的一个顶点。
            if (PolygonBool.FindOutDiffPointPoint(mainPoly_, diffPoly_, ref curPoint, ref curedge) == false)
            {
                return mainPoly;
            }
            // 初始化数据。
            Polygon2D poly = mainPoly_;
            List<Double3>[]  curPolyIntersectArray = mainPolyIntersectArray;

            while (poly != null && curedge >= 0 && curedge < poly.GetEdgeNum())
            {
                Point2D ls2d = poly.GetSimpleEdge(curedge);

                if (PolygonBool.AddPoint(listPoint, curPoint) == false)
                    break;
                Double3 nextPoint = Double3.zero;
                bool ret = PolygonBool.GetNearPointInEdge(poly, mainPoly_, diffPoly_, ls2d, curPoint, curPolyIntersectArray[curedge], ref nextPoint);
                if (ret == false)
                {
                    curedge++;
                    if (curedge >= poly.GetEdgeNum())
                    {
                        curedge = 0;
                    }
                    curPoint = poly.GetEdge(curedge).startPoint;
                }
                else // 则需要交换了。
                {
                    Point2D otherEdge = PolygonBool.GetOtherEdge(poly, mainPoly_, diffPoly_, (int)nextPoint.z);
                    if (Double2.Cross(ls2d.endPoint - ls2d.startPoint, otherEdge.endPoint - otherEdge.startPoint) > 0) // 进一步判断是否需要更换
                    {
                        curPoint = new Double2(nextPoint.x, nextPoint.y);
                        curedge = (int)nextPoint.z;
                        PolygonBool.ExChangePoly(ref poly, mainPoly_, diffPoly_, ref curPolyIntersectArray, mainPolyIntersectArray, diffPolyIntersectArray);
                    }
                    else // 不需要变换了。
                    {
                        curedge++;
                        if (curedge >= poly.GetEdgeNum())
                        {
                            curedge = 0;
                        }
                        curPoint = poly.GetEdge(curedge).startPoint;
                    }
                }
            }
            PolygonBool.ClearPolyIntersectArray(ref mainPolyIntersectArray, ref diffPolyIntersectArray);
            return listPoint.ToArray(); ;
        }
    }
}