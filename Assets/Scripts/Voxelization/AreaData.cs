using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Voxelization
{
    [System.Serializable]
    public class AreaData
    {
        public List<MapArea> myArea = new List<MapArea>();
        public List<StageGateArea> mydoor = new List<StageGateArea>();

        /// <summary>
        /// 生成轮廓数据
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="limitDistance"></param>
        /// <param name="ret"></param>
        public void GeneralContour(RLEGridMap grid, ref ContourData ret)
        {
            if (ret == null)
            {
                ret = new ContourData();
            }
            ret.Clear();
            ret.tileSize = (int)grid.TileSize;
            ret.sdfValue = (int)grid.SdfValue;
            ret.width = grid.tileXnum * ret.tileSize;
            ret.height = grid.tileYnum * ret.tileSize;
            float limitDistance = ret.tileSize;
            for (int i= 0; i< this.myArea.Count; i++)
            {
                MapArea submap = this.myArea[i];
                ContourUnit unit = ContourOptimization.GeneralContourUnit(grid, submap, limitDistance);
                ret.AddAreaMap(unit);
            }

            for (int i = 0; i < this.mydoor.Count; i++)
            {
                StageGateArea stage = this.mydoor[i];
                ContourUnit unit = ContourOptimization.GeneralContourUnit(grid, stage, limitDistance);
                unit.SetDoorData(stage.MindoorCenter, stage.MaxdoorCenter, stage.isXdir, grid.TileSize, i);
                ret.AddStage(unit);
            }
            // 分析连接关系
            ret.ParseDoorLink();
        }
    }

    [System.Serializable]
    public class MapArea : MapElement
    {
        public Float2 keyPoint;
        public override Float2 GetKeyPoint()
        {
            return keyPoint;
        }
    }
}
