using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneMgr : MonoBehaviourSingleton<SceneMgr>
{
    private static SceneMgr m_instance;
    
    private AsyncOperation ao;

    public GameObject m_loadingScreenBG;
    public Slider m_progressBar;
    public Text m_loadingText;
    public bool isFakeLoadingBar = false;
    public bool isPointer = false;
    public bool m_gotoMap_check;
    public int m_SceneNumberCheck;
    public int m_orderScene;

    public GameObject m_gameController;
    
    void Awake()
    {
        if (m_instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            m_instance = this;
        }
        m_orderScene = 4;
    }
	// Use this for initialization
    void Start()
    {        
        m_loadingScreenBG.SetActive(false);
        m_progressBar.gameObject.SetActive(false);
        m_loadingText.gameObject.SetActive(false);

        m_gameController = GameObject.FindGameObjectWithTag("GameController");
        //SceneManager.LoadScene(2, LoadSceneMode.Additive);

    }
	
    public void ChangeToScene(int _sceneNumber)
    {        
        SceneManager.LoadScene(_sceneNumber);
        m_SceneNumberCheck = _sceneNumber;
    }

    public void GoToMain()
    {
        Time.timeScale = 1;
        m_gameController.SetActive(false);
        LoadLevelScene(3);
    }

    public void ReTryStage()
    {
        LoadLevelScene(LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_MapNumber + SceneMgr.GetInstance.m_orderScene );
    }

    public void GoToSelectMap()
    {
        m_gotoMap_check = true;
        LoadLevelScene(3);                
    }
    

    public void LoadLevelScene(int _sceneNumber)
    {
        m_SceneNumberCheck = _sceneNumber;

        m_loadingScreenBG.SetActive(true);
        m_progressBar.gameObject.SetActive(true);
        m_loadingText.gameObject.SetActive(true);
        m_loadingText.text = "Loading....";
        
        if (!isFakeLoadingBar)
        {
            StartCoroutine(LoadLevelWithRealProgress(_sceneNumber));
        }
        else
        {
            //StartCoroutine(LoadLevelWithFakeProgress());
        }
    }
    public void ChangeSceneStopBGM()
    {
        SoundManager.getInstance.StopBgm();
        CharacterCreation.getInstance.DisableDisplay();
    }

    IEnumerator LoadLevelWithRealProgress(int _sceneNumber)
    {
        yield return new WaitForSeconds(1);

        ao = SceneManager.LoadSceneAsync(_sceneNumber);
        ao.allowSceneActivation = false;

        while(!ao.isDone)
        {
            m_progressBar.value = ao.progress;

            if(ao.progress == 0.9f)
            {
                m_progressBar.value = 1f;
                m_loadingText.text = "Press Touch to Continue";
                if(isPointer)
                {
                    ao.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    public void PointerClick()
    {
        isPointer = true;
    }

    public void SceneLoadTimer()
    {

    }

}
