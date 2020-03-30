using System.Collections.Generic;
using RayGraphics.Math;

/// <summary>
/// 瓦片分割区域空间管理方案
/// </summary>
namespace RayGraphics.Tree
{
    public class AreaTile <T>
    {
        /// <summary>
        /// 挡格数组，bit 存储一个挡格数据
        /// </summary>
        private static TileUnit<T>[][] s_dynblockArray ;
        /// <summary>
        /// 瓦片X的num
        /// </summary>
        private static int s_Xnum;
        /// <summary>
        /// 瓦片Z的num
        /// </summary>
        private static int s_Ynum;
        /// <summary>
        /// 瓦片X的num
        /// </summary>
        private static float s_tileSize;
        /// <summary>
        /// 起始位置
        /// </summary>
        private static Float2 s_startPos;
        /// <summary>
        /// 挡格列表
        /// </summary>
        //private static Dictionary<long, AABB> s_listBlockRc = new Dictionary<long, AABB>();
        /// <summary>
        /// 格子数据初始化
        /// </summary>
        public static void Init(Float2 startPos, float tileSize, int xnum, int ynum)
        {
            s_Xnum = xnum;
            s_Ynum = ynum;
            s_tileSize = tileSize;
            s_startPos = startPos;
            //CreateBlockArray();
        }
    }




    public class TileUnit<T>
    {
        public HashSet<T> listDyn = new HashSet<T>();

        public void Add(T blockID)
        {
            listDyn.Add(blockID);
        }

        public void Remove(T blockID)
        {
            listDyn.Remove(blockID);
        }

        public void ClearDynBlock()
        {
            listDyn.Clear();
        }

        public bool HaveBlock
        {
            get { return listDyn.Count > 0 ? true : false; }
        }
    }
}