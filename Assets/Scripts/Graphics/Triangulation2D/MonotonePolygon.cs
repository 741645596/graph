using System.Collections.Generic;
using RayGraphics.Math;
using RayGraphics.Geometric;

namespace RayGraphics.Triangulation
{
    /// <summary>
    /// 单调多边形
    /// </summary>
    public class MonotonePolygon
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="listPoints">为逆时针序列,且为单调多边形</param>
        public MonotonePolygon(List<VertexInfo> listPoints)
        {
            if (listPoints == null)
                return;
            int count = listPoints.Count;
            if (count < 3)
                return;
            // 找最小
            int minIndex = -1;
            for (int i = 0; i < count; i++)
            {
                int prev = (i - 1) < 0 ? (count - 1):(i - 1);
                int next = (i + 1) >= count ? 0 : (i + 1);
                if (listPoints[i].pos.y < listPoints[prev].pos.y && listPoints[i].pos.y <= listPoints[next].pos.y)
                {
                    minIndex = i;
                    break;
                }
            }
            if (minIndex == -1)
                return;

            // 找最大
            int maxIndex = -1;
            for (int i = 0; i < count; i++)
            {
                int prev = (i - 1) < 0 ? (count - 1) : (i - 1);
                int next = (i + 1) >= count ? 0 : (i + 1);
                if (listPoints[i].pos.y >= listPoints[prev].pos.y && listPoints[i].pos.y > listPoints[next].pos.y)
                {
                    maxIndex = i;
                    break;
                }
            }
            if (maxIndex == -1)
                return;
            // 得到单调链
            if (maxIndex > minIndex)
            {
                // left
                for (int i = maxIndex + 1; i < count; i++)
                {
                    this.left.Add(new TriVertexInfo(listPoints[i], SideType.Left));
                }
                for (int i = 0; i < minIndex; i++)
                {
                    this.left.Add(new TriVertexInfo(listPoints[i], SideType.Left));
                }
                // right
                for (int i = maxIndex ; i >= minIndex; i--)
                {
                    SideType type = (i == maxIndex || i == minIndex) ? SideType.Center : SideType.Right;
                    this.right.Add(new TriVertexInfo(listPoints[i], type));
                }
            }
            else if (maxIndex < minIndex)
            {
                // left
                for (int i = maxIndex + 1; i < minIndex; i++)
                {
                    this.left.Add(new TriVertexInfo(listPoints[i], SideType.Left));
                }
                // right
                for (int i = maxIndex; i >= 0; i--)
                {
                    SideType type = (i == maxIndex || i == minIndex) ? SideType.Center : SideType.Right;
                    this.right.Add(new TriVertexInfo(listPoints[i], type));
                }
                for (int i = 0; i <= minIndex; i++)
                {
                    SideType type = (i == maxIndex || i == minIndex) ? SideType.Center : SideType.Right;
                    this.right.Add(new TriVertexInfo(listPoints[i], type));
                }
            }
        }
        /// <summary>
        /// 单调链- left -单调降
        /// </summary>
        private List<TriVertexInfo> left = new List<TriVertexInfo>();
        /// <summary>
        /// 单调链 - right - 单调降
        /// </summary>
        private List<TriVertexInfo> right = new List<TriVertexInfo>();
        /// <summary>
        /// 生成三角形列表
        /// </summary>
        /// <param name="listTri"></param>
        public void GeneralTri(ref List<Index3> listTri)
        {
            if (listTri == null)
            {
                listTri = new List<Index3>();
            }
            // 初始化动作
            Stack<TriVertexInfo> WaitPoint = new Stack<TriVertexInfo>();
            WaitPoint.Push(GetBestPoint());
            WaitPoint.Push(GetBestPoint());
            TriVertexInfo newPoint;
            while ((newPoint = GetBestPoint()) != null)
            {
                while (WaitPoint.Count > 1)
                {
                    TriVertexInfo first = WaitPoint.Pop();
                    TriVertexInfo second = WaitPoint.Pop();
                    // 判断与stack 顶元素是否同意侧
                    if (newPoint.sideType == first.sideType)
                    {
                        bool isLeft = GeometricUtil.LeftSide(second.pos, first.pos, newPoint.pos);
                        if ((newPoint.sideType == SideType.Left && isLeft == true) ||(newPoint.sideType == SideType.Right && isLeft == false))
                        {//不是凹的
                            // 得到三角形
                            listTri.Add(new Index3(second.index, first.index, newPoint.index));
                            //然后次栈顶元素就可以解放了，原来的栈顶变成次栈顶，新元素变成栈顶
                            WaitPoint.Push(second);
                        }
                        else // 无发够成三角形
                        {
                            WaitPoint.Push(second);
                            WaitPoint.Push(first);
                            break;
                        }
                    }
                    else // 异侧
                    {
                        // 得到三角形
                        listTri.Add(new Index3(second.index, first.index, newPoint.index));
                        WaitPoint.Push(first);
                    }
                }
                WaitPoint.Push(newPoint);
            }
        }
        /// <summary>
        /// 回去最合适的点
        /// </summary>
        /// <returns></returns>
        private TriVertexInfo GetBestPoint()
        {
            TriVertexInfo ret = null;
            int leftCount = this.left.Count;
            int rightCount = this.right.Count;
            if (leftCount == 0 && rightCount == 0)
                return ret;
            if (leftCount == 0)
            {
                ret = this.right[0];
                this.right.RemoveAt(0);
                return ret;
            }
            else if (rightCount == 0)
            {
                ret = this.left[0];
                this.left.RemoveAt(0);
                return ret;
            }
            else if (this.left[0].pos.y >= this.right[0].pos.y)
            {
                ret = this.left[0];
                this.left.RemoveAt(0);
                return ret;
            }
            else 
            {
                ret = this.right[0];
                this.right.RemoveAt(0);
                return ret;
            }
        }
    }


    public class TriVertexInfo
    {
        public TriVertexInfo(VertexInfo v, SideType isLeft)
        {
            this.pos = v.pos;
            this.index = v.index;
            this.sideType = isLeft;
        }
        /// <summary>
        /// 顶点数据
        /// </summary>
        public Float2 pos;
        /// <summary>
        /// 所在顶点列表中的索引
        /// </summary>
        public int index;
        /// <summary>
        /// 是否左链的点
        /// </summary>
        public SideType sideType;
    }


    public enum SideType
    {
        Center  = 0,  // 顶部，或底部元素
        Left    = 1,  // 左链
        Right   = 2,  // 右链
    }
}