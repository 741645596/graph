using UnityEngine;
using System.Collections.Generic;

public class SectorShow : MonoBehaviour
{
    public Material material;
    public Vector3 offsetUp = Vector3.up;
    public GUIStyle style;
    public bool ShowText = false;

    void OnRenderObject()
    {


        material.SetPass(0);
        GL.wireframe = false;

        GL.Color(Color.blue);
        GL.PushMatrix();
        GL.Begin(GL.LINES);



        GL.End();
        GL.PopMatrix();


        GL.wireframe = false;

    }




#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

    }
#endif
}

