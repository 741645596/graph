using System;

namespace RayGraphics.Math
{
    /// <summary>
    /// 暴雪 hash 算法one-way hash 
    /// </summary>
    public class BlizzardHash
    {
        /// <summary>
        /// 表
        /// </summary>
        private uint[] m_CryptTable = new uint[0x500];
        /// <summary>
        /// 生成hash
        /// </summary>
        public void PrepareTable()
        {
            uint seed = 0x00100001;
            uint index1 ;
            uint index2 ;
            uint i ;
            for (index1 = 0; index1 < 0x100; index1++)
            {
                for (index2 = index1, i = 0; i < 5; i++, index2 += 0x100)
                {
                    uint temp1, temp2;
                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp1 = (seed & 0xFFFF) << 0x10;
                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp2 = (seed & 0xFFFF);
                    m_CryptTable[index2] = (temp1 | temp2);
                }
            }
        }
        /// <summary>
        ///  Blizzard的这个算法是非常高效的，被称为"One-Way Hash"
        ///  A one-way hash is a an algorithm that is constructed in such a way 
        ///  that deriving the original string (set of strings, actually) is virtually impossible
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="dwHashType">hash 类型</param>
        /// <returns></returns>
        public uint HashString(string strValue, uint dwHashType)
        {
            uint seed1 = 0x7FED7FED;
            uint seed2 = 0xEEEEEEEE;
            uint ch;
            foreach (char c in strValue)
            {
                ch = c;
                // 因为是作为文件系统使用，所以才用字符串大小不敏感。
                if (ch >= 'a' && ch <= 'z')
                {
                    ch = ch + 'A' - 'a';
                }
                seed1 = m_CryptTable[(dwHashType << 8) + ch] ^ (seed1 + seed2);
                seed2 = ch + seed1 + seed2 + (seed2 << 5) + 3;
            }
            return seed1;
        }
        /// <summary>
        /// 查找hash所在的位置
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="lpTable"></param>
        /// <param name="nTableSize"></param>
        /// <returns></returns>
        public int GetHashTablePos(string strValue, SomeHash[] lpTable, int nTableSize)
        {
            uint hashOffset = 0;
            uint hashA = 1;
            uint hashB = 2;
            //计算出字符串的三个哈希值（一个用来确定位置，另外两个用来校验)
            uint nHash = HashString(strValue, hashOffset);
            uint nHashA = HashString(strValue, hashA);
            uint nHashB = HashString(strValue, hashB);

            int nHashStart = (int)(nHash % nTableSize);
            int nHashPos = nHashStart;

            while (lpTable[nHashPos].bExists)
            {
                // 检查2个hash值是否匹对。
                if (lpTable[nHashPos].nHashA == nHashA && lpTable[nHashPos].nHashB == nHashB)
                {
                    return nHashPos;
                }
                else // 找到下一个位置。
                {
                    nHashPos = (nHashPos + 1) % nTableSize;
                }

                if (nHashPos == nHashStart)
                    break;
            }
            return -1;
        }
    }
    /// <summary>
    /// 相同的hash
    /// </summary>
    public struct SomeHash
    {
        public int nHashA;
        public int nHashB;
        public bool bExists;
    };
}
