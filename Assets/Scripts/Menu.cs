using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public GameObject m_menu;


	// Use this for initialization
	void Start () 
    {
        m_menu.SetActive(false);
	}
	
    public void OnepMenu()
    {
        m_menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void CloseMenu()
    {
        m_menu.SetActive(false);
        Time.timeScale = 1;
    }

   
}
