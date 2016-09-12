using UnityEngine;
using System.Collections;
using System.Text;

public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
{
    private static ScoreManager m_instance;
 
    private int m_tempSaveScore;
    private float m_defaultScore;
    private float m_calculateA;
    private bool m_scoreCheck;
    private bool m_timeCheck;

    public float m_getTime;
    public float m_calculateScoreBonus;
    public float m_calculateTimeBonus;
    public float m_clearScore;
    public float m_totalScore;
    public float m_newTime;
    public string m_saveTime_str;
    public string m_saveScore_str;
    

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
    }

    void Start()
    {
        //sb.Append(LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_MapName);
        //sb.Append(LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_ID);
        
        //PlayerPrefs.DeleteKey(LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_MapName);
        if (PlayerPrefs.HasKey(m_saveTime_str))
        {
            m_newTime = PlayerPrefs.GetFloat(m_saveTime_str);            
        }
        
        if(PlayerPrefs.HasKey(m_saveScore_str))
        {
            m_tempSaveScore = PlayerPrefs.GetInt(m_saveScore_str);
        }
    }

    public void CalculateScoring()
    {
        LoadMapData.getInstance.m_oldScore = PlayerPrefs.GetInt(LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_MapName);
        
        m_defaultScore = LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_score;
        
        m_calculateA = (m_defaultScore * 5.0f) - (m_defaultScore + m_getTime * 100.0f);
        m_calculateScoreBonus = (m_calculateA - m_defaultScore * 0.01f) - m_getTime * 80.0f;
        m_calculateTimeBonus = m_calculateScoreBonus * 0.2f;
        m_clearScore = (m_calculateTimeBonus + m_calculateA) * 0.1f;
        m_totalScore = m_calculateScoreBonus + m_calculateTimeBonus + m_clearScore;
        
        if(m_getTime > 3000.0f)
        {
            m_calculateA = 0;
            m_calculateScoreBonus = 0;
            m_calculateTimeBonus = 0;
            m_clearScore = 0;
            m_totalScore = 0;
        }

    }
    //void Update()
    //{
        

    //    //Debug.Log("old : " + LoadMapData.getInstance.m_oldScore);
    //    //Debug.Log("new : " + m_tempSaveScore);
    //    //Debug.Log("Stagename : " + LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_MapName);

    //    //Debug.Log("Default score: " + LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum].m_score);
    //}
    public void SaveScoring()
    {
        m_tempSaveScore = (int)m_totalScore;
        m_saveScore_str = LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum - 1].m_MapName;

        LoadMapData.getInstance.m_oldScore = PlayerPrefs.GetInt(m_saveScore_str);

        if (PlayerPrefs.HasKey(m_saveScore_str))
        {
            if (m_tempSaveScore > PlayerPrefs.GetInt(m_saveScore_str))
            {
                PlayerPrefs.SetInt(m_saveScore_str, m_tempSaveScore);
            }
        }
        else
        {
            PlayerPrefs.SetInt(m_saveScore_str, m_tempSaveScore);
        }     
    }

    public void SaveTime()
    {
        m_newTime = m_getTime;

        LoadMapData.getInstance.m_oldMapID = LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum - 1].m_ID;
        LoadMapData.getInstance.m_oldMapName = LoadMapData.getInstance.m_MapInfoList[StageClearManager.GetInstance.m_StageNum - 1].m_MapName;        
        
        LoadMapData.getInstance.m_oldTime = PlayerPrefs.GetFloat(m_saveTime_str);

        m_saveTime_str = string.Format("{0}{1}", LoadMapData.getInstance.m_oldMapName, LoadMapData.getInstance.m_oldMapID.ToString());

        
        if (PlayerPrefs.HasKey(m_saveTime_str))
        {
            if (PlayerPrefs.GetFloat(m_saveTime_str) > m_newTime)
            {               
                PlayerPrefs.SetFloat(m_saveTime_str, m_newTime);        
            }
        }
        else
        {
            PlayerPrefs.SetFloat(m_saveTime_str, m_newTime);            
        }
    }

    public bool CheckSocre()
    {
        if (m_tempSaveScore >= LoadMapData.getInstance.m_oldScore)
        {
            m_scoreCheck = true;
        }
        else if (m_tempSaveScore < LoadMapData.getInstance.m_oldScore)
        {
            m_scoreCheck = false;
        }
        return m_scoreCheck;
    }

    public bool CheckTime()
    {
        if (m_newTime <= PlayerPrefs.GetFloat(m_saveTime_str) || (PlayerPrefs.GetFloat(m_saveTime_str).Equals(0) && PlayerPrefs.HasKey(m_saveTime_str)))
        {
            m_timeCheck = true;
        }
        else if (m_newTime > PlayerPrefs.GetFloat(m_saveTime_str))
        {
            m_timeCheck = false;
        }
        return m_timeCheck;
    }


    // 디폴트 점수(m_defualtScore)는 맵 데이타 json 에 추가해서 얻어오기 .
    
    // 저장은 playerprefs ("스테이지명"+savepath )
}
