using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Bounding
{
    /// <summary>
    /// 球形包围盒
    /// </summary>
    public class Sphere : IBounding
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
