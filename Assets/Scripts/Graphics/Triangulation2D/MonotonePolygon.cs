using System.Collections.Generic;
using RayGraphics.Math;

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
        /// <param name="listPoints"></param>
        public MonotonePolygon(List<VertexInfo> listPoints)
        {

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
                    if (newPoint.isLeft == first.isLeft)
                    {
                        if (Utils2D.LeftSide(second.pos, first.pos, newPoint.pos))
                        {//不是凹的
                            // 得到三角形
                            listTri.Add(new Index3(second.index, first.index, newPoint.index));
                            //然后次栈顶元素就可以解放了，原来的栈顶变成次栈顶，新元素变成栈顶
                            WaitPoint.Push(second);
                        }
                        else
                        {//如果无法构成三角形还要进入栈中等待
                            WaitPoint.Push(second);
                            WaitPoint.Push(first);
                            WaitPoint.Push(newPoint);
                            break;
                        }
                    }
                    else
                    {
                        // 得到三角形
                        listTri.Add(new Index3(second.index, first.index, newPoint.index));
                        WaitPoint.Push(second);
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
            int leftCount = this.left.Count;
            int rightCount = this.right.Count;
            if (leftCount == 0 || rightCount == 0)
                return null;
            if (leftCount == 0)
            {
                return this.right[0];
            }
            else if (rightCount == 0)
            {
                return this.left[0];
            }
            else if (this.left[0].pos.y >= this.right[0].pos.y)
            {
                return this.left[0];
            }
            else 
            {
                return this.right[0];
            }
        }
    }


    public class TriVertexInfo
    {
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
        public bool isLeft;
    }
}