using UnityEngine;
using System.Collections;

public class Singleton<T> where T : class, new()
{
    private static object _syncobj = new object();
    private static volatile T _instance = null;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_syncobj)
                {
                    _instance = new T();
                }
            }
            return _instance;
        }
    }
}




public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    protected static T m_Instance = null;
    public static T GetInstance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (m_Instance == null)
                {
                    if (canCreate)
                        m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    if (m_Instance == null)
                    {
                        Debug.LogError("MonoBehaviourSingleton Instance Init ERROR - " + typeof(T).ToString());
                    }
                }
                else
                    m_Instance.Init();
            }
            return m_Instance;
        }
    }

    public virtual void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (m_Instance == null)
        {
            //base.Awake();
            transform = base.transform;
            gameObject = base.gameObject;
            InstanceID = GetInstanceID();

            //Debug.Log( "Instance Set : " + +GetInstanceID() );
            m_Instance = this as T;
        }
        else
        {
            if (m_Instance != this)
                //Debug.Log( "Instance Already : " + GetInstanceID() );
                DestroyImmediate(base.gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (m_Instance == this)
            m_Instance = null;
    }

    private void OnApplicationQuit()
    {
        canCreate = false;
        m_Instance = null;
    }

    public int InstanceID;
    public new Transform transform { get; private set; }
    public new GameObject gameObject { get; private set; }
    static bool canCreate = true;
}