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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="helper"></param>
        public Trapezoid(Edge left, Edge right, VertexInfo helper)
        {
            this.left = left;
            this.right = right;
            this.helper = helper;
        }
        /// <summary>
        /// 已经确定可以合并了,梯形增长
        /// </summary>
        public void Growth(VertexInfo points, PolygonChain parent)
        {
            Edge edge;
            if (this.left.end == points)
            {
                if (this.left.start.index < points.index)
                {
                    edge = new Edge(points, parent.GetVertexInfo(points.index + 1)); 
                }
                else
                {
                    edge = new Edge(points, parent.GetVertexInfo(points.index - 1));
                    
                }
                // 过滤一些节点，这种情况下，不成长梯形
                bool check = points.isConvex == true && edge.start.pos.y == edge.end.pos.y;
                if(check == false)
                   this.left = edge;
            }
            else if (this.right.end == points)
            {
                if (this.right.start.index < points.index)
                {
                    edge = new Edge(points, parent.GetVertexInfo(points.index + 1));
                }
                else
                {
                    edge = new Edge(points, parent.GetVertexInfo(points.index - 1));
                }
                // 过滤一些节点，这种情况下，不成长梯形
                bool check = points.isConvex == true && edge.start.pos.y == edge.end.pos.y;
                if (check == false)
                    this.right = edge;
            }
        }
        /// <summary>
        /// 合并梯形
        /// </summary>
        public static Trapezoid CombineTrapezoid(Trapezoid left, Trapezoid right, VertexInfo points)
        {
            return new Trapezoid(left.left, right.right, points);
        }
        /// <summary>
        /// 判断目标点是梯形夹边中。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CheckIn(VertexInfo target)
        {
            // 需在左侧
            if (left.CheckLeft(target) == false)
                return false;
            // 需在右侧
            if (right.CheckLeft(target) == true)
                return false;
            return true;
        }
        /// <summary>
        /// 确定在边上
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Edge CheckInEdge(VertexInfo target)
        {
            if (left.end == target)
                return left;
            else if (right.end == target)
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
            else if (this.right.end.index == this.left.end.index )
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public string Print()
        {
            return "[" + start.pos.x + "," + start.pos.y + "]->[" + end.pos.x + "," + end.pos.y + "]";
        }
    }
}

