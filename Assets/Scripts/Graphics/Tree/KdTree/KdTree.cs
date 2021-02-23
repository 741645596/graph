using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Tree
{
    public class KdTree : Itree
    {
        private List<TreeData> m_ListData = null;
        private KdTreeNode m_Tree = null;
        /// <summary>
        /// 限定分割条件
        /// </summary>
        public static readonly int s_LimitCount = 500;
        /// <summary>
        /// 构建KD树
        /// </summary>
        public void BuildTree(Double2 min, Double2 max, List<TreeData> listData)
        {
            m_ListData = listData;
            if (m_ListData == null || m_ListData.Count == 0)
                return;
            Double2 diff = max - min;
            bool isXCulling = diff.x >= diff.y;
            List<int> listIndex = new List<int>();
            for (int i = 0; i < m_ListData.Count; i++)
            {
                listIndex.Add(i);
            }
            m_Tree = new KdTreeNode(this, min, max, isXCulling, listIndex);
            m_Tree.BuildTreeRecursive();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TreeData GetData(int index)
        {
            if (m_ListData == null || m_ListData.Count == 0)
                return null;
            if (index >= m_ListData.Count || index == 0)
                return null;
            return m_ListData[index];
        }
        /// <summary>
        /// 判断是否需要分割。
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool CheckNeedCull(int count)
        {
            return count > s_LimitCount;
        }
    }
}


