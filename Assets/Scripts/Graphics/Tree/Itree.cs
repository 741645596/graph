using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Tree
{
    public interface Itree
    {
        void BuildTree(Double2 min, Double2 max, List<TreeData> listData);
    }
}