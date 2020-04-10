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

#if UNITY_EDITOR
 //           List<Float2> l = new List<Float2>();
 //           l.Add(near);
 //           l.AddRange(listMidPoint);
 //           l.Add(far);
 //           SkillObb.instance.TargetPoly = l.ToArray();
#endif


            List<Float2> listOptimizationLine = listMidPoint;
            // 先顺序来一次优化
            Float2 outdir = (lineEnd - lineStart).normalized;
            if (isCounterclockwiseDir == false)
            {
                outdir = Float2.Rotate(outdir, MathUtil.kPI - MathUtil.kEpsilon);
            }
            else
            {
                outdir = Float2.Rotate(outdir, -MathUtil.kPI + MathUtil.kEpsilon);
            }
            listOptimizationLine = SearchOutPoint(lineStart, far, outdir.normalized, listOptimizationLine);
            // 再倒序来一次优化
            listOptimizationLine.Reverse();
            outdir = (lineStart - lineEnd).normalized;
            if (isCounterclockwiseDir == true)
            {
                outdir = Float2.Rotate(outdir, MathUtil.kPI - MathUtil.kEpsilon);
            }
            else
            {
                outdir = Float2.Rotate(outdir, -MathUtil.kPI + MathUtil.kEpsilon);
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
                return listOutEdgePoint;
            // 开始搜寻了。
            List<Float2> lResult = new List<Float2>();
            Float2 startPoint = lineStart;
            Float2 bestPoint = Float2.zero;
            Float2 indir = far - lineStart;
            bool isHaveBestPoint = false;
            List<Float2> listinPoints = new List<Float2>();
            List<Float2> lHave = listOutEdgePoint;

            while (lHave != null && lHave.Count > 0)
            {
                // 先过滤了。
                foreach (Float2 pos in lHave)
                {
                    if (Float2.CheckPointInCorns(pos, startPoint, indir, outdir) == true)
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
                        if (Float2.CheckPointInCorns(listinPoints[i], startPoint, bestPoint - startPoint, outdir) == true)
                        {
                            bestPoint = listinPoints[i];
                        }
                    }
                    // 进行交换，执行下一轮迭代。
                    lResult.Add(bestPoint);
                    outdir = (bestPoint - startPoint).normalized;
                    startPoint = bestPoint;
                    indir = far - startPoint;
                    listinPoints.Remove(bestPoint);
                    lHave = listinPoints;
                    listinPoints = new List<Float2>();
                    isHaveBestPoint = false;
                }
                else break;
            }
            return lResult;
        }
    }

}
