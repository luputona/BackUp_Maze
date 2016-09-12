using UnityEngine;
using System.Collections;

public class CoroutineSingleton : MonoBehaviour
{
    private static CoroutineSingleton m_instance;

    public static CoroutineSingleton GetInstance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType(typeof(CoroutineSingleton)) as CoroutineSingleton;
            }
            if (m_instance == null)
            {
                GameObject obj = new GameObject("CoroutineSingleton");
                m_instance = obj.AddComponent(typeof(CoroutineSingleton)) as CoroutineSingleton;
            }
            return m_instance;
        }
    }
  
    public float m_second;
    public  WaitForSeconds m_waitSec;


    public WaitForSeconds SetSecond(float second)
    {
        m_waitSec = new WaitForSeconds(second);

        return m_waitSec;
    }

}
