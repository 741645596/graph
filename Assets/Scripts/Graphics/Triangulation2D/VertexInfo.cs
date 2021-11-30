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
}

public enum VertexType
{
    Normal      = 0,   // 正常升，降
    UpCorner    = 1,   // 向上拐 ^
    DownCorner  = 2,   // 向下拐
}
