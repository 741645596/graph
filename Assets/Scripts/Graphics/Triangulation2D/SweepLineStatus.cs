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

        private void Print()
        {
            UnityEngine.Debug.Log("next:" + listTrap.Count);
            foreach (Trapezoid v in listTrap)
            {
                v.Print();
            }
        }
        /// <summary>
        /// 更新扫描线，更新扫描线状态
        /// </summary>
        /// <param name="sl">扫描线结构</param>
        /// <param name="listDiagonal"></param>
        /// <param name="parent">父对象</param>
        /// <param name="isYdown">扫描方向 true y从大到小扫描，否则从小到到大扫描</param>
        public void UpdatePoints(ScanLine sl, ref List<Index2> listDiagonal, PolygonChain parent, bool isYdown)
        {
            // 先创建独立梯形
            Trapezoid left = null;
            Trapezoid right = null;
            foreach (VertexInfo ScanPoints in sl.LinePoints)
            {
                // 定位到分割点了
                if (ScanPoints.CheckSplitPoint(isYdown))
                {
                    int retIndex = -1;
                    Trapezoid targetTrap = FindCheckInTrapezoid(ScanPoints, isYdown, ref retIndex);
                    if (targetTrap != null)
                    {
                        if (targetTrap.helper.pos.y != ScanPoints.pos.y) // 过滤掉这种特殊情况
                        {
                            listDiagonal.Add(new Index2(targetTrap.helper.index, ScanPoints.index));
                        }
                        // 进行分割梯形为2
                        Trapezoid leftTrap = new Trapezoid(targetTrap.left, ScanPoints.GetLeftEdge(parent) , ScanPoints.GetLeftPoint());
                        Trapezoid rightTrap = new Trapezoid(ScanPoints.GetRightEdge(parent), targetTrap.right, ScanPoints.GetRightPoint());
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
                        AddTrapezoid(new Trapezoid(ScanPoints.GetLeftEdge(parent), ScanPoints.GetRightEdge(parent), ScanPoints.GetLeftPoint()));
                    }
                }
                else
                {
                    int retIndex = -1;
                    bool ret = FindParent(ScanPoints, ref left, ref right, ref retIndex);
                     if (ret == true)
                    {
                        if (left == right)
                        {
                            left.Growth(ScanPoints, parent);
                            // 剔除退化的梯形
                            if (left.CheckInvalid() == true)
                            {
                                listTrap.RemoveAt(retIndex);
                            }
                        }
                        else // 进行合并，剔除合并前的，新增合并后的
                        {
                            listTrap[retIndex] = Trapezoid.CombineTrapezoid(left, right, ScanPoints);
                            listTrap.RemoveAt(retIndex + 1);
                        }
                    }
                    else
                    {
                        AddTrapezoid(new Trapezoid(ScanPoints.GetLeftEdge(parent), ScanPoints.GetRightEdge(parent), ScanPoints.GetLeftPoint()));
                    }
                }
            }
            CullNoAreaTrapezoid();
            //Print();
        }
        /// <summary>
        /// 剔除无面积矩形
        /// </summary>
        private void CullNoAreaTrapezoid()
        {
            for (int i = 0; i < listTrap.Count;)
            {
                if (listTrap[i].CheckNoArea() == true)
                {
                    listTrap.RemoveAt(i);
                }
                else 
                {
                    i++;
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
        /// <param name="ScanPoints">扫描线点</param>
        /// <param name="left">左梯形</param>
        /// <param name="right">右梯形</param>
        /// <param name="retIndex">左梯形的索引</param>
        /// <returns></returns>
        private bool FindParent(VertexInfo ScanPoints, ref Trapezoid left, ref Trapezoid right, ref int retIndex)
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
                if (v.CheckInEdge(ScanPoints) != null)
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
        /// <param name="ScanPoints">扫描线点</param>
        /// <returns></returns>
        private Trapezoid FindCheckInTrapezoid(VertexInfo ScanPoints, bool isYdown,ref int retIndex)
        {
            retIndex = -1;
            int count = listTrap.Count;
            for (int i = 0; i < count; i++)
            {
                Trapezoid v = listTrap[i];
                if (v.CheckIn(ScanPoints, isYdown) == true)
                {
                    retIndex = i;
                    return v;
                }
            }
            return null;
        }
    }
}

