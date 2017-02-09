using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class LoadCharacterData : MonoBehaviour
{
    public static LoadCharacterData m_instance;

    public static LoadCharacterData getInstance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType(typeof(LoadCharacterData)) as LoadCharacterData;
            }
            if (m_instance == null)
            {
                GameObject obj = new GameObject("LoadCharMgr");
                m_instance = obj.AddComponent(typeof(LoadCharacterData)) as LoadCharacterData;
            }
            return m_instance;
        }
    }

    private JsonData m_jCharData;
    private string jsonString;

    public List<charInfo> m_charlist = new List<charInfo>();

    void Awake()
    {
        if (m_instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            m_instance = this;
        }

        TextAsset textAsset = Resources.Load<TextAsset>("StreamingAssets/CharacterData");
        jsonString = textAsset.ToString();
        m_jCharData = JsonMapper.ToObject(jsonString);




        ConstructCharDatabase();
    }

    public charInfo FetchAreaByID(int _id)
    {
        for (int i = 0; i < m_charlist.Count; i++)
        {
            if (m_charlist[i].m_id == _id)
            {
                return m_charlist[i];
            }
        }
        return null;
    }
	
	void ConstructCharDatabase()
    {
        for(int i = 0; i < m_jCharData.Count; i++)
        {
            m_charlist.Add(new charInfo((int)m_jCharData[i]["ID"],
                (int)m_jCharData[i]["CharNumber"],
                m_jCharData[i]["CharName"].ToString(),
                (int)m_jCharData[i]["Speed"],
                (int)m_jCharData[i]["Boost"],
                (int)m_jCharData[i]["MaxStamina"],
                (int)m_jCharData[i]["UseStamina"],
                (int)m_jCharData[i]["RecoveryStamina"],
                m_jCharData[i]["thumbnail"].ToString()));
        }
    }
}
public class charInfo
{
    public int m_id {get; set;}
    public int m_CharNumber { get; set; }
    public string m_CharName{ get; set;}
    public float m_fSpeed { get; set; }
    public float m_fBoost { get; set; }
    public int m_maxStamina { get; set; }
    public int m_useStamina { get; set; }
    public int m_recoveryStamina { get; set; }
    public Sprite m_thumbnail { get; set; }

    public charInfo(int _id, int _charNumber, string _charName, int _speed, int _boost, int _maxstamina, int _usestamina, int _recoveryStamina, string _thumbnail)
    {
        this.m_id = _id;
        this.m_CharNumber = _charNumber;
        this.m_CharName = _charName;
        this.m_fSpeed = (float)_speed;
        this.m_fBoost = (float)_boost;
        this.m_maxStamina = _maxstamina;
        this.m_useStamina = _usestamina;
        this.m_recoveryStamina = _recoveryStamina;
        this.m_thumbnail = Resources.Load<Sprite>("Images/CharacterThumnail/" + _thumbnail); ;
    }

    public charInfo()
    {
        this.m_id = -1;
    }
}