using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Geometric
{

    public class Polygon2D : AABB2D
    {
        // 顶点列表多+ 1,回头了
        public Float2[] pointArr = null;
        /// <summary>
        /// 逆时针
        /// </summary>
        public float[] distancArr = null;
        public float totalDistance = 0;
        public Polygon2D(Float2 [] points) 
        {
            if (points == null || points.Length < 3)
                return;
            this.pointArr = new Float2[points.Length + 1];
            this.distancArr = new float[points.Length + 1];
            Float2 min = points[0];
            Float2 max = points[0];
            int i = 0;
            for (i = 0; i < points.Length; i++)
            {
                this.pointArr[i] = points[i];
                if (i > 0)
                {
                    min = Float2.Min(min, points[i]);
                    max = Float2.Max(max, points[i]);
                    totalDistance += (float)Float2.Distance(points[i], points[i - 1]);
                    this.distancArr[i] = totalDistance;
                }
                else 
                {
                    min = points[i];
                    max = points[i];
                    this.distancArr[i] = 0;
                    totalDistance = 0;
                }
            }
            this.pointArr[i] = points[0];
            totalDistance += (float)Float2.Distance(points[i], points[0]);
            this.distancArr[i] = totalDistance;
            this.SetAABB(min, max);
        }


        /// <summary>
        /// 最短射线包围盒路径
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="offset">偏移值</param>
        /// <param name="paths">返回路径</param>
        /// <returns>true，表示线段与aabb有相交，并返回最短包围路径</returns>
        public override bool RayboundingNearestPath(LineSegment2D line, float offset, ref List<Float2> paths)
        {
            int index = 0;
            List<Float3> lineArray = new List<Float3>();
            Float2 intersectionPoint = Float2.zero;

            for (int i = 0; i < this.pointArr.Length -1 ; i++)
            {
                if (line.GetIntersectPoint(new LineSegment2D(this.pointArr[i], this.pointArr[i + 1]), ref intersectionPoint) == true)
                {
                    lineArray[index] = new Float3(intersectionPoint.x, intersectionPoint.y, i);
                    index++;
                }
            }

            if (index == 0)
            {
                return false;
            }
            else 
            {
                bool isIn = true;
                for (int i = 0; i < index -1; i++)
                {
                    if (isIn == true)
                    {
                        List<Float2> temppaths = new List<Float2>();
                        RayboundingNearestPath(lineArray[i], lineArray[i + 1], offset, ref temppaths);
                        paths.AddRange(temppaths);
                    }
                    isIn = !isIn;
                }
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="sl"></param>
        private void RayboundingNearestPath(Float3 p1, Float3 p2, float offset, ref List<Float2> paths)
        {
            List<Float2> listpath = new List<Float2>();
            // 先计算逆时针距离。
            float dis;
            // 对边行为
            if (p1.z < p2.z)
            {
                dis = distancArr[(int)p2.z + 1] - distancArr[(int)p1.z]
                    - (float)Float2.Distance(new Float2(p1.x, p1.y), this.pointArr[(int)p1.z])
                    - (float)Float2.Distance(new Float2(p2.x, p2.y), this.pointArr[(int)p2.z + 1]);
                if (dis < totalDistance - dis)
                {
                    listpath.Add(new Float2(p1.x, p1.y));
                    for (int i = (int)p1.z + 1; i <= (int)p2.z; i++)
                    {
                        listpath.Add(this.pointArr[i]);
                    }
                    listpath.Add(new Float2(p2.x, p2.y));
                }
                else 
                {

                }
            }
            else if (p1.z > p2.z)
            {
                dis = distancArr[(int)p2.z + 1] - distancArr[(int)p1.z]
                - (float)Float2.Distance(new Float2(p1.x, p1.y), this.pointArr[(int)p1.z])
                - (float)Float2.Distance(new Float2(p2.x, p2.y), this.pointArr[(int)p2.z + 1]);
                if (dis < totalDistance - dis)
                {

                }
                else
                {

                }
            }
            else 
            {

            }
            paths = listpath;
        }
    }
}