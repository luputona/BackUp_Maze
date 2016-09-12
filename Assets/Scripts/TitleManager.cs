using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

    void Awake()
    {
        //SettingManager.GetInstance.SaveVolume();
        SoundManager.getInstance.volume.m_bgm = PlayerPrefs.GetFloat("BGM_Volume");
        SoundManager.getInstance.volume.m_se =  PlayerPrefs.GetFloat("SE_Volume");
    }
	// Use this for initialization
	void Start () 
    {
        SoundManager.getInstance.PlayBgm("CM_LOGIN");
	}
	
}
