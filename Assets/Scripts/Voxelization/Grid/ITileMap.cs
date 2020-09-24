using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Voxelization
{
    public interface ITileMap
    {
        /// <summary>
        /// 初始化瓦片地图
        /// </summary>
        /// <param name="mapSize">地图大小</param>
        /// <param name="tileSize">格子的大小</param>
        /// <param name="sdfValue">sdf值，生成格子地图时的设置</param>
        /// <returns></returns>
        bool Init(Short2 mapSize, float tileSize, float sdfValue);
        /// <summary>
        /// 设置格子数据，
        /// </summary>
        /// <param name="lbock">都在地图范围呢，所有不做检测</param>
        /// <returns></returns>
        bool SetBlockData(List<Short2> lbock);
        /// <summary>
        /// 清理
        /// </summary>
        void Clear();
    }
}