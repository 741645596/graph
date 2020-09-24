using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Voxelization
{
    public class StageDoorData
    {
        /// <summary>
        /// 已经是偏移过的坐标系了。
        /// </summary>
        public List<Short2> listblock = new List<Short2>();
        public Short2 startPos;
        public int width;
        public int height;
        public bool _isXdir;

        public int _minAixs;
        public int _maxAixs;

        public int _minSecondAixs;
        public int _maxSecondAixs;

        public Short2[] listWalkData = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min2"></param>
        /// <param name="max2"></param>
        /// <param name="blocks"></param>
        /// <param name="isXdir"></param>
        /// <returns></returns>
        public bool CutdoorblockArea(Short2 min2, Short2 max2, List<Short2> blocks, bool isXdir)
        {
            this._isXdir = isXdir;
            int min = int.MaxValue;
            int max = int.MinValue;
            foreach (Short2 pos in blocks)
            {
                if (isXdir == true)
                {
                    min = System.Math.Min(min, pos.x);
                    max = System.Math.Max(max, pos.x);
                }
                else
                {
                    min = System.Math.Min(min, pos.y);
                    max = System.Math.Max(max, pos.y);
                }
            }
            Short2 maxPos = min2;
            Short2 minPos = max2;
            for (int i = 0; i < blocks.Count;)
            {
                if (isXdir == true)
                {
                    if (blocks[i].x < min || blocks[i].x > max)
                    {
                        blocks.RemoveAt(i);
                        continue;
                    }

                    minPos = Short2.Min(minPos, blocks[i] + min2);
                    maxPos = Short2.Max(maxPos, blocks[i] + min2);

                }
                else
                {
                    if (blocks[i].y < min || blocks[i].y > max)
                    {
                        listblock.RemoveAt(i);
                        continue;
                    }
                    minPos = Short2.Min(minPos, blocks[i] + min2);
                    maxPos = Short2.Max(maxPos, blocks[i] + min2);
                }
                i++;
            }

            if (listblock == null)
            {
                listblock = new List<Short2>();
            }
            else listblock.Clear();
            foreach (Short2 pos in blocks)
            {
                listblock.Add(pos - (minPos - min2));
            }
            this.startPos = minPos;
            Short2 diff = maxPos - minPos;
            this.width = diff.x + 1;
            this.height = diff.y + 1;
            ParseMinAixs();
            ParseMaxAixs();
            GetSecondAixs();
            return true;
        }
        /// <summary>
        /// 获取近
        /// </summary>
        private void ParseMinAixs()
        {
            if (listblock == null || listblock.Count == 0)
            {
                _minAixs = 0;
                return;
            }

            if (this._isXdir == false)
            {
                for (int i = 0; i < this.width; i++)
                {
                    bool ishave = false;
                    foreach (Short2 pos in listblock)
                    {
                        if (pos.x == i)
                        {
                            ishave = true;
                            break;
                        }
                    }
                    if (ishave == false)
                    {
                        _minAixs = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.height; i++)
                {
                    bool ishave = false;
                    foreach (Short2 pos in listblock)
                    {
                        if (pos.y == i)
                        {
                            ishave = true;
                            break;
                        }
                    }
                    if (ishave == false)
                    {
                        _minAixs = i;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取远
        /// </summary>
        private void ParseMaxAixs()
        {
            if (listblock == null || listblock.Count == 0)
            {
                if (this._isXdir == false)
                {
                    _maxAixs = this.width - 1;
                }
                else
                {
                    _maxAixs = this.height - 1;
                }
                return;
            }

            if (this._isXdir == false)
            {
                for (int i = this.width - 1; i >= 0; i--)
                {
                    bool ishave = false;
                    foreach (Short2 pos in listblock)
                    {
                        if (pos.x == i)
                        {
                            ishave = true;
                            break;
                        }
                    }
                    if (ishave == false)
                    {
                        _maxAixs = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = this.height - 1; i >= 0; i--)
                {
                    bool ishave = false;
                    foreach (Short2 pos in listblock)
                    {
                        if (pos.y == i)
                        {
                            ishave = true;
                            break;
                        }
                    }
                    if (ishave == false)
                    {
                        _maxAixs = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取第二轴。
        /// </summary>
        public void GetSecondAixs()
        {
            int min1;
            int min2;
            int max1;
            int max2;
            if (this._isXdir == true)
            {
                min1 = this.width - 1;
                min2 = this.width - 1;
                max1 = 0;
                max2 = 0;
                foreach (Short2 pos in listblock)
                {
                    if (pos.y < this._minAixs)
                    {
                        min1 = System.Math.Min(min1, pos.x);
                        max1 = System.Math.Max(max1, pos.x);
                    }
                    else if (pos.y > this._maxAixs)
                    {
                        min2 = System.Math.Min(min2, pos.x);
                        max2 = System.Math.Max(max2, pos.x);
                    }
                }
            }
            else
            {
                min1 = this.height - 1;
                min2 = this.height - 1;
                max1 = 0;
                max2 = 0;
                foreach (Short2 pos in listblock)
                {
                    if (pos.x < this._minAixs)
                    {
                        min1 = System.Math.Min(min1, pos.y);
                        max1 = System.Math.Max(max1, pos.y);
                    }
                    else if (pos.x > this._maxAixs)
                    {
                        min2 = System.Math.Min(min2, pos.y);
                        max2 = System.Math.Max(max2, pos.y);
                    }
                }
            }
            this._minSecondAixs = System.Math.Max(min1, min2);
            this._maxSecondAixs = System.Math.Min(max1, max2);
        }

        /// <summary>
        /// 获取边缘点
        /// </summary>
        /// <param name="listB"></param>
        public void GetContourData(ref List<Short2> listB, ref Float2 MindoorCenter, ref Float2 MaxdoorCenter)
        {
            if (this._isXdir == true)
            {
                listWalkData = new Short2[this.width];
                for (int i = this._minSecondAixs; i <= this._maxSecondAixs; i++)
                {
                    int min = this._minAixs;
                    // 获取小。
                    for (int y = this._minAixs; y >= 0; y--)
                    {
                        if (CheckNearBlockPoint(new Short2(i, y), new Short2(i, y - 1)) == true)
                        {
                            min = y;
                            break;
                        }
                    }
                    // 获取大的。
                    int max = this._maxAixs;
                    for (int y = this._maxAixs; y < this.height; y++)
                    {
                        if (CheckNearBlockPoint(new Short2(i, y), new Short2(i, y + 1)) == true)
                        {
                            max = y;
                            break;
                        }
                    }
                    listWalkData[i] = new Short2(min, max);
                }
            }
            else
            {
                listWalkData = new Short2[this.height];
                for (int i = this._minSecondAixs; i <= this._maxSecondAixs; i++)
                {
                    int min = this._minAixs;
                    // 获取小。
                    for (int x = this._minAixs; x >= 0; x--)
                    {
                        if (CheckNearBlockPoint(new Short2(x, i), new Short2(x - 1, i)) == true)
                        {
                            min = x;
                            break;
                        }
                    }
                    // 获取大的。
                    int max = this._maxAixs;
                    for (int x = this._maxAixs; x < this.width; x++)
                    {
                        if (CheckNearBlockPoint(new Short2(x, i), new Short2(x + 1, i)) == true)
                        {
                            max = x;
                            break;
                        }
                    }
                    listWalkData[i] = new Short2(min, max);
                }
            }

            if (listB == null)
            {
                listB = new List<Short2>();
            }
            else listB.Clear();


            if (this._isXdir == true)
            {
                for (int i = this._minSecondAixs; i <= this._maxSecondAixs; i++)
                {
                    Short2 ll = listWalkData[i];
                    // 获取小。
                    if (i == this._minSecondAixs || i == this._maxSecondAixs)
                    {
                        for (int y = ll.x; y <= ll.y; y++)
                        {
                            listB.Add(new Short2(i, y));
                        }
                        if (i == this._minSecondAixs)
                        {
                            MindoorCenter = new Float2(this._minSecondAixs, (ll.x + ll.y) * 0.5f);
                        }
                        else if (i == this._maxSecondAixs)
                        {
                            // 不知道为什么要+1 先这样改正
                            MaxdoorCenter = new Float2(this._maxSecondAixs + 1, (ll.x + ll.y) * 0.5f);
                        }
                    }
                }
            }
            else
            {
                for (int i = this._minSecondAixs; i <= this._maxSecondAixs; i++)
                {
                    Short2 ll = listWalkData[i];
                    // 获取小。
                    if (i == this._minSecondAixs || i == this._maxSecondAixs)
                    {
                        for (int x = ll.x; x <= ll.y; x++)
                        {
                            listB.Add(new Short2(x, i));
                        }
                        if (i == this._minSecondAixs)
                        {
                            MindoorCenter = new Float2((ll.x + ll.y) * 0.5f, this._minSecondAixs);
                        }
                        else if (i == this._maxSecondAixs)
                        {
                            // 不知道为什么要+1 先这样改正
                            MaxdoorCenter = new Float2((ll.x + ll.y) * 0.5f,this._maxSecondAixs + 1);
                        }
                    }
                }
            }
            // 算上世界位置。
            for (int i = 0; i < listB.Count; i++)
            {
                listB[i] += startPos;
            }
            MindoorCenter += new Float2(startPos.x, startPos.y);
            MaxdoorCenter += new Float2(startPos.x, startPos.y);
        }
        /// <summary>
        /// 边缘点
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="nextPos"></param>
        /// <returns></returns>
        public bool CheckNearBlockPoint(Short2 pos, Short2 nextPos)
        {
            if (this.listblock.Contains(pos) == false && this.listblock.Contains(nextPos) == true)
                return true;
            else return false;
        }
    }
}
