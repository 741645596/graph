using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Geometric
{
    public class Polygon3D : Plane
    {
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Double3 pt)
        {
            return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override double CalcDistance(Double3 pt)
        {
            return 0;
        }
        /// <summary>
        /// 点导几何元素的投影
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override Double3 ProjectPoint(Double3 pt)
        {
            return Double3.zero;
        }
    }
}