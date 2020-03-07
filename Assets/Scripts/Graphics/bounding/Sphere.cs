using System.Collections.Generic;
using UnityEngine;
using Graphics.Math;

namespace Graphics.Bounding
{
    /// <summary>
    /// 球形包围盒
    /// </summary>
    public class Sphere : IBounding
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
