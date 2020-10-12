using UnityEngine;
using RayGraphics.Math;
using RayGraphics.Geometric;
using System.Collections.Generic;

public class PolyBoolTest : MonoBehaviour
{
    // 测试用的。
    public Double2[] mainPoly;
    public Double2[] diffPoly;
    public Double2[] resultPoly;

    private void Start()
    {
    }
    void Update()
    {
       List<Double2> l = new List<Double2>();
       l.AddRange(diffPoly);
       l.Reverse();

        //resultPoly = Polygon2DSetDiff.CalcPoly(mainPoly, /*diffPoly*/ l.ToArray());
        resultPoly = Polygon2DSetAnd.CalcPoly(mainPoly, diffPoly);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //return;
        DrawPoly(mainPoly, 0.0f, Color.red);
        DrawPoly(diffPoly, 0.2f, Color.blue);
        DrawPoly(resultPoly, 0.4f, Color.yellow);
    }

    /// <summary>
    /// 绘制多边形
    /// </summary>
    /// <param name="Poly"></param>
    /// <param name="color"></param>
    private void DrawPoly(Double2[] Poly, float height, Color color)
    {
        // 设置颜色
        Color defaultColor = Gizmos.color;
        Gizmos.color = color;

        if (Poly != null && Poly.Length >= 3)
        {
            for (int i = 0; i < Poly.Length; i++)
            {
                if (i == Poly.Length - 1)
                {
                    DrawLine(Poly[i], Poly[0], height);
                }
                else 
                {
                    DrawLine(Poly[i], Poly[i + 1], height);
                }
            }
        }
        // 恢复默认颜色
        Gizmos.color = defaultColor;
    }
    /// <summary>
    /// 绘制线，
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="linee"></param>
    private void DrawLine(Double2 lines, Double2 linee, float height)
    {
        Gizmos.DrawLine(new Vector3((float)lines.x, height, (float)lines.y), new Vector3((float)linee.x, height, (float)linee.y));
    }

#endif
}


