using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnLockMap : MonoBehaviourSingleton<UnLockMap>
{
    
    public static UnLockMap m_instance;
    public string m_strStage = "Stage";

    [System.Serializable]
    public class UnlockInfo
    {
        public string m_sceneName;
        public int m_sceneNumber;
        public bool isUnlocked;        
    }    
    public List<UnlockInfo> m_unlocklist = new List<UnlockInfo>();   

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

        for (int i = 0; i < m_unlocklist.Count; i++)
        {
            m_unlocklist[i].m_sceneName = string.Format("{0}{1}", m_strStage , i.ToString());
            m_unlocklist[i].m_sceneNumber = i;
            m_unlocklist[i].isUnlocked  = LoadUnlockCheck(m_strStage, i);
            //m_unlocklist[i].isUnlocked = false;
            //PlayerPrefs.SetInt(m_unlocklist[i].m_sceneName, m_unlocklist[i].isUnlocked ? 1 : 0); //false면 0, true 면 1을 저장
        }
        m_unlocklist[0].isUnlocked = true;
        PlayerPrefs.SetInt(m_unlocklist[0].m_sceneName, m_unlocklist[0].isUnlocked ? 1 : 0);

    }
	
	public bool LoadUnlockCheck(string _sceneName, int _sceneNumber)
    {
        return (PlayerPrefs.GetInt(string.Format("{0}{1}",_sceneName,_sceneNumber.ToString() )) == 0) ? false : true; //0이면 false, 1이면 true반환 
    }
}
