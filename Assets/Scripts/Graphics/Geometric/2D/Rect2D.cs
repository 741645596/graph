using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    /// <summary>
    /// 矩形区域
    /// </summary>
    [System.Serializable]
    public class Rect2D : AABB2D
    {
        public Rect2D(Float2 lb, Float2 ru) : base(lb, ru)
        {
        }
    }
}
