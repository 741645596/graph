using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Voxelization
{
    [System.Serializable]
    public class ContourData
    {
        /// <summary>
        /// 实际没什么用，用于保存生成数据时的设置
        /// </summary>
        public int tileSize;
        /// <summary>
        /// 实际没什么用，用于保存生成数据时的设置
        /// </summary>
        public int sdfValue;
        /// <summary>
        /// 实际没什么用，用于保存生成数据时的设置
        /// </summary>
        public int width;
        /// <summary>
        /// 实际没什么用，用于保存生成数据时的设置
        /// </summary>
        public int height;
        /// <summary>
        /// 区域地图
        /// </summary>
        public List<ContourUnit> areaMaps = new List<ContourUnit>();
        /// <summary>
        /// 关卡
        /// </summary>
        public List<ContourUnit> Stages = new List<ContourUnit>();

        /// <summary>
        /// 添加 area map 轮廓
        /// </summary>
        /// <param name="unit"></param>
        public void AddAreaMap(ContourUnit unit)
        {
            if (unit == null)
                return;
            areaMaps.Add(unit);
        }
        /// <summary>
        /// 添加 关卡轮廓
        /// </summary>
        /// <param name="unit"></param>
        public void AddStage(ContourUnit unit)
        {
            if (unit == null)
                return;
            Stages.Add(unit);
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            foreach (ContourUnit unit in areaMaps)
            {
                unit.Clear();
            }
            areaMaps.Clear();
            foreach (ContourUnit unit in Stages)
            {
                unit.Clear();
            }
            Stages.Clear();
        }

        /// <summary>
        /// 分析连接关系
        /// </summary>
        public void ParseDoorLink()
        {
            for (int i = 0; i < Stages.Count; i++)
            {
                DoorData d1 = Stages[i].minDoor;
                if (d1 != null)
                {
                    d1.linkAreaMapID = ParseDoorLink(d1.InAraaMapPos);
                }
                DoorData d2 = Stages[i].maxDoor;
                if (d2 != null)
                {
                    d2.linkAreaMapID = ParseDoorLink(d2.InAraaMapPos);
                }
            }
        }

        /// <summary>
        /// 分析点在那个子地图里面
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int ParseDoorLink(Float2 pos)
        {
            for (int i = 0; i < areaMaps.Count; i++)
            {
                if (areaMaps[i].CheckIn(pos) == true)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}