using System.Collections.Generic;
using RayGraphics.Math;
using RayGraphics.Geometric;


namespace RayGraphics.Voxelization
{
    public class ContourOptimization
    {
        /// <summary>
        /// 生成外轮廓多边形
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="limitDistance"></param>
        /// <returns></returns>
        public static ContourUnit GeneralContourUnit(RLEGridMap grid, MapElement me, float limitDistance)
        {
            ContourPoly poly = GeneralOutPoly(grid, me, me.GetKeyPoint(), limitDistance, false);
            ContourUnit unit = new ContourUnit(poly); ;
            if (me is MapArea)
            {
                Float2 keyPoint = Float2.zero;
                while (poly.FindKeyPoint(grid, ref keyPoint) == true)
                {
                    ContourPoly poly1 = GeneralOutPoly(grid, me, keyPoint, limitDistance, true);
                    unit.AddBlockPoly(poly1);
                }
            }
            return unit;
        }
        /// <summary>
        /// 生成外轮廓多边形
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="me"></param>
        /// <param name="startSearchPoint"></param>
        /// <param name="limitDistance"></param>
        /// <param name="isBlockOutPoly">是否为挡格的轮廓多边形,是的时候需要倒序处理</param>
        /// <returns></returns>
        private static ContourPoly GeneralOutPoly(RLEGridMap grid, MapElement me, Float2 startSearchPoint, float limitDistance, bool isBlockOutPoly)
        {
            ContourPoly poly = new ContourPoly();
            List<Double2> points = me.GetContourPoints(grid, startSearchPoint);
            if (isBlockOutPoly == true)
            {
                points.Reverse();
            }
            List<int> listIndex = ConvexPolygon2D.GenerateConvexPolygonIndex(points.ToArray());
            List<Double2> polys = new List<Double2>();
            for (int i = 0; i < listIndex.Count; i++)
            {
                if (i < listIndex.Count - 1)
                {
                    OptimizationContourConvexLine(listIndex[i], listIndex[i + 1], false, limitDistance, points, ref polys);
                }
                else
                {
                    OptimizationContourConvexLine(listIndex[i], listIndex[0], true, limitDistance, points, ref polys);
                }
            }
            poly.InitData(polys);
            return poly;
        }
        /// <summary>
        /// 凸边进行轮廓优化
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="isEnd"></param>
        /// <param name="limitDistance"></param>
        /// <param name="points"></param>
        /// <param name="polys"></param>
        private static void OptimizationContourConvexLine(int startIndex, int endIndex, bool isEnd, float limitDistance, List<Double2> points, ref List<Double2> polys)
        {
            if (points == null || points.Count == 0)
                return;
            if (polys == null)
            {
                polys = new List<Double2>();
            }
            int count = points.Count;
            List<Double2> listTemp = new List<Double2>();
            if (startIndex < endIndex)
            {
                for (int k = startIndex; k <= endIndex; k++)
                {
                    listTemp.Add(points[k]);
                }
            }
            else
            {
                for (int k = startIndex; k < count; k++)
                {
                    listTemp.Add(points[k]);
                }
                for (int k = 0; k <= endIndex; k++)
                {
                    listTemp.Add(points[k]);
                }
            }
            List<Double2> list = OptimizationContourNormalLine(listTemp, limitDistance);
            if (list != null && list.Count > 0)
            {
                if (polys.Count > 0)
                {
                    if (list[0] == polys[polys.Count - 1])
                    {
                        list.RemoveAt(0);
                    }
                }
                if (isEnd == true)
                {
                    if (polys.Count > 0)
                    {
                        if (list[list.Count - 1] == polys[0])
                        {
                            list.RemoveAt(list.Count - 1);
                        }
                    }
                }
                polys.AddRange(list);
            }
        }
        /// <summary>
        /// 优化轮廓线
        /// </summary>
        /// <param name="listContour"></param>
        /// <param name="limitDistance"></param>
        /// <returns></returns>
        private static List<Double2> OptimizationContourNormalLine(List<Double2> listContour, float limitDistance)
        {
            if (listContour == null || listContour.Count < 2)
                return null;
            else if (listContour.Count == 2)
                return listContour;
            else
            {
                List<int> listIndex = new List<int>();
                OptimizationContour(listContour, 0, listContour.Count - 1, limitDistance, ref listIndex);
                List<Double2> listResult = new List<Double2>();
                foreach (int index in listIndex)
                {
                    listResult.Add(listContour[index]);
                }
                return listResult;
            }
        }
        /// <summary>
        /// 优化轮廓线
        /// https://www.cnblogs.com/Hichy/p/9149055.html 使用Douglas-Peucker 轨迹压缩算法
        /// </summary>
        /// <param name="listContour"></param>
        /// <returns></returns>
        private static void OptimizationContour(List<Double2> listContour, int startIndex, int endIndex, float limitDistance, ref List<int> listIndex)
        {
            // 首先判断合法性。
            if (listContour == null || listContour.Count < 2 || startIndex < 0 || endIndex >= listContour.Count || endIndex <= startIndex)
                return;
            if (listIndex == null)
            {
                listIndex = new List<int>();
            }
            //建立线段
            double maxDistance = 0;
            int index = 0;
            LineSegment2D line = new LineSegment2D(listContour[startIndex], listContour[endIndex]);
            for (int i = startIndex + 1; i < endIndex; i++)
            {
                double distance = line.CalcDistance(listContour[i]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    index = i;
                }
            }

            if (maxDistance > limitDistance)
            {
                // 分成2段处理
                OptimizationContour(listContour, startIndex, index, limitDistance, ref listIndex);
                OptimizationContour(listContour, index, endIndex, limitDistance, ref listIndex);
            }
            else
            {
                if (listIndex.Contains(startIndex) == false)
                {
                    listIndex.Add(startIndex);
                }
                if (listIndex.Contains(endIndex) == false)
                {
                    listIndex.Add(endIndex);
                }
            }
        }

    }
}

