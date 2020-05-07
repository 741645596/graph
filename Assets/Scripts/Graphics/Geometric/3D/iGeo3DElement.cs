using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;


namespace RayGraphics.Geometric
{
    public interface iGeo3DElement
    {
        /// <summary>
        /// 判断点是否在几何元素上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        bool CheckIn(Double3 pt);
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        double CalcDistance(Double3 pt);
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Double3 ProjectPoint(Double3 pt);
        /// <summary>
        /// 点到几何元素轴向量（垂直），交点-》pt
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Double3 AixsVector(Double3 pt);
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
        bool GetIntersectPoint(Line3D line, ref Double3 intersectPoint);
    }
}
