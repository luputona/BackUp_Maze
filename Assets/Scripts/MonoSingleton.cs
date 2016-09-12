using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public int m_nInstanceID;
    public new Transform transform { get; private set; }
    public new GameObject m_gameObj { get; private set; }

    static bool m_bCancreate = true;

    private static T m_Instance = null;

    public static T GetInstance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if(m_Instance == null)
                {
                    if(m_bCancreate)
                    {
                        m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    }
                    if(m_Instance == null)
                    {
                        Debug.Log("MonoBehaviourSingleton Instance Init ERROR - " + typeof(T).ToString());
                    }
                }
            }
            else
            {
                m_Instance.Init();
            }
            return m_Instance;
        }
    }
    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        if(m_Instance == null)
        {
            transform = base.transform;
            m_gameObj = base.gameObject;
            m_nInstanceID = GetInstanceID();

            m_Instance = this as T;
        }
        else
        {
            if(m_Instance != this)
            {
                DestroyImmediate(base.gameObject);
            }
        }
    }
	
    public void OnDestory()
    {
        if(m_Instance ==this)
        {
            m_Instance = null;
        }
    }

    void OnApplicationQuit()
    {
        m_bCancreate = false;
        m_Instance = null;
    }
    
}


//public class Singleton<T> : MonoBehaviour
//{
//    private static Singleton<T> m_instance;

//    public static Singleton<T> getInstance
//    {
//        get
//        {
//            if (m_instance == null)
//            {
//                m_instance = GameObject.FindObjectOfType(typeof(Singleton<T>)) as Singleton<T>;
//            }
//            if (m_instance == null)
//            {
//                GameObject obj = new GameObject("obj");
//                m_instance = obj.AddComponent(typeof(Singleton<T>)) as Singleton<T>;
//            }
//            return m_instance;
//        }
//    }
//}