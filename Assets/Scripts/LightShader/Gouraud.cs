using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;
/// <summary>
/// 光照函数
/// </summary>
namespace RayGraphics.LishtShader
{
    /// <summary>
    /// https://zh.wikipedia.org/wiki/Gouraud%E7%9D%80%E8%89%B2%E6%B3%95
    /// Gouraud着色法是计算机图形学中的一种插值方法，可以为多边形网格表面生成连续的明暗变化。
    /// 实际使用时，通常先计算三角形每个顶点的光照，再通过双线性插值计算三角形区域中其它像素的颜色。
    /// 双线性插值，又称为双线性内插。在数学上，双线性插值是对线性插值在二维直角网格上的扩展，
    /// 用于对双变量函数（例如 x 和 y）进行插值。其核心思想是在两个方向分别进行一次线性插值。
    /// 参考：https://zh.wikipedia.org/wiki/%E5%8F%8C%E7%BA%BF%E6%80%A7%E6%8F%92%E5%80%BC
    /// </summary>
    public class LightShader
    {
        public PointInfo p1;
        public PointInfo p2;
        public PointInfo p3;
        /// <summary>
        /// 计算point的在三角形的双线性参数
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Double3 BilinearInterpolation(Double3 point)
        {
            return Double3.zero;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interPolationParam">双线性插值参数</param>
        /// <param name="value1">三个顶点的value</param>
        /// <param name="value2">三个顶点的value</param>
        /// <param name="value3">三个顶点的value</param>
        /// <returns></returns>
        public Double3 Gouraud(Double3 interPolationParam, Double3 value1, Double3 value2, Double3 value3)
        {
            return value1 * interPolationParam.x + value2 * interPolationParam.y + value3 * interPolationParam.z;
        }

    }

    public class PointInfo
    {
        /// <summary>
        /// uv 信息
        /// </summary>
        public Double2 pos;
        /// <summary>
        /// 颜色
        /// </summary>
        public Double3 color;
    }
}
