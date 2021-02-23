using RayGraphics.Math;

namespace RayGraphics.Tree
{
    /// <summary>
    /// ���������������
    /// </summary>
    public interface TreeData
    {
        Double2 min { get; }
        Double2 max { get; }

        int Count { get; }

    }
}
