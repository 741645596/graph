using RayGraphics.Math;

/// <summary>
/// 观察者坐标系表述
/// </summary>
namespace RayGraphics.CoordinateSystem
{
    public class ViewCoordinateSystem: CoordinateSystem3D
    {
        /// <summary>
        /// 近截面
        /// </summary>
        public double near;
        /// <summary>
        /// 远界面
        /// </summary>
        public double far;

    }
}
