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
        public static List<Float2> GenerateConvexPolygon(Float2[] listpt)
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

    }
}
