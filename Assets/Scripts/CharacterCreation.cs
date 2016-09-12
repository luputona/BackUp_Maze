using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

// select오브젝트가 select창에 있을경우 transform position.x = 28, y 0.2 , z -4
// scale all 1.5
// 메인으로 갈때는 포지션x = 1 
public class CharacterCreation : MonoBehaviour
{
    
    public GameObject selectChar;
    public GameObject[] m_Charbtn;
    public charInfo[] m_charinfo;
    public Text m_charInfoText;
    public List<GameObject> m_models = new List<GameObject>();
    public bool m_bRotateCheck;

    private static CharacterCreation m_instance;    
    private int m_nSelectionIndex = 0;
    private Transform tr;
    private Vector2 m_prevPoint;
    private int m_index;


    //caching
    private string m_selectCharacter = "SelectCharacter";


    public static CharacterCreation getInstance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType(typeof(CharacterCreation)) as CharacterCreation;
            }
            if (m_instance == null)
            {
                GameObject obj = new GameObject("CreateMgr");
                m_instance = obj.AddComponent(typeof(CharacterCreation)) as CharacterCreation;
            }
            return m_instance;
        }
    }
    void Awake()
    {
        
        //if (m_instance != null)
        //{
        //    GameObject.Destroy(gameObject);
        //}
        //else
        //{
        //    GameObject.DontDestroyOnLoad(gameObject);
        //    m_instance = this;
        //}
       // m_models = new List<GameObject>();

        int children = selectChar.transform.childCount;

        for (int i = 0; i < children; i++)
        {
            m_models.Add(selectChar.transform.GetChild(i).gameObject);
            m_models[i].gameObject.GetComponent<Rigidbody>().useGravity = false;
            m_models[i].gameObject.SetActive(false);
            //m_models[i].gameObject.GetComponent<Motor>().enabled = false;
            m_models[i].gameObject.GetComponent<ThirdPersonCharacter>().enabled = false;
            m_models[i].gameObject.GetComponent<ThirdPersonUserControl>().enabled = false;
        }

        for (int i = 0; i < LoadCharacterData.getInstance.m_charlist.Count; i++)
        {
            m_Charbtn[i].GetComponentInChildren<Text>().text = string.Format("{0}", LoadCharacterData.getInstance.m_charlist[i].m_CharName);
            m_Charbtn[i].GetComponentInChildren<Image>().sprite = LoadCharacterData.getInstance.m_charlist[i].m_thumbnail;
        }
        m_charInfoText.text = string.Format("Name : {0}\nSpeed : {1}\nBoost : {2}\nMaxStamina : {3}\nUseStamina : {4}\nRecoveryStamina : {5}",
            LoadCharacterData.getInstance.m_charlist[PlayerPrefs.GetInt(m_selectCharacter)].m_CharName,
            LoadCharacterData.getInstance.m_charlist[PlayerPrefs.GetInt(m_selectCharacter)].m_fSpeed,
            LoadCharacterData.getInstance.m_charlist[PlayerPrefs.GetInt(m_selectCharacter)].m_fBoost,
            LoadCharacterData.getInstance.m_charlist[PlayerPrefs.GetInt(m_selectCharacter)].m_maxStamina,
            LoadCharacterData.getInstance.m_charlist[PlayerPrefs.GetInt(m_selectCharacter)].m_useStamina,
            LoadCharacterData.getInstance.m_charlist[PlayerPrefs.GetInt(m_selectCharacter)].m_recoveryStamina);

        m_models[PlayerPrefs.GetInt(m_selectCharacter)].SetActive(true);

        m_nSelectionIndex = PlayerPrefs.GetInt(m_selectCharacter);
    }

	// Use this for initialization
	void Start () 
    {
        m_bRotateCheck = true;               
	}

    void FixedUpdate()
    {
        TouchRotate(m_nSelectionIndex);

    }

	public void Select(int _index) //캐릭터 선택  버튼에 붙인 스크립트
    {
        m_index = _index;
        if (_index.Equals(m_nSelectionIndex))
        {
            return;
        }
        if (_index < 0 || _index >= m_models.Count)
        {
            return;
        }

        m_models[m_nSelectionIndex].SetActive(false);
        m_nSelectionIndex = _index;
        m_models[m_nSelectionIndex].SetActive(true);

        m_charInfoText.text = string.Format("Name : {0}\nSpeed : {1}\nBoost : {2}\nMaxStamina : {3}\nUseStamina : {4}\nRecoveryStamina : {5}",
            LoadCharacterData.getInstance.m_charlist[_index].m_CharName,
            LoadCharacterData.getInstance.m_charlist[_index].m_fSpeed,
            LoadCharacterData.getInstance.m_charlist[_index].m_fBoost,
            LoadCharacterData.getInstance.m_charlist[_index].m_maxStamina,
            LoadCharacterData.getInstance.m_charlist[_index].m_useStamina,
            LoadCharacterData.getInstance.m_charlist[_index].m_recoveryStamina);

        PlayerPrefs.SetInt(m_selectCharacter, _index);
    }

    public void ChangeSelectCharPosition(bool _check)
    {
        if(_check)
        {
            //selectChar.gameObject.transform.position = new Vector3(1f, 0, 0);
            m_models[0].transform.localPosition = new Vector3(0, 0, 0);
            m_models[1].transform.localPosition = new Vector3(0, 0.65f, 0);
            m_models[2].transform.localPosition = new Vector3(0, 0, 0);
            m_models[3].transform.localPosition = new Vector3(0, 0, 0);
            m_models[4].transform.localPosition = new Vector3(0, 0.8f, 0);
            m_models[5].transform.localPosition = new Vector3(0, 0.73f, 0);
            m_models[6].transform.localPosition = new Vector3(0, 0.76f, 0);
            m_models[7].transform.localPosition = new Vector3(0, 0.0f, 0);
            m_models[8].transform.localPosition = new Vector3(0, 1f, 0);

            m_models[8].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            for(int i = 0; i < m_models.Count; i++)
            {
                m_models[i].transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180f, 0.0f));
            }

        }
        else if(!_check)
        {
            //selectChar.gameObject.transform.position = new Vector3(28.0f, 0.2f, -0.4f);   
            
            m_models[0].transform.localPosition = new Vector3(18f, 0, -4f);
            m_models[1].transform.localPosition = new Vector3(18f, 0.4f, -4f);
            m_models[2].transform.localPosition = new Vector3(18f, 0, -4f);
            for (int i = 3; i < 7; i++)
            {
                m_models[i].transform.localPosition = new Vector3(18f, 0.5f, -4f);
            }
            m_models[7].transform.localPosition = new Vector3(18f, 0, -4f);
            m_models[8].transform.localPosition = new Vector3(18f, 0, -4f);

            for (int i = 0; i < m_models.Count; i++)
            {
                m_models[i].transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180f, 0.0f));
            }

            m_models[7].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            m_models[8].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        }
    }
    void TouchRotate(int _select)
    {
        
        if (m_bRotateCheck)
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase.Equals(TouchPhase.Began))
                {
                    m_prevPoint = Input.GetTouch(0).position;
                }
                if (Input.GetTouch(0).phase.Equals(TouchPhase.Moved))
                {
                    m_models[_select].gameObject.transform.Rotate(0, -(Input.GetTouch(0).position.x - m_prevPoint.x) * Time.deltaTime * 7f, 0);

                    m_prevPoint = Input.GetTouch(0).position;
                }
            }
            else
            {
                m_models[_select].gameObject.transform.Translate(0, 0, 0);
            }
        }
        
    }

    public void DisableDisplay()
    {
        m_models[PlayerPrefs.GetInt(m_selectCharacter)].SetActive(false);
        //selectChar.gameObject.SetActive(false);
    }
    public void EnableDispaly()
    {
        m_models[PlayerPrefs.GetInt(m_selectCharacter)].SetActive(true);
        //selectChar.gameObject.SetActive(true);
    }
}
