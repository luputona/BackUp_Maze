using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;


public class FieldManager : MonoBehaviour {
    
    public enum SCORE_RESULT
    {
        E_TIME,
        E_CLEAR_SCORE,
        E_TIME_BONUS,
        E_CLEAR_BONUS,
        E_TOTAL,
        E_TIME_LABEL,
        E_CLEAR_LABEL,
        E_TIME_BONUS_LABEL,
        E_CLEAR_BONUS_LABEL,
        E_TOTAL_LABEL,       
        E_NEW_TOTAL_SPRITE,
        E_NEWTIME_SPRITE
    }

    //minimap 
    public enum MINIMAP
    {
        E_MINIMAP_UI_ICON,
        E_MINIMAP_OBJECT
    }
    public GameObject[] m_minimap_Obj = new GameObject[2];
    public Camera m_minimapCamera;
    

    //get item ui
    public UISprite m_getItem_UI;
    public UILabel m_getItem_Label;

    public GameObject m_key_UI_BG;

    //play state ui
    public UIProgressBar staminaBar;
    public UILabel m_staminaLabel;
    public UILabel m_timeLabel;
    public GameObject m_resultSprite;
    public List<GameObject> m_resulteChild = new List<GameObject>();
    public GameObject m_boosterPivot;
    public GameObject m_gameController;
    //public GameObject m_controllAnchor;
    public GameObject m_clearMenu;
    public GameObject m_State_UI_Panel;
    public bool m_buttonCheck = false; // boost btn input check
    public float m_boostPivotRot = 40.0f;
    public float m_currentPlayTime;
    public float m_currentStamina;

    private bool m_crouchCheck = false;
    private Quaternion Right = Quaternion.identity;
    private Vector2 m_resultUIPosition;
    private Vector3 m_clearUIPosition;
    private Vector2 m_resultLabelPos;
    private CanvasGroup canvasGroup;
    private float m_stopTime;

    //player 
    public GameObject targetObj;
    public static Transform m_targetObj;

    //camera 
    public GameObject m_cameraRig;
    public GameObject m_camera;
    public GameObject m_goalObj;
    
    //Getcomponent caching
    [SerializeField]
    private ThirdPersonCharacter m_thirdPersonCharacter;
    [SerializeField]
    private ThirdPersonUserControl m_thirdPersonUserControl;
    [SerializeField]
    private Animator m_targetObjAnim;
    [SerializeField]
    private CameraMotor m_caching_CameraRigCaching;
    [SerializeField]
    private CameraCollision m_caching_camera;
    [SerializeField]
    private UILabel[] m_resultLabel = new UILabel[6];
    [SerializeField]
    private ItemColliderCheck m_itemCollCheck;

    //vector, transform caching


    //string caching
    private string m_selectCharacter = "SelectCharacter";
    private string m_strStamina;

    //int caching
    private int m_playTime_min;
    private int m_playTime_sec;

    //PlayerPrefs caching
    private int m_playerPrefs;


    //field
    
    public GameObject m_mapObject;
    public GameObject m_keyObject;
    public GameObject m_torch_Object;
    public MapRader m_mapRader;    
    public BoxCollider m_stage_Goal_coll;
    
    public float m_backupSpeed;
    public float m_backupAnimSpeed;
    public float m_backupForwardAmount;
    
    //public GameObject spawnposition;    
    //public GameObject m_VirtualJoystickContainer;
    //public VirtualJoystick moveJoystick;
    //private Transform camTranform;
    //private static FieldCharacterSpawn m_instance;
    //public static FieldCharacterSpawn getInstance
    //{
    //    get
    //    {
    //        if (m_instance == null)
    //        {
    //            m_instance = GameObject.FindObjectOfType(typeof(FieldCharacterSpawn)) as FieldCharacterSpawn;
    //        }
    //        if (m_instance == null)
    //        {
    //            GameObject obj = new GameObject("spawn");
    //            m_instance = obj.AddComponent(typeof(FieldCharacterSpawn)) as FieldCharacterSpawn;
    //        }
    //        return m_instance;
    //    }
    //}

	// Use this for initialization
    void Awake()
    {

        //CharacterCreation.getInstance.m_bRotateCheck = false;
        InitializeSpawnPosition();
        m_gameController = GameObject.FindGameObjectWithTag("GameController");
        m_gameController.SetActive(true);
        m_State_UI_Panel = GameObject.FindGameObjectWithTag("State_UI_Panel");
        //SoundManager.getInstance.PlayBgm("CM_DUNGEON_BGM3");
        m_clearMenu.SetActive(false);
    }

	void Start () 
    {
        //m_VirtualJoystickContainer = GameObject.Find("VirtualJoystickContainer");
        //moveJoystick = m_VirtualJoystickContainer.gameObject.GetComponent<VirtualJoystick>();

        //camTranform = Camera.main.transform;
        m_currentStamina = LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_maxStamina;

        InitCachingComponent();

        Right.eulerAngles = new Vector3(0, 180.0f, 0);

        int children = m_resultSprite.transform.childCount;
        
        for (int i = 0; i < children; i++ )
        {
            m_resulteChild.Add(m_resultSprite.transform.GetChild(i).gameObject);
        }
        m_resultSprite.SetActive(false);
        for (int i = 0; i <= 5; i++)
        {
            m_resultLabel[i] = m_resulteChild[i].GetComponentInChildren<UILabel>();
        }
        m_resulteChild[(int)SCORE_RESULT.E_NEW_TOTAL_SPRITE].SetActive(false);
        m_resulteChild[(int)SCORE_RESULT.E_NEWTIME_SPRITE].SetActive(false);

        InitializeItemUI();
        
        m_resultUIPosition.x = 510.0f;
        m_resultUIPosition.y = 0.0f;
        m_currentPlayTime = 0.0f;
        m_resultLabelPos.x = -220.0f;

       
        m_stage_Goal_coll.isTrigger = false;
	}

    void Update()
    {
        m_mapRader.DrawMapIcons();
        Timer();
    }

    void FixedUpdate()
    {
        
        if(StageClearManager.GetInstance.check)
        {
            StartCoroutine(ClearCameraRotMotion());
            StartCoroutine(ClearUITimer());
            StartCoroutine(DisplayResultUI());                        
        }
        GetBoost();       
    }

    void Timer()
    {
        m_currentPlayTime += Time.deltaTime * m_stopTime;
        ScoreManager.GetInstance.CalculateScoring();

        if (!StageClearManager.GetInstance.check && !StageClearManager.GetInstance.isTimer && !StageClearManager.GetInstance.effectCheck)
        {
            m_stopTime = 1.0f;
        }
        else
        {
            m_stopTime = 0.0f;
        }
        
        ScoreManager.GetInstance.m_getTime = m_currentPlayTime;
        m_playTime_min = (int)m_currentPlayTime / 60;
        m_playTime_sec = (int)m_currentPlayTime % 60;
        m_timeLabel.text = string.Format("{0}:{1:00}", m_playTime_min , m_playTime_sec);
    }

    IEnumerator ClearUITimer()
    {
        //m_clearUIPosition = m_cameraRig.transform.position;
        m_clearUIPosition.x = m_cameraRig.transform.localPosition.x - 0.3f;
        if(StageClearManager.GetInstance.isTimer)
        {
            yield return Yielders.Get(3.0f);
            m_cameraRig.transform.position = Vector3.Lerp(m_cameraRig.transform.position, m_clearUIPosition, -1.5f);
            StartCoroutine(StageClearManager.GetInstance.DisplayClearSprtie());
        }
        else if (StageClearManager.GetInstance.isTimer == false)
        {
            m_clearUIPosition.x = m_cameraRig.transform.localPosition.x;
        }
        yield return Yielders.Get(0.3f);
        StageClearManager.GetInstance.isTimer = false;
        
        //yield return null;
    }
    
    IEnumerator ClearCameraRotMotion()
    {
        m_targetObjAnim.SetBool("Salute", true);
        
        m_thirdPersonCharacter.m_currentSpeed = 0.0f;
        
        //targetObj.gameObject.GetComponent<ThirdPersonCharacter>().m_AnimSpeed = 0.0f;
        //targetObj.gameObject.GetComponent<ThirdPersonCharacter>().m_tempForwardAmount = 0.0f;
        //targetObj.gameObject.GetComponent<ThirdPersonCharacter>().SetForwardAmount();
        m_thirdPersonUserControl.enabled = false;

        m_caching_CameraRigCaching.enabled = false;
        //m_caching_camera.enabled = false;                

        yield return new WaitForSeconds(0.5f);
       
        Right *= Quaternion.Euler(targetObj.transform.up * 2.0f);
        m_cameraRig.transform.rotation = Quaternion.Slerp(m_cameraRig.transform.rotation, Right, Time.deltaTime * 3.0f);

        yield return new WaitForSeconds(10.5f); //나중에 화면 전환시 true로 되게 변경

        
        //m_cameraRig.GetComponent<CameraMotor>().enabled = true;
        //m_camera.GetComponent<CameraCollision>().enabled = true;
        //targetObj.gameObject.GetComponent<ThirdPersonUserControl>().enabled = true;
        //targetObj.gameObject.GetComponent<Animator>().SetBool("Salute", false);

        //targetObj.gameObject.GetComponent<ThirdPersonCharacter>().m_currentSpeed = m_backupSpeed;
        //targetObj.gameObject.GetComponent<ThirdPersonCharacter>().m_AnimSpeed = m_backupAnimSpeed;
        //targetObj.gameObject.GetComponent<ThirdPersonCharacter>().m_tempForwardAmount = m_backupForwardAmount;
        StageClearManager.GetInstance.check = false;
    }
	
    IEnumerator DisplayResultUI()
    {
        m_State_UI_Panel.SetActive(false);
        
        m_resultSprite.SetActive(true);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 0.5f;
        }
        
        yield return Yielders.Get(5.5f);
        canvasGroup.interactable = false;        

        m_resultSprite.transform.localPosition = Vector2.Lerp(m_resultSprite.transform.localPosition, m_resultUIPosition, Time.deltaTime * 5.0f);
        m_clearMenu.SetActive(true);
                
        yield return Yielders.Get(1.0f);
        m_resulteChild[(int)SCORE_RESULT.E_TIME].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_TIME].transform.localPosition, new Vector3(0,150.0f,0), Time.deltaTime * 5.0f);
        yield return Yielders.Get(0.30f);
        m_resulteChild[(int)SCORE_RESULT.E_CLEAR_SCORE].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_CLEAR_SCORE].transform.localPosition, new Vector3(0,0,0) , Time.deltaTime * 7.0f);
        yield return Yielders.Get(0.20f);
        m_resulteChild[(int)SCORE_RESULT.E_TIME_BONUS].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_TIME_BONUS].transform.localPosition, new Vector3(0, -145.0f, 0), Time.deltaTime * 7.0f);
        yield return Yielders.Get(0.20f);
        m_resulteChild[(int)SCORE_RESULT.E_CLEAR_BONUS].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_CLEAR_BONUS].transform.localPosition, new Vector3(0, -285.0f, 0), Time.deltaTime * 7.0f);
        yield return Yielders.Get(0.20f);
        m_resulteChild[(int)SCORE_RESULT.E_TOTAL].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_TOTAL].transform.localPosition, new Vector3(0, -435.0f, 0), Time.deltaTime * 7.0f);

        yield return Yielders.Get(0.1f);
        m_resultLabelPos.y = 160.0f;
        m_resulteChild[(int)SCORE_RESULT.E_TIME_LABEL].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_TIME_LABEL].transform.localPosition, m_resultLabelPos,Time.deltaTime * 7.0f);
        yield return Yielders.Get(0.1f);
        m_resultLabelPos.y = 10.0f;
        m_resulteChild[(int)SCORE_RESULT.E_CLEAR_LABEL].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_CLEAR_LABEL].transform.localPosition, m_resultLabelPos, Time.deltaTime * 7.0f);
        yield return Yielders.Get(0.1f);
        m_resultLabelPos.y = -150.0f;
        m_resulteChild[(int)SCORE_RESULT.E_TIME_BONUS_LABEL].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_TIME_BONUS_LABEL].transform.localPosition, m_resultLabelPos, Time.deltaTime * 7.0f);
        yield return Yielders.Get(0.1f);
        m_resultLabelPos.y = -290.0f;
        m_resulteChild[(int)SCORE_RESULT.E_CLEAR_BONUS_LABEL].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_CLEAR_BONUS_LABEL].transform.localPosition, m_resultLabelPos, Time.deltaTime * 7.0f);
        yield return Yielders.Get(0.1f);
        m_resultLabelPos.y = -435.0f;
        m_resulteChild[(int)SCORE_RESULT.E_TOTAL_LABEL].transform.localPosition = Vector2.Lerp(m_resulteChild[(int)SCORE_RESULT.E_TOTAL_LABEL].transform.localPosition, m_resultLabelPos, Time.deltaTime * 7.0f);

        yield return Yielders.Get(0.5f);
        m_resultLabel[0].text = string.Format("{0} : {1:00}", (int)ScoreManager.GetInstance.m_getTime / 60, (int)ScoreManager.GetInstance.m_getTime % 60);

        if(ScoreManager.GetInstance.CheckTime())
        {
            m_resulteChild[(int)SCORE_RESULT.E_NEWTIME_SPRITE].SetActive(true);
        }
        else if(!ScoreManager.GetInstance.CheckTime())
        {
            m_resulteChild[(int)SCORE_RESULT.E_NEWTIME_SPRITE].SetActive(false);
        }

        yield return Yielders.Get(0.3f);
        m_resultLabel[1].text = string.Format("{0}", (int)ScoreManager.GetInstance.m_clearScore);        
        yield return Yielders.Get(0.3f);
        m_resultLabel[2].text = string.Format("{0}", (int)ScoreManager.GetInstance.m_calculateTimeBonus);        
        yield return Yielders.Get(0.3f);
        m_resultLabel[3].text = string.Format("{0}", (int)ScoreManager.GetInstance.m_calculateScoreBonus);        
        yield return Yielders.Get(0.3f);
        m_resultLabel[4].text = string.Format("{0}", (int)ScoreManager.GetInstance.m_totalScore);

        
        if (ScoreManager.GetInstance.CheckSocre())
        {
            m_resulteChild[(int)SCORE_RESULT.E_NEW_TOTAL_SPRITE].SetActive(true);
        }
        else if (!ScoreManager.GetInstance.CheckSocre())
        {
            m_resulteChild[(int)SCORE_RESULT.E_NEW_TOTAL_SPRITE].SetActive(false);
        }

    }

    void InitCachingComponent()
    {
        m_thirdPersonCharacter = targetObj.gameObject.GetComponent<ThirdPersonCharacter>();
        m_thirdPersonUserControl = targetObj.gameObject.GetComponent<ThirdPersonUserControl>();
        m_targetObjAnim = targetObj.gameObject.GetComponent<Animator>();
        m_caching_CameraRigCaching = m_cameraRig.GetComponent<CameraMotor>();
        m_caching_camera = m_camera.GetComponent<CameraCollision>();
        m_itemCollCheck = targetObj.gameObject.GetComponent<ItemColliderCheck>();

        canvasGroup = m_gameController.gameObject.GetComponent<CanvasGroup>();
        staminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<UIProgressBar>();

    }
    void InitializeItemUI()
    {
        m_itemCollCheck.m_getItemUI = m_getItem_UI;
        m_itemCollCheck.m_getItemUI_Label = m_getItem_Label;
        m_getItem_Label.gameObject.SetActive(false);
        m_getItem_UI.gameObject.SetActive(false);
        m_minimap_Obj[(int)MINIMAP.E_MINIMAP_UI_ICON].SetActive(false);
        m_minimap_Obj[(int)MINIMAP.E_MINIMAP_OBJECT].SetActive(false);
        m_minimapCamera.gameObject.SetActive(false);

        m_key_UI_BG.SetActive(false);
    }
    
	void InitializeSpawnPosition()
    {
        m_playerPrefs = PlayerPrefs.GetInt(m_selectCharacter);

        if (m_playerPrefs.Equals(0)) // unitychan
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/unitychan");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(1)) // witch
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/ChaWitch");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(2)) // maid
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/ToonMaid_Cool_1");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(3)) // jelly
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/JellyFishGirl");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(4)) //misaki
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/Misaki_sum_humanoid");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(5)) // kohaku
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/Utc_sum_humanoid");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(6)) // yuko
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/Yuko_sum_humanoid");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(7)) // thief
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/Thief1");
            InitObjPosition();
        }
        else if (m_playerPrefs.Equals(8)) //dragon
        {
            targetObj = (GameObject)Resources.Load("Prefabs/Characters/ChaDragon");
            InitObjPosition();
        }
        //var targetObj = GameObject.FindGameObjectWithTag("Player");        
        m_backupSpeed = targetObj.GetComponent<ThirdPersonCharacter>().m_currentSpeed;
        m_backupAnimSpeed = targetObj.GetComponent<ThirdPersonCharacter>().m_AnimSpeed;
        m_backupForwardAmount = targetObj.GetComponent<ThirdPersonCharacter>().m_tempForwardAmount;

    }

    void InitObjPosition()
    {
        targetObj = (GameObject)Instantiate(targetObj, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.identity);
        targetObj.gameObject.SetActive(true);
        targetObj.GetComponent<ThirdPersonCharacter>().m_currentSpeed = LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_fSpeed;
        m_targetObj = targetObj.transform;
        
    }

    public void GoToGoal()
    {
        targetObj.transform.position = m_goalObj.gameObject.transform.position;
        
        m_stage_Goal_coll.isTrigger = true;
    }

    public void PressBoostPivotRotation()
    {
        if (m_boosterPivot.transform.localRotation.z <= -0.8f)
        {
            m_boostPivotRot = -300.0f;
        }
        else
        {
            m_boostPivotRot = 40.0f;
        }
        
        m_boosterPivot.transform.RotateAround(m_boosterPivot.transform.position, Vector3.back, m_boostPivotRot * Time.deltaTime);
    }

    public void ReleaseBoostPivotRotation()
    {
        m_boostPivotRot = 40.0f;
        if(m_boosterPivot.transform.localRotation.z >= 0)
        {
            m_boostPivotRot = 0.0f;
        }
        m_boosterPivot.transform.RotateAround(m_boosterPivot.transform.position, Vector3.forward, m_boostPivotRot * Time.deltaTime);
    }

    public void PressCrouch()
    {
        //m_crouchCheck = true;
        m_thirdPersonUserControl.m_crouchCheck = true;
        
    }
    public void ReleaseCrouch()
    {
        //m_crouchCheck = false;
        m_thirdPersonUserControl.m_crouchCheck = false;        
    }

    public void GetBoost()
    {
        if (m_currentStamina != LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_maxStamina)
        {
            m_strStamina = string.Format("{0}/{1}", m_currentStamina.ToString("N0"), LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_maxStamina.ToString());
            m_staminaLabel.text = m_strStamina;
            staminaBar.value = (m_currentStamina) / (LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_maxStamina);
        }       

        if (m_buttonCheck && m_currentStamina >= 0 && !StageClearManager.GetInstance.check) // 부스터 사용
        {
            m_currentStamina -= LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_useStamina * Time.deltaTime;
            m_thirdPersonCharacter.m_currentSpeed = LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_fBoost;
            m_thirdPersonCharacter.m_AnimSpeed = 2.0f;
            
            PressBoostPivotRotation();
        }
        if (m_buttonCheck && m_currentStamina <= 0 || m_buttonCheck && StageClearManager.GetInstance.check)// 부스터 미사용
        {
            m_buttonCheck = false;
            m_currentStamina += LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_recoveryStamina * Time.deltaTime;
            m_thirdPersonCharacter.m_currentSpeed = LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_fSpeed;
            m_thirdPersonCharacter.m_AnimSpeed = 1.0f;
            
            ReleaseBoostPivotRotation();
        }
        if (!m_buttonCheck)// 부스터 미사용
        {
            m_thirdPersonCharacter.m_currentSpeed = LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_fSpeed;
            m_thirdPersonCharacter.m_AnimSpeed = 1.0f;
            
            ReleaseBoostPivotRotation();
            
        }
        if (!m_buttonCheck && m_currentStamina <= LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_maxStamina)// 부스터 미사용
        {
            m_currentStamina += LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_recoveryStamina * Time.deltaTime;
            m_thirdPersonCharacter.m_currentSpeed = LoadCharacterData.getInstance.m_charlist[m_playerPrefs].m_fSpeed;
            m_thirdPersonCharacter.m_AnimSpeed = 1.0f;
            
            ReleaseBoostPivotRotation();
        }        
    }
    public void InitBoost()
    {
        if (m_currentStamina <= 0)
        {
            m_buttonCheck = false;
        }   
        if (m_currentStamina >= 0)
        {
            m_buttonCheck = true;
        }             
    }

    public void ReleaseBoost()
    {
        m_buttonCheck = false;
    }

    public void GetItem()
    {
        if (m_itemCollCheck.GetItem() == "Minimap")
        {
            m_minimap_Obj[(int)MINIMAP.E_MINIMAP_UI_ICON].SetActive(true);
            m_mapObject.SetActive(false);
            m_getItem_UI.gameObject.SetActive(false);
            m_getItem_Label.gameObject.SetActive(false);
        }
        else if (m_itemCollCheck.GetItem() == "Key")
        {
            m_key_UI_BG.SetActive(true);            
            m_keyObject.SetActive(false);
            m_getItem_UI.gameObject.SetActive(false);
            m_getItem_Label.gameObject.SetActive(false);

            
            m_stage_Goal_coll.isTrigger = true;
        }        
    }

    public void OpenMinimap()
    {
        m_minimapCamera.gameObject.SetActive(true);
        m_minimap_Obj[(int)MINIMAP.E_MINIMAP_OBJECT].SetActive(true);
        m_State_UI_Panel.SetActive(false);
        m_torch_Object.SetActive(false);
        canvasGroup.alpha = 0;
        
    }
    public void CloseMinimap()
    {
        m_minimap_Obj[(int)MINIMAP.E_MINIMAP_OBJECT].SetActive(false);
        m_minimapCamera.gameObject.SetActive(false);
        m_State_UI_Panel.SetActive(true);
        m_torch_Object.SetActive(true);
        canvasGroup.alpha = 1;        
    }


    //public void MotorMgr(Rigidbody _controller, float _moveSpeed)
    //{
    //    //dir.x = Input.GetAxis("Horizontal");
    //    //dir.z = Input.GetAxis("Vertical");
    //    //dir.y = 0;
    //    //dir.x = moveJoystick.Horizontal();
    //    //dir.z = moveJoystick.Vertical();
    //    Vector3 dir = new Vector3(moveJoystick.Horizontal(), 0, moveJoystick.Vertical());

    //    Vector3 rotatedDir = Vector3.zero;
    //    rotatedDir.y = 0;
    //    rotatedDir = rotatedDir.normalized; // forward

    //    if (dir.sqrMagnitude > 0.001f)
    //    {
    //        rotatedDir = camTranform.TransformDirection(dir);
    //        rotatedDir.y = 0;
    //        rotatedDir.Normalize();
    //        Motor.getInstance.transform.forward = rotatedDir;
    //        dir.Normalize();
    //    }

    //    if (moveJoystick.InputDirection != Vector3.zero)
    //    {
    //        dir = moveJoystick.InputDirection;
    //    }
    //    rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);
    //    rotatedDir = rotatedDir.normalized *dir.magnitude;

    //    Quaternion rotation = Quaternion.LookRotation(rotatedDir);
    //    Quaternion tempq = Motor.getInstance.transform.rotation;

    //    Motor.getInstance.transform.rotation = Quaternion.Slerp(tempq, rotation, Time.deltaTime * 100);

    //    //Rotate our direction vector with camera        
    //    _controller.AddForce(rotatedDir * _moveSpeed);

    //}
}
