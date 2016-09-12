using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class MainUIMgr : MonoBehaviour {

    enum EUiList
    {
        E_MainPanel = 0,
        E_MainMenuPanel,
        E_MapPanel,
        E_SelectCharPanel,
        E_LoadingPanel,
        E_LoadingBGPanel,
        E_SettingPanel
    };

    public GameObject[] m_mainUilist;
    public RectTransform m_mapPanel;
    public RectTransform m_centerToCanvas;
    public GameObject m_fakeLoadingBG;

    void Awake()
    {
        SoundManager.getInstance.PlayBgm("DI4");
    }

	// Use this for initialization
	void Start () 
    {
        InitializeUI();
       
	}

    public void InitializeUI()
    {
        //for (int i = 0; i < m_mainUilist.Length; i++)
        //{
        //    //m_mainUilist[i].SetActive(false);
        //    m_mainUilist[i].GetComponent<Animator>().SetBool("isCheck", false);
        //}
        
        m_mainUilist[(int)EUiList.E_MainPanel].SetActive(true);
        m_mainUilist[(int)EUiList.E_MainMenuPanel].SetActive(true);
        m_mainUilist[(int)EUiList.E_MapPanel].SetActive(false);
        m_mainUilist[(int)EUiList.E_SettingPanel].SetActive(false);

        m_mainUilist[(int)EUiList.E_MainPanel].GetComponent<Animator>().SetBool("isCheck", true);
    }


    public void ChangeToUI(string _uiname)
    {
        if(_uiname == "selectmap")
        {
            CharacterCreation.getInstance.DisableDisplay();
            //StartCoroutine(BackgroundTimer());
            m_mainUilist[(int)EUiList.E_MainPanel].SetActive(false);
            m_mainUilist[(int)EUiList.E_MapPanel].SetActive(true);
            
            //m_mainUilist[0].GetComponent<Animator>().SetBool("isCheck", false);
            m_mainUilist[(int)EUiList.E_MapPanel].GetComponent<Animator>().SetBool("isCheck", true);
            
            
        }
        if(_uiname == "gotomain")
        {
            //StartCoroutine(BackgroundTimer());
            m_fakeLoadingBG.SetActive(true);
            StartCoroutine(MainUITimer());
            CharacterCreation.getInstance.DisableDisplay();

            //m_mainUilist[(int)EUiList.E_MainPanel].SetActive(true);
            m_mainUilist[(int)EUiList.E_MainPanel].GetComponent<Animator>().SetBool("isCheck", true);

            m_mainUilist[(int)EUiList.E_MapPanel].GetComponent<Animator>().SetBool("isCheck", false);          
            m_mainUilist[(int)EUiList.E_SelectCharPanel].GetComponent<Animator>().SetBool("isCheck", false);
            m_mainUilist[(int)EUiList.E_SettingPanel].GetComponent<Animator>().SetBool("isCheck", false);
            CharacterCreation.getInstance.ChangeSelectCharPosition(true);

            StartCoroutine(EnableCharacter());
            StartCoroutine(MapUITimer());
            
        }
        if(_uiname.Equals("selectChar"))
        {
            CharacterCreation.getInstance.DisableDisplay();
            //StartCoroutine(BackgroundTimer());
            m_mainUilist[(int)EUiList.E_MainPanel].SetActive(false);
            m_mainUilist[(int)EUiList.E_MainPanel].GetComponent<Animator>().SetBool("isCheck", false);
            m_mainUilist[(int)EUiList.E_SelectCharPanel].GetComponent<Animator>().SetBool("isCheck", true);

            CharacterCreation.getInstance.ChangeSelectCharPosition(false);
            StartCoroutine(EnableCharacter());

        }
        if(_uiname.Equals("Setting"))
        {
            CharacterCreation.getInstance.DisableDisplay();
            //StartCoroutine(BackgroundTimer());
            m_mainUilist[(int)EUiList.E_MainPanel].SetActive(false);
            m_mainUilist[(int)EUiList.E_SettingPanel].SetActive(true);
            m_mainUilist[(int)EUiList.E_SettingPanel].GetComponent<Animator>().SetBool("isCheck", true);
            
        }
       
    }

    //추후 로딩화면으로 재구현
    IEnumerator BackgroundTimer()
    {
        m_fakeLoadingBG.SetActive(true);
        yield return Yielders.Get(1.0f);
        m_fakeLoadingBG.SetActive(false);
    }

    IEnumerator MainUITimer()
    {
        yield return Yielders.Get(1.0f);
        m_mainUilist[(int)EUiList.E_MainPanel].SetActive(true);
    }
    IEnumerator MapUITimer()
    {
        yield return Yielders.Get(0.5f);
        m_mainUilist[(int)EUiList.E_MapPanel].SetActive(false);
    }

    IEnumerator EnableCharacter()
    {
        yield return Yielders.Get(1.0f);
        CharacterCreation.getInstance.EnableDispaly();
    }

}
