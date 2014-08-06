using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour 
{
    public float damage = 10, speed = 0.5f, range = 5, bullets = 20, rotateSpeed = 0.1f;
    public bool isAutomatic = true;

    private GameObject target, bullet;
    private float lastShootTime;

    public void FindTarget()
    {
        //находим цели в радиусе, и проверяем, есть ли они врагами
        Collider[] targets = Physics.OverlapSphere(transform.position, range);
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].tag == "Enemy")
            {
                target = targets[i].gameObject;
                break;
            }
        }
    }
    public void Shoot()
    {
        //если турель автоматическая
        if (isAutomatic)
        {
            //наносим урон по цели
            target.SendMessage("DealDamage", damage, SendMessageOptions.RequireReceiver);

            //выпускаем пулю
            GameObject temp = (GameObject)Instantiate(bullet, transform.forward + transform.position, Quaternion.identity);
            //назначаем пуле цель
            temp.GetComponent<Bullet>().target = target;
        }
        //если ручная
        else
        {
            //пускаем луч
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range))
            {
                //если луч пересек объект Enemy, то наносим урон
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.SendMessage("DealDamage", damage, SendMessageOptions.RequireReceiver);

                    //выпускаем пулю
                    GameObject temp = (GameObject)Instantiate(bullet, transform.forward + transform.position, Quaternion.identity);
                    //назначаем пуле цель
                    temp.GetComponent<Bullet>().target = target;
                }
            }
        }
        //обновляем таймер стрельбы
        lastShootTime = Time.time;
    }
    public void RotateTurret()
    {
        //если пушка автоматическая
        if (isAutomatic)
        {
            //и у пушки есть цель
            if (target != null)
                //поворачиваем пушку к цели
                transform.forward = Vector3.MoveTowards(transform.forward, target.transform.position - transform.position, rotateSpeed);
        }
        //если пушка ручная
        else
        {
            //если палец тронул экран
            if (Input.touchCount > 0)
            {
                //поворачиваем турель к пальцу
                Vector3 direction = Camera.allCameras[0].ScreenToWorldPoint(Input.GetTouch(0).position);
                direction.z = 0;

                transform.forward = Vector3.MoveTowards(transform.forward, direction - transform.position, rotateSpeed);
            }
            //по-другому используем мышь для поворота
            else
            {
                //поворачиваем турель к мышке
                Vector3 direction = Camera.allCameras[0].ScreenToWorldPoint(Input.mousePosition);
                direction.z = 0;

                transform.forward = Vector3.MoveTowards(transform.forward, direction - transform.position, rotateSpeed);
            }
        }
    }

    void Start()
    {
        //находим префаб пули
        bullet = Resources.LoadAssetAtPath("Assets/Prefabs/Bullet.prefab", typeof(GameObject)) as GameObject;
    }
    void FixedUpdate()
    {
        //если турель готова стрелять
        if (lastShootTime + speed <= Time.time)
        {
            //если турель автоматическая
            if (isAutomatic)
            {
                //если цель существует и турель обернулась к цели
                if (target != null && Vector3.Angle(transform.forward, target.transform.position - transform.position) < 5f)
                {
                    //если текущая цель в радиусе действия - то стреляем, если нет - то находим другую цель
                    if (Vector2.Distance(target.transform.position, transform.position) <= range)
                        Shoot();
                    else
                        FindTarget();
                }
                else
                    FindTarget();
            }
            //если ручная
            else
            {
                //стреляем
                Shoot();
            }
        }
        //поворачиваем турель
        RotateTurret();
    }

    void OnMouseDown()
    {
        //делаем пушку ручной
        isAutomatic = false;
    }
    void OnMouseUp()
    {
        //делаем пушку автоматической
        isAutomatic = true;
    }
}
