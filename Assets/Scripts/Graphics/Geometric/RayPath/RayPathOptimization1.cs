using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    /// <summary>
    /// bake 版本
    /// </summary>
    public class RayPathOptimization1
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
            List<Double2> listResult = new List<Double2>();
            if (listMidPoint == null || listMidPoint.Count == 0)
                return listResult;
            int nearestIndex = -1;
            int farestIndex = -1;
            CalcNearFarest(lineStart, lineEnd,listMidPoint, ref nearestIndex, ref farestIndex);
            if (nearestIndex >= 0 && nearestIndex < listMidPoint.Count)
            {
                Double2 nearPoint = listMidPoint[nearestIndex];
                // step1
                List<Double2> list = new List<Double2>();
                for (int i = 0; i < nearestIndex; i++)
                {
                    list.Add(listMidPoint[i]);
                }
                list = OptimizationStepLine(lineStart, nearPoint, lineStart, nearPoint, isCounterclockwiseDir, list);
                if (list != null && list.Count > 0)
                {
                    listResult.AddRange(list);
                }
                //
                listResult.Add(nearPoint);

                if (farestIndex == -1 || farestIndex <= nearestIndex || farestIndex >= listMidPoint.Count)
                {
                    list = new List<Double2>();
                    for (int i = nearestIndex + 1; i < listMidPoint.Count; i++)
                    {
                        list.Add(listMidPoint[i]);
                    }
                    list = OptimizationStepLine(nearPoint, lineEnd, nearPoint, lineEnd, isCounterclockwiseDir, list);
                    if (list != null && list.Count > 0)
                    {
                        listResult.AddRange(list);
                    }
                }
                else
                {
                    Double2 farPoint = listMidPoint[farestIndex];
                    list = new List<Double2>();
                    for (int i = nearestIndex + 1; i < farestIndex; i++)
                    {
                        list.Add(listMidPoint[i]);
                    }
                    list = OptimizationStepLine(nearPoint, farPoint, nearPoint, farPoint, isCounterclockwiseDir, list);
                    if (list != null && list.Count > 0)
                    {
                        listResult.AddRange(list);
                    }
                    listResult.Add(farPoint);
                    // step3
                    list = new List<Double2>();
                    for (int i = farestIndex + 1; i < listMidPoint.Count; i++)
                    {
                        list.Add(listMidPoint[i]);
                    }
                    list = OptimizationStepLine(farPoint, lineEnd, farPoint, lineEnd, isCounterclockwiseDir, list);
                    if (list != null && list.Count > 0)
                    {
                        listResult.AddRange(list);
                    }
                }
            }
            else 
            {
                if (farestIndex >= 0 && farestIndex <= listMidPoint.Count - 1)
                {
                    Double2 keyPoint = listMidPoint[farestIndex];
                    // step1
                    List<Double2> list = new List<Double2>();
                    for (int i = 0; i < farestIndex; i++)
                    {
                        list.Add(listMidPoint[i]);
                    }
                    list = OptimizationStepLine(lineStart, keyPoint, near, keyPoint, isCounterclockwiseDir, list);
                    if (list != null && list.Count > 0)
                    {
                        listResult.AddRange(list);
                    }
                    //
                    listResult.Add(keyPoint);
                    // step2
                    list = new List<Double2>();
                    for (int i = farestIndex + 1; i < listMidPoint.Count; i++)
                    {
                        list.Add(listMidPoint[i]);
                    }
                    list = OptimizationStepLine(keyPoint, lineEnd, keyPoint, lineEnd, isCounterclockwiseDir, list);
                    if (list != null && list.Count > 0)
                    {
                        listResult.AddRange(list);
                    }
                }
                else
                {
                    listResult = OptimizationStepLine(lineStart, lineEnd, lineStart, lineEnd, isCounterclockwiseDir, listMidPoint);
                }
            }
            return listResult;
        }


        private static List<Double2> OptimizationStepLine(Double2 lineStart, Double2 lineEnd, Double2 near, Double2 far, bool isCounterclockwiseDir, List<Double2> listMidPoint)
        {
            if (listMidPoint == null || listMidPoint.Count == 0)
                return listMidPoint;

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

            outdir = (lineStart - lineEnd).normalized;
            if (isCounterclockwiseDir == true)
            {
                outdir = Double2.Rotate(outdir, MathUtil.kPI - MathUtil.kEpsilon);
            }
            else
            {
                outdir = Double2.Rotate(outdir, -MathUtil.kPI + MathUtil.kEpsilon);
            }
            listOptimizationLine.Reverse();
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
                //SkillObb.instance.AdddirData(startPoint, outdir, indir);
                // 先过滤了。
                for (int i = 0; i < lHave.Count; i++)
                {
                    Double2 pos = lHave[i];
                    if (Double2.CheckPointInCorns(pos, startPoint, indir, outdir) == true)
                    {
                        bestPoint = pos;
                        isHaveBestPoint = true;
                        for (int k = i; k < lHave.Count; k++)
                        {
                            listinPoints.Add(lHave[k]);
                        }
                        break;
                    }
                }
                // 再则优。
                if (isHaveBestPoint == true)
                {
                    for (int i = 1; i < listinPoints.Count; i++)
                    {
                        // 比较更好的点。
                        if (Double2.CheckPointInCorns(listinPoints[i], startPoint, bestPoint - startPoint, outdir) == true)
                        {
                            bestPoint = listinPoints[i];
                        }
                    }
                    // 进行交换，执行下一轮迭代。
                    lResult.Add(bestPoint); 
                    listinPoints.Remove(bestPoint);
                    if (listinPoints.Count > 0)
                    {
                        outdir = (bestPoint - startPoint).normalized;
                        startPoint = bestPoint;
                        indir = far - startPoint;
                        lHave = listinPoints;
                        listinPoints = new List<Double2>();
                        isHaveBestPoint = false;
                    }
                    else break;
                }
                else break;
            }
            return lResult;
        }
        /// <summary>
        /// 计算超出线段范围
        /// </summary>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <param name="listMidPoint"></param>
        /// <param name="nearestIndex"></param>
        /// <param name="farestIndex"></param>
        private static void CalcNearFarest(Double2 lineStart, Double2 lineEnd, List<Double2> listMidPoint, ref int nearestIndex, ref int farestIndex)
        {
            if (listMidPoint == null || listMidPoint.Count == 0)
                return;
            nearestIndex = -1;
            farestIndex = -1;
            double outStartdis = 0;
            double outEnddis = 0;
            LineSegment2D line = new LineSegment2D(lineStart, lineEnd);
            for (int i = 0; i < listMidPoint.Count; i++)
            {
                ProjectPointInLine  pp=line.CheckProjectInLine(listMidPoint[i]);
                if (pp == ProjectPointInLine.In)
                    continue;
                Double2 projectPoint = line.ProjectPoint(listMidPoint[i]);
                if (pp == ProjectPointInLine.OutStart)
                {
                    double dis = (projectPoint - lineStart).sqrMagnitude;
                    if (dis > outStartdis)
                    {
                        outStartdis = dis;
                        nearestIndex = i;
                    }
                }
                else if (pp == ProjectPointInLine.OutStart)
                {
                    double dis = (projectPoint - lineEnd).sqrMagnitude;
                    if (dis > outEnddis)
                    {
                        outEnddis = dis;
                        farestIndex = i;
                    }
                }
            }
        }
    }

}
