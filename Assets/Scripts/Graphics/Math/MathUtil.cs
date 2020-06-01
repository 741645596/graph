using System;

namespace RayGraphics.Math
{
    public class MathUtil
    {
        /// <summary>
        /// 直角增加的IntegrationValue
        /// </summary>
        public static readonly Double StraightDistance = 1.0f;
        /// <summary>
        /// 对角增加的IntegrationValue
        /// </summary>
        public static readonly Double DiagonalDistance = 1.414214f;
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
        /// <summary>
        /// 获取2点评估距离/ 跳点距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetCompareDis(Double2 start, Double2 end)
        {
            Double2 diff = start - end;
            return diff.sqrMagnitude;
        }
        /// <summary>
        /// 获取2点评估距离/ 跳点距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetNearDistance(Double2 start, Double2 end)
        {
            Double2 diff = start - end;
            return GetNearDistance(diff);
        }
        /// <summary>
        /// 计算向量的长度
        /// </summary>
        /// <param name="diff"></param>
        /// <returns></returns>
        public static double GetNearDistance(Double2 diff)
        {
            double dx = System.Math.Abs(diff.x);
            double dy = System.Math.Abs(diff.y);
            if (dy > dx)
            {
                return (dy - dx) * MathUtil.StraightDistance + dx * MathUtil.DiagonalDistance;
            }
            else
            {
                return (dx - dy) * MathUtil.StraightDistance + dy * MathUtil.DiagonalDistance;
            }
        }
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
    /// <summary>
    /// 投影点在线上的关系
    /// </summary>
    public enum ProjectPointInLine
    {
        /// <summary>
        /// 重合
        /// </summary>
        OutStart = 0,
        /// <summary>
        /// 平行
        /// </summary>
        In = 1,
        /// <summary>
        /// 相交
        /// </summary>
        OutEnd = 2,
    }
    /// <summary>
    /// 射线包围盒返回类型
    /// </summary>
    public enum RBIResultType
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Succ = 1,
        /// <summary>
        /// 通不过
        /// </summary>
        UnCross = 2,
        /// <summary>
        /// 通不过
        /// </summary>
        UnCrossstartPointIn = 3,
        /// <summary>
        /// 通不过
        /// </summary>
        UnCrossendPointIn = 4,
    }
}
