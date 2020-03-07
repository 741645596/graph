using System.Collections;
using System.Collections.Generic;
using Graphics.Math;


namespace Graphics.Geometric
{
    public class Polygon3D : Plane
    {
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Float3 pt)
        {
            return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override float CalcDistance(Float3 pt)
        {
            return 0;
        }
        /// <summary>
        /// 点导几何元素的投影
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override Float3 ProjectPoint(Float3 pt)
        {
            return Float3.zero;
        }
    }
}