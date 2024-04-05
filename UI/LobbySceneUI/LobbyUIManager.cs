using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [Space(10f)]
    [Header("title")]
    public TextMeshProUGUI BlinkTextUI;
    public Image titleLogo;
    public Image TitleImage;
    public List<Sprite> TitleSprite;
    public float BG_Change_Time;

    [Space(10f)]
    [Header("��� �����ִ� Panel")]
    public GameObject AnimePanel;
    bool m_bPanelOn = false;
    public MinionsPortrait minions;

    [Space(10f)]
    [Header("Popup Panel")]
    public GameObject SettingPanel;
    private bool SettingScreenOn;
    public GameObject PopupPanel;
    public GameObject InfoPanel;
    public TextMeshProUGUI Title_text;
    public TextMeshProUGUI Content_text;
    public Button ConfirmBtn;
    public Button cancelBtn;

    [Space(10f)]
    [Header("Stage_Info")]
    public GameObject StageInfo_Screen;
    public TextMeshProUGUI StageName_Text;
    public GameObject EditPanel;
    public TextMeshProUGUI BestLvText;


    //public GameObject MinionStand;

    delegate void FunctionPointer();

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if (BlinkTextUI != null && TitleImage != null)
        {
            BG_Change_Time = 10f;
            StartCoroutine(BlinkText());
            StartCoroutine(ChangeBG());
        }

        if (PopupPanel != null)
            PopupPanel.SetActive(false);
        if (StageInfo_Screen != null)
            StageInfo_Screen.SetActive(false);
        if (EditPanel != null)
            EditPanel.SetActive(false);
        if (InfoPanel != null)
            InfoPanel.SetActive(false);
        if (SettingPanel != null)
        {
            SettingScreenOn = false;
            SettingPanel.SetActive(false);
        }
        //btn.onClick.AddListener(SaveConfirm);     // ��ư ���� ��
    }

    #region Titlefunc

    IEnumerator ChangeBG()
    {
        int BgCount = 0;
        float bgAlpha = TitleImage.color.a;
        while (true)
        {
            BgCount = Random.Range(0, TitleSprite.Count);
            for (; bgAlpha > 0.0f; bgAlpha -= .01f)
            {
                BGAlphaChange(bgAlpha);
                
                yield return new WaitForSeconds(0.01f);
            }
            TitleImage.sprite = TitleSprite[BgCount];

            for (; bgAlpha < 1.0f; bgAlpha += .01f)
            {
                BGAlphaChange(bgAlpha);
                yield return new WaitForSeconds(.01f);
            }
            yield return new WaitForSeconds(BG_Change_Time);
        }
    }


    IEnumerator BlinkText()
    {
        float TextAlpha = BlinkTextUI.color.a;

        while (BlinkTextUI == true)
        {
            for (; TextAlpha > 0.0f; TextAlpha -= .1f)
            {
                ColorAlphaChange(TextAlpha);
                yield return new WaitForSeconds(0.1f);
            }

            for (; TextAlpha < 1.0f; TextAlpha += .1f)
            {
                ColorAlphaChange(TextAlpha);
                yield return new WaitForSeconds(.1f);
            }
        }
    }

    void ColorAlphaChange(float TextAlpha)
    {
        Color c_Dummy = BlinkTextUI.color;
        c_Dummy.a = (float)TextAlpha;
        BlinkTextUI.color = c_Dummy;
    }

    void BGAlphaChange(float Alpha )
    {
        Color c_Dummy = TitleImage.color;
        c_Dummy.a = (float)Alpha;
        TitleImage.color = c_Dummy;
        titleLogo.color = c_Dummy;
    }


    #endregion


    public void LoadScene(string SceneName)
    {

        SceneManager.LoadScene(SceneName);
        Debug.Log(SceneName + "������ �̵�");
    }

    public void LoadGachaScene()
    {
        Debug.Log("�̱� �� ���");
    }

    public void SettingScreen()
    {
        if (SettingPanel != null)
        {
            if (SettingScreenOn == false)
            {
                SettingPanel.SetActive(true);
                SettingScreenOn = true;
            }
            else
            {
                SettingPanel.SetActive(false);
                SettingScreenOn = false;
            }
        }
        Debug.Log("���� �� �����ֱ�");
    }

    public void LoadStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene_2");
        Debug.Log("�������� ���� �� �����ֱ�");
    }

    public void PreparingButton()
    {
        if (m_bPanelOn == false)
            StartCoroutine(PanelPlay());
    }

    public void ClearEditList()
    {
        if (PopupPanel != null)
        {
            PopupPanel.SetActive(true);
            PanelEdit("�ʱ�ȭ", "���� �ʱ�ȭ�Ͻðڽ��ϱ�?");
            ConfirmBtn.onClick.AddListener(ClearConfirm);
            cancelBtn.onClick.AddListener(PanelCancel);
        }
        else
        {
            Debug.Log("Clear EditList Confirm Error");
        }
    }


    void PanelEdit(string m_title, string m_contents)
    {
        Title_text.text = m_title;
        Content_text.text = m_contents;
    }
    public void ClearConfirm()
    {
        PanelCancel();
        EditList.Instance.ListClear();
    }

    public void SaveEditList()
    {
        if(PopupPanel != null)
        {
            PopupPanel.SetActive(true);
            PanelEdit("����", "������ �����Ͻðڽ��ϱ�?");


            ConfirmBtn.onClick.AddListener(SaveConfirm);
            cancelBtn.onClick.AddListener(PanelCancel);
        }
        else
        {
            Debug.Log("Error SaveEditList Confirm");
        }
    }
    public void SaveConfirm()
    {
        Debug.Log("����!");
        EditList.Instance.SaveMinionsData();
        PanelCancel();
    }

    public void ExitScene()
    {
        PopupPanel.SetActive(true);
        PanelEdit("������", "������ �����ðڽ��ϱ�?");
        ConfirmBtn.onClick.AddListener(ExitConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }
    public void ExitConfirm()
    {
        Debug.Log("������!");
        PanelCancel();
        LoadScene("MainScene");
    }

    public void ShowPanel(GameObject obj)
    {
        SetPanelActive(obj, true);
    }

    public void HidePanel(GameObject obj)
    {
        SetPanelActive(obj, false);
    }

    public void SetPanelActive(GameObject obj , bool b) => obj.SetActive(b);

    

    public void PanelCancel()
    {
        ConfirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        PopupPanel.SetActive(false);
    }

    void CreateSaveDirectory()
    {
        #if UNITY_EDITOR

        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resource", "Levels");
        AssetDatabase.Refresh();

       #endif
    }

    public IEnumerator PanelPlay()
    {
        m_bPanelOn = true;
        AnimePanel.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        AnimePanel.GetComponent<Animator>().SetTrigger("Hide");
        yield return new WaitForSeconds(1.0f);
        AnimePanel.SetActive(false);
        m_bPanelOn = false;
    }

    public void ShowStageInfo(GameObject obj)
    {
        StageInfo_Screen.SetActive(true);
        StageManager.Instance.SetTargetStage(obj);
        StageName_Text.text = obj.GetComponent<StageInfo>().pro_stagename;
        BestLvText.text = "Best Lv: " + obj.GetComponent<StageInfo>().pro_BestLv.ToString();
    }

    public void HideStageInfo()
    {
        StageManager.Instance.PopTargetStage();
        StageInfo_Screen.SetActive(false);
    }

    public void ShowMinionsInfo()
    {
        minions.gameObject.SetActive(true);
    }

}
