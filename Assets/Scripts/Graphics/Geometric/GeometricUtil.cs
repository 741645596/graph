using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    public class GeometricUtil
    {
        /// <summary>
        /// 判断点在射线from to 的左侧
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool LeftSide(Float2 from, Float2 to, Float2 target)
        {
            Float2 fromTo = to - from;
            Float2 fromTarget = target - from;
            float cross = Float2.Cross(fromTo, fromTarget);
            return (cross > 0f);
        }
    }
}