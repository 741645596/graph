using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Bounding
{
    /// <summary>
    /// AABB包围盒(Axis-aligned bounding box)
    /// </summary>
    public class AABB : IBounding
    {
        private Float3 _min = Float3.zero;
        private Float3 _max = Float3.zero;

        public Float3 Center {
            get { return (_min + _max) * 0.5f; }
        }


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