using System.Collections.Generic;


namespace RayGraphics.Triangulation
{
    /// <summary>
    /// 扫描线结构体
    /// </summary>
    public class ScanLine
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
        public ScanLine(VertexInfo v)
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
        /// 二分插入
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        private void BinaryInsert(VertexInfo targetValue, int minIndex, int maxIndex)
        {
            float targetValueX = targetValue.pos.x;
            while (minIndex <= maxIndex)
            {
                if (this.listXIndex[minIndex].pos.x <= targetValueX && this.listXIndex[maxIndex].pos.x >= targetValueX)
                {
                    int middle = (minIndex + maxIndex) / 2;

                    if (middle == minIndex)
                    {
                        this.listXIndex.Insert(minIndex + 1, targetValue);
                        return;
                    }
                    else
                    {
                        if (this.listXIndex[middle].pos.x <= targetValueX)
                        {
                            minIndex = middle;
                        }
                        else
                        {
                            maxIndex = middle;
                        }
                    }
                }
                else if (this.listXIndex[minIndex].pos.x > targetValueX)
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
        /// 对能合并的点进行合并
        /// </summary>
        public void Combine(PolygonChain parent)
        {
            if (listXIndex == null || listXIndex.Count < 2)
                return;
            for (int i = 0; i < listXIndex.Count - 1; i++)
            {
                // 不用采取连续合并的策略
                if (CombineVertex.CheckCanCombine(listXIndex[i], listXIndex[i + 1], parent) == true)
                {
                    listXIndex[i] = new CombineVertex(listXIndex[i], listXIndex[i + 1]);
                    listXIndex.RemoveAt(i + 1);
                }
            }
        }
    }
}
