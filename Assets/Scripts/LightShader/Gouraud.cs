using System.Collections;
using System.Collections.Generic;
using RayGraphics.Math;
/// <summary>
/// ���պ���
/// </summary>
namespace RayGraphics.LishtShader
{
    /// <summary>
    /// https://zh.wikipedia.org/wiki/Gouraud%E7%9D%80%E8%89%B2%E6%B3%95
    /// Gouraud��ɫ���Ǽ����ͼ��ѧ�е�һ�ֲ�ֵ����������Ϊ���������������������������仯��
    /// ʵ��ʹ��ʱ��ͨ���ȼ���������ÿ������Ĺ��գ���ͨ��˫���Բ�ֵ�����������������������ص���ɫ��
    /// ˫���Բ�ֵ���ֳ�Ϊ˫�����ڲ塣����ѧ�ϣ�˫���Բ�ֵ�Ƕ����Բ�ֵ�ڶ�άֱ�������ϵ���չ��
    /// ���ڶ�˫�������������� x �� y�����в�ֵ�������˼��������������ֱ����һ�����Բ�ֵ��
    /// �ο���https://zh.wikipedia.org/wiki/%E5%8F%8C%E7%BA%BF%E6%80%A7%E6%8F%92%E5%80%BC
    /// </summary>
    public class LightShader
    {
        public PointInfo p1;
        public PointInfo p2;
        public PointInfo p3;
        /// <summary>
        /// ����point���������ε�˫���Բ���
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Double3 BilinearInterpolation(Double3 point)
        {
            return Double3.zero;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interPolationParam">˫���Բ�ֵ����</param>
        /// <param name="value1">���������value</param>
        /// <param name="value2">���������value</param>
        /// <param name="value3">���������value</param>
        /// <returns></returns>
        public Double3 Gouraud(Double3 interPolationParam, Double3 value1, Double3 value2, Double3 value3)
        {
            return value1 * interPolationParam.x + value2 * interPolationParam.y + value3 * interPolationParam.z;
        }

    }

    public class PointInfo
    {
        /// <summary>
        /// uv ��Ϣ
        /// </summary>
        public Double2 pos;
        /// <summary>
        /// ��ɫ
        /// </summary>
        public Double3 color;
    }
}
