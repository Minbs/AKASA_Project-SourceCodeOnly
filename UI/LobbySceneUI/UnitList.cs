using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
// �� Ŭ������ ĳ���͸� �߰�, ����, �����ϴ� Ŭ�������� ���. 
public class UnitList : Singleton<UnitList>, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool SelectUnit = false;
    private string UnitListFile = "myUnits";    // �ҷ��� ���� �̸�
    private List<string> myUnits = new List<string>();
    private GameObject Dummy;
    private Vector2 MousePos;
    private RaycastHit hit;
    private GraphicRaycaster gr;
    [SerializeField] private List<MinionsPortrait> MinionList;
    [SerializeField] private MinionsPortrait Prefab;
    [SerializeField] private List<MinionsPortrait> temp;
    [SerializeField] public List<Sprite> Minions_Illust;
    [SerializeField] public List<Sprite> Minions_Standing;
    Canvas myCanves;
    Camera myCamera;
    private void Start()
    {
        myCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        myCanves = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {

    }

    public void SetEditting(MinionsPortrait up)
    {
        try
        {
            if (EditList.Instance.ListCheck(Dummy.GetComponent<MinionsPortrait>()))
            {
                // ���� ����
                //Dummy.GetComponent<MinionsPortrait>().GetData(ref up);
                //EditList.Instance.e_NameList.Add(up.pro_Minion_e_Name);
                up.updateInfo(Dummy.GetComponent<MinionsPortrait>().pro_info);
                up.init();
            }
            else
            {
                Debug.Log("���� ĳ���͸� �߰��� �� �����ϴ�.");
            }
        }
        catch
        {
            Debug.Log("Editor Setting Error");
        }

    }

    public void CreateDummy(Collider2D other)
    {
        //Debug.Log("Ȱ��ȭ");
        if (!SelectUnit)
        {
            SelectUnit = true;
            GameObject a = other.gameObject;
            Dummy = Instantiate<GameObject>(a, Input.mousePosition, Quaternion.identity);
            Dummy.GetComponent<RectTransform>().sizeDelta = new Vector2(125, 125);
            Dummy.transform.parent = myCanves.transform;
        }
    }

    private void DestroyDummy()
    {
        //Debug.Log("��Ȱ��ȭ");
        if (SelectUnit)
        {
            Destroy(Dummy);
            SelectUnit = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        try
        {
            Debug.Log("�����ʹٿ�");
        }
        catch(System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
        if (eventData.button == PointerEventData.InputButton.Left)
            Debug.Log("Left");
        else if (eventData.button == PointerEventData.InputButton.Right)
            Debug.Log("Right");
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle");
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        try
        {
            //Debug.Log("���巡��");
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count <= 0) return;
            for(int i = 0; i  < results.Count; i++)
            {
                if (results[i].gameObject.tag == "Minions_Inventory")
                {
                    CreateDummy(results[i].gameObject.GetComponent<Collider2D>());
                    break;
                }
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            DestroyDummy();
            //Debug.Log("���� �巡��");
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count <= 0) return;
            if (results[0].gameObject.tag == "Minions_EditList")
            {
                SetEditting(results[0].gameObject.GetComponent<MinionsPortrait>());
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        try
        {
            //Debug.Log("�巡��");
            if (SelectUnit)
            {
                MousePos = Input.mousePosition;

                Dummy.transform.position = MousePos;
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
    }


    [SerializeField] private bool SortType;         // true == ��������, false == ��������
    public void SortReverse()
    {
        if (SortType)
            MinionList.Reverse();
    }
    // ���� Sort
    public void SortingName()       // �̸����� ����
    {
        try
        {
            MinionList.Sort((MinionA, MinionB) => MinionA.pro_e_name.CompareTo(MinionB.pro_e_name));   // �̸� ��?
            SortReverse();
        }
        catch
        {
            Debug.Log("Sorting Error : Name");
        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            MinionsPortrait g = Instantiate<MinionsPortrait>(MinionList[i]);

            temp.Add(g);

            g.transform.parent = this.transform;

        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            Destroy(MinionList[i].gameObject);
        }


        MinionList = temp;

        temp = new List<MinionsPortrait>();

    }

    public void SortingLv()         // ������ ����
    {
        try
        {
            MinionList.Sort((MinionA, MinionB) => MinionA.pro_lv.CompareTo(MinionB.pro_lv));
            SortReverse();
        }
        catch
        {
            Debug.Log("Sorting Error : Lv");
        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            MinionsPortrait g = Instantiate<MinionsPortrait>(MinionList[i]);

            temp.Add(g);

            g.transform.parent = this.transform;

        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            Destroy(MinionList[i].gameObject);
        }


        MinionList = temp;

        temp = new List<MinionsPortrait>();
    }

    public void SortingRank()       // ������� ����
    {

    }

    public void SortingGetTime()    // �Լ��Ϸ� ����
    {

    }


}
