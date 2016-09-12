using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour 
{
    private Text m_getText;

	// Use this for initialization
	void Start () 
    {
        m_getText = this.GetComponent<Text>();
	}
	
	

    void Update()
    {
        Display();
    }

    void Display()
    {
        m_getText.text = string.Format("{0} : {1:00}", (int)Singleton<TimeMgr>.instance.m_fmin, (int)TimeMgr.getInstance.m_fsec);
        
    }
}
