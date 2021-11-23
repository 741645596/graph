using RayGraphics.Math;
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
    /// 在多边形中是否为凸点
    /// </summary>
    public VertexType vType;
}

public enum VertexType
{
    Normal      = 0,   // 正常升，降
    UpCorner    = 1,   // 向上拐 ^
    DownCorner  = 2,   // 向下拐
}
