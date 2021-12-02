using RayGraphics.Math;

namespace RayGraphics.Triangulation
{
    /// <summary>
    /// 顶点信息
    /// </summary>
    public class VertexInfo
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
        /// 在多边形中是否为凸点
        /// </summary>
        public bool isConvex;
        /// <summary>
        /// 是否为歧点
        /// </summary>
        public VertexType vType;
        /// <summary>
        /// 确定是否歧点，分割点
        /// </summary>
        /// <param name="isYdown"></param>
        /// <returns></returns>
        public bool CheckSplitPoint(bool isYdown)
        {
            if (this.isConvex == true)
                return false;

            if (this.vType == VertexType.UpCorner && isYdown == true)
            {
                return true;
            }
            else if (this.vType == VertexType.DownCorner && isYdown == false)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取左边
        /// </summary>
        /// <returns></returns>
        public virtual Edge GetLeftEdge(PolygonChain parent)
        {
            VertexInfo prev = parent.GetVertexInfo(index - 1);
            VertexInfo next = parent.GetVertexInfo(index + 1);
            if (prev.pos.x <= this.pos.x)
            {
                return new Edge(this, prev);
            }
            else
            {
                return new Edge(this, next);
            }
        }
        /// <summary>
        /// 获取右边
        /// </summary>
        /// <returns></returns>
        public virtual Edge GetRightEdge(PolygonChain parent)
        {
            VertexInfo prev = parent.GetVertexInfo(index - 1);
            VertexInfo next = parent.GetVertexInfo(index + 1);
            if (prev.pos.x >= this.pos.x)
            {
                return new Edge(this, prev);
            }
            else
            {
                return new Edge(this, next);
            }
        }
    }

    /// <summary>
    /// 组合点，满足条件，y值相同，在同一条边上，凹凸性相同
    /// </summary>
    public class CombineVertex : VertexInfo
    {
        private VertexInfo left;
        private VertexInfo right;
        /// <summary>
        /// 使用左顶点代替组合顶点数据
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        CombineVertex(VertexInfo left, VertexInfo right)
        {
            this.left = left;
            this.right = right;
            this.pos = this.left.pos;
            this.vType = this.left.vType;
            this.isConvex = this.left.isConvex;
            this.index = this.left.index;
        }
    }

    public enum VertexType
    {
        Normal = 0,   // 正常升，降
        UpCorner = 1,   // 向上拐 ^
        DownCorner = 2,   // 向下拐
    }
}
