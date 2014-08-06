using UnityEngine;
using System.Collections;

public class HouseController : MonoBehaviour 
{
    public float speed = 0.3f;

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, Time.deltaTime / 100f * speed);
    }
}
