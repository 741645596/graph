using System.Collections.Generic;
using UnityEngine;
using Graphics.Math;
/// <summary>
/// 包围盒
/// </summary>
namespace Graphics.Bounding
{
    public interface IBounding
    {
        // Start is called before the first frame update
        void Generate(List<Float3> listPt);
        void Generate(Float3[] ptArray);
        void Generate(Mesh mesh);

        bool CheckIN();
    }
}
