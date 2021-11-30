using RayGraphics.Math;
using System.Collections.Generic;


namespace RayGraphics.Triangulation
{
    /// <summary>
    /// 多边形链
    /// </summary>
    public class PolygonChain
    {
        /// <summary>
        /// 最高点索引
        /// </summary>
        private int maxYIndex;
        private int countPonts;
        /// <summary>
        /// 顶点数量
        /// </summary>
        public int CountPoints
        {
            get { return this.CountPoints; }
        }
        /// <summary>
        /// 对所用顶点进行按Y从低到高进行排序
        /// </summary>
        private List<YPoints> listYPoints = new List<YPoints>();
        /// <summary>
        /// 顶点列表
        /// </summary>
        private List<VertexInfo> listPoints = new List<VertexInfo>();
        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="minYIndex"></param>
        /// <param name="maxYIndex"></param>
        /// <param name="listPts"></param>
        public PolygonChain(int maxYIndex, List<VertexInfo> listPts)
        {
            this.maxYIndex = maxYIndex;
            for (int i = 0; i < maxYIndex; i++)
            {
                VertexInfo v = listPts[i];
                v.vType = VertexType.Normal;
                if (i > 0 && i < maxYIndex - 1)
                {
                    v.vType = GetPointVertexType(listPts[i - 1], v, listPts[i + 1]);
                }
                AddPoints(v);
            }
            int totalCount = listPts.Count;
            for (int i = maxYIndex; i < totalCount; i++)
            {
                VertexInfo v = listPts[i];
                v.vType = VertexType.Normal;
                if (i > maxYIndex && i < totalCount - 1)
                {
                    v.vType = GetPointVertexType(listPts[i - 1], v, listPts[i + 1]);
                }
                AddPoints(v);
            }
            this.countPonts = listPoints.Count;
        }
        /// <summary>
        /// 生成单调多边形
        /// </summary>
        /// <returns></returns>
        public List<MonotonePolygon> GeneralMonotonePolygon()
        {
            List<MonotonePolygon> listMp = new List<MonotonePolygon>();
            List<Index2> listDiagonal = TriangulationDiagonal();
            GeneralMonotonePolygon(listDiagonal, this.listPoints, ref listMp);
            return listMp;
        }
        /// <summary>
        /// 得到剖分对象线
        /// </summary>
        /// <returns></returns>
        private List<Index2> TriangulationDiagonal()
        {
            List<Index2> listDiagonal = new List<Index2>();
            SweepLineStatus sls = new SweepLineStatus();
            int count = listYPoints.Count;
            // y 从小到大
            for (int i = 0; i < count; i++)
            {
                sls.UpdatePoints(listYPoints[i].LinePoints, ref listDiagonal, this, false);
            }
            // y 从大到小
            sls.Clear();
            for (int i = count -1; i >= 0; i--)
            {
                sls.UpdatePoints(listYPoints[i].LinePoints, ref listDiagonal, this, true);
            }
            return listDiagonal;
        }
        /// <summary>
        /// 通过分隔对角线得到单调多边形
        /// </summary>
        /// <param name="listDiagonal"></param>
        /// <param name="listMp"></param>
        private void GeneralMonotonePolygon(List<Index2> listDiagonal, List<VertexInfo> listPts, ref List<MonotonePolygon> listMp)
        {
            if (listMp == null)
            {
                listMp = new List<MonotonePolygon>();
            }
            if (listDiagonal == null || listMp.Count == 0)
            {
                listMp.Add(new MonotonePolygon(listPts));
            }
            else
            {
                // 后续根据算法测试调整取值
                Index2 first = listDiagonal[0];
                listDiagonal.RemoveAt(0);
                List<VertexInfo> listCut1 = null;
                List<VertexInfo> listCut2 = null;
                if (CutPoints(first, listPts, ref listCut1, ref listCut2) == true)
                {
                    List<Index2> listDiagonal1 = GetDiagonal(ref listDiagonal, listCut1);
                    List<Index2> listDiagonal2 = GetDiagonal(ref listDiagonal, listCut2);
                    GeneralMonotonePolygon(listDiagonal1, listCut1, ref listMp);
                    GeneralMonotonePolygon(listDiagonal2, listCut2, ref listMp);
                }
            }
        }
        /// <summary>
        /// 分隔链条
        /// </summary>
        /// <param name="cutInfo"></param>
        /// <param name="listPts"></param>
        /// <param name="listCut1"></param>
        /// <param name="listCut2"></param>
        /// <returns></returns>
        private bool CutPoints(Index2 cutInfo, List<VertexInfo> listPts, ref List<VertexInfo> listCut1, ref List<VertexInfo> listCut2)
        {
            Index2 ret = FindPointsIndex(listPts, cutInfo);
            if (ret.p1 == -1 || ret.p2 == -1 || ret.p1 == ret.p2)
                return false;
            int maxIndex = System.Math.Max(ret.p1, ret.p2);
            int minIndex = System.Math.Min(ret.p1, ret.p2);
            // 分隔1
            if (listCut1 == null)
            {
                listCut1 = new List<VertexInfo>();
            }
            listCut1.Clear();
            for (int i = minIndex; i <= maxIndex; i++)
            {
                listCut1.Add(listPts[i]);
            }
            // 分隔2
            if (listCut2 == null)
            {
                listCut2 = new List<VertexInfo>();
            }
            listCut2.Clear();
            int count = listPts.Count;
            for (int i = maxIndex; i < count; i++)
            {
                listCut2.Add(listPts[i]);
            }
            for (int i = 0; i <= minIndex; i++)
            {
                listCut2.Add(listPts[i]);
            }
            return true;
        }
        /// <summary>
        /// 获取多边形中包含的对角线，并且进行剔除
        /// </summary>
        /// <returns></returns>
        private List<Index2> GetDiagonal(ref List<Index2> listDiagonal, List<VertexInfo> listPts)
        {
            List<Index2> listRet = new List<Index2>();
            if (listDiagonal != null && listDiagonal.Count > 0)
            {
                for (int i = 0; i < listDiagonal.Count;)
                {
                    Index2 v = listDiagonal[i];
                    Index2 ret = FindPointsIndex(listPts, v);
                    if (ret.p1 != -1 && ret.p2 != -1 && ret.p1 != ret.p2)
                    {
                        listRet.Add(v);
                        listDiagonal.RemoveAt(i);
                    }
                    else i++;
                }
            }
            return listRet;
        }
        /// <summary>
        /// 查找对角线所在顶点
        /// </summary>
        /// <param name="listPts"></param>
        /// <param name="Diagonal"></param>
        /// <returns></returns>
        private Index2 FindPointsIndex(List<VertexInfo> listPts, Index2 Diagonal)
        {
            int index1 = FindPointsIndex(listPts, Diagonal.p1);
            int index2 = FindPointsIndex(listPts, Diagonal.p2);
            return new Index2(index1, index2);
        }
        /// <summary>
        /// 查找索引
        /// </summary>
        /// <param name="vertexIndex"></param>
        /// <param name="listPts"></param>
        /// <returns></returns>
        private int FindPointsIndex(List<VertexInfo> listPts, int vertexIndex)
        {
            if (listPoints == null || listPts.Count == 0)
                return -1;
            int count = listPts.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.listPoints[i].index == vertexIndex)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取点的其点属性
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="cur"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private VertexType GetPointVertexType(VertexInfo prev, VertexInfo cur, VertexInfo next)
        {
            if (cur.pos.y >= next.pos.y && cur.pos.y >= prev.pos.y)
            {
                return VertexType.UpCorner;
            }
            else if (cur.pos.y <= next.pos.y && cur.pos.y <= prev.pos.y)
            {
                return VertexType.DownCorner;
            }
            else return VertexType.Normal;
        }

        /// <summary>
        /// 加入并排好序
        /// </summary>
        /// <param name="v"></param>
        public void AddPoints(VertexInfo v)
        {
            int count = this.listYPoints.Count;
            if (count == 0)
            {
                this.listYPoints.Add(new YPoints(v));
            }
            else
            {
                BinaryInsert(v, 0, count - 1);
            }
            this.listPoints.Add(v);
        }

        /// <summary>
        /// 二分插入
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        private void BinaryInsert(VertexInfo targetValue, int minIndex, int maxIndex)
        {
            float targetValueY = targetValue.pos.y;
            while (minIndex <= maxIndex)
            {
                float minYvalue = this.listYPoints[minIndex].y;
                float maxYvalue = this.listYPoints[maxIndex].y;
                if (minYvalue < targetValueY && maxYvalue > targetValueY)
                {
                    int middle = (minIndex + maxIndex) / 2;
                    if (middle == minIndex)
                    {
                        this.listYPoints.Insert(minIndex + 1, new YPoints(targetValue));
                        return;
                    }
                    else
                    {
                        float minddleY = this.listYPoints[middle].y;
                        if (targetValueY == minddleY)
                        {
                            this.listYPoints[middle].AddPoints(targetValue);
                            return;
                        }
                        else if (targetValueY > minddleY)
                        {
                            minIndex = middle + 1;
                        }
                        else if (targetValueY < minddleY)
                        {
                            maxIndex = middle - 1;
                        }
                    }
                }
                else if (minYvalue == targetValueY)
                {
                    this.listYPoints[minIndex].AddPoints(targetValue);
                    return;
                }
                else if (maxYvalue == targetValueY)
                {
                    this.listYPoints[maxIndex].AddPoints(targetValue);
                    return;
                }
                else if (minYvalue > targetValue.pos.y)
                {
                    this.listYPoints.Insert(0, new YPoints(targetValue));
                    return;
                }
                else if (maxYvalue > targetValueY)
                {
                    this.listYPoints.Add(new YPoints(targetValue));
                    return;
                }
            }
        }
        /// <summary>
        /// 获取左边
        /// </summary>
        /// <returns></returns>
        public Edge GetMinXEdge(VertexInfo targetPoint)
        {
            int index = targetPoint.index;
            int prev = (index - 1) < 0 ? countPonts - 1 : (index - 1);
            int next = (index + 1) >= countPonts ? 0 : (index + 1);
            VertexInfo p = listPoints[prev];
            if (listPoints[prev].pos.x <= targetPoint.pos.x)
            {
                return new Edge(targetPoint, p);
            }
            else
            {
                return new Edge(targetPoint, listPoints[next]);
            }
        }
        /// <summary>
        /// 获取右边
        /// </summary>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        public Edge GetMaxXEdge(VertexInfo targetPoint)
        {
            int index = targetPoint.index;
            int prev = (index - 1) < 0 ? countPonts - 1 : (index - 1);
            int next = (index + 1) >= countPonts ? 0 : (index + 1);
            VertexInfo p = listPoints[prev];
            if (listPoints[prev].pos.x >= targetPoint.pos.x)
            {
                return new Edge(targetPoint, p);
            }
            else
            {
                return new Edge(targetPoint, listPoints[next]);
            }
        }
        /// <summary>
        /// 获取顶点数据,安全的获取
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VertexInfo GetVertexInfo(int index)
        {
            if (index >= 0 || index < listPoints.Count)
            {
                return listPoints[index];
            }
            else if (index < 0)
            {
                return this.listPoints[countPonts - 1];
            }
            else
            {
                return this.listPoints[0];
            }
        }
    }
    /// <summary>
    /// 保存同一个高度
    /// </summary>
    public class YPoints
    {
        /// <summary>
        /// y的高度
        /// </summary>
        public float y;
        /// <summary>
        /// 保持x 坐标的值，及对应在多边形链表中的索引
        /// // x: 保存x坐标。 y保存索引
        /// 同时按x 从小到大进行了排序
        /// </summary>
        private List<VertexInfo> listXIndex = new List<VertexInfo>();
        public List<VertexInfo> LinePoints
        {
            get { return listXIndex; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="v"></param>
        public YPoints(VertexInfo v)
        {
            this.y = v.pos.y;
            AddPoints(v);
        }
        /// <summary>
        /// 加入并排好序
        /// </summary>
        /// <param name="v"></param>
        public void AddPoints(VertexInfo v)
        {
            int count = listXIndex.Count;
            if (count == 0)
            {
                this.y = v.pos.y;
                this.listXIndex.Add(v);
            }
            else
            {
                if (v.pos.y == this.y)
                {
                    BinaryInsert(v, 0, count - 1);
                }
            }
        }
        /// <summary>
        /// 二分查找
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        private int BinarySearch(int targetValue, int minIndex, int maxIndex)
        {
            while (minIndex <= maxIndex)
            {
                int middle = (minIndex + maxIndex) / 2;
                if (targetValue == this.listXIndex[middle].pos.x)
                {
                    return middle;
                }
                else if (targetValue > this.listXIndex[middle].pos.x)
                {
                    minIndex = middle + 1;
                }
                else if (targetValue < this.listXIndex[middle].pos.x)
                {
                    maxIndex = middle - 1;
                }
            }
            return -1;
        }
        /// <summary>
        /// 二分查找
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        private void BinaryInsert(VertexInfo targetValue, int minIndex, int maxIndex)
        {
            while (minIndex <= maxIndex)
            {
                if (this.listXIndex[minIndex].pos.x <= targetValue.pos.x && this.listXIndex[maxIndex].pos.x >= targetValue.pos.x)
                {
                    int middle = (minIndex + maxIndex) / 2;

                    if (middle == minIndex)
                    {
                        this.listXIndex.Insert(minIndex + 1, targetValue);
                        return;
                    }
                    else
                    {
                        if (this.listXIndex[middle].pos.x <= targetValue.pos.x)
                        {
                            minIndex = middle;
                        }
                        else
                        {
                            maxIndex = middle;
                        }
                    }
                }
                else if (this.listXIndex[minIndex].pos.x > targetValue.pos.x)
                {
                    this.listXIndex.Insert(0, targetValue);
                    return;
                }
                else
                {
                    this.listXIndex.Add(targetValue);
                    return;
                }
            }
        }
        /// <summary>
        /// 得到梯形列表
        /// </summary>
        /// <returns></returns>
        public List<Trapezoid> GetTrapezoid(List<VertexInfo> listPoints)
        {
            List<Trapezoid> listRet = new List<Trapezoid>();
            int TotalPts = listPoints.Count;
            int count = this.listXIndex.Count;
            for (int i = 0; i < count;)
            {
                VertexInfo l = this.listXIndex[i];
                if (i < count - 1 && CheckCanCombine(l, this.listXIndex[i + 1]) == true)
                {
                    VertexInfo r = this.listXIndex[i + 1];
                    listRet.Add(new Trapezoid(
                        new Edge(l, listPoints[l.index == 0 ? (TotalPts - 1) : (l.index - 1)]),
                        new Edge(r, listPoints[r.index == TotalPts - 1 ? 0 : (r.index + 1)]),
                        l));
                    i += 2;
                }
                else
                {
                    listRet.Add(new Trapezoid(
                             new Edge(l, listPoints[l.index == 0 ? (TotalPts - 1) : (l.index - 1)]),
                             new Edge(l, listPoints[l.index == TotalPts - 1 ? 0 : (l.index + 1)]),
                             l));
                    i++;
                }
            }
            return listRet;
        }
        /// <summary>
        /// 特定的要求设定
        /// 判断梯形顶点的合并
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool CheckCanCombine(VertexInfo a, VertexInfo b)
        {
            int diff = a.index - b.index;
            if (diff == 1 || diff == -1)
                return true;
            else return false;
        }
    }
}

