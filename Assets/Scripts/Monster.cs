using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour 
{
    public GameObject house;
    public float speed;
    private float damage;

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
            transform.position = Vector3.MoveTowards(transform.position, house.transform.position, Time.deltaTime * speed);
        }
    }
}
