using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class SettingManager : MonoBehaviour
{

    public Slider[] m_soundSlider = new Slider[3];
    public Image[] m_speakerImage = new Image[3];

    private StringBuilder sb = new StringBuilder();

	// Use this for initialization
	void Start () 
    {
        SoundManager.getInstance.volume.m_bgm = PlayerPrefs.GetFloat("BGM_Volume");
        SoundManager.getInstance.volume.m_se = PlayerPrefs.GetFloat("SE_Volume");
        m_soundSlider[0].value = PlayerPrefs.GetFloat("BGM_Volume");
        m_soundSlider[1].value = PlayerPrefs.GetFloat("SE_Volume");

        //m_soundSlider[2].value = SoundManager.getInstance.volume.m_bgm; // 나중에 voice 추가 

        for (int i = 0; i < m_speakerImage.Length; i++ )
        {
            m_speakerImage[i].gameObject.SetActive(false);
        }
	}

	void FixedUpdate()
    {
        SaveVolume();
    }

    public void MapResetButton()
    {
        for(int i = 1; i<UnLockMap.GetInstance.m_unlocklist.Count; i++)
        {
            UnLockMap.GetInstance.m_unlocklist[i].isUnlocked = false;
            PlayerPrefs.SetInt(UnLockMap.GetInstance.m_unlocklist[i].m_sceneName, UnLockMap.GetInstance.m_unlocklist[i].isUnlocked ? 1 : 0);
        }        
    }

    public void ResetScore()
    {
        for (int i = 0; i < LoadMapData.getInstance.m_MapInfoList.Count; i++ )
        {            
            PlayerPrefs.DeleteKey(LoadMapData.getInstance.m_MapInfoList[i].m_MapName);
            PlayerPrefs.DeleteKey(LoadMapData.getInstance.m_MapInfoList[i].m_MapName + LoadMapData.getInstance.m_MapInfoList[i].m_ID);
            //Debug.Log(LoadMapData.getInstance.m_MapInfoList[i].m_MapName + LoadMapData.getInstance.m_MapInfoList[i].m_ID +" : " +PlayerPrefs.GetFloat(LoadMapData.getInstance.m_MapInfoList[i].m_MapName + LoadMapData.getInstance.m_MapInfoList[i].m_ID));
            
        }
        
    }

    public void SaveVolume()
    {
        SoundManager.getInstance.volume.m_bgm = m_soundSlider[0].value;
        PlayerPrefs.SetFloat("BGM_Volume", SoundManager.getInstance.volume.m_bgm);

        if (SoundManager.getInstance.volume.m_bgm <= 0)
        {
            m_speakerImage[0].gameObject.SetActive(true);
        }
        else
        {
            m_speakerImage[0].gameObject.SetActive(false);
        }

        SoundManager.getInstance.volume.m_se = m_soundSlider[1].value;
        PlayerPrefs.SetFloat("SE_Volume", SoundManager.getInstance.volume.m_se);

        if (SoundManager.getInstance.volume.m_se <= 0)
        {
            m_speakerImage[1].gameObject.SetActive(true);
        }
        else
        {
            m_speakerImage[1].gameObject.SetActive(false);
        }

    }


}
