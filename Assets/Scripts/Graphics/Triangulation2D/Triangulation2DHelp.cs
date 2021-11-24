using RayGraphics.Math;
using System.Collections.Generic;

public class Triangulation2DHelp 
{
    /// <summary>
    /// 生成三角形
    /// </summary>
    /// <param name="listPts"></param>
    /// <param name="maxYIndex">y max 最大的索引， 0 元素为最小的</param>
    /// <param name="listTri">得到切割出来的三角形</param>
    public static bool GeneralTri(List<VertexInfo> listPts, int maxYIndex, ref List<Index3> listTri)
    {
        if (listTri != null && listTri.Count > 0)
        {
            listTri.Clear();
        }
        else 
        {
            listTri = new List<Index3>();
        }
        // 先得到多边形链
        PolygonChain pc = new PolygonChain(maxYIndex, listPts);
        // 得到分解后的单调多边形
        List<MonotonePolygon> listMp= pc.GeneralMonotonePolygon();
        // 分解单调多边形得到三角形
        if (listMp != null && listMp.Count > 0)
        {
            foreach (MonotonePolygon mp in listMp)
            {
                mp.GeneralTri(ref listTri);
            }
        }
        return true;
    }
}
