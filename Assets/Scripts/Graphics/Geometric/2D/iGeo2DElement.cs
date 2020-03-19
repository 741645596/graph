using Graphics.Math;


namespace Graphics.Geometric
{
    public interface iGeo2DElement
    {
        /// <summary>
        /// 判断点是否在几何元素上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        bool CheckIn(Float2 pt);
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        float CalcDistance(Float2 pt);
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Float2 ProjectPoint(Float2 pt);
        /// <summary>
        /// 点到几何元素轴向量（垂直），交点-》pt
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        Float2 AixsVector(Float2 pt);
        /// <summary>
        /// 与直线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        LineRelation CheckLineRelation(Line2D line);
        /// <summary>
        /// 与射线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        LineRelation CheckLineRelation(Rays2D line);
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        LineRelation CheckLineRelation(LineSegment2D line);
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        bool GetIntersectPoint(Line2D line, ref Float2 intersectPoint);
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        bool GetIntersectPoint(Rays2D line, ref Float2 intersectPoint);
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        bool GetIntersectPoint(LineSegment2D line, ref Float2 intersectPoint);
        /// <summary>
        /// 镜面但
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        Float2 GetMirrorPoint(Float2 point);
#if Client
        /// <summary>
        /// draw
        /// </summary>
        void Draw();
        /// <summary>
        /// DrawGizmos
        /// </summary>
        void DrawGizmos();
#endif
    }
}
