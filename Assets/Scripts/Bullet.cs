using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public GameObject target;
    public float speed = 0.3f;

    void FixedUpdate()
    {
        //если есть цель
        if (target != null)
        {
            //пуля летит к цели
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);

            //когда долетела - уничтожается
            if (transform.position == target.transform.position)
                Destroy(gameObject);
        }
        //если цели нет - сразу уничтожаем пулю
        else
        {
            Destroy(gameObject);
        }
    }
}
