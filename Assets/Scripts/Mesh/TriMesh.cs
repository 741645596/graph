using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Mesh
{
    /// <summary>
    /// ��������
    /// </summary>
    public class Point
    {
        /// <summary>
        /// ����λ��
        /// </summary>
        public Double3 pos;
        /// <summary>
        /// uv ��Ϣ
        /// </summary>
        public Double2 uv;
        /// <summary>
        /// ����
        /// </summary>
        public Double3 normal;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public class Triangle
    {
        int[] pts = new int[3];
    }

    public class TriangleMesh
    {
        /// <summary>
        /// �����б�
        /// </summary>
        public List<Point> listPoint;

    }
}


