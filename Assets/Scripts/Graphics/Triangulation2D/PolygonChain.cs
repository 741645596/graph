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
        /// 链的特点
        /// </summary>
        private PolygonChainType m_Type;
        /// <summary>
        /// 顶点数量
        /// </summary>
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
        private List<ScanLine> listScanLine = new List<ScanLine>();
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
        public PolygonChain(List<VertexInfo> listPts)
        {
            int totalCount = listPts.Count;
            for (int i = 0; i < totalCount; i++)
            {
                VertexInfo v = listPts[i];
                v.vType = VertexType.Normal;
                // 在多边形链中的索引
                v.indexInPolygonChain = i;
                int prev = (i - 1) < 0 ? totalCount - 1 : (i - 1);
                int next = (i + 1) >= totalCount ? 0 : (i + 1);
                v.vType = GetPointVertexType(listPts[prev], v, listPts[next]);
                SetPolygonChainType(v.vType);
                AddPoints(v);
            }
            CombineScanLine();
            this.countPonts = listPoints.Count;
        }
        /// <summary>
        /// 设置多边形链的类型
        /// </summary>
        /// <param name="type"></param>
        private void SetPolygonChainType(VertexType type)
        {
            if (type == VertexType.UpCorner)
            {
                if (m_Type == PolygonChainType.DownCorner)
                {
                    m_Type = PolygonChainType.Both;
                }
                else
                {
                    m_Type = PolygonChainType.UpCorner;
                }
            }
            else if (type == VertexType.DownCorner)
            {
                if (m_Type == PolygonChainType.UpCorner)
                {
                    m_Type = PolygonChainType.Both;
                }
                else
                {
                    m_Type = PolygonChainType.DownCorner;
                }
            }
        }
        /// <summary>
        /// 合并扫描线
        /// </summary>
        private void CombineScanLine()
        {
            foreach (ScanLine v in listScanLine)
            {
                v.Combine(this);
            }
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
        /// 测试分割链条
        /// </summary>
        /// <returns></returns>
        public List<List<Float2>> TestPolygon()
        {
            List<Index2> listDiagonal = TriangulationDiagonal();
            List<List<Float2>> list = new List<List<Float2>>();
            GeneralTestPolygon(listDiagonal, this.listPoints, ref list);
            return list;
        }
        /// <summary>
        /// 测试分割链条
        /// </summary>
        /// <returns></returns>
        public List<Index2> TestDiagonal()
        {
            return TriangulationDiagonal();
        }
        /// <summary>
        /// 得到剖分对象线
        /// </summary>
        /// <returns></returns>
        private List<Index2> TriangulationDiagonal()
        {
            List<Index2> listDiagonal = new List<Index2>();
            SweepLineStatus sls = new SweepLineStatus();
            int count = listScanLine.Count;
            if (m_Type == PolygonChainType.Both || m_Type == PolygonChainType.UpCorner)
            {
                // y 从大到小
                for (int i = count - 1; i >= 0; i--)
                {
                    sls.UpdatePoints(listScanLine[i], ref listDiagonal, this, true);
                }
            }
            else 
            {
                // y 从小到大
                for (int i = 0; i < count; i++)
                {
                    sls.UpdatePoints(listScanLine[i], ref listDiagonal, this, false);
                }
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
            if (listDiagonal == null || listDiagonal.Count == 0)
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
        /// 通过分隔对角线得到单调多边形
        /// </summary>
        /// <param name="listDiagonal"></param>
        /// <param name="listMp"></param>
        private void GeneralTestPolygon(List<Index2> listDiagonal, List<VertexInfo> listPts, ref List<List<Float2>> listMp)
        {
            if (listMp == null)
            {
                listMp = new List<List<Float2>>();
            }
            if (listDiagonal == null || listDiagonal.Count == 0)
            {
                List<Float2> l = new List<Float2>();
                foreach (VertexInfo v in listPts)
                {
                    l.Add(v.pos);
                }
                listMp.Add(l);
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
                    GeneralTestPolygon(listDiagonal1, listCut1, ref listMp);
                    GeneralTestPolygon(listDiagonal2, listCut2, ref listMp);
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
        /// <returns>得到的是决定的顶点序列</returns>
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
            if (listPts == null || listPts.Count == 0)
                return -1;
            int count = listPts.Count;
            for (int i = 0; i < count; i++)
            {
                if (listPts[i].fixIndex == vertexIndex)
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
            int count = this.listScanLine.Count;
            if (count == 0)
            {
                this.listScanLine.Add(new ScanLine(v));
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
                float minYvalue = this.listScanLine[minIndex].y;
                float maxYvalue = this.listScanLine[maxIndex].y;
                if (minYvalue < targetValueY && maxYvalue > targetValueY)
                {
                    int middle = (minIndex + maxIndex) / 2;
                    if (middle == minIndex)
                    {
                        this.listScanLine.Insert(minIndex + 1, new ScanLine(targetValue));
                        return;
                    }
                    else
                    {
                        float minddleY = this.listScanLine[middle].y;
                        if (targetValueY == minddleY)
                        {
                            this.listScanLine[middle].AddPoints(targetValue);
                            return;
                        }
                        else if (targetValueY > minddleY)
                        {
                            minIndex = middle;
                        }
                        else if (targetValueY < minddleY)
                        {
                            maxIndex = middle;
                        }
                    }
                }
                else if (minYvalue == targetValueY)
                {
                    this.listScanLine[minIndex].AddPoints(targetValue);
                    return;
                }
                else if (maxYvalue == targetValueY)
                {
                    this.listScanLine[maxIndex].AddPoints(targetValue);
                    return;
                }
                else if (minYvalue > targetValueY)
                {
                    this.listScanLine.Insert(0, new ScanLine(targetValue));
                    return;
                }
                else if (maxYvalue < targetValueY)
                {
                    this.listScanLine.Add(new ScanLine(targetValue));
                    return;
                }
            }
        }
        /// <summary>
        /// 获取顶点数据,安全的获取
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VertexInfo GetVertexInfo(int index)
        {
            if (index >= 0 && index < listPoints.Count)
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
        /// <summary>
        /// 查找边的另外一个顶点，边的edge 连接的
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public VertexInfo GetOtherPoints(Edge edge)
        {
            int endIndex = edge.end.indexInPolygonChain;
            int startIndex = edge.start.indexInPolygonChain;
            int diff = System.Math.Abs(endIndex - startIndex);
            if (diff == 1)
            {
                if (endIndex > startIndex)
                {
                    return GetVertexInfo(endIndex + 1);
                }
                else
                {
                    return GetVertexInfo(endIndex - 1);
                }
            }
            else  // 说明end 要不是0 ，要不是最后一个点
            {
                if (endIndex > startIndex)
                {
                    return GetVertexInfo(endIndex - 1);
                }
                else
                {
                    return GetVertexInfo(endIndex + 1);
                }
            }
        }
        /// <summary>
        /// 判断是同一条边上的点
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public bool CheckSameEdge(int index1, int index2)
        {
            int diff = System.Math.Abs(index1 - index2);
            if (diff == 1 || diff == this.countPonts - 1)
                return true;
            else return false;
        }
    }

    /// <summary>
    /// 多边形链特点
    /// </summary>
    public enum PolygonChainType
    {
        None       = 0,  // 不存在，也就是单调链了
        UpCorner   = 1,   // 向上拐 ^
        DownCorner = 2,   // 向下拐
        Both       = 3,   // 都存在
    }
}

