using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ParticleHit : MonoBehaviourSingleton<ParticleHit> {

    public enum ParticleNumber
    {
        E_CLEAR,
        E_TOUCH
    }

    public List<ParticleSystem> m_particleList = new List<ParticleSystem>();

    public GameObject m_particle;
    public Camera m_camera;

    void Awake()
    {
        m_particle = GameObject.FindGameObjectWithTag("GameMgr");
        int children = m_particle.transform.childCount;
       
        for(int i =0; i<children; i++)
        {
            m_particleList.Add(m_particle.transform.GetChild(i).GetComponentInChildren<ParticleSystem>());
            
        }
    }
    public void UpdateParticle()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_particleList[0].Play();
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Ended && touch.tapCount == 1)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 6.0f));

                m_particleList[(int)ParticleNumber.E_TOUCH].transform.position = position;

                m_particleList[(int)ParticleNumber.E_TOUCH].Play();
                //Debug.Log(position);
            }
        }   
    }
}
