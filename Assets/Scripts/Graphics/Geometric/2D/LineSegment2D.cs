using Graphics.Math;


namespace Graphics.Geometric
{
    /// <summary>
    /// 直线
    /// </summary>
    [System.Serializable]
    public struct LineSegment2D : iGeo2DElement
    {
        /// <summary>
        /// 直线上得点
        /// </summary>
        public Float2 startPoint;
        /// <summary>
        /// 直线上得点
        /// </summary>
        public Float2 endPoint;
        /// <summary>
        /// 方向为单位向量
        /// </summary>
        public Float2 normalizedDir;
        /// <summary>
        /// 长度
        /// </summary>
        public float length
        {
            get { return (endPoint - startPoint).magnitude; }
        }
        /// <summary>
        /// 构建线段
        /// </summary>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        public LineSegment2D(Float2 startPt, Float2 endPt)
        {
            this.startPoint = startPt;
            this.endPoint = endPt;
            this.normalizedDir = (endPt - startPt).normalized;
        }
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool CheckIn(Float2 pt)
        {
            Float2 diff1 = pt - this.startPoint;
            Float2 diff2 = pt - this.endPoint;
            if (Float2.CheckInLine(diff1, diff2) == true && Float2.Dot(diff1, diff2) <= 0)
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
            Float2 diff1 = pt - this.startPoint;
            Float2 diff2 = pt - this.endPoint;
            if (Float2.Dot(diff1, this.normalizedDir) < 0)
            {
                return diff1.magnitude;
            }
            else if (Float2.Dot(diff2, this.normalizedDir) > 0)
            {
                return diff2.magnitude;
            }
            else
            {
                return aixsVector.magnitude;
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
        /// 与线的关系
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
                Float2 aixsVector1 = line.AixsVector(this.startPoint);
                Float2 aixsVector2 = line.AixsVector(this.endPoint);
                if (Float2.Dot(aixsVector1, aixsVector2) < 0)
                {
                    return LineRelation.Intersect;
                    
                }
                else
                {
                    return LineRelation.Detach;
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
                Float2 aixsVector1 = line.AixsVector(this.startPoint);
                Float2 aixsVector2 = line.AixsVector(this.endPoint);
                if (Float2.Dot(aixsVector1, aixsVector2) < 0)
                {
                    Float2 aixsVector = this.AixsVector(line.startPoint);
                    if (Float2.Dot(aixsVector, line.normalizedDir) > 0)
                    {
                        return LineRelation.Detach;
                    }
                    else return LineRelation.Intersect;
                }
                else
                {
                    return LineRelation.Detach;
                }
            }
        }
        /// <summary>
        /// 与线段的关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineRelation CheckLineRelation(LineSegment2D line)
        {
            // 快速排斥实验,后续优化实现
            // 进行跨立实验
            Float2 diff1 = line.startPoint - this.startPoint;
            Float2 diff2 = line.endPoint - this.startPoint;
            if (Float2.Cross(this.normalizedDir, diff1) * Float2.Cross(this.normalizedDir, diff2) > 0)
            {
                return LineRelation.Detach;
            }
            diff1 = this.startPoint - line.startPoint;
            diff2 = this.endPoint - line.startPoint;
            if (Float2.Cross(line.normalizedDir, diff1) * Float2.Cross(line.normalizedDir, diff2) > 0)
            {
                return LineRelation.Detach;
            }
            return LineRelation.Intersect;
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
                    Float2 point = this.startPoint - distance / dot * this.normalizedDir;
                    Float2 diff = point - this.endPoint;
                    if (Float2.Dot(this.normalizedDir, diff) <= 0)
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
                    Float2 diff = point - this.endPoint;
                    Float2 diff2 = point - line.startPoint;
                    if (Float2.Dot(this.normalizedDir, diff) <= 0 && Float2.Dot(diff2, line.normalizedDir) > 0)
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
            if (CheckLineRelation(line) == LineRelation.Intersect)
            {
                Float2 aixsVector = AixsVector(line.startPoint);
                float distance = aixsVector.magnitude;
                if (distance == 0)
                {
                    intersectPoint = line.startPoint;
                    return true;
                }
                else
                {
                    float dot = Float2.Dot(aixsVector.normalized, line.normalizedDir);
                    if (dot == 0)
                    {
                        return false;
                    }
                    else
                    {
                        intersectPoint = line.startPoint - distance / dot * line.normalizedDir;
                        return true;
                    }
                }
            }
            return false;
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
            get { return endPoint.V3; }
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
            endPoint.DrawGizmos();
        }
#endif
    }
}