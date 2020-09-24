using RayGraphics.Math;
using RayGraphics.Geometric;
using System.Collections.Generic;

namespace RayGraphics.Voxelization
{
    [System.Serializable]
    public class ContourPoly
    {
        /// <summary>
        /// 轮廓点
        /// </summary>
        public List<Double2> pts = new List<Double2>();
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            pts.Clear();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="points"></param>
        public void InitData(List<Double2> points)
        {
            pts.Clear();
            if (points != null && points.Count >= 3)
            {
                pts.AddRange(points);
            }
        }
        /// <summary>
        /// 判断点是否在轮廓里面
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CheckIn(Float2 pos)
        {
            Polygon2D poly = new Polygon2D(pts.ToArray());
            bool ret =  poly.CheckIn(new Double2(pos.x, pos.y));
            return ret;
        }
        /// <summary>
        /// 在轮廓中查找keyPoint
        /// </summary>
        /// <returns></returns>
        public bool FindKeyPoint(RLEGridMap grid, ref Float2 keyPoint)
        {
            if (grid == null)
                return false;
            Polygon2D poly = new Polygon2D(pts.ToArray());
            Double2 lb = poly.leftBottom;
            Double2 ru = poly.rightUp;

            Short2 slb = grid.GetIndex(lb) + Short2.one;
            Short2 sru = grid.GetIndex(ru) - Short2.one;
            for (int x = slb.x; x < sru.x; x++)
            {
                double xpos = x * (double)grid.TileSize;
                Double2 yminPos1 = Double2.zero;
                Double2 ymaxPos2 = Double2.zero;
                if (poly.GetPointsInAreabyXaixs(xpos, 1.0f, ref yminPos1, ref ymaxPos2) == true)
                {
                    int ymin = (int)(yminPos1.y /grid.TileSize);
                    int ymax = (int)(ymaxPos2.y / grid.TileSize);
                    if (grid.CheckXDirHaveBlock(x, ymin, ymax) == true)
                    {
                        keyPoint = new Float2(x * grid.TileSize, ymax * grid.TileSize);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
