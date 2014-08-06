using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour 
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
