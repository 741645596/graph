using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    public class RayPathOptimization
    {
        /// <summary>
        /// 优化路线
        /// </summary>
        /// <param name="lineStart">线段起点</param>
        /// <param name="lineEnd">线段终点</param>
        /// <param name="near">与动态挡格相交 near点</param>
        /// <param name="far">与动态挡格相交 far点</param>
        /// <param name="listMidPoint">2个交点之间的路线</param>
        /// <returns></returns>
        public static List<Float2> OptimizationLine(Float2 lineStart, Float2 lineEnd, Float2 near, Float2 far, bool isCounterclockwiseDir, List<Float2> listMidPoint)
        {
            if (listMidPoint == null || listMidPoint.Count == 0)
                return listMidPoint;
            List<Float2> listOptimizationLine = listMidPoint;
            // 先顺序来一次优化
            Float2 outdir = (lineEnd - lineStart).normalized;
            if (isCounterclockwiseDir == false)
            {
                outdir = Float2.Rotate(outdir, MathUtil.kPI - 0.1f);
            }
            else
            {
                outdir = Float2.Rotate(outdir, -MathUtil.kPI + 0.1f);
            }
            listOptimizationLine = SearchOutPoint(lineStart, far, outdir.normalized, listOptimizationLine);
            // 再倒序来一次优化
            listOptimizationLine.Reverse();
            outdir = (lineStart - lineEnd).normalized;
            if (isCounterclockwiseDir == true)
            {
                outdir = Float2.Rotate(outdir, MathUtil.kPI - 0.1f);
            }
            else
            {
                outdir = Float2.Rotate(outdir, -MathUtil.kPI + 0.1f);
            }
            listOptimizationLine = SearchOutPoint(lineEnd, near, outdir.normalized, listOptimizationLine);
            listOptimizationLine.Reverse();
            //
            return listOptimizationLine;

        }
        /// <summary>
        /// 获取最边缘一圈的顶点。
        /// </summary>
        /// <param name="lineStart">起点</param>
        /// <param name="far">远处的交点</param>
        /// <param name="outdir">外包边</param>
        /// <param name="listOutEdgePoint"></param>
        /// <returns></returns>
        private static List<Float2> SearchOutPoint(Float2 lineStart, Float2 far, Float2 outdir, List<Float2> listOutEdgePoint)
        {
            if (listOutEdgePoint == null || listOutEdgePoint.Count == 0)
                return null;
            // 开始搜寻了。
            List<Float2> lResult = new List<Float2>();
            Float2 startPoint = lineStart;
            Float2 bestPoint = Float2.zero;
            Float2 indir = (far - lineStart).normalized;
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
                    outdir = (bestPoint - startPoint).normalized;
                    startPoint = bestPoint;
                    indir = (far - startPoint).normalized;
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

            float ret = Float2.Cross(outdir, diff) * Float2.Cross(indir, diff);
            if (ret < 0) return true;
            return false;
        }
    }
}
