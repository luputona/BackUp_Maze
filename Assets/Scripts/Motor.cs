using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour {

  //  public float m_moveSpeed = 150.0f;
  //  public float m_drag = 0.5f;
  //  public float m_terminalRotationSpeed = 25.0f;

   // public GameObject m_VirtualJoystickContainer;
   // public VirtualJoystick moveJoystick;

    public bool m_bSceneCheck = false;

    private Rigidbody m_controller;

    private static Motor m_instance;
    //public static Motor getInstance
    //{
    //    get
    //    {
    //        if (m_instance == null)
    //        {
    //            m_instance = GameObject.FindObjectOfType(typeof(Motor)) as Motor;
    //        }
    //        if (m_instance == null)
    //        {
    //            GameObject obj = new GameObject("Motor");
    //            m_instance = obj.AddComponent(typeof(Motor)) as Motor;
    //        }
    //        return m_instance;
    //    }
    //}

    void Start()
    {
        //this.m_VirtualJoystickContainer = FieldCharacterSpawn.getInstance.m_VirtualJoystickContainer;
        //this.moveJoystick = FieldCharacterSpawn.getInstance.moveJoystick;

       // m_controller = GetComponent<Rigidbody>();
       // m_controller.maxAngularVelocity = m_terminalRotationSpeed;
       // m_controller.drag = m_drag;

        //tr = GameObject.FindGameObjectWithTag("unitychan").GetComponent<Transform>();
    }

    //void Update()
    //{

    //    //Vector3 dir = Vector3.zero;
    //    //dir.x = Input.GetAxis("Horizontal");
    //    //dir.z = Input.GetAxis("Vertical");

    //    //if(dir.magnitude > 1)
    //    //{
    //    //    dir.Normalize();
    //    //}

    //    //if(moveJoystick.InputDirection != Vector3.zero)
    //    //{
    //    //    dir = moveJoystick.InputDirection;
    //    //}

    //    //m_controller.AddForce(dir * m_moveSpeed);

    //    Move();
    //}

    //void Move()
    //{
        
    //    FieldCharacterSpawn.getInstance.MotorMgr(m_controller, m_moveSpeed );
        
    //}
}
