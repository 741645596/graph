using RayGraphics.Math;

namespace RayGraphics.Tree
{
    /// <summary>
    /// 定义树的相关数据
    /// </summary>
    public interface TreeData
    {
        Double2 min { get; }
        Double2 max { get; }

        int Count { get; }

    }
}
