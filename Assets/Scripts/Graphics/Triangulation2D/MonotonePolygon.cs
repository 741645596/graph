using System.Collections.Generic;
using RayGraphics.Math;
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
    /// 单调链- left
    /// </summary>
    private MonotoneChain left;
    /// <summary>
    /// 单调链 - right
    /// </summary>
    private MonotoneChain right;
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
    }
}

/// <summary>
/// 单调链
/// </summary>
public class MonotoneChain
{
    /// <summary>
    /// 多边形链
    /// </summary>
    public List<VertexInfo> listPoints = new List<VertexInfo>();
}