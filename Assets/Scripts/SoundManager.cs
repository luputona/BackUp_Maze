using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    protected static SoundManager m_instance;
    public static SoundManager getInstance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (m_instance == null)
                {
                    Debug.LogError("SoundManager Instance Error");
                }
            }
            return m_instance;
        }
    }

    public SoundVolume volume = new SoundVolume();

    const int m_cNumChannel = 16;

    private AudioClip[] m_seClips;
    private AudioClip[] m_bgmClips;

    private AudioSource m_bgmSource;
    private AudioSource[] m_seSource = new AudioSource[m_cNumChannel];

    private Dictionary<string, int> m_seIndex = new Dictionary<string, int>();
    private Dictionary<string, int> m_bgmIndex = new Dictionary<string, int>();
    
    Queue<int> m_seRequestQueue = new Queue<int>();


    void Awake()
    {
        //if(this != m_Instance)
        //{
        //    Destroy(this);
        //    return;
        //}

        m_bgmSource = gameObject.AddComponent<AudioSource>();
        m_bgmSource.maxDistance = 200.0f;
        m_bgmSource.loop = true;

        for(int i = 0; i < m_seSource.Length; i++)
        {
            m_seSource[i] = gameObject.AddComponent<AudioSource>();
            m_seSource[i].maxDistance = 20.0f;
        }

        m_seClips = Resources.LoadAll<AudioClip>("Audio/SE");
        m_bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");

        for (int i = 0; i < m_seClips.Length; i++ )
        {
            m_seIndex[m_seClips[i].name] = i;
        }

        for (int i = 0; i < m_bgmClips.Length; i++)
        {
            m_bgmIndex[m_bgmClips[i].name] = i;
        }
    }
   public void UpdateSound()
    {
        m_bgmSource.mute = volume.m_mute;
        for (int i = 0; i < m_seSource.Length; i++)
        {
            m_seSource[i].mute = volume.m_mute;
        }

        m_bgmSource.volume = volume.m_bgm;

        for (int i = 0; i < m_seSource.Length; i++)
        {
            m_seSource[i].volume = volume.m_se;
        }

        int count = m_seRequestQueue.Count;
        if (count != 0)
        {
            int sound_index = m_seRequestQueue.Dequeue();
            PlaySeImpl(sound_index);
        }

        StageBGMCheck();

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySe(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
            PlaySe(1);
        if (Input.GetKeyDown(KeyCode.D))
            PlaySe(2);
        if (Input.GetKeyDown(KeyCode.F))
            PlaySe("Walk_01");
    }

    void StageBGMCheck()
    {
        if (SceneMgr.GetInstance.m_SceneNumberCheck.Equals(0) && SceneMgr.GetInstance.isPointer.Equals(true))
        {
            PlayBgm("CM_LOGIN");
            //Debug.Log("Tile");
        }
        else if (SceneMgr.GetInstance.m_SceneNumberCheck.Equals(3) && SceneMgr.GetInstance.isPointer.Equals(true))
        {
            PlayBgm("DI4");
            //Debug.Log("Main");
        }
        else if (SceneMgr.GetInstance.m_SceneNumberCheck.Equals(4) && SceneMgr.GetInstance.isPointer.Equals(true))
        {
            PlayBgm("CM_DUNGEON_BGM3");
            //Debug.Log("Stage01");
        }
        else if(SceneMgr.GetInstance.m_SceneNumberCheck.Equals(7) && SceneMgr.GetInstance.isPointer.Equals(true))
        {
            PlayBgm("CM_DUNGEON_BGM5_1");
        }
       

    }

    void PlaySeImpl(int index)
    {
        if( 0 > index || m_seClips.Length <= index)
        {
            return;
        }

        for(int i = 0; i < m_seSource.Length; i++)
        {
            if(m_seSource[i].isPlaying ==false)
            {
                m_seSource[i].clip = m_seClips[index];
                m_seSource[i].Play();
                return;
            }
        }
    }

    public int GetSeIndex(string name)
    {
        return m_seIndex[name];
    }

    public int GetBgmIndex(string name)
    {
        return m_bgmIndex[name];
    }

    public void PlayBgm(string name)
    {
        int index = m_bgmIndex[name];
        PlayBgm(index);
    }
    public void PlayBgm(int index)
    {
        if( 0 > index || m_bgmClips.Length <= index)
        {
            return;
        }

        if (m_bgmSource.clip == m_bgmClips[index])
        {
            return;
        }

        m_bgmSource.Stop();
        m_bgmSource.clip = m_bgmClips[index];
        m_bgmSource.Play();
    }

    public void StopBgm()
    {
        m_bgmSource.Stop();
        m_bgmSource.clip = null;
    }

    public void PlaySe(string name)
    {
        PlaySe(GetSeIndex(name));
    }

    public void PlaySe(int index)
    {
        if(!m_seRequestQueue.Contains(index))
        {
            m_seRequestQueue.Enqueue(index);
        }
    }
    public void StopSe()
    {
        ClearAllRequest();
        for(int i = 0; i< m_seSource.Length;i++)
        {
            m_seSource[i].Stop();
            m_seSource[i].clip = null;
        }
    }

    public void ClearAllRequest()
    {
        m_seRequestQueue.Clear();
    }
}

public class SoundVolume
{
    public float m_bgm = 1.0f;
    public float m_se = 1.0f;

    public bool m_mute = false;

    public void Init()
    {
        m_bgm = 1.0f;
        m_se = 1.0f;
        m_mute = false;

    }

}