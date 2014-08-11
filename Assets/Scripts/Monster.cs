using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour 
{
    public List<GameObject> path = new List<GameObject>();
    public GameObject house;
    public float speed = 0.1f, gravityDamageMultiply = 3f;

    private float lastVelocity;
    private float damage;
    private int currentPathPoint = 0;
    private bool moveToHouse = false;

    void Start()
    {
        //урон монстра - его хп
        damage = GetComponent<Health>().maxHealth;

        for (int i = 0; i < path.Count; i++)
        {
            if (path[i] == null)
                path.Remove(path[i]);
        }

        //если не задана траектория, то двигаем врага сразу к дому
        if (path.Count <= 0)
            moveToHouse = true;
        else
            rigidbody.useGravity = false;
    }
    void FixedUpdate()
    {
        //если монстр стукнулся с большой высоты
        if (Mathf.Abs(rigidbody.velocity.y - lastVelocity) >= 5f && rigidbody.velocity.y == 0)
        {
            SendMessage("DealDamage", GetComponent<Health>().maxHealth);
            //SendMessage("DealDamage", Mathf.Abs(rigidbody.velocity.y - lastVelocity - 5) * gravityDamageMultiply);
        }
        lastVelocity = rigidbody.velocity.y;

        //если монстр долетел до дома - наносим урон дому
        if (Vector3.Distance(house.transform.position, transform.position) < 1f)
        {
            house.transform.parent.SendMessage("DealDamage", damage);
            Destroy(gameObject);
        }
        //если нет - летим
        else
        {
            if (!moveToHouse)
            {
                //если задана траектория движения - движем врага по ней
                if (path.Count > 0)
                {
                    //если вейпоинт не существует
                    if (path[currentPathPoint] == null)
                    {
                        //и он был последний в списке, то двигаем врага к дому
                        if (currentPathPoint == path.Count - 1)
                            moveToHouse = true;
                        currentPathPoint = Mathf.Clamp(currentPathPoint + 1, 0, path.Count - 1);
                        return;
                    }

                    //двигаем врага к вейпоинту
                    transform.position = Vector3.MoveTowards(transform.position, path[currentPathPoint].transform.position, speed / 100f);
                    if (transform.position == path[currentPathPoint].transform.position)
                    {
                        currentPathPoint = Mathf.Clamp(currentPathPoint + 1, 0, path.Count - 1);
                        if (currentPathPoint == path.Count - 1)
                            moveToHouse = true;
                    }
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, house.transform.position, speed / 100f);
            }
        }
    }
}
