using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Geometric
{
    /// <summary>
    /// 生成polygon 
    /// </summary>
    public class MakePolygon2D
    {
        /// <summary>
        /// 获取凸包
        /// </summary>
        /// <param name="listpt"></param>
        /// <returns></returns>
        public static List<Float2> MakePolygon(Float2[] listpt)
        {
            if (listpt == null || listpt.Length == 0)
                return null;
            // 先生成aabb。
            Float2 min = Float2.positiveInfinity;
            Float2 max = Float2.negativeInfinity;

            foreach (Float2 pos in listpt)
            {
                min = Float2.Min(min, pos);
                max = Float2.Max(max, pos);
            }
            // 收集条边上的点，按顺时针收集。lb放置在bottom边上，rb 放置在right边上，ru顶点放置up边上, lu顶点放置在left边上
            List<Float2>[] edgePointArray = new List<Float2>[4];
            for (int i = 0; i < 4; i++)
            {
                edgePointArray[i] = new List<Float2>();
            }
            // 不在边上的顶点
            List<Float2> listOutEdgePoint = new List<Float2>();
            // 进行收集了。
            foreach (Float2 pos in listpt)
            {
                // 第一条边 bottom edge
                if (pos.y == min.y && pos.x < max.x)
                {
                    edgePointArray[0].Add(pos);
                }
                else if (pos.x == max.x && pos.y < max.y) //  right edge
                {
                    edgePointArray[1].Add(pos);
                }
                else if (pos.y == max.y && pos.x > min.x) //  up edge
                {
                    edgePointArray[2].Add(pos);
                }
                else if (pos.x == min.x && pos.y > min.y) //  left edge
                {
                    edgePointArray[3].Add(pos);
                }
                else  // 内部的顶点了。
                {
                    listOutEdgePoint.Add(pos);
                }
            }
            // 边上的顶点按顺时针排序了。
            edgePointArray[0].Sort((x, y) => x.x.CompareTo(y.x));
            edgePointArray[1].Sort((x, y) => x.y.CompareTo(y.y));
            edgePointArray[2].Sort((x, y) => -x.x.CompareTo(y.x));
            edgePointArray[3].Sort((x, y) => -x.y.CompareTo(y.y));
            // 
            List<Float2> listPoly = new List<Float2>();

            for (int i = 0; i < 4; i++)
            {
                int length = edgePointArray[i].Count;
                if (length > 0)
                {
                    listPoly.AddRange(edgePointArray[i]);
                }
                List<Float2> llinkPoint = SearchOutPoint(i, edgePointArray[i][length - 1], edgePointArray[(i + 1) % 4][0], listOutEdgePoint);
                if (llinkPoint != null && llinkPoint.Count > 0)
                {
                    listPoly.AddRange(llinkPoint);
                }
            }
            return listPoly;
        }

        /// <summary>
        /// 获取最边缘一圈的顶点。
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="listOutEdgePoint"></param>
        /// <returns></returns>
        private static List<Float2> SearchOutPoint(int edgeIndex, Float2 s, Float2 e, List<Float2> listOutEdgePoint)
        {
            if (listOutEdgePoint == null || listOutEdgePoint.Count == 0)
                return null;

            Float2 outdir;
            Float2 indir;
            if (edgeIndex == 0)
            {
                if (s.y == e.y)
                    return null;
                outdir = Float2.right;
                indir = (e - s).normalized;
            }
            else if (edgeIndex == 1)
            {
                if (s.x == e.x)
                    return null;
                outdir = Float2.up;
                indir = (e - s).normalized;
            }
            else if (edgeIndex == 2)
            {
                if (s.y == e.y)
                    return null;
                outdir = Float2.left;
                indir = (e - s).normalized;
            }
            else if (edgeIndex == 3)
            {
                if (s.x == e.x)
                    return null;
                outdir = Float2.down;
                indir = (e - s).normalized;
            }
            else return null;
            // 开始搜寻了。
            List<Float2> lResult = new List<Float2>();
            Float2 startPoint = s;
            Float2 bestPoint = Float2.zero;
            bool isHaveBestPoint = false;
            List<Float2> listinPoints = new List<Float2>();
            List<Float2> lHave = listOutEdgePoint;

            while (lHave != null && lHave.Count > 0)
            {
                // 先过滤了。
                foreach (Float2 pos in lHave)
                {
                    if (CheckPointInCorns(pos, startPoint, indir, outdir) == true)
                    {
                        listinPoints.Add(pos);
                        if (isHaveBestPoint == false)
                        {
                            bestPoint = pos;
                            isHaveBestPoint = true;
                        }
                    }
                }
                // 再则优。
                if (isHaveBestPoint == true)
                {
                    for (int i = 1; i < listinPoints.Count; i++)
                    {
                        // 比较更好的点。
                        if (CheckPointInCorns(listinPoints[i], startPoint, (bestPoint - startPoint).normalized, outdir) == true)
                        {
                            bestPoint = listinPoints[i];
                        }
                    }
                    // 进行交换，执行下一轮迭代。
                    lResult.Add(bestPoint);
                    startPoint = bestPoint;
                    indir = (e - startPoint).normalized;
                    listinPoints.Remove(bestPoint);
                    lHave = listinPoints;
                    listinPoints = new List<Float2>();
                    isHaveBestPoint = false;
                }
                else break;
            }
            return lResult;
        }
        /// <summary>
        /// 判断指定的点，在夹角内。不包括在边上。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startPoint"></param>
        /// <param name="sindir"></param>
        /// <param name="outdir"></param>
        /// <returns></returns>
        private static bool CheckPointInCorns(Float2 target, Float2 startPoint, Float2 indir, Float2 outdir)
        {
            Float2 diff = target - startPoint;
            if (diff == Float2.zero)
                return false;
            if (Float2.Dot(target - startPoint, outdir) < 0)
                return false;

            float ret = Float2.Cross(outdir, diff) * Float2.Cross(indir, diff);
            if (ret < 0) return true;
            return false;
        }

        /// <summary>
        /// 求多边形的并。
        /// </summary>
        /// <param name="polygon1"></param>
        /// <param name="polygon2"></param>
        /// <returns></returns>
        public static Float2[] MakePolygon(Float2[] polygon1, Float2[] polygon2)
        {
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
            // 



            List<Float2> listPoint = new List<Float2>();
            // 
            Polygon2D poly = poly1;
            List<Float3>[] curPolyIntersectArray = Poly1IntersectArray;
            int curedge = 0;
            int edgeDir = 1;
            while (poly != null && curedge >=0 && curedge < poly.EdgeNum)
            {
                LineSegment2D ls2d = poly.GetEdge(curedge);
                Float2 normalDir = poly.GetNormal(curedge);

                if (AddPoint(ref listPoint, ls2d.startPoint) == false)
                    break;
                // 判断边上有没交点。有交点切换多边形。
                if (curPolyIntersectArray[curedge] != null && curPolyIntersectArray[curedge].Count > 0)
                {
                    Float2 first = new Float2(curPolyIntersectArray[curedge][0].x, curPolyIntersectArray[curedge][0].y);
                    if (first != ls2d.startPoint)
                    {
                        if (AddPoint(ref listPoint, ls2d.startPoint) == false)
                            break;
                    }
                    // 换到另一个多边形上去。
                    curedge = (int)curPolyIntersectArray[curedge][0].z;
                    ExChangePoly(ref poly, poly1, poly2, ref curPolyIntersectArray, Poly1IntersectArray, Poly2IntersectArray);
                    LineSegment2D ls = poly.GetEdge(curedge);
                    if (Float2.Dot(ls.normalizedDir, normalDir) > 0)
                    {
                        edgeDir = 1;
                    }
                    else 
                    {
                        edgeDir = -1;
                    }
                }
                else 
                {
                    curedge += edgeDir;
                    if (curedge >= poly.EdgeNum)
                    {
                        curedge = 0;
                    }
                    else if (curedge < 0)
                    {
                        curedge = poly.EdgeNum - 1;
                    }
                }

            }
            return listPoint.ToArray(); ;
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
        private static void GetAllIntersectPoint(Float2 lsStartPoint,int lsindex, List<Float3>[] PolyIntersectArray, ref List<Float3> paths)
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
