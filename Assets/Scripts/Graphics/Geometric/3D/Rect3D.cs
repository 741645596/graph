using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    public class Rect3D : Plane
    {
        /// <summary>
        /// 轴1，带长度
        /// </summary>
        protected Double3 _aixs1;
        public Double3 Aixs1
        {
            get { return _aixs1; }
        }
        /// <summary>
        /// 轴2，带长度
        /// </summary>
        protected Double3 _aixs2;
        public Double3 Aixs2
        {
            get { return _aixs2; }
        }
        /// <summary>
        /// 判断点是否在矩形内
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override bool CheckIn(Double3 pt)
        {
            // 首先判断是在矩形所在的平面内。
            if (base.CheckIn(pt) == false)
                return false;
            // pt 在矩形的2条边之间
            Double3 diff = pt - this.Pt;
            if (Double3.Dot(Double3.Cross(diff, Aixs1), Double3.Cross(diff, Aixs2)) > 0)
                return false;
            // diff 在2个轴上的分量小于轴的长度
            double length1 = Double3.Dot(diff, Aixs1);
            if (length1 < 0 || length1 > Aixs1.sqrMagnitude)
                return false;
            length1 = Double3.Dot(diff, Aixs2);
            if (length1 < 0 || length1 > Aixs2.sqrMagnitude)
                return false;
            return true;
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
    }
}

