using RayGraphics.Math;
using System.Collections.Generic;
/// <summary>
/// 瓦片地图基类
/// </summary>
namespace RayGraphics.Voxelization
{
    public class TileBaseMap :ITileMap
    {
        /// <summary>
        /// 挡格数组，1 挡格， 0非挡格
        /// </summary>
        protected byte[][] _blockArray = null;
        /// <summary>
        /// 地图大小
        /// </summary>
        protected Short2 _mapSize;
        public int tileXnum
        {
            get { return _mapSize.x; }
        }
        public int tileYnum
        {
            get { return _mapSize.y; }
        }
        /// <summary>
        /// 瓦片大小
        /// </summary>
        protected float _tileSize = 1;
        public float TileSize
        {
            get { return _tileSize; }
        }
        /// <summary>
        /// sdf value
        /// </summary>
        protected float _sdfValue;
        public float SdfValue
        {
            get { return _sdfValue; }
        }
        /// <summary>
        /// 基础初始化
        /// </summary>
        /// <param name="mapSize"></param>
        /// <param name="tileSize"></param>
        /// <param name="sdfValue"></param>
        /// <returns></returns>
        public bool Init(Short2 mapSize, float tileSize, float sdfValue)
        {
            if (mapSize.x == 0 || mapSize.y == 0 || sdfValue < 0 || tileSize <= 0)
                return false;
            this._mapSize = mapSize;
            this._tileSize = tileSize;
            _blockArray = new byte[tileYnum][];
            for (short y = 0; y < tileYnum; y++)
            {
                _blockArray[y] = new byte[tileXnum];
                for (short x = 0; x < tileXnum; x++)
                {
                    _blockArray[y][x] = 0;
                }
            }
            return true;
        }
        /// <summary>
        /// 获取格子坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Short2 GetIndex(Float2 pos)
        {
            Float2 diff = (pos - Float2.zero)/ _tileSize;
            return new Short2((int)diff.x, (int)diff.x);
        }
        /// <summary>
        /// 获取格子坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Short2 GetIndex(Double2 pos)
        {
            Double2 diff = (pos - Double2.zero) / _tileSize;
            return new Short2((int)diff.x, (int)diff.x);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CheckInMap(Short2 pos)
        {
            if (pos >= Short2.zero && pos < this._mapSize)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 判断是否为挡格
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CheckBlock(Short2 pos)
        {
            return CheckBlock(pos.x, pos.y);
        }
        /// <summary>
        /// 判断是否为挡格
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns> true 为挡格</returns>
        public bool CheckBlock(int x, int y)
        {
            if (_blockArray == null)
                return true;

            if (CheckInMap(new Short2(x, y)) == true)
            {
                byte value = _blockArray[y][x];
                return value != 0 ? true : false;
            }
            else return true;
        }
        /// <summary>
        /// 设置为挡格
        /// </summary>
        /// <param name="pos"></param>
        public void SetBlock(Short2 pos)
        {
            if (_blockArray == null)
                return;
            if (CheckInMap(pos) == true)
            {
                _blockArray[pos.y][pos.x] = 1;
            }
        }
        /// <summary>
        /// 设置为非挡格
        /// </summary>
        /// <param name="pos"></param>
        public void SetUnBlock(Short2 pos)
        {
            if (_blockArray == null)
                return;
            if (CheckInMap(pos) == true)
            {
                _blockArray[pos.y][pos.x] = 0;
            }
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (_blockArray == null)
                return;
            if (_blockArray != null)
            {
                for (short y = 0; y < _blockArray.Length; y++)
                {
                    if (_blockArray[y] != null)
                    {
                        _blockArray[y] = null;
                    }
                }
            }
            _blockArray = null;
        }
        /// <summary>
        /// 确定X方向上有挡格
        /// </summary>
        /// <param name="x"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public bool CheckXDirHaveBlock(int x, int yMin, int yMax)
        {
            if (_blockArray == null)
                return true;
            Short2 min = new Short2(x, yMin);
            Short2 max = new Short2(x, yMax);
            if (CheckInMap(min) == false || CheckInMap(max) == false)
                return true;

            for (int y = yMin; y <= yMax; y++)
            {
                if (_blockArray[y][x] != 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 确定Y方向上有挡格
        /// </summary>
        /// <param name="y"></param>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <returns></returns>
        public bool CheckYDirHaveBlock(int y, int xMin, int xMax)
        {
            if (_blockArray == null)
                return true;
            Short2 min = new Short2(xMin, y);
            Short2 max = new Short2(xMax, y);
            if (CheckInMap(min) == false || CheckInMap(max) == false)
                return true;

            for (int x= xMin; x <= xMax; x--)
            {
                if (_blockArray[y][x] != 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 设置格子数据，
        /// </summary>
        /// <param name="lbock">都在地图范围呢，所有不做检测</param>
        /// <returns></returns>
        public virtual bool SetBlockData(List<Short2> lbock)
        {
            if (_blockArray == null)
                return false;
            if (lbock != null && lbock.Count > 0)
            {
                foreach (Short2 pos in lbock)
                {
                    _blockArray[pos.y][pos.x] = 1;
                }
            }
            return true;
        }
        /// <summary>
        /// 设置未sdf的挡格数据。
        /// </summary>
        /// <param name="lbock"></param>
        public bool SetUnSDFBlockData(ref List<Short2> lbock)
        {
            if (_blockArray == null)
                return false;
            // SDF 处理
            int range = (int)(this.SdfValue / this.TileSize);
            range = (range == 0) ? 1 : range;
            SdfProcess(ref lbock, range);
            SetBlockData(lbock);
            return true;
        }
        /// <summary>
        /// sdf 处理，整个挡格扩大一圈
        /// </summary>
        /// <param name="lbock"></param>
        /// <param name="range">扩大的范围</param>
        private void SdfProcess(ref List<Short2> lbock, int range)
        {
            List<Short2> listNew = new List<Short2>();
            foreach (Short2 pos in lbock)
            {
                if (CheckInMap(pos) == false)
                    continue;
                for (int y = pos.y - range; y <= pos.y + range; y++)
                {
                    for (int x = pos.x - range; x <= pos.x + range; x++)
                    {
                        Short2 p = new Short2(x, y);
                        if (CheckInMap(p) == false)
                            continue;
                        listNew.Add(p);
                    }
                }
            }
            lbock = listNew;
        }
    }
}
