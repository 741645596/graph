using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Voxelization
{

    [System.Serializable]
    public class ContourUnit
    {
        public ContourUnit() { }
        public ContourUnit(ContourPoly outPoly)
        {
            OutPoly = outPoly;
        }
        /// <summary>
        /// out 多边形
        /// </summary>
        public ContourPoly OutPoly = new ContourPoly();
        /// <summary>
        /// block 多边形
        /// </summary>
        public List<ContourPoly> blockPolys = new List<ContourPoly>();
        /// <summary>
        /// 只有关卡又这个数据
        /// </summary>
        public DoorData minDoor = null;
        public DoorData maxDoor = null;
        /// <summary>
        /// 设置关卡地图door的数据
        /// </summary>
        /// <param name="minDoorPos"></param>
        /// <param name="maxDoorPos"></param>
        /// <param name="isXdir"></param>
        /// <param name="TileSize"></param>
        /// <param name="stageMapIndex"></param>
        public void SetDoorData(Float2 minDoorPos, Float2 maxDoorPos, bool isXdir, float TileSize, int stageMapIndex)
        {
            this.minDoor = new DoorData();
            this.minDoor.Init(minDoorPos, isXdir, TileSize, false);
            this.minDoor.linkStageMapID = stageMapIndex;
            //
            this.maxDoor = new DoorData();
            this.maxDoor.Init(maxDoorPos, isXdir, TileSize, true);
            this.maxDoor.linkStageMapID = stageMapIndex;
        }

        /// <summary>
        /// 判断点是否在轮廓里面
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CheckIn(Float2 pos)
        {
            return OutPoly.CheckIn(pos);
        }
        /// <summary>
        /// 添加挡格轮廓多边形
        /// </summary>
        /// <param name="unit"></param>
        public void AddBlockPoly(ContourPoly poly)
        {
            if (poly == null)
                return;
            blockPolys.Add(poly);
        }

        public void Clear()
        {
            OutPoly.Clear();
            foreach (ContourPoly poly in blockPolys)
            {
                poly.Clear();
            }
            blockPolys.Clear();
        }


    }

    [System.Serializable]
    public class DoorData
    {
        public DoorData()
        { }

        public DoorData(DoorData v)
        {
            this.ID = v.ID;
            this.centerPos = v.centerPos;
            this.InAraaMapPos = v.InAraaMapPos;
            this.InStageMapPos = v.InStageMapPos;
            this.linkAreaMapID = v.linkAreaMapID;
            this.linkStageMapID = v.linkStageMapID;
        }

        public DoorData(DoorData v, int newID)
        {
            this.ID = newID;
            this.centerPos = v.centerPos;
            this.InAraaMapPos = v.InAraaMapPos;
            this.InStageMapPos = v.InStageMapPos;
            this.linkAreaMapID = v.linkAreaMapID;
            this.linkStageMapID = v.linkStageMapID;
        }

        public int ID;
        /// <summary>
        /// 只有关卡又这个数据
        /// </summary>
        public Float2 centerPos;
        public Float2 InAraaMapPos;
        public Float2 InStageMapPos;
        public int linkStageMapID;
        public int linkAreaMapID;


        public void Init(Float2 pos, bool isXdir, float TileSize, bool isMax)
        {
            float step = TileSize * 2;
            this.centerPos = pos;
            if (isXdir == true)
            {
                if (isMax == false)
                {
                    this.InAraaMapPos = this.centerPos - Float2.right * step;
                    this.InStageMapPos = this.centerPos + Float2.right * step;
                }
                else 
                {
                    this.InAraaMapPos = this.centerPos + Float2.right * step;
                    this.InStageMapPos = this.centerPos - Float2.right * step;
                }
            }
            else
            {
                if (isMax == false)
                {
                    this.InAraaMapPos = this.centerPos - Float2.up * step;
                    this.InStageMapPos = this.centerPos + Float2.up * step;
                }
                else
                {
                    this.InAraaMapPos = this.centerPos + Float2.up * step;
                    this.InStageMapPos = this.centerPos - Float2.up * step;
                }
            }
        }
    }
}
