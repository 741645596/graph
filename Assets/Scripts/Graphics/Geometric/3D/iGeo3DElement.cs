using System.Collections;
using System.Collections.Generic;
using Graphics.Math;


namespace Graphics.Geometric
{
    public interface iGeo3DElement
    {
        /// <summary>
        /// 判断点是否在几何元素上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        bool CheckIn(Float3 pt);
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        float CalcDistance(Float3 pt);
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Float3 ProjectPoint(Float3 pt);
        /// <summary>
        /// 点到几何元素轴向量（垂直），交点-》pt
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Float3 AixsVector(Float3 pt);
        /// <summary>
        /// 与线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        LineRelation CheckLineRelation(Line3D line);
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        bool GetIntersectPoint(Line3D line, ref Float3 intersectPoint);
    }
}
