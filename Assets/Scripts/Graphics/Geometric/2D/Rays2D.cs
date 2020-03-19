using Graphics.Math;


namespace Graphics.Geometric
{
    /// <summary>
    /// 直线
    /// </summary>
    [System.Serializable]
    public struct Rays2D : iGeo2DElement
    {
        /// <summary>
        /// 直线上得点
        /// </summary>
        public Float2 startPoint;
        /// <summary>
        /// 方向为单位向量
        /// </summary>
        public Float2 normalizedDir;

        public Rays2D(Float2 startPt, Float2 dir)
        {
            this.startPoint = startPt;
            this.normalizedDir = dir.normalized;
        }
        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool CheckIn(Float2 pt)
        {
            Float2 diff = pt - this.startPoint;
            if (Float2.CheckInLine(diff, this.normalizedDir) == true && Float2.Dot(diff, this.normalizedDir) > 0)
            {
                 return true;
            }
            return false;
        }
        /// <summary>
        /// 点导几何元素的距离
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public float CalcDistance(Float2 pt)
        {
            Float2 aixsVector = this.AixsVector(pt);
            Float2 diff = pt - this.startPoint;
            if (Float2.Dot(diff, this.normalizedDir) >= 0)
            {
                return aixsVector.magnitude;
            }
            else 
            {
                return diff.magnitude;
            }    
        }
        /// <summary>
        /// 点导几何元素的投影点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 ProjectPoint(Float2 pt)
        {
            Float2 diff = pt - this.startPoint;
            if (diff == Float2.zero)
                return pt;
            return Float2.Project(diff, this.normalizedDir) + this.startPoint;
        }
        /// <summary>
        /// 求轴向量
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Float2 AixsVector(Float2 pt)
        {
            Float2 diff = pt - this.startPoint;
            if (diff == Float2.zero)
                return Float2.zero;
            return diff - Float2.Project(diff, this.normalizedDir);
        }
        /// <summary>
        /// 与射线的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(Line2D line)
        {
            // 共线判断
            if (Float2.CheckInLine(this.normalizedDir, line.normalizedDir) == true)
            {
                Float2 diff = line.startPoint - this.startPoint;
                // 贡献判断
                if (Float2.CheckInLine(this.normalizedDir, diff) == true)
                {
                    return LineRelation.Coincide;
                }
                else
                {
                    return LineRelation.Parallel;
                }
            }
            else
            {
                Float2 aixsVector = line.AixsVector(this.startPoint);
                if (Float2.Dot(aixsVector, this.normalizedDir) > 0)
                {
                    return LineRelation.Detach;
                }
                else
                {
                    return LineRelation.Intersect;
                }
            }
        }
        /// <summary>
        /// 射线与射线间的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(Rays2D line)
        {
            // 共线判断
            if (Float2.CheckInLine(this.normalizedDir, line.normalizedDir) == true)
            {
                Float2 diff = line.startPoint - this.startPoint;
                // 共线判断
                if (Float2.CheckInLine(this.normalizedDir, diff) == true)
                {
                    return LineRelation.Coincide;
                }
                else
                {
                    return LineRelation.Parallel;
                }
            }
            else
            {
                // 先判断this 是否与line 所在直线相交
                Float2 aixsVector = line.AixsVector(this.startPoint);
                if (Float2.Dot(aixsVector, this.normalizedDir) > 0)
                {
                    return LineRelation.Detach;
                }
                // 先判断line 是否与this 所在直线相交
                aixsVector = this.AixsVector(line.startPoint);
                if (Float2.Dot(aixsVector, line.normalizedDir) > 0)
                {
                    return LineRelation.Detach;
                }
                return LineRelation.Intersect;
            }
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(LineSegment2D line)
        {
            return line.CheckLineRelation(this);
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(Line2D line, ref Float2 intersectPoint)
        {
            Float2 aixsVector = line.AixsVector(this.startPoint);
            float distance = aixsVector.magnitude;
            if (distance == 0)
            {
                intersectPoint = this.startPoint;
                return true;
            }
            else
            {
                float dot = Float2.Dot(aixsVector.normalized, this.normalizedDir);
                if (dot < 0)
                {

                    intersectPoint = this.startPoint - distance / dot * this.normalizedDir;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(Rays2D line, ref Float2 intersectPoint)
        {
            Float2 aixsVector = line.AixsVector(this.startPoint);
            float distance = aixsVector.magnitude;
            if (distance == 0)
            {
                intersectPoint = this.startPoint;
                return true;
            }
            else
            {
                float dot = Float2.Dot(aixsVector.normalized, this.normalizedDir);
                if (dot < 0)
                {
                    Float2 point = this.startPoint - distance / dot * this.normalizedDir;
                    Float2 diff = point - line.startPoint;
                    if (Float2.Dot(diff, line.normalizedDir) > 0)
                    {
                        intersectPoint = point;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool GetIntersectPoint(LineSegment2D line, ref Float2 intersectPoint)
        {
            return line.GetIntersectPoint(this, ref intersectPoint);
        }
        /// <summary>
        /// 镜面但
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Float2 GetMirrorPoint(Float2 point)
        {
            return point - 2 * AixsVector(point);
        }
#if Client
        /// <summary>
        /// start point
        /// </summary>
        public UnityEngine.Vector3 StartPoint
        {
            get { return startPoint.V3; }
        }
        /// <summary>
        /// End point
        /// </summary>
        public UnityEngine.Vector3 EndPoint
        {
            get { return (startPoint + 10 * normalizedDir).V3; }
        }
        /// <summary>
        /// draw
        /// </summary>
        public void Draw()
        {
            UnityEngine.GL.Vertex(this.StartPoint);
            UnityEngine.GL.Vertex(this.EndPoint);
        }
        /// <summary>
        /// DrawGizmos
        /// </summary>
        public void DrawGizmos()
        {
            startPoint.DrawGizmos();
            Float2 v1 = Float2.Perpendicular(normalizedDir).normalized * 0.2f;
            Float2 diff = 9.8f * normalizedDir;
            UnityEngine.Gizmos.DrawLine((startPoint + diff + v1).V3, EndPoint);
            UnityEngine.Gizmos.DrawLine((startPoint + diff - v1).V3, EndPoint);
        }
#endif
    }
}