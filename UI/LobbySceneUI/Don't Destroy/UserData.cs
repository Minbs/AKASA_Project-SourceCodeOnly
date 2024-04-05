using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : Singleton<UserData>
{
    // �� ��ũ��Ʈ�� ������ �����ؾ� �� �����͸� ������ �ִ� ��ũ��Ʈ���Ʒ�.

    [SerializeField]
    private string userName = "����������";

    [Space(10f)]
    [Header("������ ���� ���� �̴Ͼ� ������")]
    [SerializeField]
    private List<MinionsInfo> MinionData;
    [Space(10f)]
    [Header("�ΰ��� ��ȭ")]
    [SerializeField]
    private int coin;
    [SerializeField]
    private int ether;

    void Start()
    {
        if (UserData.Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //Instance.SetEdit();
            Destroy(this.gameObject);
        }
    }

    public void SaveUnitData(List<MinionsPortrait> portrait)
    {
        MinionData.Clear();
        int m_count = portrait.Count;
        for(int i = 0; i < m_count; i++)
        {
            MinionData.Add(portrait[i].pro_info);
        }
        Debug.Log("������ ����");
    }

    public List<MinionsInfo> LoadUnitData()
    {
        return MinionData;
    }
}
