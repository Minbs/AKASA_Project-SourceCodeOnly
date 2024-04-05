using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Turret : Object
{
    public GameObject cannon;
    public float rotateSpeed;

    public float attackTimer = 0; // Ÿ�̸�
    public float attackSpeed; // ��Ÿ��

    public float attackDistance; // ���� ���� ����
    public float attackRange; // ���� ����

    public float attackDamage;

    public Image HPBar;
    public Image BarrierBar;

    private Ray ray;

    public float barrierHp;

    private bool isAttacking = false;

    public GameObject barrier;

    protected virtual void Start()
    {
        base.Start();
        attackTimer = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHp <= 0)
         GameManager.Instance.StageFailEvent();


        attackTimer += Time.deltaTime;
        HPBar.GetComponent<Image>().fillAmount = currentHp / maxHp;
        BarrierBar.GetComponent<Image>().fillAmount = barrierHp / 48;

        if (!GameManager.Instance.state.Equals(State.BATTLE))
            return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default")))
        {

            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(Shoot(hit.point));
            }
        }

        if (barrierHp <= 0)
            barrier.SetActive(false);
    }

    IEnumerator Shoot(Vector3 targetPos)
    {
        if (attackTimer < attackSpeed
            || Vector3.Distance(targetPos, cannon.transform.position) > attackDistance
            || isAttacking
            || SkillManager.Instance.isSkillActing)
            yield break;

        Quaternion quaternion = cannon.transform.localRotation;
        float proceed = 0;
        isAttacking = true;

        

        while (proceed < 1)
        {
            var lookPos = targetPos - cannon.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x - 90, rotation.eulerAngles.y, rotation.eulerAngles.z - 90);
            proceed += Time.deltaTime * rotateSpeed * GameManager.Instance.gameSpeed;
            cannon.transform.localRotation = Quaternion.Lerp(quaternion, rotation, proceed);
            yield return null;
        }

        EffectManager.Instance.InstantiateAttackEffect("hit", targetPos);
        attackTimer = 0;
        Collider[] targets = Physics.OverlapSphere(targetPos, attackRange);

        foreach (var e in targets)
        {
            if (e.tag != "Enemy")
                continue;
            
            e.transform.parent.GetComponent<Unit>().Deal(attackDamage);
        }

        isAttacking = false;
    }

    public void Deal(float damage)
    {
        float damageSum = damage;


       damageSum = Mathf.Max(damageSum, 0.5f);
       
        if(barrierHp > 0)
        {
            float temp = barrierHp;


            barrierHp -= damageSum;
            damageSum -= temp;
            damageSum = Mathf.Max(damageSum, 0);

            barrier.GetComponent<ParticleSystem>().Clear();
            barrier.GetComponent<ParticleSystem>().time = 0;
            barrier.GetComponent<ParticleSystem>().Play();
        }

        currentHp -= damageSum;

        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
    }
}
