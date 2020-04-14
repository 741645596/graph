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
            List<Float2> listResult = new List<Float2>();
            if (listMidPoint == null || listMidPoint.Count == 0)
                return listResult;
            int nearestIndex = -1;
            int farestIndex = -1;
            CalcNearFarest(lineStart, lineEnd,listMidPoint, ref nearestIndex, ref farestIndex);
            if (nearestIndex >= 0 && nearestIndex < listMidPoint.Count)
            {
                Float2 nearPoint = listMidPoint[nearestIndex];
                // step1
                List<Float2> list = new List<Float2>();
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
                    list = new List<Float2>();
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
                    Float2 farPoint = listMidPoint[farestIndex];
                    list = new List<Float2>();
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
                    list = new List<Float2>();
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
                    Float2 keyPoint = listMidPoint[farestIndex];
                    // step1
                    List<Float2> list = new List<Float2>();
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
                    list = new List<Float2>();
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
                    listResult = OptimizationStepLine(lineStart, lineEnd, near, far, isCounterclockwiseDir, listMidPoint);
                }
            }
            return listResult;
        }


        private static List<Float2> OptimizationStepLine(Float2 lineStart, Float2 lineEnd, Float2 near, Float2 far, bool isCounterclockwiseDir, List<Float2> listMidPoint)
        {
            if (listMidPoint == null || listMidPoint.Count == 0)
                return listMidPoint;

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

            outdir = (lineStart - lineEnd).normalized;
            if (isCounterclockwiseDir == true)
            {
                outdir = Float2.Rotate(outdir, MathUtil.kPI - MathUtil.kEpsilon);
            }
            else
            {
                outdir = Float2.Rotate(outdir, -MathUtil.kPI + MathUtil.kEpsilon);
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
                for (int i = 0; i < lHave.Count; i++)
                {
                    Float2 pos = lHave[i];
                    if (Float2.CheckPointInCorns(pos, startPoint, indir, outdir) == true)
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
                        if (Float2.CheckPointInCorns(listinPoints[i], startPoint, bestPoint - startPoint, outdir) == true)
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
                        listinPoints = new List<Float2>();
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
        private static void CalcNearFarest(Float2 lineStart, Float2 lineEnd, List<Float2> listMidPoint, ref int nearestIndex, ref int farestIndex)
        {
            if (listMidPoint == null || listMidPoint.Count == 0)
                return;
            nearestIndex = -1;
            farestIndex = -1;
            float outStartdis = 0;
            float outEnddis = 0;
            LineSegment2D line = new LineSegment2D(lineStart, lineEnd);
            for (int i = 0; i < listMidPoint.Count; i++)
            {
                ProjectPointInLine  pp=line.CheckProjectInLine(listMidPoint[i]);
                if (pp == ProjectPointInLine.In)
                    continue;
                Float2 projectPoint = line.ProjectPoint(listMidPoint[i]);
                if (pp == ProjectPointInLine.OutStart)
                {
                    float dis = (projectPoint - lineStart).sqrMagnitude;
                    if (dis > outStartdis)
                    {
                        outStartdis = dis;
                        nearestIndex = i;
                    }
                }
                else if (pp == ProjectPointInLine.OutStart)
                {
                    float dis = (projectPoint - lineEnd).sqrMagnitude;
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
