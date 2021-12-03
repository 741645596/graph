using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Triangulation
{
    public class Triangulation2DHelp
    {
        /// <summary>
        /// 生成三角形
        /// </summary>
        /// <param name="listPts"></param>
        /// <param name="listTri">得到切割出来的三角形</param>
        public static bool GeneralTri(List<VertexInfo> listPts, ref List<Index3> listTri)
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
            PolygonChain pc = new PolygonChain(listPts);
            // 得到分解后的单调多边形
            List<MonotonePolygon> listMp = pc.GeneralMonotonePolygon();
            // 分解单调多边形得到三角形
            if (listMp != null && listMp.Count > 0)
            {
                for (int i = 0; i < listMp.Count; i++)
                {
                    listMp[i].GeneralTri(ref listTri);
                }
            }
            return true;
        }

        /// <summary>
        /// 测试分隔对角线
        /// </summary>
        /// <param name="listPts"></param>
        /// <param name="listTri">得到切割出来的三角形</param>
        public static bool TestGeneralTri(List<VertexInfo> listPts, ref List<Index2> listTri)
        {
            // 先得到多边形链
            PolygonChain pc = new PolygonChain(listPts);
            listTri = pc.TestDiagonal();
            return true;
        }
        /// <summary>
        /// 测试分隔多边形
        /// </summary>
        /// <param name="listPts"></param>
        /// <param name="listTri">得到切割出来的三角形</param>
        public static bool TestGeneralTri(List<VertexInfo> listPts, ref List<List<Float2>> listTri)
        {
            // 先得到多边形链
            PolygonChain pc = new PolygonChain(listPts);
            listTri = pc.TestPolygon();
            return true;
        }
    }
}

