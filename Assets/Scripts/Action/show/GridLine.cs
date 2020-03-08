using UnityEngine;
using Graphics.Geometric;


public class GridLine : MonoBehaviour
{
    public LineSegment2D ls2d;
    public Line2D line;
    public Rays2D rayline;
    public Material material;


    void OnRenderObject()
    {
        material.SetPass(0);
        GL.wireframe = false;
        GL.Color(Color.yellow);
        GL.PushMatrix();
        GL.Begin(GL.LINES);

        ls2d.Draw();
        line.Draw();
        rayline.Draw();


        GL.End();
        GL.PopMatrix();

        GL.wireframe = false;
    }

    void OnDrawGizmos()
    {
        ls2d.DrawGizmos();
        line.DrawGizmos();
        rayline.DrawGizmos();
    }

}
