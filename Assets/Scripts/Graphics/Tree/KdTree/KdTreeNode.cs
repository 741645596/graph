using RayGraphics.Math;
using System.Collections.Generic;

namespace RayGraphics.Tree
{
    public class KdTreeNode
    {
        private KdTree parent;
        private Double2  min;
        private Double2  max;
        private List<int> listIndex = null;
        /// <summary>
        /// �������ڵ�����
        /// </summary>
        private KdTreeNode left = null;
        /// <summary>
        /// ����������
        /// </summary>
        private KdTreeNode right = null;
        /// <summary>
        /// �����ķָ�ֵ
        /// </summary>
        public double CullingValue;
        /// <summary>
        /// �����ָ���
        /// </summary>
        public bool isXCulling;
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public KdTreeNode() { }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="listIndex"></param>
        public KdTreeNode(KdTree parent, Double2 min, Double2 max, bool isXCulling, List<int> listIndex)
        {
            this.min = min;
            this.max = max;
            this.listIndex = listIndex;
            this.parent = parent;
            this.isXCulling = isXCulling;
        }
        /// <summary>
        /// �ݹ鹹��Agent ��
        /// </summary>
        /// <param name="begin">agent ��ʼ����������begin</param>
        /// <param name="end">agent ����������������end</param>
        /// <param name="node"></param>
        public void BuildTreeRecursive()
        {
            if (this.parent == null)
                return;
            if (this.listIndex == null || this.listIndex.Count < 2)
                return;
            int total = CalcTotalCount();
            if (this.parent.CheckNeedCull(total) == false)
                return;
            // ���зָ
            CullUnitHelp.Clear();
            foreach (int index in this.listIndex)
            {
                TreeData data = this.parent.GetData(index);
                CullUnitHelp.Process(index, data, this.isXCulling);
            }
            // ��ȡ�ָ���
            bool ret = CullUnitHelp.GetBestCulling(ref CullingValue);
            if (ret == true)
            {
                List<int> listLeft = null;
                List<int> listRight = null;
                CullProcess(ref listLeft, ref listRight);
                if (listLeft != null && listRight != null)
                {
                    if (this.isXCulling == true)
                    {
                        this.left = new KdTreeNode(this.parent, this.min, new Double2(this.CullingValue, this.max.y), !this.isXCulling, listLeft);
                        this.right = new KdTreeNode(this.parent, new Double2(this.CullingValue, this.min.y), this.max, !this.isXCulling, listRight);
                    }
                    else 
                    {
                        this.left = new KdTreeNode(this.parent, this.min, new Double2(this.max.x,this.CullingValue), !this.isXCulling, listLeft);
                        this.right = new KdTreeNode(this.parent, new Double2(this.min.x,this.CullingValue), this.max, !this.isXCulling, listRight);
                    }
                    this.left.BuildTreeRecursive();
                    this.right.BuildTreeRecursive();
                }
            }
        }
        /// <summary>
        /// ͳ�ƽڵ����ݹ�ģ��
        /// </summary>
        /// <returns></returns>
        private int CalcTotalCount()
        {
            if (this.parent == null)
                return 0;
            if (this.listIndex == null || this.listIndex.Count == 0)
                return 0;
            int total = 0;
            foreach (int index in this.listIndex)
            {
                TreeData data = this.parent.GetData(index);
                if (data != null)
                {
                    total += data.Count;
                }
            }

            return 0;
        }
        /// <summary>
        /// ����ָ�
        /// </summary>
        /// <param name="listLeft"></param>
        /// <param name="listRight"></param>
        private void CullProcess(ref List<int> listLeft, ref List<int>listRight)
        {
            if (listLeft == null)
            {
                listLeft = new List<int>();
            }
            listLeft.Clear();
            if (listRight == null)
            {
                listRight = new List<int>();
            }
            listRight.Clear();

            if (this.parent == null || this.listIndex == null || this.listIndex.Count == 0)
                return;

            foreach (int index in this.listIndex)
            {
                TreeData data = this.parent.GetData(index);
                if(data == null)
                    continue;
                if (this.isXCulling == true)
                {
                    if (data.max.x < this.CullingValue)
                    {
                        listLeft.Add(index);
                    }
                    else
                    {
                        listRight.Add(index);
                    }
                }
                else 
                {
                    if (data.max.y < this.CullingValue)
                    {
                        listLeft.Add(index);
                    }
                    else
                    {
                        listRight.Add(index);
                    }
                }
            }
        }
    }

    
    public class CullUnitHelp
    {
        /// <summary>
        /// ��С�����ź�����ˣ�x����ķָ��ˡ�
        /// </summary>
        private static List<CullUnit> s_ListLineUnit = new List<CullUnit>();
        /// <summary>
        /// ������
        /// </summary>
        public static void Clear()
        {
            s_ListLineUnit.Clear();
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="ContourPolyIndex"></param>
        /// <param name="ab"></param>
        public static void Process(int index, TreeData data, bool isXCuling)
        {
            if (data == null)
                return;
            CullUnit unit = null;
            if (isXCuling == true)
            {
                unit = new CullUnit(data.min.x, data.max.x, index, data.Count);
            }
            else 
            {
                unit = new CullUnit(data.min.y, data.max.y, index, data.Count);
                
            }
            AddLineUnit(ref s_ListLineUnit, unit);
        }
        /// <summary>
        /// ���ҵ��ཻ������
        /// </summary>
        /// <param name="listLineUnit"></param>
        /// <param name="unit"></param>
        /// <param name="retIndex"></param>
        /// <returns></returns>
        private static void AddLineUnit(ref List<CullUnit> listLineUnit, CullUnit unit)
        {
            if (listLineUnit.Count > 0)
            {
                bool isHaveCombine = false;
                // �ҵ���ʼλ��
                for (int i = 0; i < listLineUnit.Count; i++)
                {
                    if (CullUnit.CheckIntersection(unit, listLineUnit[i]) == true)
                    {
                        if (isHaveCombine == false)
                        {
                            isHaveCombine = true;
                            listLineUnit[i].Combine(unit);
                            unit = listLineUnit[i];
                        }
                        else
                        {
                            unit.Combine(listLineUnit[i]);
                            listLineUnit.RemoveAt(i);
                            i--;
                        }
                    }
                    else if (unit.max < listLineUnit[i].min)
                    {
                        if (isHaveCombine == false)
                        {
                            listLineUnit.Insert(i, unit);
                            isHaveCombine = true;
                        }
                        break;
                    }
                }
                if (isHaveCombine == false)
                {
                    listLineUnit.Insert(listLineUnit.Count, unit);
                }
            }
            else
            {
                listLineUnit.Add(unit);
            }
        }
        /// <summary>
        /// ��ȡy ��ѷָ���
        /// </summary>
        /// <param name="cullingValue"></param>
        /// <returns></returns>
        public static bool GetBestCulling(ref double cullingValue)
        {
            return GetBestCulling(s_ListLineUnit, ref cullingValue);
        }
        /// <summary>
        /// ��ȡ��ѷָ���
        /// </summary>
        /// <param name="listLineUnit"></param>
        /// <param name="cullingValue"></param>
        /// <returns></returns>
        private static bool GetBestCulling(List<CullUnit> listLineUnit, ref double cullingValue)
        {
            if (listLineUnit == null || listLineUnit.Count < 2)
                return false;
            cullingValue = 0;
            // �Ȼ�ȡ����
            int totalCount = 0;
            for (int i = 0; i < listLineUnit.Count; i++)
            {
                totalCount += listLineUnit[i].pointCounts;
            }
            //
            double value = listLineUnit[0].pointCounts * 1.0f / totalCount;
            int total = listLineUnit[0].pointCounts;
            for (int i = 1; i < listLineUnit.Count; i++)
            {
                total += listLineUnit[i].pointCounts;
                double v = total * 1.0f / totalCount;
                if (System.Math.Abs(value - 0.5f) < System.Math.Abs(v - 0.5f))
                {
                    cullingValue = (listLineUnit[i - 1].max + listLineUnit[i].min) * 0.5f;
                    return true;
                }
                else
                {
                    value = v;
                }
            }
            return false;
        }
    }
    public class CullUnit
    {
        public double min;
        public double max;
        /// <summary>
        /// �����ĵ�������
        /// </summary>
        public List<int> listIndex = null;
        /// <summary>
        /// �����ĵ��������ܶ�����
        /// </summary>
        public int pointCounts;

        public CullUnit(double min, double max, int index, int pointCounts)
        {
            this.min = min;
            this.max = max;
            this.pointCounts = pointCounts;
            this.listIndex = new List<int>();
            this.listIndex.Add(index);
        }
        /// <summary>
        /// �ж��Ƿ��ཻ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CheckIntersection(CullUnit a, CullUnit b)
        {
            if (b.max < a.min)
                return false;
            if (b.min > a.max)
                return false;
            return true;
        }
        /// <summary>
        /// �ϲ�����
        /// </summary>
        /// <param name="v"></param>
        public void Combine(CullUnit v)
        {
            this.min = System.Math.Min(this.min, v.min);
            this.max = System.Math.Max(this.max, v.max);
            if (this.listIndex == null)
            {
                this.listIndex = new List<int>();
            }
            if (v.listIndex != null && v.listIndex.Count > 0)
            {
                this.listIndex.AddRange(v.listIndex);
            }
            this.pointCounts += v.pointCounts;
        }
    }
    
}