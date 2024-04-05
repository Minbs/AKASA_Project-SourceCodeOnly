using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string synergyObj;
    public Tooltip tooltip;

    public void GetTooltip(string synergy)
    {
        switch (synergy)
        {
            case "Guardian":
                tooltip.SetupTooltip(synergy, "[����� Ŭ����]", 
                    "<color=red>����� Ŭ����</color> ��������\n" +
                    "���� ������ ��ġ�� ������\n" +
                    "�޴� ���ذ� "+ SynergyManager.Instance.guardianCoefficient+ "% �����Ѵ�.");
                break;
            case "Rescue":
                tooltip.SetupTooltip(synergy, "[����ť Ŭ����]",
                    "<color=red>����ť Ŭ����</color> ��������\n" +
                    "���� ������ ��ġ�� ������\n" +
                    "�޴� �������� " + SynergyManager.Instance.rescueCoefficient + "%�����Ѵ�.");
                break;
            case "Buster":
                tooltip.SetupTooltip(synergy, "[������ Ŭ����]",
                    "<color=red>������ Ŭ����</color> ��������\n" +
                    "���� 1ĭ�� ��ġ �� �Ʊ� ������ \n" +
                    "���ݷ� " + SynergyManager.Instance.busterCoefficient + "(" + SynergyManager.Instance.busterCoefficient * 2 + ")%\n" +
                    "ü��" + SynergyManager.Instance.busterCoefficient + "(" + SynergyManager.Instance.busterCoefficient * 2 + ")%" + "����.");
                break;
            case "Chaser":
                tooltip.SetupTooltip(synergy, "[ü�̼� Ŭ����]", 
                    "<color=red>ü�̼� Ŭ����</color> ��������\n" +
                    "���� 2ĭ�� ��ġ�� �Ʊ� ������\n" +
                    "���ݼӵ�" + +SynergyManager.Instance.chaserCoefficient + "(" + SynergyManager.Instance.busterCoefficient*2 + ")%\n");
                break;
            default:
                tooltip.SetupTooltip(synergy, "[�׽�Ʈ Ŭ���� �̸�]", "<color=red>�׽�Ʈ Ŭ����</color> �׽�Ʈ Ŭ���� ����");
                break;
        }

    }

    public void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GetTooltip(synergyObj);
        tooltip.gameObject.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
