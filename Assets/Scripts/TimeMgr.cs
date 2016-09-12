using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeMgr : MonoBehaviour
{
    private static TimeMgr m_instance;

    public static TimeMgr getInstance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType(typeof(TimeMgr)) as TimeMgr;
            }
            if (m_instance == null)
            {
                GameObject obj = new GameObject("TimeMgr");
                m_instance = obj.AddComponent(typeof(TimeMgr)) as TimeMgr;
            }
            return m_instance;
        }
    }
    
    public float m_fTime;
    public float m_fmin;
    public float m_fsec;

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
        Application.targetFrameRate = 30;
    }

   
   
	// Update is called once per frame
	void Update () 
    {
        GetTime();
        SoundManager.getInstance.UpdateSound();
        ParticleHit.GetInstance.UpdateParticle();

       
	}

    public void GetTime()
    {
        m_fTime += Time.deltaTime;

        m_fmin = m_fTime / 60;
        m_fsec = m_fTime % 60;              
    }

    public void DisplayTime()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GetTime();           
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {            
            GetTime();
        }     
    }
}
