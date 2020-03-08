using UnityEngine;

public class AllBlockShow : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Vector3 pos = Vector3.zero;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.25f);
    }
}