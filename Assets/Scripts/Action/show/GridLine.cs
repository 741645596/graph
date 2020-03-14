using UnityEngine;
using System.Collections;
using Graphics.Geometric;
using Graphics.Math;


public class GridLine : MonoBehaviour
{
    public Material material;
    public LineSegment2D ls2d;
    public LineSegment2D ls21;
    public Float2 point;

    private Rays2D vectorAix;

    void Start()
    {
        ls2d = new LineSegment2D(Float2.zero, new Float2(10, 10));
        InvokeRepeating("Doline", 0.1f, 1.0f);
    }

    void Doline()
    {
        Float2 normalizedDir = Float2.Rotate(ls2d.normalizedDir, 0.1f);
        ls2d = new LineSegment2D(ls2d.startPoint, ls2d.startPoint + normalizedDir * ls2d.length);
        vectorAix = new Rays2D(ls2d.ProjectPoint(point), ls2d.AixsVector(point)); 
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnRenderObject()
    {
        material.SetPass(0);
        GL.wireframe = false;
        GL.Color(Color.yellow);
        GL.PushMatrix();
        GL.Begin(GL.LINES);

        ls2d.Draw();
        //vectorAix.Draw();
        ls21.Draw();


        GL.End();
        GL.PopMatrix();

        GL.wireframe = false;
    }

    void OnDrawGizmos()
    {
        ls2d.DrawGizmos();
        ls21.DrawGizmos();
        //point.DrawGizmos();

        //ls2d.ProjectPoint(point).DrawGizmos();
        //ls2d.GetMirrorPoint(point).DrawGizmos();


        Float2 interPoint = Float2.zero;
        if (ls2d.GetIntersectPoint(ls21, ref interPoint) == true)
        {
            interPoint.DrawGizmos();
        }
    }

}
