using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;

    public float smooth = 0.05f;

    public CameraMotor camMotor = new CameraMotor();
    public CollisionHandler collision = new CollisionHandler();
    public DebugSettings debug = new DebugSettings();

    private Vector3 destination = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 adjustedDestination = Vector3.zero;
    private Vector3 camVel = Vector3.zero;
    private bool smoothFollow = true;

    

    void Start()
    {
        target = camMotor.m_lookAt;
        targetPos = camMotor.m_lookAt.position;

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        //draw debug lines
        for(int i = 0; i<5; i++)
        {
            if(debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }
            if(debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }
        collision.CheckColliding(targetPos); // using raycasts here
        camMotor.m_distance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
    }

    void FixedUpdate()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if(collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(camMotor.m_currentX, camMotor.m_currentY + camMotor.m_lookAt.eulerAngles.y, 0) * -Vector3.forward * camMotor.m_distance;
            adjustedDestination += targetPos;

            if(smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, smooth);
            }
            else
            {
                transform.position = adjustedDestination;
            }
        }
        else
        {
            if (smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, smooth);
            }
            else
            {
                transform.position = destination;
            }
        }
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }
        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion arRotation, ref Vector3[] intoArray)
        {
            if(!camera)
            {
                return;
            }

            //clear the content of intoArray
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            //top left
            intoArray[0] = (arRotation * new Vector3(-x, y, z)) + cameraPosition; // added and rotation the point relative to camera
            //top right
            intoArray[1] = (arRotation * new Vector3(x, y, z)) + cameraPosition; // added and rotation the point relative to camera
            //bottom left
            intoArray[2] = (arRotation * new Vector3(-x, -y, z)) + cameraPosition; // added and rotation the point relative to camera
            //bottm right
            intoArray[3] = (arRotation * new Vector3(x, -y, z)) + cameraPosition; // added and rotation the point relative to camera
            // camera's positon
            intoArray[4] = cameraPosition - camera.transform.forward;
        }
        bool CollisionDetectedAtClipPoints(Vector3[] ClipPoints, Vector3 fromPosition)
        {
            for(int i = 0; i< ClipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, ClipPoints[i] - fromPosition);
                float distance = Vector3.Distance(ClipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }
            return false;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++ )
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if (distance.Equals(-1))
                        distance = hit.distance;
                    else
                    {
                        if(hit.distance < distance)
                        {
                            distance = hit.distance;
                        }
                    }
                }
            }

            if (distance == -1)
                return 0;
            else
                return distance;
        }

        public void CheckColliding(Vector3 targetPosition)
        {
            if(CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                colliding = true;
            }
            else
            {
                colliding = false;
            }
        }
    }
}
