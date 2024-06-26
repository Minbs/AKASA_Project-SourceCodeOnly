using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;

using System.IO;

public class EditList : Singleton<EditList>
{
    // GraphicRaycaster gr;
    // 이 곳은 파티 편집기에 등록한 파티를 저장하고 초기화하는 기능
    public List<MinionsPortrait> myList = new List<MinionsPortrait>();
    public GameObject Contents;
    public int EditMax = 10;
    //public MainObjectDataa ArrayItem = new MainObjectDataa();
    public MinionsPortrait EditPanelPrefab;

    MinionsPortrait dummy;

    public List<string> e_NameList;

    private void Awake()
    {

    }

    public GameObject pro_Contents
    {
        get
        {
            if (Contents == null)
                Contents = GameObject.Find("EditContent");
            return Contents;
        }
        set
        {
            //SetList();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SetEdit();

    }

    public void SetEdit()
    {
        myList.Clear();
        if (EditMax / 2 == 1)       // 무조건 짝수로 변환해줘야 함.
            EditMax++;
        for (int i = 0; i < EditMax; i++)
        {
            MinionsPortrait go = Instantiate<MinionsPortrait>(EditPanelPrefab);

            myList.Add(go);

            go.transform.parent = pro_Contents.transform;
        }
        // 여기서 로드!
        //LoadJson();
        LoadData();
        
    }

    public void SaveMinionsData()
    {
        if(myList.Count >0 )
        {
            Debug.Log("저장 시도");
            UserData.Instance.SaveUnitData(myList);
        }
        else
        {
            Debug.Log("저장하기엔 리스트가 비었습니다.");
        }
    }

    public void LoadData()
    {
        Debug.Log("데이터 로드");
        try
        {
            int m_count = UserData.Instance.LoadUnitData().Count;
            if (m_count > 0)
            {
                for (int i = 0; i < m_count; i++)
                {
                    myList[i].updateInfo(UserData.Instance.LoadUnitData()[i]);
                }
            }
            else
            {
                Debug.Log("데이터 없음");
            }
        }
        catch
        {
            Debug.Log("Not Have A UserData");
        }
    }

    public void ListOut(MinionsInfo p)
    {
        for(int i = 0; i < myList.Count; i++)
        {
            if(myList[i].m_info.pro_k_name == p.pro_k_name)
            {
                // 이름이 같은 것을 찾아 그 곳을 비워줌
                MinionsPortrait go = Instantiate<MinionsPortrait>(EditPanelPrefab);
                Destroy(myList[i].gameObject);
                //myList.Add(go);
                myList[i] = go;
                go.transform.parent = this.transform;
                return;
            }
        }
    }

    public void ListClear()
    {
        EditMax = 10;
        if (EditMax / 2 == 1) // 무조건 짝수로 변환해줘야 함.
            EditMax++;
        
        for(int i = 0;  i < myList.Count;i++)
        {
            Destroy(myList[i].gameObject);
        }
        myList.Clear();
        
        for (int i = 0; i < EditMax; i++)
        {
            MinionsPortrait go = Instantiate<MinionsPortrait>(EditPanelPrefab);

            myList.Add(go);
            go.transform.parent = this.transform;
        }
    }

    public bool ListCheck(MinionsPortrait up)
    {
        foreach (MinionsPortrait minions in myList)
        {
            if (minions.pro_k_name == up.pro_k_name)    // 추후 일련번호로 수정
                return false;               // Panel 띄워주기
        }
        return true;
    }
}
