using RayGraphics.Math;

namespace RayGraphics.Voxelization
{

    [System.Serializable]
    public class StageGateArea: MapElement
    {
        public Float2 leftBottom;
        public Float2 rightUp;

        /// <summary>
        /// 出入口位置数据。
        /// </summary>
        [System.NonSerialized]
        public Float2 MindoorCenter;
        [System.NonSerialized]
        public Float2 MaxdoorCenter;
        /// <summary>
        /// 为门的时候，朝向， true ：x方向， false： y方向
        /// </summary>
        public bool isXdir = false;

        public override Float2 GetKeyPoint()
        {
            return (this.leftBottom + this.rightUp) / 2;
        }
    }
}