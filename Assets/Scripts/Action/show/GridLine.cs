using UnityEngine;
using System.Collections;
using RayGraphics.Geometric;
using RayGraphics.Math;


public class GridLine : MonoBehaviour
{
    public Material material;
    public Rect2D rc2d;
    //public LineSegment2D ls2d;
    public LineSegment2D ls2d1;
    //public Float2 point;

    private Rays2D vectorAix;

    void Start()
    {
        rc2d = new Rect2D(Float2.zero, Float2.one * 10);
        //ls2d = new LineSegment2D(Float2.zero, new Float2(10, 0));
        ls2d1 = new LineSegment2D(new Float2(-20, 5), new Float2(20, 5));
        //InvokeRepeating("Doline", 0.1f, 1.0f);
    }

    void Doline()
    {
        //Float2 normalizedDir = Float2.Rotate(ls2d.normalizedDir, 0.1f);
        //ls2d = new LineSegment2D(ls2d.startPoint, ls2d.startPoint + normalizedDir * ls2d.length);
        //vectorAix = new Rays2D(ls2d.ProjectPoint(point), ls2d.AixsVector(point)); 
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

        //ls2d.Draw();
        //vectorAix.Draw();
        rc2d.Draw();
        ls2d1.Draw();


        GL.End();
        GL.PopMatrix();

        GL.wireframe = false;
    }

    void OnDrawGizmos()
    {
        //ls2d.DrawGizmos();
        ls2d1.DrawGizmos();
        rc2d.DrawGizmos();
        //point.DrawGizmos();

        //ls2d.ProjectPoint(point).DrawGizmos();
        //ls2d.GetMirrorPoint(point).DrawGizmos();


        /*Float2 interPoint = Float2.zero;
        if (ls2d.GetIntersectPoint(ls2d1, ref interPoint) == true)
        {
            interPoint.DrawGizmos();
        }*/
    }

}
