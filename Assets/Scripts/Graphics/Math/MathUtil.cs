using System.Collections.Generic;

namespace RayGraphics.Math
{
    public class MathUtil
    {
        /// <summary>
        /// 误差
        /// </summary>
        public const float kEpsilon = 1E-05F;
        /// <summary>
        /// 误差
        /// </summary>
        public const float kEpsilonNormalSqrt = 1E-15F;
        /// <summary>
        /// pi 常量
        /// </summary>
        public const float kPI = 3.14159265358979f;
    }
    /// <summary>
    /// 线关系
    /// </summary>
    public enum LineRelation
    {
        /// <summary>
        /// 异常情况
        /// </summary>
        None = 0,
        /// <summary>
        /// 重合
        /// </summary>
        Coincide = 1,
        /// <summary>
        /// 平行
        /// </summary>
        Parallel = 2,
        /// <summary>
        /// 相交
        /// </summary>
        Intersect = 3,
        /// <summary>
        /// 分离
        /// </summary>
        Detach = 4,
    }

    /// <summary>
    /// 对其方式
    /// </summary>
    public enum AligentStyle
    {
        /// <summary>
        /// 
        /// </summary>
        LeftBottom = 0,
        /// <summary>
        /// 
        /// </summary>
        LeftMiddle = 1,
        /// <summary>
        /// 
        /// </summary>
        LeftUp = 2,
        /// <summary>
        /// 
        /// </summary>
        MiddleBottom = 3,
        /// <summary>
        /// 
        /// </summary>
        MiddleMiddle = 4,
        /// <summary>
        /// 
        /// </summary>
        MiddleUp = 5,
        /// <summary>
        /// 
        /// </summary>
        RightBottom = 6,
        /// <summary>
        /// 
        /// </summary>
        RightMiddle = 7,
        /// <summary>
        /// 
        /// </summary>
        RightUp = 8,
    }

    /// <summary>
    /// 多边形
    /// </summary>
    public enum PolygonType
    {
        /// <summary>
        /// 凸多边形
        /// </summary>
        Convex = 0,
        /// <summary>
        /// 凹多边形
        /// </summary>
        Concave = 1,
    }
}
