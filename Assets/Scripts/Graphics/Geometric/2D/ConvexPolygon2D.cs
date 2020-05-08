using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Geometric
{
    /// <summary>
    /// 生成polygon 
    /// </summary>
    public class ConvexPolygon2D
    {
        /// <summary>
        /// 获取凸多边形的索引。
        /// </summary>
        /// <param name="listpt"></param>
        /// <returns></returns>
        public static List<int> GenerateConvexPolygonIndex(Double2[] listpt)
        {
            List<int> listIndex = new List<int>();
            List<Double2> listPoint= GenerateConvexPolygon(listpt);
            if (listPoint == null || listPoint.Count == 0)
                return listIndex;
            foreach (Double2 pos in listPoint)
            {
                int index = GetIndex(pos, listpt);
                if (index != -1)
                {
                    listIndex.Add(index);
                }
            }
            return listIndex;
        }
        private static int GetIndex(Double2 pos, Double2[] listpt)
        {
            if (listpt == null || listpt.Length == 0)
                return -1;
            for (int i = 0; i < listpt.Length; i++)
            {
                if (listpt[i] == pos)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 获取凸包
        /// </summary>
        /// <param name="listpt"></param>
        /// <returns></returns>
        public static List<Double2> GenerateConvexPolygon(Double2[] listpt)
        {
            if (listpt == null || listpt.Length == 0)
                return null;
            // 先生成aabb。
            Double2 min = Double2.positiveInfinity;
            Double2 max = Double2.negativeInfinity;

            foreach (Double2 pos in listpt)
            {
                min = Double2.Min(min, pos);
                max = Double2.Max(max, pos);
            }
            // 收集条边上的点，按顺时针收集。lb放置在bottom边上，rb 放置在right边上，ru顶点放置up边上, lu顶点放置在left边上
            Double2[][] edgePointArray = new Double2[4][];
            for (int i = 0; i < 4; i++)
            {
                edgePointArray[i] = new Double2[2];
                if (i < 2)
                {
                    edgePointArray[i][0] = Double2.positiveInfinity;
                    edgePointArray[i][1] = Double2.negativeInfinity;
                }
                else 
                {
                    edgePointArray[i][0] = Double2.negativeInfinity;
                    edgePointArray[i][1] = Double2.positiveInfinity;
                }
            }
            // 不在边上的顶点
            List<Double2> listOutEdgePoint = new List<Double2>();
            // 进行收集了。
            foreach (Double2 pos in listpt)
            {
                bool isInedge = false;
                // 第一条边 bottom edge
                if (pos.y == min.y)
                {
                    edgePointArray[0][0] = Double2.Min(pos, edgePointArray[0][0]);
                    edgePointArray[0][1] = Double2.Max(pos, edgePointArray[0][1]);
                    isInedge = true;
                }
                if (pos.x == max.x) //  right edge
                {
                    edgePointArray[1][0] = Double2.Min(pos, edgePointArray[1][0]);
                    edgePointArray[1][1] = Double2.Max(pos, edgePointArray[1][1]);
                    isInedge = true;
                }
                if(pos.y == max.y ) //  up edge
                {
                    edgePointArray[2][0] = Double2.Max(pos, edgePointArray[2][0]);
                    edgePointArray[2][1] = Double2.Min(pos, edgePointArray[2][1]);
                    isInedge = true;
                }
                if(pos.x == min.x) //  left edge
                {
                    edgePointArray[3][0] = Double2.Max(pos, edgePointArray[3][0]);
                    edgePointArray[3][1] = Double2.Min(pos, edgePointArray[3][1]);
                    isInedge = true;
                }
                if(isInedge == false)
                {
                    listOutEdgePoint.Add(pos);
                }
            }
            // 边上的顶点按顺时针排序了。
            // 
            List<Double2> listPoly = new List<Double2>();
            Double2 startPoint ;
            Double2 endPoint = Double2.negativeInfinity;
            for (int i = 0; i < 4; i++)
            {
                startPoint = edgePointArray[i][0];
                if (startPoint != endPoint)
                {
                    listPoly.Add(startPoint);
                }
                endPoint = edgePointArray[i][1];
                if (startPoint != endPoint)
                {
                    listPoly.Add(endPoint);
                }
                if (endPoint != edgePointArray[(i + 1) % 4][0])
                {
                    List<Double2> llinkPoint = SearchOutPoint(i, endPoint, edgePointArray[(i + 1) % 4][0], listOutEdgePoint);
                    if (llinkPoint != null && llinkPoint.Count > 0)
                    {
                        listPoly.AddRange(llinkPoint);
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                edgePointArray[i] = null;
            }
            edgePointArray = null;
            if (listPoly.Count > 1)
            {
                if (listPoly[0] == listPoly[listPoly.Count - 1])
                {
                    listPoly.RemoveAt(listPoly.Count - 1);
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
        private static List<Double2> SearchOutPoint(int edgeIndex, Double2 s, Double2 e, List<Double2> listOutEdgePoint)
        {
            if (listOutEdgePoint == null || listOutEdgePoint.Count == 0)
                return null;

            Double2 outdir;
            Double2 indir;
            if (edgeIndex == 0)
            {
                if (s.y == e.y)
                    return null;
                outdir = Double2.right;
                indir = (e - s).normalized;
            }
            else if (edgeIndex == 1)
            {
                if (s.x == e.x)
                    return null;
                outdir = Double2.up;
                indir = (e - s).normalized;
            }
            else if (edgeIndex == 2)
            {
                if (s.y == e.y)
                    return null;
                outdir = Double2.left;
                indir = (e - s).normalized;
            }
            else if (edgeIndex == 3)
            {
                if (s.x == e.x)
                    return null;
                outdir = Double2.down;
                indir = (e - s).normalized;
            }
            else return null;
            // 开始搜寻了。
            List<Double2> lResult = new List<Double2>();
            Double2 startPoint = s;
            Double2 bestPoint = Double2.zero;
            bool isHaveBestPoint = false;
            List<Double2> listinPoints = new List<Double2>();
            List<Double2> lHave = listOutEdgePoint;

            while (lHave != null && lHave.Count > 0)
            {
                // 先过滤了。
                foreach (Double2 pos in lHave)
                {
                    if (Double2.CheckPointInCorns(pos, startPoint, indir, outdir) == true)
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
                        if (Double2.CheckPointInCorns(listinPoints[i], startPoint, (bestPoint - startPoint).normalized, outdir) == true)
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
                    listinPoints = new List<Double2>();
                    isHaveBestPoint = false;
                }
                else break;
            }
            return lResult;
        }
    }
}
