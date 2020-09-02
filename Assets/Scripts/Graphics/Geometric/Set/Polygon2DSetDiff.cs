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
        /// <param name="diffpoly">逆时针序列</param>
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
            GetAllEdgeInterSectPoint(mainPoly_, diffPoly_, ref mainPolyIntersectArray, ref diffPolyIntersectArray);

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
                ClearPolyIntersectArray(ref mainPolyIntersectArray, ref diffPolyIntersectArray);
                return mainPoly;
            }
            // 有交点的处理
            List<Double2> listPoint = new List<Double2>();
            int curedge = 0;
            Double2 curPoint = Double2.zero;
            // 查找在diffpoly外的一个顶点。
            if (FindOutDiffPointPoint(mainPoly_, diffPoly_, ref curPoint, ref curedge) == false)
            {
                return mainPoly;
            }
            // 初始化数据。
            bool SearchDir = true;
            Polygon2D poly = mainPoly_;
            List<Double3>[]  curPolyIntersectArray = mainPolyIntersectArray;

            while (poly != null && curedge >= 0 && curedge < poly.GetEdgeNum())
            {
                Point2D ls2d = poly.GetSimpleEdge(curedge);

                if (AddPoint(listPoint, curPoint) == false)
                    break;
                Double3 nextPoint = Double3.zero;
                bool ret = GetNearPointInEdge(ls2d, SearchDir, curPoint, curPolyIntersectArray[curedge], ref nextPoint);
                if (ret == false)
                {
                    if (SearchDir == true)
                    {
                        curedge++;
                        if (curedge >= poly.GetEdgeNum())
                        {
                            curedge = 0;
                        }
                        curPoint = poly.GetEdge(curedge).startPoint;
                    }
                    else
                    {
                        curedge--;
                        if (curedge < 0)
                        {
                            curedge = poly.GetEdgeNum() - 1;
                        }
                        curPoint = poly.GetEdge(curedge).endPoint;
                    }
                }
                else // 则需要交换了。
                {
                    curPoint = new Double2(nextPoint.x, nextPoint.y);
                    curedge = (int)nextPoint.z;
                    ExChangePoly(ref poly, mainPoly_, diffPoly_, ref curPolyIntersectArray, mainPolyIntersectArray, diffPolyIntersectArray);
                    // 核心。主多边形逆时针。diff多边形顺时针了。
                    if (poly == mainPoly_)
                    {
                        SearchDir = true;
                    }
                    else
                    {
                        SearchDir = false;
                    }
                }
            }
            ClearPolyIntersectArray(ref mainPolyIntersectArray, ref diffPolyIntersectArray);
            return listPoint.ToArray(); ;
        }
        /// <summary>
        /// 清理动作
        /// </summary>
        /// <param name="Poly1IntersectArray"></param>
        /// <param name="Poly2IntersectArray"></param>
        private static void ClearPolyIntersectArray(ref List<Double3>[] Poly1IntersectArray, ref List<Double3>[] Poly2IntersectArray)
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
        /// 多边形决策。
        /// </summary>
        /// <param name="ls2d"></param>
        /// <param name="SearchDir">边的方向，true 逆时针方向， false 顺时针方向</param>
        /// <param name="targetPoint"></param>
        /// <param name="middlePoint"></param>
        private static bool GetNearPointInEdge(Point2D ls2d, bool SearchDir, Double2 curPoint, List<Double3> listMiddlePoint, ref Double3 nextPoint)
        {
            // true 为主多边形。
            if (SearchDir == true)
            {
                if (listMiddlePoint == null || listMiddlePoint.Count == 0)
                {
                    return false;
                }
                else
                {
                    if (curPoint == ls2d.startPoint)
                    {
                        nextPoint = listMiddlePoint[0];
                        // 加入异常处理
                        if (curPoint == new Double2(nextPoint.x, nextPoint.y))
                            return false;
                        else return true;
                    }
                    else
                    {
                        for (int i = 0; i < listMiddlePoint.Count; i++)
                        {
                            if (curPoint == new Double2(listMiddlePoint[i].x, listMiddlePoint[i].y) == true && i < listMiddlePoint.Count - 1)
                            {
                                nextPoint = listMiddlePoint[i + 1];
                                if (i < listMiddlePoint.Count - 2 && (listMiddlePoint[i+1].x == listMiddlePoint[i+ 2].x && listMiddlePoint[i + 1].y == listMiddlePoint[i + 2].y))
                                {
                                    if (listMiddlePoint[i + 2].z > listMiddlePoint[i + 1].z)
                                    {
                                        if (listMiddlePoint[i + 1].z == 0.0f)
                                        {
                                            nextPoint = listMiddlePoint[i + 2];
                                        }
                                    }
                                    else
                                    {
                                        if (listMiddlePoint[i + 2].z != 0.0f)
                                        {
                                            nextPoint = listMiddlePoint[i + 2];
                                        }
                                    }
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            else // 为diff多边形的搜索情况。
            {
                if (listMiddlePoint == null || listMiddlePoint.Count == 0)
                {
                    return false;
                }
                else
                {
                    if (curPoint == ls2d.endPoint)
                    {
                        nextPoint = listMiddlePoint[listMiddlePoint.Count - 1];
                        // 加入异常处理
                        if (curPoint == new Double2(nextPoint.x, nextPoint.y))
                            return false;
                        else return true;
                    }
                    else
                    {
                        for (int i = listMiddlePoint.Count - 1; i >= 0; i--)
                        {
                            if (curPoint == new Double2(listMiddlePoint[i].x, listMiddlePoint[i].y) == true && i > 0)
                            {
                                nextPoint = listMiddlePoint[i - 1];
                                if (i > 1 && (listMiddlePoint[i - 1].x == listMiddlePoint[i - 2].x && listMiddlePoint[i - 1].y == listMiddlePoint[i - 2].y))
                                {
                                    if (listMiddlePoint[i - 2].z > listMiddlePoint[i - 1].z)
                                    {
                                        if (listMiddlePoint[i - 1].z != 0.0f)
                                        {
                                            nextPoint = listMiddlePoint[i - 2];
                                        }
                                    }
                                    else
                                    {
                                        if (listMiddlePoint[i - 2].z != 0.0f)
                                        {
                                            nextPoint = listMiddlePoint[i - 2];
                                        }
                                    }
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 主多边形上找到一个顶点，不在diff多边形内的。
        /// </summary>
        /// <param name="mainPoly"></param>
        /// <param name="diffpoly"></param>
        /// <param name="poly"></param>
        /// <param name="curedge"></param>
        private static bool FindOutDiffPointPoint(Polygon2D mainPoly, Polygon2D diffpoly, ref Double2 curPoint, ref int curedge)
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
        /// 加入顶点
        /// </summary>
        /// <param name="listPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static bool AddPoint(List<Double2> listPoint, Double2 point)
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
        /// 交换数据
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="poly1"></param>
        /// <param name="poly2"></param>
        private static void ExChangePoly(ref Polygon2D poly, Polygon2D poly1, Polygon2D poly2,
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
        private static void GetAllEdgeInterSectPoint(Polygon2D mainPoly, Polygon2D diffpoly, ref List<Double3>[] mainPolyIntersectArray, ref List<Double3>[] Poly2IntersectArray)
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
                GetAllIntersectPoint(diffpoly.GetEdge(i).startPoint, i, mainPolyIntersectArray, ref Poly2IntersectArray[i]);
            }
        }
        /// <summary>
        /// 获取线段与多边形的所有交点，并按从近到远排序。，float3 z记录与多边形相交的边。
        /// </summary>
        /// <param name="lsStartPoint">线段起点</param>
        /// <param name="lsindex">线段索引</param>
        /// <param name="PolyIntersectArray">多边形相交数据</param>
        /// <param name="paths"></param>
        private static void GetAllIntersectPoint(Double2 lsStartPoint, int lsindex, List<Double3>[] PolyIntersectArray, ref List<Double3> paths)
        {
            List<Double3> listpath = new List<Double3>();
            for (int i = 0; i < PolyIntersectArray.Length; i++)
            {
                if (PolyIntersectArray[i] != null && PolyIntersectArray[i].Count > 0)
                {
                    foreach (Double3 pos in PolyIntersectArray[i])
                    {
                        if (pos.z == lsindex)
                        {
                            listpath.Add(new Double3(pos.x, pos.y, i));
                        }
                    }
                }
            }
            // 从近到远排好队。
            if (listpath.Count > 1)
            {
                listpath.Sort((x, y) => MathUtil.GetCompareDis(new Double2(x.x, x.y), lsStartPoint).CompareTo(MathUtil.GetCompareDis(new Double2(y.x, y.y), lsStartPoint)));
            }
            paths = listpath;
        }
    }
}