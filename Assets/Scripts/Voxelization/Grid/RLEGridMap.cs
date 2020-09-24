using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Voxelization
{
    /// <summary>
    /// 存储有行程压缩的挡格地图数据
    /// RLE（Run Length Encoding行程编码）
    /// </summary>
    public partial class RLEGridMap: TileBaseMap 
    {
        /// <summary>
        /// 设置RLE（Run Length Encoding行程编码）格子数据，
        /// </summary>
        /// <param name="listRleBlockArray">(x,y)挡格起点，z挡格连续长度[X方向]</param>
        /// <returns></returns>
        public bool SetRleBlockData(List<Short3> listRleBlockArray)
        {
            if (_blockArray == null)
                return false;

            if (listRleBlockArray != null && listRleBlockArray.Count > 0)
            {
                foreach (Short3 RleUnit in listRleBlockArray)
                {
                    for (int x = 0; x <= RleUnit.z; x++)
                    {
                        _blockArray[RleUnit.y][RleUnit.x + x] = 1;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 生成rle 挡格数据
        /// </summary>
        /// <returns></returns>
        public List<Short3> MakeRleBlock()
        {
            List<Short3> listRleBlockArray = new List<Short3>();
            Short2 start = Short2.zero;
            Short2 prev = Short2.zero;
            bool isfirst = false;
            for (short y = 0; y < this.tileYnum; y++)
            {
                for (short x = 0; x < this.tileXnum; x++)
                {
                    if (_blockArray[y][x] == 1)
                    {
                        if (isfirst == false)
                        {
                            start = new Short2(x, y);
                            prev = start;
                            isfirst = true;
                        }
                        else
                        {
                            if (y == start.y && x == prev.x + 1)
                            {
                                prev = new Short2(x, y);
                            }
                            else
                            {
                                listRleBlockArray.Add(new Short3(start.x, start.y, prev.x - start.x));
                                start = new Short2(x, y);
                                prev = start;
                            }
                        }
                    }
                }
            }
            if (start.y == this.tileYnum - 1 && prev.y == this.tileYnum - 1 && isfirst == true)
            {
                listRleBlockArray.Add(new Short3(start.x, start.y, prev.x - start.x));
            }
            return listRleBlockArray;
        }
        /// <summary>
        /// 获取march cube value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetSquareValue(int x, int y)
        {
            /*
            checking the 2x2 pixel grid, assigning these values to each pixel, if not transparent

            +---+---+
            | 1 | 2 |
            +---+---+
            | 1 | 8 | <- current pixel (pX,pY)
            +---+---+
            */
            int squareValue = 0;
            // checking upper left pixel
            if (CheckBlock(x - 1, y - 1) == true)
            {
                squareValue += 1;
            }
            // checking upper pixel
            if (CheckBlock(x, y - 1) == true)
            {
                squareValue += 2;
            }
            // checking left pixel
            if (CheckBlock(x - 1, y) == true)
            {
                squareValue += 4;
            }
            // checking the pixel itself
            if (CheckBlock(x, y) == true)
            {
                squareValue += 8;
            }
            return squareValue;
        }
        /// <summary>
        /// 裁剪挡格数据
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="listblock"></param>
        /// <param name="minPos"></param>
        /// <param name="maxPos"></param>
        /// <returns></returns>
        public bool CutBlockArea(StageGateArea ar, ref List<Short2> listblock, ref Short2 minPos, ref Short2 maxPos)
        {
            if (ar == null)
                return false;
            Float2 min = Float2.Min(ar.leftBottom, ar.rightUp);
            Float2 max = Float2.Max(ar.leftBottom, ar.rightUp);

            Short2 min2 = GetIndex(min);
            Short2 max2 = GetIndex(max);

            Short2 diff = max2 - min2;
            int width = diff.x + 1;
            int height = diff.y + 1;

            maxPos = min2;
            minPos = max2;
            List<Short2> list = new List<Short2>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Short2 pos = new Short2(x + min2.x, y + min2.y);
                    if (CheckBlock(pos) == true)
                    {
                        list.Add(new Short2(x, y));
                        minPos = Short2.Min(minPos, pos);
                        maxPos = Short2.Max(maxPos, pos);
                    }
                }
            }
            if (listblock == null)
            {
                listblock = new List<Short2>();
            }
            listblock.Clear();
            foreach (Short2 pos in list)
            {
                listblock.Add(pos - (minPos - min2));
            }
            return true;
        }
        /// <summary>
        /// 清理连续挡格。
        /// </summary>
        /// <param name="pos"></param>
        public void EmptyContinuousBlock(Short2 pos)
        {
            if (CheckBlock(pos) == false)
                return;

            List<Short2> listOpenPts = new List<Short2>();
            listOpenPts.Add(pos);
            while (listOpenPts.Count > 0)
            {
                Short2 p = listOpenPts[0];
                listOpenPts.RemoveAt(0);
                if (CheckBlock(p) == false)
                {
                    continue;
                }
                SetUnBlock(p);
                // 加入邻居1
                Short2 p1 = p + Short2.left;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }

                // 加入邻居2
                p1 = p + Short2.leftDown;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }
                // 加入邻居3
                p1 = p + Short2.leftUp;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }
                // 加入邻居4
                p1 = p + Short2.up;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }
                // 加入邻居5
                p1 = p + Short2.down;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }
                // 加入邻居6
                p1 = p + Short2.right;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }
                // 加入邻居7
                p1 = p + Short2.rightUp;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }
                // 加入邻居8
                p1 = p + Short2.rightDown;
                if (CheckInMap(p1) && CheckBlock(p1) == true && listOpenPts.Contains(p1) == false)
                {
                    listOpenPts.Add(p1);
                }
            }
        }

    }

}

