using UnityEngine;
using System.Collections;

public class PersonCamera : MonoBehaviour {

    public VirtualJoystick m_cameraJoystick;

    public Transform m_lookAt;

    private float m_distance = 10.0f;
    private float m_currentX = 0.0f;
    private float m_currentY = 0.0f;
    private float m_sensivityX = 3.0f;
    private float m_sensivityY = 1.0f;
	
	// Update is called once per frame
    //void Update () 
    //{
    //    m_currentX += m_cameraJoystick.InputDirection.x * m_sensivityX;
    //    m_currentY += m_cameraJoystick.InputDirection.z * m_sensivityY;
    //}

    //void LateUpdate()
    //{
    //    Vector3 dir = new Vector3(0, 0, -m_distance);
    //    Quaternion rotation = Quaternion.Euler(m_currentY, m_currentX, 0);
    //    transform.position = m_lookAt.position + rotation * dir;
    //    transform.LookAt(m_lookAt);
    //}
}
