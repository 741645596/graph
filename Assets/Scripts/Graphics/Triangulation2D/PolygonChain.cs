using RayGraphics.Math;
using System.Collections.Generic;
/// <summary>
/// 多边形链
/// </summary>
public class PolygonChain 
{
    /// <summary>
    /// 最低点索引
    /// </summary>
    private int minYIndex;
    /// <summary>
    /// 最高点索引
    /// </summary>
    private int maxYIndex;
    /// <summary>
    /// 对所用顶点进行按Y从低到高进行排序
    /// </summary>
    private List<YPoints> listYPoints = new List<YPoints>();
    /// <summary>
    /// 顶点列表
    /// </summary>
    private List<VertexInfo> listPoints = new List<VertexInfo>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="minYIndex"></param>
    /// <param name="maxYIndex"></param>
    /// <param name="listPts"></param>
    public PolygonChain(int minYIndex, int maxYIndex, List<VertexInfo> listPts)
    {
        this.minYIndex = minYIndex;
        this.maxYIndex = maxYIndex;
        for (int i = 0; i < maxYIndex; i++)
        {
            listPoints.Add(listPts[i]);
        }
        for (int i = maxYIndex; i < listPts.Count; i++)
        {
            listPoints.Add(listPts[i]);
        }
    }
    /// <summary>
    /// 生成单调多边形
    /// </summary>
    /// <returns></returns>
    public List<MonotonePolygon> GeneralMonotonePolygon()
    {
        List<Index2> listDiagonal = TriangulationDiagonal();
        return GeneralMonotonePolygon(listDiagonal);
    }
    /// <summary>
    /// 得到剖分对象线
    /// </summary>
    /// <returns></returns>
    private List<Index2> TriangulationDiagonal()
    {
        return null;
    }
    /// <summary>
    /// 通过分隔对角线得到单调多边形
    /// </summary>
    /// <param name="listDiagonal"></param>
    /// <returns></returns>
    private List<MonotonePolygon> GeneralMonotonePolygon(List<Index2> listDiagonal)
    {
        return null;
    }
    /// <summary>
    /// 判断是歧点
    /// 考虑性能需求，不做异常检查
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    private bool CheckDivergencePoint(VertexInfo cur, VertexInfo next)
    {
        if (cur.isConvex == false)
            return false;
        if ( cur.pos.y >= next.pos.y )
        {
            return true;
        }
        else if ( cur.pos.y <= next.pos.y )
        {
            return true;
        }
        return false;
    }

}
/// <summary>
/// 保存同一个高度
/// </summary>
public class YPoints
{
    /// <summary>
    /// y的高度
    /// </summary>
    public float y;
    /// <summary>
    /// 保持x 坐标的值，及对应在多边形链表中的索引
    /// // x: 保存x坐标。 y保存索引
    /// 同时按x 从小到大进行了排序
    /// </summary>
    public List<Float2> listXIndex = new List<Float2>();
}
