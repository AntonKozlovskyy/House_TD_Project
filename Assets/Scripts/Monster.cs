using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour 
{
    public List<Vector3> path = new List<Vector3>();
    public GameObject house;
    public float speed;

    private float damage;
    private int currentPathPoint = 0;

    void Start()
    {
        //урон монстра - его хп
        damage = GetComponent<Health>().maxHealth;
    }
    void Update()
    {
        //если монстер долетел до дома - наносим урон дому
        if (Vector3.Distance(house.transform.position, transform.position) < 1f)
        {
            house.transform.parent.SendMessage("DealDamage", damage);
            Destroy(gameObject);
        }
        //если нет - летим
        else
        {
            if (path.Count > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, path[currentPathPoint], speed);
                if (transform.position == path[currentPathPoint])
                {
                    currentPathPoint = Mathf.Clamp(currentPathPoint + 1, 0, path.Count);
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, house.transform.position, Time.deltaTime * speed);
        }
    }
}
