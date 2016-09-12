using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System.Text;

public class LoadMapData : MonoBehaviour 
{

    private static LoadMapData m_instance;

    public static LoadMapData getInstance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType(typeof(LoadMapData)) as LoadMapData;
            }
            if (m_instance == null)
            {
                GameObject obj = new GameObject("LoadMapMgr");
                m_instance = obj.AddComponent(typeof(LoadMapData)) as LoadMapData;
            }
            return m_instance;
        }
    }

    private JsonData m_jMapData;
    private string jsonString;
    public List<MapInfo> m_MapInfoList = new List<MapInfo>();

    public int m_oldScore;    
    public int m_oldMapID;
    public float m_oldTime;
    public string m_oldMapName;

    void Awake()
    {
        //text.text = string.Format("{0}", Application.persistentDataPath);
        if (m_instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            m_instance = this;
        }

        //안드로이드 기기 내에서 json 데이타를 불러오는 방법
        TextAsset textAsset = Resources.Load<TextAsset>("StreamingAssets/MapInfor");
        jsonString = textAsset.ToString();
        m_jMapData = JsonMapper.ToObject(jsonString);
        
        //m_jMapData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Json_Data/MapInfor.json"));
        //m_jMapData = JsonMapper.ToObject(textAsset.text);
        ConstructMapDatabase();
    }

    void Start()
    {
        //sb.AppendFormat("{0}{1}", m_oldMapName, m_oldMapID);

        for(int i = 0; i< m_MapInfoList.Count; i++)
        {            
            m_oldScore = PlayerPrefs.GetInt(m_oldMapName);
            //Debug.Log(m_MapInfoList[i].m_MapName + m_MapInfoList[i].m_ID + " : " + PlayerPrefs.GetFloat(m_MapInfoList[i].m_MapName + m_MapInfoList[i].m_ID));
        }
    }

    public MapInfo FetchAreaByID(int _id)
    {
        for(int i = 0; i < m_MapInfoList.Count; i++)
        {
            if(m_MapInfoList[i].m_ID == _id)
            {
                return m_MapInfoList[i];
            }
        }
        return null;
    }
    void ConstructMapDatabase()
    {
        for (int i = 0; i < m_jMapData.Count; i++)
        {
            m_MapInfoList.Add(new MapInfo((int)m_jMapData[i]["ID"], (int)m_jMapData[i]["MapNumber"], m_jMapData[i]["MapName"].ToString(), (int)m_jMapData[i]["MapLv"], (bool)m_jMapData[i]["Clear"], m_jMapData[i]["thumbnail"].ToString(), m_jMapData[i]["background"].ToString(), (int)m_jMapData[i]["DefaultScore"]));
        }
    }    
}

public class MapInfo
{
    
    public int m_ID { get; set; }
    public int m_MapNumber { get; set; }
    public string m_MapName { get; set; }
    public int m_Difficulty { get; set; }
    public bool m_Clear { get; set;}
    public string m_strMapBG { get; set; }
    public string m_strThumbnail { get; set; }
    public Sprite m_BackGround { get; set; }
    public Sprite m_Thumbnail { get; set; }
    public int m_score { get; set; }

    public MapInfo(int _id, int _mapnumber, string _mapname, int _dfficulty, bool _clear, string thumbnail, string background, int score)
    {
        this.m_ID = _id;
        this.m_MapNumber = _mapnumber;
        this.m_MapName = _mapname;
        this.m_Difficulty = _dfficulty;
        this.m_Clear = _clear;
        this.m_strThumbnail = thumbnail;
        this.m_strMapBG = background;
        this.m_Thumbnail = Resources.Load<Sprite>("Images/Mapthumbnail/" + thumbnail);
        this.m_BackGround = Resources.Load<Sprite>("Images/MapBG/" + background);
        this.m_score = score;
    }
    public MapInfo()
    {
        this.m_ID = -1;
    }

}
