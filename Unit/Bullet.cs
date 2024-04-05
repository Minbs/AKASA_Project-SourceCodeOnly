using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private float damage;
    private GameObject target;

    public float duration;
    public bool isPoison = false;
    public float power;

    void Start()
    {
    }

    public void Init(float damage, GameObject target)
    {
        this.damage = damage;
        this.target = target;

        Vector2 camPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 targetCamPos = Camera.main.WorldToScreenPoint(target.transform.position);

        Vector2 diff = targetCamPos
    - camPos;
        diff.Normalize();



        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion qt = Quaternion.AngleAxis(rot_z, Vector3.forward);
    
        transform.eulerAngles = qt.eulerAngles;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + 133f - 90, transform.eulerAngles.y, transform.eulerAngles.z + 90);
    }

    void Update()
    {
        if (target == null || target.activeSelf == false)
        {
            ObjectPool.Instance.PushToPool("Bullet", gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= 0.1f )
        {
            ObjectPool.Instance.PushToPool("Bullet", gameObject);

            if(target.GetComponent<Unit>().currentHp > 0)
            {
               target.GetComponent<Unit>().Deal(damage);

                if (damage > 0)
                    EffectManager.Instance.InstantiateAttackEffect("hit", transform.position);
                else
                    EffectManager.Instance.InstantiateHomingEffect("heal", target, 2);
            }

      

   
        }

        Vector3 des = target.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, des, speed * Time.deltaTime *  GameManager.Instance.gameSpeed);
    }


}
