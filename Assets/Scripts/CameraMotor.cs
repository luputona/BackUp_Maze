using UnityEngine;
using System.Collections;

public class CameraMotor : MonoBehaviour {

    public Transform m_lookAt;
    public VirtualJoystick m_cameraJoystick;
    public Vector3 m_offset;

    public float m_distance = 0.1f;
    //private float m_yOffset = 1.5f;

    //private float m_distance = 10.0f;
    public float m_currentX = 0.0f;
    public float m_currentY = 0.0f;
    private float m_sensivityX = 6.0f;
    private float m_sensivityY = 1.0f;
   
    public float m_rotX;
    public float m_maxheight;
    public float m_minheight;

    //caching transform
    [SerializeField]
    private Transform m_thisTransform;


	void Start () 
    {
        m_thisTransform = this.transform;
        InitCameraPositionAndRotation();
	}

    void Update()
    {
        m_currentX += m_cameraJoystick.InputDirection.x * m_sensivityX;
        m_currentY += m_cameraJoystick.InputDirection.z * m_sensivityY;

        //transform.position = m_lookAt.position + m_offset;

        m_currentY = Mathf.Clamp(m_currentY, m_minheight, m_maxheight);
    }

   
    void LateUpdate()
    {
        Quaternion rotation1 = Quaternion.Euler(m_currentY, m_currentX, 0);
        m_thisTransform.position = m_lookAt.position + rotation1 * m_offset;
        //transform.position = m_lookAt.position + rotation1 * m_offset;

        m_thisTransform.LookAt(m_lookAt);
        //transform.LookAt(m_lookAt);
        m_thisTransform.rotation *= Quaternion.Euler(m_rotX, 0, 0);
        //transform.rotation *= Quaternion.Euler(m_rotX, 0, 0);
    }

    void InitCameraPositionAndRotation()
    {
        if (PlayerPrefs.GetInt("SelectCharacter").Equals(0)) // unitychan
        {
            m_lookAt = GameObject.FindGameObjectWithTag("unitychan").gameObject.transform;
            //m_offset = new Vector3(0, 0.1f, -2.2f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.1f;
            m_offset.z = -2.2f * m_distance;
                
            m_rotX = -20.5f;
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(1)) // witch
        {
            m_lookAt = GameObject.FindGameObjectWithTag("ChaWitch").gameObject.transform;
            //m_offset = new Vector3(0, 0, -0.1f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.0f;
            m_offset.z = -0.1f * m_distance;
            m_rotX = -5.9003f;
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(2)) // maid
        {
            m_lookAt = GameObject.FindGameObjectWithTag("ToonMaid_Cool").gameObject.transform;
            //m_offset = new Vector3(0, 0, -0.2f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0f;
            m_offset.z = -0.2f * m_distance;
            m_rotX = -3.19f;
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(3)) // jelly
        {
            m_lookAt = GameObject.FindGameObjectWithTag("JellyFishGirl").gameObject.transform;
            //m_offset = new Vector3(0, 0, -0.1f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.0f;
            m_offset.z = -0.1f * m_distance;
            m_rotX = 8.5f;
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(8.2f, 0, 0));
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(4)) //misaki
        {            
            m_lookAt = GameObject.FindGameObjectWithTag("Misaki").gameObject.transform;
            //m_offset = new Vector3(0, 0 , -1f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.0f;
            m_offset.z = -1f * m_distance;
            //m_offset = new Vector3(0, 3.1f, -8.5f * m_distance);
            //m_rotX = -11.8f; //디폴트값
            m_rotX = 1.0f; 
            //m_lookAt.rotation = Quaternion.Euler(new Vector3(8.2f, 0, 0));
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(5)) // kohaku
        {
            m_lookAt = GameObject.FindGameObjectWithTag("Utc").gameObject.transform;
            //m_offset = new Vector3(0, 0, -1.0f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.0f;
            m_offset.z = -1.0f * m_distance;
            m_rotX = 1.0f;            
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(6)) // yuko
        {
            m_lookAt = GameObject.FindGameObjectWithTag("Yuko").gameObject.transform;
            //m_offset = new Vector3(0, 0, -1.0f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.0f;
            m_offset.z = -1.0f * m_distance;
            m_rotX = 1.0f;            
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(7)) // thief
        {
            m_lookAt = GameObject.FindGameObjectWithTag("Thief").gameObject.transform;
            //m_offset = new Vector3(0, 0, -1.0f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.0f;
            m_offset.z = -1.0f * m_distance;
            m_rotX = -5.48f;
        }
        else if (PlayerPrefs.GetInt("SelectCharacter").Equals(8)) //dragon
        {
            m_lookAt = GameObject.FindGameObjectWithTag("ChaDragon").gameObject.transform;
            //m_offset = new Vector3(0, 0, -1f * m_distance);
            m_offset.x = 0.0f;
            m_offset.y = 0.0f;
            m_offset.z = -1.0f * m_distance;
            m_rotX = 3.0f;
        }
    }
}
