using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public GameObject target;
    public Vector3 gunForward;
    public float speed = 0.3f;

    private bool haveTarget = false;
    private GameObject house;
    
    void Start()
    {
        //находим объект дома
        house = GameObject.FindGameObjectWithTag("House");

        //булева для проверки не уничтожен ли монстр
        if (target != null)
            haveTarget = true;
    }

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
        //если цель была уничтожена, то удаляем пулю
        else if (haveTarget)
        {
            Destroy(gameObject);
        }
        //если цели нет, то пуля летит просто на 100 метров вперед
        else
        {
            //пуля движется
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed);

            //если дистанция между домом и пулей больше 100 метров - удаляем пулю
            if (Vector3.Distance(transform.position, house.transform.position) > 100)
                Destroy(gameObject);
        }
    }
}
