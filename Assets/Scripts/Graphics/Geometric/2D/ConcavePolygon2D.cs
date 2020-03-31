using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Geometric
{

    public class ConcavePolygon2D
    {
        /// <summary>
        /// 生成凹多边形
        /// </summary>
        /// <param name="listpolygon"></param>
        /// <returns></returns>
        public static Float2[] GenerateConcavePolygon(List<Float2[]> listpolygon)
        {
            if (listpolygon == null || listpolygon.Count == 0)
                return null;
            if (listpolygon.Count == 1)
                return listpolygon[0];

            List<Float2[]> listUnCombine = new List<Float2[]>();
            // 首轮合并
            Float2[] poly = listpolygon[0];
            for (int i = 1; i < listpolygon.Count; i++)
            {
                bool isCombine = false;
                poly = CombinePolygon(poly, listpolygon[i], ref isCombine);
                if (isCombine == false)
                {
                    listUnCombine.Add(listpolygon[i]);
                }
            }
            // 检测有未被合并的。
            int Total = listpolygon.Count - 1;
            listpolygon.Clear();
            if (listUnCombine.Count > 0)
            {
                listpolygon.AddRange(listUnCombine);
                listUnCombine.Clear();
            }
            while (Total > listpolygon.Count && listpolygon.Count > 0)
            {
                Total = listpolygon.Count;
                for (int i = 0; i < listpolygon.Count; i++)
                {
                    bool isCombine = false;
                    poly = CombinePolygon(poly, listpolygon[i], ref isCombine);
                    if (isCombine == false)
                    {
                        listUnCombine.Add(listpolygon[i]);
                    }
                }
                listpolygon.Clear();
                if (listUnCombine.Count > 0)
                {
                    listpolygon.AddRange(listUnCombine);
                    listUnCombine.Clear();
                }
            }
            if (listUnCombine.Count > 0)
            {
                listUnCombine.Clear();
            }
            if (listpolygon.Count > 0)
            {
                listpolygon.Clear();
            }
            return poly;
        }

        /// <summary>
        /// 求多边形的并。
        /// </summary>
        /// <param name="polygon1"></param>
        /// <param name="polygon2"></param>
        /// <returns></returns>
        private static Float2[] CombinePolygon(Float2[] polygon1, Float2[] polygon2, ref bool isCombine)
        {
            isCombine = true;
            if (polygon1 == null || polygon1.Length < 3)
                return null;
            if (polygon2 == null || polygon2.Length < 3)
                return polygon1;
            //
            Polygon2D poly1 = new Polygon2D(polygon1);
            Polygon2D poly2 = new Polygon2D(polygon2);
            // 获取poly1每条线段上的交点。
            List<Float3>[] Poly1IntersectArray = new List<Float3>[poly1.EdgeNum];
            List<Float3>[] Poly2IntersectArray = new List<Float3>[poly2.EdgeNum];
            GetAllEdgeInterSectPoint(poly1, poly2, ref Poly1IntersectArray, ref Poly2IntersectArray);

            bool CheckIntersect = false;
            foreach (List<Float3> list in Poly1IntersectArray)
            {
                if (list != null && list.Count > 0)
                {
                    CheckIntersect = true;
                    break;
                }
            }
            if (CheckIntersect == false)
            {
                isCombine = false;
                ClearPolyIntersectArray(ref Poly1IntersectArray, ref Poly2IntersectArray);
                return polygon1;
            }
            // 
            List<Float2> listPoint = new List<Float2>();
            Polygon2D poly = null;
            int curedge = 0;
            Float2 curPoint = Float2.zero;
            bool SearchDir = true;
            List<Float3>[] curPolyIntersectArray = null;
            SetInitData(poly1, poly2, ref poly, ref curPoint, ref curedge);
            if (poly == poly1)
            {
                curPolyIntersectArray = Poly1IntersectArray;
            }
            else if (poly == poly2)
            {
                curPolyIntersectArray = Poly2IntersectArray;
            }

            while (poly != null && curedge >= 0 && curedge < poly.EdgeNum)
            {
                LineSegment2D ls2d = poly.GetEdge(curedge);
                Float2 normalDir = poly.GetNormal(curedge);

                if (AddPoint(ref listPoint, curPoint) == false)
                    break;
                Float3 nextPoint = Float3.zero;
                bool ret = GetNearPointInEdge(ls2d, SearchDir, curPoint, curPolyIntersectArray[curedge], ref nextPoint);
                if (ret == false)
                {
                    if (SearchDir == true)
                    {
                        curedge++;
                        if (curedge >= poly.EdgeNum)
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
                            curedge = poly.EdgeNum - 1;
                        }
                        curPoint = poly.GetEdge(curedge).endPoint;
                    }
                }
                else // 则需要交换了。
                {
                    curPoint = new Float2(nextPoint.x, nextPoint.y);
                    curedge = (int)nextPoint.z;
                    ExChangePoly(ref poly, poly1, poly2, ref curPolyIntersectArray, Poly1IntersectArray, Poly2IntersectArray);
                    LineSegment2D ls = poly.GetEdge(curedge);

                    if (Float2.Dot(ls.normalizedDir, normalDir) > 0)
                    {
                        SearchDir = true;
                    }
                    else
                    {
                        SearchDir = false;
                    }
                }
            }
            ClearPolyIntersectArray(ref Poly1IntersectArray, ref Poly2IntersectArray);
            return listPoint.ToArray(); ;
        }
        /// <summary>
        /// 清理动作
        /// </summary>
        /// <param name="Poly1IntersectArray"></param>
        /// <param name="Poly2IntersectArray"></param>
        private static void ClearPolyIntersectArray(ref List<Float3>[] Poly1IntersectArray, ref List<Float3>[] Poly2IntersectArray)
        {
            if (Poly1IntersectArray != null)
            {
                foreach (List<Float3> v in Poly1IntersectArray)
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
                foreach (List<Float3> v in Poly2IntersectArray)
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
        /// <param name="dir"></param>
        /// <param name="targetPoint"></param>
        /// <param name="middlePoint"></param>
        private static bool GetNearPointInEdge(LineSegment2D ls2d, bool dir, Float2 curPoint, List<Float3> listMiddlePoint, ref Float3 nextPoint)
        {
            if (dir == true)
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
                        return true;
                    }
                    else
                    {
                        for (int i = 0; i < listMiddlePoint.Count; i++)
                        {
                            if (curPoint == new Float2(listMiddlePoint[i].x, listMiddlePoint[i].y) == true && i < listMiddlePoint.Count - 1)
                            {
                                nextPoint = listMiddlePoint[i + 1];
                                return true;
                            }
                        }
                    }
                }
            }
            else
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
                        return true;
                    }
                    else
                    {
                        for (int i = listMiddlePoint.Count - 1; i >= 0; i--)
                        {
                            if (curPoint == new Float2(listMiddlePoint[i].x, listMiddlePoint[i].y) == true && i > 0)
                            {
                                nextPoint = listMiddlePoint[i - 1];
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poly1"></param>
        /// <param name="poly2"></param>
        /// <param name="poly"></param>
        /// <param name="curedge"></param>
        private static void SetInitData(Polygon2D poly1, Polygon2D poly2, ref Polygon2D poly, ref Float2 curPoint, ref int curedge)
        {
            if (poly1 == null || poly2 == null || poly1.EdgeNum < 3 || poly2.EdgeNum < 3)
                return;
            // 先找poly1 最小的。
            poly = poly1;
            curedge = 0;
            Float2 min = poly1.GetPoint(0);
            for (int i = 1; i < poly1.EdgeNum; i++)
            {
                Float2 point = poly1.GetPoint(i);
                if (point.y < min.y || (point.y == min.y && point.x < min.x))
                {
                    min = point;
                    curedge = i;
                }
            }
            //
            for (int i = 0; i < poly2.EdgeNum; i++)
            {
                Float2 point = poly2.GetPoint(i);
                if (point.y < min.y || (point.y == min.y && point.x < min.x))
                {
                    min = point;
                    curedge = i;
                    poly = poly2;
                }
            }
            curPoint = min;
        }
        /// <summary>
        /// 加入顶点
        /// </summary>
        /// <param name="listPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static bool AddPoint(ref List<Float2> listPoint, Float2 point)
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
            ref List<Float3>[] curPolyIntersectArray, List<Float3>[] poly1IntersectArray, List<Float3>[] poly2IntersectArray)
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
        /// <param name="poly1"></param>
        /// <param name="poly2"></param>
        /// <param name="Poly1IntersectArray"></param>
        /// <param name="Poly2IntersectArray"></param>
        private static void GetAllEdgeInterSectPoint(Polygon2D poly1, Polygon2D poly2, ref List<Float3>[] Poly1IntersectArray, ref List<Float3>[] Poly2IntersectArray)
        {
            if (poly1 == null || poly2 == null)
                return;

            for (int i = 0; i < poly1.EdgeNum; i++)
            {
                poly2.GetAllIntersectPoint(poly1.GetEdge(i), ref Poly1IntersectArray[i]);
            }
            //
            for (int i = 0; i < poly2.EdgeNum; i++)
            {
                GetAllIntersectPoint(poly2.GetEdge(i).startPoint, i, Poly1IntersectArray, ref Poly2IntersectArray[i]);
            }
        }
        /// <summary>
        /// 获取线段与多边形的所有交点，并按从近到远排序。，float3 z记录与多边形相交的边。
        /// </summary>
        /// <param name="lsStartPoint">线段起点</param>
        /// <param name="lsindex">线段索引</param>
        /// <param name="PolyIntersectArray">多边形相交数据</param>
        /// <param name="paths"></param>
        private static void GetAllIntersectPoint(Float2 lsStartPoint, int lsindex, List<Float3>[] PolyIntersectArray, ref List<Float3> paths)
        {
            List<Float3> listpath = new List<Float3>();
            for (int i = 0; i < PolyIntersectArray.Length; i++)
            {
                if (PolyIntersectArray[i] != null && PolyIntersectArray[i].Count > 0)
                {
                    foreach (Float3 pos in PolyIntersectArray[i])
                    {
                        if (pos.z == lsindex)
                        {
                            listpath.Add(new Float3(pos.x, pos.y, i));
                        }
                    }
                }
            }
            // 从近到远排好队。
            if (listpath.Count > 1)
            {
                listpath.Sort((x, y) => Float2.Distance(new Float2(x.x, x.y), lsStartPoint).CompareTo(Float2.Distance(new Float2(y.x, y.y), lsStartPoint)));
            }
            paths = listpath;
        }
    }
}
