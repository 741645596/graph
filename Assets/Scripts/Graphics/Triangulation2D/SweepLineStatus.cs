using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Triangulation
{
    /// <summary>
    /// 扫描线状态结构
    /// 维持一个梯形列表
    /// </summary>
    public class SweepLineStatus
    {
        /// <summary>
        /// 梯形列表
        /// </summary>
        private List<Trapezoid> listTrap = new List<Trapezoid>();
        /// <summary>
        /// 清理工作
        /// </summary>
        public void Clear()
        {
            this.listTrap.Clear();
        }
        /// <summary>
        /// 更新扫描线，更新扫描线状态
        /// </summary>
        /// <param name="sweepLinePts"></param>
        /// <param name="listDiagonal"></param>
        /// <param name="parent">父对象</param>
        /// <param name="isYdown">扫描方向 true y从大到小扫描，否则从小到到大扫描</param>
        public void UpdatePoints(List<VertexInfo> sweepLinePts, ref List<Index2> listDiagonal, PolygonChain parent, bool isYdown)
        {
            // 先创建独立梯形
            Trapezoid left = null;
            Trapezoid right = null;
            foreach (VertexInfo targetV in sweepLinePts)
            {
                // 定位到分割点了
                if (targetV.CheckSplitPoint(isYdown))
                {
                    int retIndex = -1;
                    Trapezoid targetTrap = FindCheckInTrapezoid(targetV, ref retIndex);
                    if (targetTrap != null)
                    {
                        if (targetTrap.helper.pos.y != targetV.pos.y) // 过滤掉这种特殊情况
                        {
                            listDiagonal.Add(new Index2(targetTrap.helper.index, targetV.index));
                        }
                        // 进行分割梯形为2
                        Trapezoid leftTrap = new Trapezoid(targetTrap.left, parent.GetMinXEdge(targetV), targetV);
                        Trapezoid rightTrap = new Trapezoid(parent.GetMaxXEdge(targetV), targetTrap.right, targetV);
                        bool isLeft = !leftTrap.CheckInvalid();
                        bool isRight = !rightTrap.CheckInvalid();
                        if (isLeft && isRight)
                        {
                            listTrap[retIndex] = leftTrap;
                            listTrap.Insert(retIndex + 1, rightTrap);
                        }
                        else if (isLeft)
                        {
                            listTrap[retIndex] = leftTrap;
                        }
                        else if (isRight)
                        {
                            listTrap[retIndex] = rightTrap;
                        }
                        else 
                        {
                            listTrap.RemoveAt(retIndex);
                        }
                    }
                    else
                    {
                        AddTrapezoid(new Trapezoid(parent.GetMinXEdge(targetV), parent.GetMaxXEdge(targetV), targetV));
                    }
                }
                else
                {
                    int retIndex = -1;
                    bool ret = FindParent(targetV, ref left, ref right, ref retIndex);
                    if (ret == true)
                    {
                        if (left == right)
                        {
                            left.Growth(targetV, parent);
                            // 剔除退化的梯形
                            if (left.CheckInvalid() == true)
                            {
                                listTrap.RemoveAt(retIndex);
                            }
                        }
                        else // 进行合并，剔除合并前的，新增合并后的
                        {
                            listTrap[retIndex] = Trapezoid.CombineTrapezoid(left, right, targetV);
                            listTrap.RemoveAt(retIndex + 1);
                        }
                    }
                    else
                    {
                        AddTrapezoid(new Trapezoid(parent.GetMinXEdge(targetV), parent.GetMaxXEdge(targetV), targetV));
                    }
                }
            }
        }
        /// <summary>
        /// 加入梯形
        /// </summary>
        private void AddTrapezoid(Trapezoid v)
        {
            if (v == null)
                return;
            if (listTrap.Count == 0)
            {
                listTrap.Add(v);
            }
            else 
            {
                BinaryInsert(v, 0, listTrap.Count - 1);
            }
        }
        /// <summary>
        /// 找老大，最多找到2个梯形
        /// </summary>
        /// <param name="points"></param>
        /// <param name="left">左梯形</param>
        /// <param name="right">右梯形</param>
        /// <param name="retIndex">左梯形的索引</param>
        /// <returns></returns>
        private bool FindParent(VertexInfo points, ref Trapezoid left, ref Trapezoid right, ref int retIndex)
        {
            left = null;
            right = null;
            retIndex = -1;
            int count = listTrap.Count;
            if (count == 0)
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                Trapezoid v = listTrap[i];
                if (v.CheckInEdge(points) != null)
                {
                    if (left == null)
                    {
                        left = v;
                        retIndex = i;
                    }
                    else if (right == null)
                    {
                        right = v;
                    }
                }
            }
            if (left == null && right == null)
            {
                return false;
            }
            else
            {
                if (right == null)
                {
                    right = left;
                }
                return true;
            }
        }
        /// <summary>
        /// 二分查找
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="minIndex"></param>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        private void BinaryInsert(Trapezoid targetValue, int minIndex, int maxIndex)
        {
            while (minIndex <= maxIndex)
            {
                if (this.listTrap[minIndex].X <= targetValue.X && this.listTrap[maxIndex].X >= targetValue.X)
                {
                    int middle = (minIndex + maxIndex) / 2;

                    if (middle == minIndex)
                    {
                        this.listTrap.Insert(minIndex + 1, targetValue);
                        return;
                    }
                    else
                    {
                        if (this.listTrap[middle].X <= targetValue.X)
                        {
                            minIndex = middle;
                        }
                        else
                        {
                            maxIndex = middle;
                        }
                    }
                }
                else if (this.listTrap[minIndex].X > targetValue.X)
                {
                    this.listTrap.Insert(0, targetValue);
                    return;
                }
                else
                {
                    this.listTrap.Add(targetValue);
                    return;
                }
            }
        }
        /// <summary>
        /// 查找到所在的梯形
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private Trapezoid FindCheckInTrapezoid(VertexInfo points, ref int retIndex)
        {
            retIndex = -1;
            int count = listTrap.Count;
            for (int i = 0; i < count; i++)
            {
                Trapezoid v = listTrap[i];
                if (v.CheckIn(points) == true)
                {
                    retIndex = i;
                    return v;
                }
            }
            return null;
        }
    }
}

