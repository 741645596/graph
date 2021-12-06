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
        /// 单调链- left -单调降
        /// </summary>
        private List<TriVertexInfo> left = new List<TriVertexInfo>();
        /// <summary>
        /// 单调链 - right - 单调降
        /// </summary>
        private List<TriVertexInfo> right = new List<TriVertexInfo>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="listPoints">为逆时针序列,且为单调多边形</param>
        public MonotonePolygon(List<VertexInfo> listPoints)
        {
            // 先判断是否为单调多边形
            /*if (CheckMonotonePolygon(listPoints) == false)
            {
                UnityEngine.Debug.Log("非单调多边形");
            }*/
                
            int count = listPoints.Count;
            // 找最小
            float targetY;
            int minIndex = -1;
            float minValue = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                int prev = (i - 1) < 0 ? (count - 1):(i - 1);
                int next = (i + 1) >= count ? 0 : (i + 1);
                targetY = listPoints[i].pos.y;
                if (targetY < listPoints[prev].pos.y && targetY <= listPoints[next].pos.y && targetY <= minValue)
                {
                    minIndex = i;
                    minValue = targetY;
                }
            }
            if (minIndex == -1)
                return;

            // 找最大
            int maxIndex = -1;
            float maxValue = float.MinValue;
            for (int i = 0; i < count; i++)
            {
                int prev = (i - 1) < 0 ? (count - 1) : (i - 1);
                int next = (i + 1) >= count ? 0 : (i + 1);
                targetY = listPoints[i].pos.y;
                if (targetY >= listPoints[prev].pos.y && targetY > listPoints[next].pos.y && targetY >=maxValue)
                {
                    maxIndex = i;
                    maxValue = targetY;
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
                for (int i = count -1; i >= minIndex; i--)
                {
                    SideType type = (i == maxIndex || i == minIndex) ? SideType.Center : SideType.Right;
                    this.right.Add(new TriVertexInfo(listPoints[i], type));
                }
            }
        }

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
                    TriVertexInfo stackUp = WaitPoint.Pop();
                    TriVertexInfo stackDown = WaitPoint.Pop();
                    // 判断与stack 顶元素是否同意侧
                    if (newPoint.sideType == stackUp.sideType)
                    {
                        bool isLeft = GeometricUtil.LeftSide(stackDown.pos, stackUp.pos, newPoint.pos);
                        if ((newPoint.sideType == SideType.Left && isLeft == true) ||(newPoint.sideType == SideType.Right && isLeft == false))
                        {//不是凹的
                            // 得到三角形
                            AddTri(ref listTri, stackDown, stackUp, newPoint);
                            WaitPoint.Push(stackDown);
                        }
                        else // 无发够成三角形
                        {
                            WaitPoint.Push(stackDown);
                            WaitPoint.Push(stackUp);
                            break;
                        }
                    }
                    else // 异侧
                    {
                        // 得到三角形
                        AddTri(ref listTri, stackDown, stackUp, newPoint);
                        TriVertexInfo botton = stackUp;
                        if (WaitPoint.Count > 0)
                        {
                            while (WaitPoint.Count > 0)
                            {
                                 stackUp = stackDown;
                                 stackDown = WaitPoint.Pop();
                                AddTri(ref listTri, stackDown, stackUp, newPoint);
                            }
                        }
                        WaitPoint.Push(botton);
                    }
                }
                WaitPoint.Push(newPoint);
            }
        }
        /// <summary>
        /// 添加三角形
        /// </summary>
        /// <param name="listTri"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private void AddTri(ref List<Index3> listTri, TriVertexInfo a, TriVertexInfo  b, TriVertexInfo c)
        {
            // 得到三角形
            if (b.sideType == SideType.Left)
            {
                listTri.Add(new Index3(c.index, b.index, a.index));
            }
            else 
            {
                listTri.Add(new Index3(a.index, b.index, c.index));
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
        /// <summary>
        /// 判断是否为单调多边形
        /// </summary>
        /// <param name="listPoints"></param>
        /// <returns></returns>
        public static bool CheckMonotonePolygon(List<VertexInfo> listPoints)
        {
            if (listPoints == null)
            return false;

            int count = listPoints.Count;
            if (count < 3)
                return false;
            // 找最小
            int minIndex = -1;
            float minValue = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                if (listPoints[i].pos.y < minValue)
                {
                    minIndex = i;
                    minValue = listPoints[i].pos.y;
                }
            }
            if (minIndex == -1)
                return false;

            // 找最大
            int maxIndex = -1;
            float maxValue = float.MinValue;
            for (int i = 0; i < count; i++)
            {
                if (listPoints[i].pos.y >= maxValue)
                {
                    maxIndex = i;
                    maxValue = listPoints[i].pos.y;
                }
            }
            if (maxIndex == -1)
                return false;

            // 得到单调链
            if (maxIndex > minIndex)
            {
                for (int i = maxIndex; i > minIndex; i--)
                {
                    if (listPoints[i].pos.y < listPoints[i - 1].pos.y)
                    {
                        return false;

                    }
                }
                for (int i = maxIndex; i < count; i++)
                {
                    int next = i + 1;
                    if (next == count)
                    {
                        next = 0;
                    }
                    if (listPoints[i].pos.y < listPoints[next].pos.y)
                    {
                        return false;
                    }
                }
                for (int i = 0; i < minIndex -1; i ++)
                {
                    int next = i + 1;
                    if (listPoints[i].pos.y < listPoints[next].pos.y)
                    {
                        return false;
                    }
                }
            }
            else if (maxIndex < minIndex)
            {
                for (int i = maxIndex; i < minIndex; i++)
                {
                    if (listPoints[i].pos.y < listPoints[i + 1].pos.y)
                    {
                        return false;
                    }
                }

                for (int i = maxIndex; i < count; i++)
                {
                    int next = i + 1;
                    if (next == count)
                    {
                        next = 0;
                    }
                    if (listPoints[i].pos.y < listPoints[next].pos.y)
                    {
                        return false;
                    }
                }
                for (int i = 0; i < minIndex - 1; i++)
                {
                    int next = i + 1;
                    if (listPoints[i].pos.y < listPoints[next].pos.y)
                    {
                        return false;
                    }
                }
            }
            return true;

        }
    }


    public class TriVertexInfo
    {
        public TriVertexInfo(VertexInfo v, SideType isLeft)
        {
            this.pos = v.pos;
            this.index = v.fixIndex;
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