using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Bounding
{
    /// <summary>
    /// OBB方向包围盒(Oriented bounding box)
    /// </summary>
    public class OBB : IBounding
    {
        public void Generate() { }
        public void Generate(List<Float3> listPt) { }
        public void Generate(Float3[] ptArray) { }
#if Client
        public void Generate(UnityEngine.Mesh mesh) { }
#endif

        public bool CheckIN()
        {
            return true;
        }
    }
}