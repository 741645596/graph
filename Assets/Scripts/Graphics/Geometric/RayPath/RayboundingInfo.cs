using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Geometric
{
    /// <summary>
    /// 挡格射线包围盒信息。
    /// </summary>
    public class RayboundingInfo
    {
        public float distance;
        public Float2 nearPoint;
        public Float2 farPoint;
        public List<Float2> listpath = null;
        /// <summary>
        /// <summary>
        /// 路线在线段区域，是否逆时针方向
        /// </summary>
        public bool isCounterclockwiseDir;
        /// <summary>
        /// 计算其他辅助数据
        /// </summary>
        /// <param name="line"></param>
        public void CalcHelpData(LineSegment2D line, float offset, Float2 near, Float2 far)
        {
            if (this.listpath != null && this.listpath.Count > 0)
            {
                this.nearPoint = near;
                this.nearPoint -= offset * line.normalizedDir;

                this.farPoint = far;
                this.farPoint += offset * line.normalizedDir;
                this.ReviseNearFar(line);
                // 计算时针方向
                this.isCounterclockwiseDir = CaclCounterclockwiseDir(line, this.listpath);

                this.distance = (line.startPoint - nearPoint).sqrMagnitude;
            }
        }
        /// <summary>
        /// 计算时钟方向
        /// </summary>
        /// <param name="line"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        protected bool CaclCounterclockwiseDir(LineSegment2D line, List<Float2> paths)
        {
            if (paths == null || paths.Count == 0)
                return true;
            Float2 f = Float2.zero;
            for (int i = 0; i < paths.Count; i++)
            {
                f += paths[i];
            }
            f /= paths.Count;
            float sinAngle = Float2.SinAngle(line.normalizedDir, f - line.startPoint);
            if (sinAngle < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 修正near far 点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="nearPoint"></param>
        /// <param name="farPoint"></param>
        protected void ReviseNearFar(LineSegment2D line)
        {
            if (Float2.Dot(nearPoint - line.startPoint, line.normalizedDir) <= 0)
            {
                nearPoint = line.startPoint;
            }
            if (Float2.Dot(line.endPoint - farPoint, line.normalizedDir) <= 0)
            {
                farPoint = line.endPoint;
            }
        }


        public void Clear()
        {
            if (listpath != null)
            {
                listpath.Clear();
                listpath = null;
            }
        }
    }
}
