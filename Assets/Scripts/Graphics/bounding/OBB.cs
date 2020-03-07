using System.Collections.Generic;
using UnityEngine;
using Graphics.Math;

namespace Graphics.Bounding
{
    /// <summary>
    /// OBB方向包围盒(Oriented bounding box)
    /// </summary>
    public class OBB : IBounding
    {
        public void Generate() { }
        public void Generate(List<Float3> listPt) { }
        public void Generate(Float3[] ptArray) { }
        public void Generate(Mesh mesh) { }

        public bool CheckIN()
        {
            return true;
        }
    }
}