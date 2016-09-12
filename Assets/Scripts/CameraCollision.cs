using UnityEngine;
using System.Collections;

public class CameraCollision : MonoBehaviour {

    public CameraMotor m_camMotor = new CameraMotor();
    public Transform center_Point;
    public Transform mainCamera;
    public float m_distance;

    private Vector3 dest;
    
    private RaycastHit hit;

	// Use this for initialization
	void Start () 
    {
        m_distance = 2.9f;
	}

    //void Update()
    //{
    //    //center_Point.position = gameObject.transform.position + new Vector3(0, 1.5f, 0);

    //    Debug.DrawLine(center_Point.position + Vector3.up * -0.5f, transform.position, Color.green);
    //}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        dest = center_Point.position + center_Point.forward * -1.0f * m_distance;
        if (Physics.Linecast(center_Point.position + Vector3.up * -0.5f, dest, out hit))
        {            
            this.transform.position = hit.point + hit.normal * 1.0f;
        }

        this.transform.position = Vector3.Slerp(this.transform.position, dest, Time.deltaTime * 10.0f);

	}

    
}
