using System;
/// <summary>
/// https://blog.csdn.net/pkueecser/article/details/15504753
/// </summary>
namespace RayGraphics.Math
{
    /// <summary>
    /// hash函数
    /// </summary>
    public class HashCodeHelp 
    {
        /// <summary>
        /// String中hashCode方法
        /// 算法：s[0]*31^(n-1) + s[1]*31^(n-2) + … + s[n-1]
        /// </summary>
        /// <returns></returns>
        public int hashCode(string value)
        {
            int hash = 0;
            if (hash == 0 && value.Length > 0)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    hash = 31 * hash + value[i];
                }
            }
            return hash;
        }

        /// <summary>
        /// 一个比较好的字符串hash算法，add到这里，听说是微软用
        /// 这个算法要保证str的长度是8的倍数，如果不够用0补齐，
        /// 不然会导致越界，计算出的hash就不准确了。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        
        uint getHashCode(string value)
        {
            /*uint32_t chPtr[MAX_LEN];
                memset(chPtr,0,sizeof(chPtr));
                int length = strlen(str);
                memcpy(chPtr, str, length);
                UINT32 num = 0x15051505;
                UINT32 num2 = num;
                UINT32* numPtr = (UINT32*)chPtr;
                int i = 0;
            for(i=length;i>0;i-=4)
            {   
                num = (((num << 5) + num) + (num >> 0x1b)) ^ numPtr[0];
                if(i<=2) break;
                num2=(((num2 << 5) + num2) + (num2 >> 0x1b)) ^ numPtr[1];
                numPtr+=2;
            }   
            return (num + (num2* 0x5d588b65));*/
            return 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint SDBMHash(string value)
        {
            uint hash = 0;
            foreach(char c in value)
            {
                hash = c + (hash << 6) + (hash << 16) - hash;
            }
            return (hash & 0x7FFFFFFF);
        }
        /// <summary>
        /// RS Hash Function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint RSHash(string value)
        {
            uint b = 378551;
            uint a = 63689;
            uint hash = 0;

            foreach (char c in value)
            {
                hash = hash * a + c;
                a *= b;
            }
            return (hash & 0x7FFFFFFF);
        }
        /// <summary>
        /// JS Hash Function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint JSHash(string value)
        {
            uint hash = 1315423911;
            foreach (char c in value)
            {
                hash ^= ((hash << 5) + c + (hash >> 2));
            }

            return (hash & 0x7FFFFFFF);
        }
        /// <summary>
        /// P. J. Weinberger Hash Function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint PJWHash(string value)
        {
            uint hash = 0;
            /*uint BitsInUnignedInt = (unsigned int)(sizeof(unsigned int) *8);
            uint ThreeQuarters = (unsigned int)((BitsInUnignedInt * 3) / 4);
            uint OneEighth = (unsigned int)(BitsInUnignedInt / 8);
            uint HighBits = (unsigned int)(0xFFFFFFFF) << (BitsInUnignedInt - OneEighth);
            uint hash = 0;
            uint test = 0;

            foreach (char c in value)
            {
                hash = (hash << OneEighth) + c;
                if ((test = hash & HighBits) != 0)
                {
                    hash = ((hash ^ (test >> ThreeQuarters)) & (~HighBits));
                }
            }*/
            return (hash & 0x7FFFFFFF);
        }
        /// <summary>
        /// ELF Hash Function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint ELFHash(string value)
        {
            uint hash = 0;
            uint x;
            foreach (char c in value)
            {
                hash = (hash << 4) + c;
                if ((x = hash & 0xF0000000) != 0)
                {
                    hash ^= (x >> 24);
                    hash &= ~x;
                }
            }
            return (hash & 0x7FFFFFFF);
        }
        /// <summary>
        /// BKDR Hash Function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint BKDRHash(string value)
        {
            uint seed = 131; // 31 131 1313 13131 131313 etc..
            uint hash = 0;

            foreach (char c in value)
            {
                hash = hash * seed + c;
            }

            return (hash & 0x7FFFFFFF);
        }

        /// <summary>
        /// DJB Hash Function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint DJBHash(string value)
        {
            uint hash = 5381;

            foreach (char c in value)
            {
                hash += (hash << 5) + c;
            }

            return (hash & 0x7FFFFFFF);
        }
        /// <summary>
        /// AP Hash Function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint APHash(string value)
        {
            uint hash = 0;
            int i =0;

            foreach (char c in value)
            {
                if ((i & 1) == 0)
                {
                    hash ^= ((hash << 7) ^ c ^ (hash >> 3));
                }
                else
                {
                    hash ^= (~((hash << 11) ^ c ^ (hash >> 5)));
                }
                i++;
            }

            return (hash & 0x7FFFFFFF);
        }
    }
}
