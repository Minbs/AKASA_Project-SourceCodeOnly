using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    /// <summary>
    /// target�� ���󰡴� ����Ʈ ����
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator HomingEffect(string effectName, GameObject target, float duration)
    {
        GameObject attackEffect = null;
        foreach (var e in EffectManager.Instance.effectsList)
        {
            if (e.name == effectName)
            {
                attackEffect = e;
                break;
            }
        }


        float timer = 0f;


        while (timer < duration)
        {
            if (target == null || !target.activeSelf || target.GetComponent<Unit>().currentHp <= 0)
            {
                Destroy(gameObject);
                break;
            }


            timer += Time.deltaTime * GameManager.Instance.gameSpeed;
            transform.position = target.transform.position;
            yield return null;
        }


        Destroy(gameObject);
    }
}
