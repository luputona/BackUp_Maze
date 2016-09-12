using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;

public class ScrollRectSnap_CS : MonoBehaviour 
{
    public enum RECORD_LABEL
    {
        E_RECORDING_TIME_TEXT,
        E_RECORDING_SCORE_TEXT
    }

    public Text[] m_recordingText;

	// Public Variables
	public RectTransform m_panel;	// To hold the ScrollPanel
	public Button[] m_btn;
	public RectTransform m_center;	// Center to compare the distance for each button
    public Vector2 m_vHighLightScale;
    public Vector2 m_vDefaultScale;
    public Vector2 m_vBackScale;
    public float m_fHighLightScaleFactor;

    //데이터 체크 Data Check
    public int[] m_nLog;
    public int m_increaseIndex;
    public int m_decreaseIndex;
    public bool m_bMoveCheck;
    public int m_nCount;
    public bool m_bIncreaseCheck =false;
    public bool m_bDecreaseCheck =false;
    public MapInfo[] m_mapinfo;
    public Sprite[] m_sprite;
    public int m_mapNumber;
    public Button m_playBtn;

    public Text[] m_MapNameText;
    public GameObject[] m_mapImage;
    public Image[] m_lockedimage;

    //cache Getcomponent 
    [SerializeField]
    private RectTransform[] m_btnRect;
    [SerializeField]
    private Image[] m_btnImage;

	// Private Variables
    [SerializeField]
	private float[] m_fDistance;	// All buttons' distance to the center
    [SerializeField]
	private float[] m_fDistReposition;
    
	private bool m_bDragging = false;	// Will be true, while we drag the panel
	private int m_bttnDistance;	// Will hold the distance between the buttons
	private int m_minButtonNum;	// To hold the number of the button, with smallest distance to center
	private int m_bttnLength;
    private int m_stageSceneNumber;
    private string m_stage = "Stage";

	void Start()
	{
        //이미지 동적변경 방법 
        //sprite = Resources.Load<Sprite>("Images/mainBG_01");
        //m_btn[0].GetComponent<Image>().overrideSprite = sprite;
        //Debug.Log(sprite);

       
        m_bIncreaseCheck = false;
        m_bDecreaseCheck = false;

        m_nCount = 0;

        m_bMoveCheck = false;
        m_increaseIndex = 0;        

        m_bttnLength = m_btn.Length;
        m_fDistance = new float[m_bttnLength];
        m_fDistReposition = new float[m_bttnLength];

        m_nLog = new int[m_bttnLength];
        m_mapinfo = new MapInfo[m_bttnLength];
        m_btnRect = new RectTransform[m_bttnLength];
        m_btnImage = new Image[m_bttnLength];

        m_fHighLightScaleFactor = 2.0f;
		// Get distance between buttons

        for (int i = 0; i < m_btn.Length; i++ )
        {
            m_btnRect[i] = m_btn[i].GetComponent<RectTransform>();
            m_btnImage[i] = m_btn[i].GetComponent<Image>();
        }

        m_bttnDistance = (int)Mathf.Abs(m_btnRect[1].anchoredPosition.x - m_btnRect[0].anchoredPosition.x);

        for (int i = 0; i < m_btn.Length; i++)
        {
            m_vDefaultScale = new Vector2(1, 1);

            if( i < 4 )
            {
                m_mapinfo[i] = LoadMapData.getInstance.m_MapInfoList[m_increaseIndex];
                m_btn[i].GetComponentInChildren<Text>().text = string.Format("{0} : {1}", m_btn[i].name, m_mapinfo[i].m_ID.ToString());
                m_increaseIndex++;
                m_decreaseIndex = m_increaseIndex;
            }
            if( i > 3)
            {
                m_mapinfo[i] = LoadMapData.getInstance.m_MapInfoList[LoadMapData.getInstance.m_MapInfoList.Count - m_decreaseIndex + 1];
                m_btn[i].GetComponentInChildren<Text>().text = string.Format("{0} : {1}", m_btn[i].name, m_mapinfo[i].m_ID.ToString());
                m_decreaseIndex--;                
            }            
        }
        MapInfo(m_nCount);

        m_increaseIndex = m_increaseIndex - 1;
        m_decreaseIndex = 0;

        m_vHighLightScale = m_vDefaultScale * m_fHighLightScaleFactor;

        m_vBackScale = m_vDefaultScale / 2.0f;
	}

	void FixedUpdate()
	{        
        updateUI();        
	}

	void LerpToBttn(float position)
	{
        float newX = Mathf.Floor( Mathf.Lerp(m_panel.anchoredPosition.x, position, Time.deltaTime * 30f)/ 0.001f) * 0.001f ;
        Vector2 newPosition = new Vector2(newX, m_panel.anchoredPosition.y);

        m_panel.anchoredPosition = newPosition;
	}
    
    public void PlayBtn()
    {
        PlayerPrefs.SetInt("CurStageNum",m_mapNumber);
        PlayerPrefs.Save();
        SceneMgr.GetInstance.LoadLevelScene(m_stageSceneNumber);
        for(int i = 0; i < CharacterCreation.getInstance.m_models.Count; i++)
        {
            //CharacterCreation.getInstance.m_models[i].gameObject.GetComponent<Motor>().enabled = true;
            CharacterCreation.getInstance.m_models[i].gameObject.GetComponent<Rigidbody>().useGravity = true;
            CharacterCreation.getInstance.m_models[i].gameObject.GetComponent<ThirdPersonCharacter>().enabled = true;
            CharacterCreation.getInstance.m_models[i].gameObject.GetComponent<ThirdPersonUserControl>().enabled = true;
            CharacterCreation.getInstance.DisableDisplay();

        }
        
        //CharacterCreation.getInstance.EnableDispaly();
        CharacterCreation.getInstance.m_bRotateCheck = false;

        SoundManager.getInstance.StopBgm();

        //Motor.getInstance.m_bSceneCheck = true;

    }
    
    MapInfo GetLoadMapInfo(int _mapNumber)
    {
        return LoadMapData.getInstance.m_MapInfoList[_mapNumber];
    }

    void PlayBtnCheck(int _mapNumber)
    {
        
        m_MapNameText[0].text = string.Format("{0}", GetLoadMapInfo(_mapNumber).m_MapName);
        m_MapNameText[1].text = string.Format("{0}", GetLoadMapInfo(_mapNumber).m_Difficulty);
        m_recordingText[(int)RECORD_LABEL.E_RECORDING_TIME_TEXT].text = string.Format("{0} : {1:00}", (int)(PlayerPrefs.GetFloat(string.Format("{0}{1}", GetLoadMapInfo(_mapNumber).m_MapName, GetLoadMapInfo(_mapNumber).m_ID)) / 60), (int)(PlayerPrefs.GetFloat(string.Format("{0}{1}", GetLoadMapInfo(_mapNumber).m_MapName, GetLoadMapInfo(_mapNumber).m_ID)) % 60));

        m_recordingText[(int)RECORD_LABEL.E_RECORDING_SCORE_TEXT].text = string.Format("{0}",PlayerPrefs.GetInt(string.Format("{0}",GetLoadMapInfo(_mapNumber).m_MapName)));

        m_mapImage[0].GetComponent<Image>().sprite = LoadMapData.getInstance.m_MapInfoList[_mapNumber].m_BackGround;
        m_stageSceneNumber = LoadMapData.getInstance.m_MapInfoList[_mapNumber].m_MapNumber + SceneMgr.GetInstance.m_orderScene;
        m_mapNumber = _mapNumber;

        if (!UnLockMap.GetInstance.LoadUnlockCheck(m_stage, _mapNumber))
        {
            m_playBtn.interactable = false;
        }
        else if (UnLockMap.GetInstance.LoadUnlockCheck(m_stage, _mapNumber))
        {
            m_playBtn.interactable = true;
        }
    }

    public void MapInfo(int _mapNumber)
    {
        switch(_mapNumber)
        {
            case 0 :
                PlayBtnCheck(_mapNumber);
                break;
            case 1:
                PlayBtnCheck(_mapNumber);
                break;
            case 2:
                PlayBtnCheck(_mapNumber);
                break;
            case 3:
                PlayBtnCheck(_mapNumber);
                break;
            case 4:
                PlayBtnCheck(_mapNumber);
                break;
            case 5:
                PlayBtnCheck(_mapNumber);
                break;
            case 6:
                PlayBtnCheck(_mapNumber);
                break;
            case 7:
                PlayBtnCheck(_mapNumber);
                break;
            case 8:
                PlayBtnCheck(_mapNumber);
                break;
            case 9:
                PlayBtnCheck(_mapNumber);
                break;
            case 10:
                PlayBtnCheck(_mapNumber);
                break;
        }
    }

	public void StartDrag()
	{
        m_bDragging = true;
        m_bMoveCheck = true;
	}

	public void EndDrag()
	{
        m_bDragging = false;
        m_bMoveCheck = false;
	}

    public void SelectionMap(string _select)
    {
        if (_select.Equals("pre") )
        {
            m_panel.anchoredPosition += new Vector2(300.0f, 0.0f);
            m_bMoveCheck = true;
        }
        else if(_select.Equals("next"))
        {
            m_panel.anchoredPosition -= new Vector2(300.0f, 0.0f);
        }
    }

    void UnlockedCheck(int _btnNum , int _mapinfoNumber)
    {
        if (!UnLockMap.GetInstance.LoadUnlockCheck(m_stage, _mapinfoNumber))
        {
            m_lockedimage[_btnNum].enabled = true;
        }
        else if (UnLockMap.GetInstance.LoadUnlockCheck(m_stage, _mapinfoNumber))
        {
            m_lockedimage[_btnNum].enabled = false;
        }
    }

    void updateUI()
    {
        for (int i = 0; i < m_btn.Length; i++)
        {
            m_fDistReposition[i] = Mathf.Floor(((m_center.position.x - m_btnRect[i].position.x) / 0.001f)) * 0.001f;
            m_fDistance[i] = Mathf.Abs(m_fDistReposition[i]);

            m_btn[i].GetComponentInChildren<Text>().text = string.Format("{0} : {1} : \n{2}", m_btn[i].name, m_mapinfo[i].m_MapName, m_mapinfo[i].m_MapNumber); //단순 출력
            //m_nLog[i] = m_mapinfo[i].m_MapNumber;

            //m_mapImage[0].GetComponent<Image>().sprite = LoadMapData.getInstance.m_MapInfoList[_mapNumber].m_Thumbnail;
            m_btn[i].GetComponent<Image>().sprite = m_mapinfo[i].m_Thumbnail;

            UnlockedCheck(i, m_mapinfo[i].m_MapNumber);
            MapInfo( m_nCount);
            
            if (m_fDistReposition[i] > 5) //오른쪽
            {
                float curX = m_btnRect[i].anchoredPosition.x;
                float curY = m_btnRect[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (m_bttnLength * m_bttnDistance), curY);
                m_btnRect[i].anchoredPosition = newAnchoredPos;
                
                m_bIncreaseCheck = true;

                m_nCount++;
                if (m_nCount > LoadMapData.getInstance.m_MapInfoList.Count - 1)
                {
                    m_nCount = 0;
                }
                if (m_nCount <= LoadMapData.getInstance.m_MapInfoList.Count - 4)
                {                    
                    m_increaseIndex = m_nCount + 3;
                }
                else if (m_nCount >= LoadMapData.getInstance.m_MapInfoList.Count - 3 && m_nCount < LoadMapData.getInstance.m_MapInfoList.Count)
                {
                    m_increaseIndex = m_nCount - (LoadMapData.getInstance.m_MapInfoList.Count - 3);
                }
                
            }
            else
            {
                m_bIncreaseCheck = false;
            }

            if (m_fDistReposition[i] < -5) //왼쪽, 좌표 초기화때문에 처음 3번실행됨 
            {
                float curX = m_btnRect[i].anchoredPosition.x;
                float curY = m_btnRect[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX - (m_bttnLength * m_bttnDistance), curY);
                m_btnRect[i].anchoredPosition = newAnchoredPos;

                // index 감소문  

                if (m_bMoveCheck == true)
                {
                    m_bDecreaseCheck = true;

                    m_nCount--;

                    if (m_nCount < 0)
                    {
                        m_nCount = LoadMapData.getInstance.m_MapInfoList.Count - 1;
                    }
                    if (m_nCount >= 3)
                    {
                        m_decreaseIndex = m_nCount - 3;
                    }          
                    else if (m_nCount <= 2)
                    {
                        m_decreaseIndex = m_nCount + (LoadMapData.getInstance.m_MapInfoList.Count - 3);
                    }
                }
            }
            else
            {
                m_bDecreaseCheck = false;
            }

            if(m_bIncreaseCheck == true)
            {
                m_mapinfo[i] = LoadMapData.getInstance.m_MapInfoList[m_increaseIndex]; // 리스트 번호에 따른 버튼배열의 갱신 
            }
            if(m_bDecreaseCheck == true)
            {
                m_mapinfo[i] = LoadMapData.getInstance.m_MapInfoList[m_decreaseIndex];    // 리스트 번호에 따른 버튼배열의 갱신             
            }

            if (m_fDistReposition[i] <= 1 && m_fDistReposition[i] >= -1)
            {
                m_btnRect[i].localScale = Vector2.Lerp(m_btnRect[i].localScale, m_vHighLightScale, Time.deltaTime * 10);
            }
            else if (m_fDistReposition[i] > 2.5f || m_fDistReposition[i] < -2.5f)
            {
                m_btnRect[i].localScale = Vector2.Lerp(m_btnRect[i].localScale, m_vBackScale, Time.deltaTime * 20);
            }
            else
            {
                m_btnRect[i].localScale = Vector2.Lerp(m_btnRect[i].localScale, m_vDefaultScale, Time.deltaTime * 20);
            }

            if (m_fDistReposition[i] < 4f && m_fDistReposition[i] > -4f)
            {
                m_btnImage[i].enabled = true;
                m_btn[i].enabled = true;
                m_btn[i].GetComponentInChildren<Text>().enabled = true;
            }
            else
            {
                m_btnImage[i].enabled = false;
                m_btn[i].enabled = false;
                m_btn[i].GetComponentInChildren<Text>().enabled = false;
                m_lockedimage[i].enabled = false;
            }
        }

        float minDistance = Mathf.Min(m_fDistance);	// Get the min distance

        for (int a = 0; a < m_btn.Length; a++)
        {
            if (minDistance == m_fDistance[a])
            {
                m_minButtonNum = a;
            }
        }

        if (!m_bDragging )
        {
            //	LerpToBttn(minButtonNum * -bttnDistance);
            LerpToBttn(-m_btnRect[m_minButtonNum].anchoredPosition.x);
            m_bMoveCheck = true;
        }
    }
}













