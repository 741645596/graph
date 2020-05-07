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
        public static List<Double2> OptimizationLine(Double2 lineStart, Double2 lineEnd, Double2 near, Double2 far, bool isCounterclockwiseDir, List<Double2> listMidPoint)
        {
            if (listMidPoint == null || listMidPoint.Count == 0)
                return listMidPoint;
/*
#if UNITY_EDITOR
            List<Float2> l = new List<Float2>();
            l.Add(near);
            l.AddRange(listMidPoint);
            l.Add(far);
            SkillObb.instance.TargetPoly = l.ToArray();
#endif
*/

            List<Double2> listOptimizationLine = listMidPoint;
            // 先顺序来一次优化
            Double2 outdir = (lineEnd - lineStart).normalized;
            if (isCounterclockwiseDir == false)
            {
                outdir = Double2.Rotate(outdir, MathUtil.kPI - MathUtil.kEpsilon);
            }
            else
            {
                outdir = Double2.Rotate(outdir, -MathUtil.kPI + MathUtil.kEpsilon);
            }
            listOptimizationLine = SearchOutPoint(lineStart, far, outdir.normalized, listOptimizationLine);
            // 再倒序来一次优化
            listOptimizationLine.Reverse();
            outdir = (lineStart - lineEnd).normalized;
            if (isCounterclockwiseDir == true)
            {
                outdir = Double2.Rotate(outdir, MathUtil.kPI - MathUtil.kEpsilon);
            }
            else
            {
                outdir = Double2.Rotate(outdir, -MathUtil.kPI + MathUtil.kEpsilon);
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
        private static List<Double2> SearchOutPoint(Double2 lineStart, Double2 far, Double2 outdir, List<Double2> listOutEdgePoint)
        {
            if (listOutEdgePoint == null || listOutEdgePoint.Count == 0)
                return listOutEdgePoint;
            // 开始搜寻了。
            List<Double2> lResult = new List<Double2>();
            Double2 startPoint = lineStart;
            Double2 bestPoint = Double2.zero;
            Double2 indir = far - lineStart;
            bool isHaveBestPoint = false;
            List<Double2> listinPoints = new List<Double2>();
            List<Double2> lHave = listOutEdgePoint;

            while (lHave != null && lHave.Count > 0)
            {
                // 先过滤了。
                foreach (Double2 pos in lHave)
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
                        if (CheckPointInCorns(listinPoints[i], startPoint, bestPoint - startPoint, outdir) == true)
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
                    listinPoints = new List<Double2>();
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
        /// <param name="indir"></param>
        /// <param name="outdir"></param>
        /// <returns></returns>
        private static bool CheckPointInCorns(Double2 target, Double2 startPoint, Double2 indir, Double2 outdir)
        {
            Double2 diff = target - startPoint;
            if (diff == Double2.zero)
                return false;

            double ret = Double2.Cross(outdir, diff) * Double2.Cross(indir.normalized, diff);
            if (ret < 0)
            {
                // 添加异常处理,防止在反方向
                /*if (Float2.Dot(diff, indir.normalized + outdir) < 0)
                    return false;
                else return true;*/
                return true;
            }  
            else if (ret == 0)
            {
                if (Double2.Dot(diff, indir) <= 0)
                    return false;

                if (indir.sqrMagnitude < diff.sqrMagnitude)
                    return true;
                else return false;
            }
            return false;
        }
    }
}
