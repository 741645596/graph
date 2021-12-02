using RayGraphics.Geometric;

namespace RayGraphics.Triangulation
{
    /// <summary>
    /// 梯形
    /// </summary>
    public class Trapezoid
    {
        /// <summary>
        /// 左边
        /// </summary>
        public Edge left;
        /// <summary>
        /// 右边
        /// </summary>
        public Edge right;
        /// <summary>
        /// helper 点
        /// </summary>
        public VertexInfo helper;
        /// <summary>
        /// 得到X坐标
        /// </summary>
        public float X
        {
            get { return helper.pos.x; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="left">保证不为组合点</param>
        /// <param name="right">保证不为组合点</param>
        /// <param name="helper">保证不为组合点</param>
        public Trapezoid(Edge left, Edge right, VertexInfo helper)
        {
            this.left = left;
            this.right = right;
            this.helper = helper;
        }
        /// <summary>
        /// 已经确定可以合并了,梯形增长
        /// </summary>
        /// <param name="ScanPoints">扫描线点</param>
        /// <param name="parent"></param>
        public void Growth(VertexInfo ScanPoints, PolygonChain parent)
        {
            if (this.left.end.CheckHit(ScanPoints) == true)
            {
                this.left = ScanPoints.GetGrowEdge(this.left, ref this.helper, parent);
            }
            else if (this.right.end.CheckHit(ScanPoints) == true)
            {
                this.right = ScanPoints.GetGrowEdge(this.right, ref this.helper, parent);
            }
        }
        /// <summary>
        /// 合并梯形
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="ScanPoints">扫描线点</param>
        /// <returns></returns>
        public static Trapezoid CombineTrapezoid(Trapezoid left, Trapezoid right, VertexInfo ScanPoints)
        {
            return new Trapezoid(left.left, right.right, ScanPoints.GetLeftPoint());
        }
        /// <summary>
        /// 判断目标点是梯形夹边中。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CheckIn(VertexInfo target, bool isYdown)
        {
            if (isYdown == true)
            {
                // 需在左侧
                if (left.CheckLeft(target) == false)
                    return false;
                // 需在右侧
                if (right.CheckLeft(target) == true)
                    return false;
                return true;
            }
            else 
            {
                // 需在左侧
                if (left.CheckLeft(target) == true)
                    return false;
                // 需在右侧
                if (right.CheckLeft(target) == false)
                    return false;
                return true;
            }
        }
        /// <summary>
        /// 确定在边上
        /// </summary>
        /// <param name="ScanPoints">扫描线点，可能为符合点</param>
        /// <returns></returns>
        public Edge CheckInEdge(VertexInfo ScanPoints)
        {
            if (left.end.CheckHit(ScanPoints) == true)
                return left;
            else if (right.end.CheckHit(ScanPoints) == true)
                return right;
            return null;
        }
        /// <summary>
        /// 判断是否为无效的梯形
        /// </summary>
        /// <returns>true 无效梯形， false 有效</returns>
        public bool CheckInvalid()
        {
            if (this.left.start.index == this.right.end.index && this.left.end.index == this.right.start.index)
            {
                return true;
            }
            // 退化的梯形
            else if (this.right.end.index == this.left.end.index)
            {
                return true;
            }
            else if (this.left.CheckLevelEdge() == true || this.right.CheckLevelEdge() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 没面积的梯形，一条边的终点 等于一条边的起点高度，特指这类梯形
        /// 边的节点== end
        /// </summary>
        /// <returns></returns>
        public bool CheckNoArea()
        {
            if (this.left.end.pos.y == this.right.start.pos.y || this.left.start.pos.y == this.right.end.pos.y)
                return true;
            else return false;
        }

        public void Print()
        {
            string str = "left:" + left.Print() + " right:" + right.Print() + " helper:" + "[" + helper.pos.x + "," + helper.pos.y + "]";
            UnityEngine.Debug.Log(str);
        }
    }

    /// <summary>
    /// 边
    /// </summary>
    public class Edge
    {
        public VertexInfo start;
        public VertexInfo end;

        public Edge(VertexInfo start, VertexInfo end)
        {
            this.start = start;
            this.end = end;
        }
        /// <summary>
        /// 判断目标点是在左侧
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CheckLeft(VertexInfo target)
        {
            return GeometricUtil.LeftSide(start.pos, end.pos, target.pos);
        }
        /// <summary>
        /// 判断是否为水平边，y 值相同
        /// </summary>
        /// <returns></returns>
        public bool CheckLevelEdge()
        {
            if (start.pos.y == end.pos.y)
                return true;
            else return false;
        }

        public string Print()
        {
            return "[" + start.pos.x + "," + start.pos.y + "]->[" + end.pos.x + "," + end.pos.y + "]";
        }
    }
}

