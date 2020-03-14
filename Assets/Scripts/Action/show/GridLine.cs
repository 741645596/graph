using UnityEngine;
using System.Collections;
using Graphics.Geometric;
using Graphics.Math;


public class GridLine : MonoBehaviour
{
    public Material material;
    public LineSegment2D ls2d;
    public Float2 point;

    void Start()
    {
        ls2d = new LineSegment2D(Float2.zero, new Float2(10, 10));
        InvokeRepeating("Doline", 0.1f, 1.0f);
    }

    void Doline()
    {
        float x = Random.Range(-10.0f, 10.0f);
        float y = Random.Range(-10.0f, 10.0f);
        ls2d = new LineSegment2D(Float2.zero, new Float2(x, y));
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


        GL.End();
        GL.PopMatrix();

        GL.wireframe = false;
    }

    void OnDrawGizmos()
    {
        ls2d.DrawGizmos();
        point.DrawGizmos();
        
        ls2d.ProjectPoint(point).DrawGizmos();
        ls2d.GetMirrorPoint(point).DrawGizmos();


    }

}
