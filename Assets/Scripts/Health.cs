using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
    public float curHealth, maxHealth;

    public void DealDamage(float damage)
    {
        curHealth -= damage;
    }

    void Dead()
    {
        if (gameObject.tag == "House")
            Application.LoadLevel("GameOver");
        else
            Destroy(gameObject);
    }
    void Start()
    {
        curHealth = maxHealth;
    }
    void Update()
    {
        curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
        maxHealth = Mathf.Clamp(maxHealth, 1, Mathf.Infinity);

        if (curHealth == 0)
            Dead();
    }
}
