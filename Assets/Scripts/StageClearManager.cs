using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class StageClearManager : MonoBehaviourSingleton<StageClearManager>
{

    public int m_StageNum;
    public GameObject m_clearSprite;
    public bool effectCheck;
    public bool check;
    public bool isTimer;

    private Vector3 m_HighLightScaleFactor;
    private Vector3 m_clearSpriteMovePosition;
   
    void Start()
    {
        m_StageNum = PlayerPrefs.GetInt("CurStageNum");
        effectCheck = false;
        check = false;
        isTimer = false;
        m_HighLightScaleFactor.x = 1.2f;
        m_HighLightScaleFactor.y = 1.2f;
        m_clearSprite.gameObject.transform.localScale = new Vector3(0, 0, 0);
        m_clearSpriteMovePosition.x = 530.0f;
        m_clearSpriteMovePosition.y = 350.0f;
        //m_clearSpriteMovePosition.z = -50.0f;
        //m_clearSprite.SetActive(false);
    }
   
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Player") )
        {
            m_StageNum++;
            UnLockMap.GetInstance.m_unlocklist[m_StageNum].isUnlocked = true;
            PlayerPrefs.SetInt(UnLockMap.GetInstance.m_unlocklist[m_StageNum].m_sceneName, UnLockMap.GetInstance.m_unlocklist[m_StageNum].isUnlocked ? 1 : 0);
        }
        check = true;
        effectCheck = true;

        ScoreManager.GetInstance.SaveScoring();
        ScoreManager.GetInstance.SaveTime();
        if (check && effectCheck )
        {
            isTimer = true;
            StartCoroutine(Timer());
        }
        //ScoreManager.GetInstance.SaveScoring();
        if(!effectCheck)
        {
           // ParticleHit.GetInstance.m_particleList[0].Stop();
        }
        
    }

    IEnumerator Timer()
    {
        yield return Yielders.Get(3.3f);
        //StartCoroutine(DisplayClearSprtie());
        StartCoroutine(ClearEffect(2.0f));
        //StartCoroutine(StopClearEffect());

        m_StageNum = LoadMapData.getInstance.m_MapInfoList[m_StageNum - 1].m_MapNumber;        
    }
   
    public IEnumerator ClearEffect(float delayTime)
    { 
        Vector3 position = m_clearSprite.transform.position;
        ParticleHit.GetInstance.m_particleList[0].transform.position = position;

        if(effectCheck)
        {
            ParticleHit.GetInstance.m_particleList[0].Play();

            yield return Yielders.Get(delayTime);
            StartCoroutine(ClearEffect(2.0f));
        }        
    }
    
    public IEnumerator StopClearEffect()
    {
        yield return Yielders.Get(8.0f);
        effectCheck = false;
    }
    
    public IEnumerator DisplayClearSprtie()
    {
        m_clearSprite.gameObject.transform.localScale = Vector3.Lerp(m_clearSprite.gameObject.transform.localScale, m_HighLightScaleFactor, Time.deltaTime * 10.0f );
        yield return Yielders.Get(0.5f);
        m_HighLightScaleFactor.x = 1;
        m_HighLightScaleFactor.y = 1;
        m_clearSprite.gameObject.transform.localScale = Vector3.Lerp(m_clearSprite.gameObject.transform.localScale, m_HighLightScaleFactor, Time.deltaTime * 10.0f);

        yield return Yielders.Get(2.0f);
        
        m_clearSprite.gameObject.transform.localPosition = Vector3.Lerp(m_clearSprite.gameObject.transform.localPosition, m_clearSpriteMovePosition, Time.deltaTime * 6.5f);
        
        if(!effectCheck)
        {
            m_HighLightScaleFactor.x = 0;
            m_HighLightScaleFactor.y = 0;
        }
    }
}
