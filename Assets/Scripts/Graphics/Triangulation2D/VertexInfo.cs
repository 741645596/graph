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
        public int fixIndex;
        /// <summary>
        /// 在多边形链的序列
        /// </summary>
        public int indexInPolygonChain;
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
            VertexInfo prev = parent.GetVertexInfo(this.indexInPolygonChain - 1);
            VertexInfo next = parent.GetVertexInfo(this.indexInPolygonChain + 1);
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
            VertexInfo prev = parent.GetVertexInfo(this.indexInPolygonChain - 1);
            VertexInfo next = parent.GetVertexInfo(this.indexInPolygonChain + 1);
            if (prev.pos.x >= this.pos.x)
            {
                return new Edge(this, prev);
            }
            else
            {
                return new Edge(this, next);
            }
        }
        /// <summary>
        /// 获取左点
        /// </summary>
        /// <returns></returns>
        public virtual VertexInfo GetLeftPoint()
        {
            return this;
        }
        /// <summary>
        /// 获取右点
        /// </summary>
        /// <returns></returns>
        public virtual VertexInfo GetRightPoint()
        {
            return this;
        }
        /// <summary>
        /// 是否hit。
        /// </summary>
        /// <param name="ScanPoints">扫描线点，可能为符合点</param>
        /// <returns></returns>
        public bool CheckHit(VertexInfo ScanPoints)
        {
            if (this == ScanPoints.GetLeftPoint() || this == ScanPoints.GetRightPoint())
                return true;
            else return false;
        }
        /// <summary>
        /// 对边进行成长
        /// </summary>
        /// <param name="growEdge"></param>
        /// <returns></returns>
        public virtual Edge GetGrowEdge(Edge growEdge, ref VertexInfo helper, PolygonChain parent)
        {
            helper = growEdge.end;
            return new Edge(growEdge.end, parent.GetOtherPoints(growEdge));
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
        public CombineVertex(VertexInfo left, VertexInfo right)
        {
            this.left = left;
            this.right = right;
            this.pos = this.left.pos;
            this.vType = this.left.vType;
            this.isConvex = this.left.isConvex;
            this.fixIndex = this.left.fixIndex;
            this.indexInPolygonChain = this.left.indexInPolygonChain;
        }
        /// <summary>
        /// 确定2个点能否合并
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool CheckCanCombine(VertexInfo left, VertexInfo right, PolygonChain parent)
        {
            if (left == null || right == null || parent == null)
                return false;
            if (left.pos.y != right.pos.y)
                return false;
            if (parent.CheckSameEdge(left.indexInPolygonChain, right.indexInPolygonChain) == false)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取左边
        /// </summary>
        /// <returns></returns>
        public override Edge GetLeftEdge(PolygonChain parent)
        {
            int leftIndex = this.left.indexInPolygonChain;
            VertexInfo prev = parent.GetVertexInfo(leftIndex - 1);
            VertexInfo next = parent.GetVertexInfo(leftIndex + 1);
            if (prev.pos.x <= this.left.pos.x)
            {
                return new Edge(this.left, prev);
            }
            else
            {
                return new Edge(this.left, next);
            }
        }
        /// <summary>
        /// 获取右边
        /// </summary>
        /// <returns></returns>
        public override Edge GetRightEdge(PolygonChain parent)
        {
            int rightIndex = this.right.indexInPolygonChain;
            VertexInfo prev = parent.GetVertexInfo(rightIndex - 1);
            VertexInfo next = parent.GetVertexInfo(rightIndex + 1);
            if (prev.pos.x >= this.right.pos.x)
            {
                return new Edge(this.right, prev);
            }
            else
            {
                return new Edge(this.right, next);
            }
        }
        /// <summary>
        /// 获取左点
        /// </summary>
        /// <returns></returns>
        public override VertexInfo GetLeftPoint()
        {
            return this.left;
        }
        /// <summary>
        /// 获取右点
        /// </summary>
        /// <returns></returns>
        public override VertexInfo GetRightPoint()
        {
            return this.right;
        }
        /// <summary>
        /// 对边进行成长
        /// </summary>
        /// <param name="growEdge"></param>
        /// <returns></returns>
        public override Edge GetGrowEdge(Edge growEdge, ref VertexInfo helper, PolygonChain parent)
        {
            if (growEdge.end == this.left)
            {
                helper = this.right;
                return new Edge(this.right, parent.GetOtherPoints(new Edge(this.left, this.right)));
            }
            else 
            {
                helper = this.left;
                return new Edge(this.left, parent.GetOtherPoints(new Edge(this.right, this.left)));
            }
        }
    }

    public enum VertexType
    {
        Normal = 0,   // 正常升，降
        UpCorner = 1,   // 向上拐 ^
        DownCorner = 2,   // 向下拐
    }
}
