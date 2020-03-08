using System.Collections.Generic;
using UnityEngine;

public class wayPointShow : MonoBehaviour
{
    public Material material;
    public Vector3 offsetUp = Vector3.up;

    public bool showDetail = false;

    void OnRenderObject()
    {
        material.SetPass(0);
        GL.wireframe = false;

        GL.Color(Color.blue);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        //
        GL.End();
        GL.PopMatrix();

        GL.wireframe = false;
    }
}
