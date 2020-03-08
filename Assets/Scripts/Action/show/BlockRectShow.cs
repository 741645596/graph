using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.PathFinder
{
    public class BlockRectShow : MonoBehaviour
    {
        public Material material;

        public Vector3 offsetUp = Vector3.up;

        private void OnRenderObject()
        {
            if (material == null)
                return;

            material.SetPass(0);
            GL.wireframe = false;

            GL.Color(Color.red);
            GL.PushMatrix();
            GL.Begin(GL.LINES);


            GL.End();
            GL.PopMatrix();

            GL.wireframe = false;
        }


    }
}