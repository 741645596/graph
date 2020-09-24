using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Voxelization
{

    [System.Serializable]
    public class MapElement
    {
        public virtual Float2 GetKeyPoint()
        {
            return Float2.zero;
        }
        /// <summary>
        /// 获取轮廓
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public virtual List<Double2> GetContourPoints(RLEGridMap grid, Float2 startSearchPoint)
        {
            List<Double2> listPoint = new List<Double2>();

            Short2 ContourStartPoint = Short2.zero;
            Short2 ContourStartBlockPoint = Short2.zero;
            if (GetStartingPoint(ref ContourStartPoint, ref ContourStartBlockPoint, startSearchPoint, grid) == true)
            {
                List<Short2> list = MarchingSquares(ContourStartPoint, grid);
                foreach (Short2 v in list)
                {
                    Double2 pos = new Double2(v.x, v.y) * grid.TileSize;
                    listPoint.Add(pos);
                }
                // 轮廓挡格区域点。 不能这样清理哦。
                grid.EmptyContinuousBlock(ContourStartBlockPoint);
                //Core.BlockTool.BlockTexureExport.SaveBlockTexture(2, grid);
            }
            return listPoint;
        }
        /// <summary>
        /// 获取轮廓起点及轮廓挡格起点
        /// </summary>
        /// <param name="ContourStartPoint">轮廓起点</param>
        /// <param name="ContourStartBlockPoint">轮廓挡格起点</param>
        /// <param name="startSearchPoint">悬针搜索点</param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public bool GetStartingPoint(ref Short2 ContourStartPoint, ref Short2 ContourStartBlockPoint, Float2 startSearchPoint, RLEGridMap grid)
        {
            Short2 center = grid.GetIndex(startSearchPoint);
            if (grid.CheckInMap(center) == false)
                return false;

            for (int j = 0; j <= center.y; j++)
            {
                if (grid.CheckBlock(center - new Short2(0, j)) == true)
                {
                    ContourStartPoint = center - new Short2(0, j - 1);
                    ContourStartBlockPoint = center - new Short2(0, j);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 使用移动cubes 获取轮廓线。Contour 轮廓
        /// </summary>
        /// <returns></returns>
        public List<Short2> MarchingSquares(Short2 startPoint, RLEGridMap grid)
        {
            List<Short2> listPoint = new List<Short2>();
            int pX = startPoint.x;
            int pY = startPoint.y;
            int stepX = 0;
            int stepY = 0;
            int prevX = 0;
            int prevY = 0;
            bool closedLoop = false;
            while (!closedLoop)
            {
                // the core of the script is getting the 2x2 square value of each pixel
                int squareValue = grid.GetSquareValue(pX, pY);
                switch (squareValue)
                {
                    /* going UP with these cases:
                    +---+---+   +---+---+   +---+---+
                    | 1 |   |   | 1 |   |   | 1 |   |
                    +---+---+   +---+---+   +---+---+
                    |   |   |   | 4 |   |   | 4 | 8 |
                    +---+---+  	+---+---+  	+---+---+
                    */
                    case 1:
                    case 5:
                    case 13:
                        stepX = 0;
                        stepY = -1;
                        break;
                    /* going DOWN with these cases:
                    +---+---+   +---+---+   +---+---+
                    |   |   |   |   | 2 |   | 1 | 2 |
                    +---+---+   +---+---+   +---+---+
                    |   | 8 |   |   | 8 |   |   | 8 |
                    +---+---+  	+---+---+  	+---+---+
                    */
                    case 8:
                    case 10:
                    case 11:
                        stepX = 0;
                        stepY = 1;
                        break;
                    /* going LEFT with these cases:

                    +---+---+   +---+---+   +---+---+
                    |   |   |   |   |   |   |   | 2 |
                    +---+---+   +---+---+   +---+---+
                    | 4 |   |   | 4 | 8 |   | 4 | 8 |
                    +---+---+  	+---+---+  	+---+---+
                    */
                    case 4:
                    case 12:
                    case 14:
                        stepX = -1;
                        stepY = 0;
                        break;
                    /* going RIGHT with these cases:

                    +---+---+   +---+---+   +---+---+
                    |   | 2 |   | 1 | 2 |   | 1 | 2 |
                    +---+---+   +---+---+   +---+---+
                    |   |   |   |   |   |   | 4 |   |
                    +---+---+  	+---+---+  	+---+---+
                    */
                    case 2:
                    case 3:
                    case 7:
                        stepX = 1;
                        stepY = 0;
                        break;
                    case 6:
                        /* special saddle point case 1:
                        +---+---+ 
                        |   | 2 | 
                        +---+---+
                        | 4 |   |
                        +---+---+
                        going LEFT if coming from UP
                        else going RIGHT 

                        */
                        if (prevX == 0 && prevY == -1)
                        {
                            stepX = -1;
                            stepY = 0;
                        }
                        else
                        {
                            stepX = 1;
                            stepY = 0;
                        }
                        break;
                    case 9:
                        /* special saddle point case 2:

                            +---+---+ 
                            | 1 |   | 
                            +---+---+
                            |   | 8 |
                            +---+---+

                            going UP if coming from RIGHT
                            else going DOWN 

                            */
                        if (prevX == 1 && prevY == 0)
                        {
                            stepX = 0;
                            stepY = -1;
                        }
                        else
                        {
                            stepX = 0;
                            stepY = 1;
                        }
                        break;
                }
                // moving onto next point
                pX += stepX;
                pY += stepY;
                // saving contour point
                listPoint.Add(new Short2(pX, pY));
                prevX = stepX;
                prevY = stepY;
                // if we returned to the first point visited, the loop has finished
                if (pX == startPoint.x && pY == startPoint.y)
                {
                    closedLoop = true;
                }
            }
            return listPoint;
        }
    }
}
