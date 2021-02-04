using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Mesh
{
    /// <summary>
    /// 顶点属性
    /// </summary>
    public class Point
    {
        /// <summary>
        /// 顶点位置
        /// </summary>
        public Double3 pos;
        /// <summary>
        /// uv 信息
        /// </summary>
        public Double2 uv;
        /// <summary>
        /// 法线
        /// </summary>
        public Double3 normal;
    }

    /// <summary>
    /// 顶点属性
    /// </summary>
    public class Triangle
    {
        int[] pts = new int[3];
    }

    public class TriangleMesh
    {
        /// <summary>
        /// 顶点列表
        /// </summary>
        public List<Point> listPoint;

    }
}


