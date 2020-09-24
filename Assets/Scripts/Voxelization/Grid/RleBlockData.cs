using System.Collections.Generic;
// <summary>
/// 行程压缩数据
/// </summary>
namespace RayGraphics.Voxelization
{
    [System.Serializable]
    public class RleBlockData
    {
        /// <summary>
        /// 按行从小到达排序好了。
        /// </summary>
        public List<LineRleUnit> listSort = new List<LineRleUnit>();
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (listSort == null)
                return;
            if (listSort != null)
            {
                listSort.Clear();
            }
            listSort = null;
        }
    }

    public class LineRleUnit
    {
        /// <summary>
        /// 行
        /// </summary>
        public int line;
        /// <summary>
        /// 从区间小到大排序好的了。
        /// </summary>
        public List<RleUnit> listSort = new List<RleUnit>();
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="unit"></param>
        public void Remove(RleUnit unit)
        {
            listSort.Remove(unit);
        }
        /// <summary>
        /// 插入一条。插入正确的位置。不考虑重合的情况
        /// </summary>
        /// <param name="unit"></param>
        public void Insert(RleUnit unit)
        {
            int count = listSort.Count;
            if (count == 0)
            {
                listSort.Add(unit);
            }
            else if (count == 1)
            {
                if (unit <= listSort[0])
                {
                    listSort.Insert(0, unit);
                }
                else 
                {
                    listSort.Add(unit);
                }
            }
            else 
            {
                int low = 0;
                int high = count - 1;
                do
                {
                    if (unit <= listSort[low])
                    {
                        listSort.Insert(low, unit);
                        break;
                    }
                    else if (listSort[high] <= unit)
                    {
                        listSort.Insert(high + 1, unit);
                        break;
                    }
                    else
                    {
                        int middle = (low + high) / 2;
                        if (unit <= listSort[middle])
                        {
                            high = middle;
                        }
                        else
                        {
                            low = middle;
                        }
                    }
                }
                while (low <= high);
            }
        }
        /// <summary>
        /// 二分法查找插入位置
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="key">要查找的对象</param>
        /*public int BinarySearch(RleUnit unit)
        {
            int low = 0;
            int high = listSort.Count - 1;
            while (low <= high)
            {
                int middle = (low + high) / 2;
                if (unit.start == listSort[middle].start)
                {
                    return middle;//如果找到了就直接返回这个元素的索引
                }
                else if (unit.start > listSort[middle].start)
                {
                    low = middle + 1;
                }
                else
                {
                    high = middle - 1;
                }
            }
            return -1;//如果找不到就返回-1；
        }*/
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (listSort == null)
                return;
            if (listSort != null)
            {
                listSort.Clear();
            }
            listSort = null;
        }
    }


    [System.Serializable]
    public struct RleUnit
    {
        /// <summary>
        /// 起始点
        /// </summary>
        public int start;
        /// <summary>
        /// 结束点
        /// </summary>
        public int end;
        public RleUnit(int start, int end)
        {
            this.start = start;    
            this.end = end;
        }
        /// <summary>
        /// ！=
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(RleUnit v1, RleUnit v2)
        {
            return v1.start != v2.start || v1.end != v2.end;
        }
        /// <summary>
        /// ==
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(RleUnit v1, RleUnit v2)
        {
            return v1.start == v2.start && v1.end == v2.end;
        }
        /// <summary>
        /// > 运算符, 只比较start
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >(RleUnit v1, RleUnit v2)
        {
            return v1.start > v2.start;
        }
        /// <summary>
        /// < 运算符， 只比较start
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <(RleUnit v1, RleUnit v2)
        {
            return v1.start < v2.start;
        }
        /// <summary>
        /// > 运算符, 只比较start
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >=(RleUnit v1, RleUnit v2)
        {
            return v1.start >= v2.start;
        }
        /// <summary>
        /// < 运算符， 只比较start
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <=(RleUnit v1, RleUnit v2)
        {
            return v1.start <= v2.start;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            RleUnit p = (RleUnit)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (start == p.start) && (end == p.end);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
