using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

namespace UnityStandardAssets.Cameras
{
    public abstract class AbstractTargetFollower : MonoBehaviour
    {
        public enum UpdateType // The available methods of updating are:
        {
            FixedUpdate, // Update in FixedUpdate (for tracking rigidbodies).
            LateUpdate, // Update in LateUpdate. (for tracking objects that are moved in Update)
            ManualUpdate, // user must call to update camera
        }

        [SerializeField] protected Transform m_Target;            // The target object to follow
        [SerializeField] private bool m_AutoTargetPlayer = true;  // Whether the rig should automatically target the player.
        [SerializeField] private UpdateType m_UpdateType;         // stores the selected update type

        protected Rigidbody targetRigidbody;
        private GameObject targetObj;


        protected virtual void Awake()
        {
            if (m_AutoTargetPlayer)
            {
                FindAndTargetPlayer();

            }
            if (m_Target == null) return;
            targetRigidbody = m_Target.GetComponent<Rigidbody>();
        }

        protected virtual void Start()
        {     
            // if auto targeting is used, find the object tagged "Player"
            // any class inheriting from this should call base.Start() to perform this action!
            

            
        }


        private void FixedUpdate()
        {
            // we update from here if updatetype is set to Fixed, or in auto mode,
            // if the target has a rigidbody, and isn't kinematic.
            if (m_AutoTargetPlayer && (m_Target == null || !m_Target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            if (m_UpdateType == UpdateType.FixedUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }


        private void LateUpdate()
        {
            // we update from here if updatetype is set to Late, or in auto mode,
            // if the target does not have a rigidbody, or - does have a rigidbody but is set to kinematic.
            if (m_AutoTargetPlayer && (m_Target == null || !m_Target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            if (m_UpdateType == UpdateType.LateUpdate)
            {
                FollowTarget(Time.deltaTime);
            }            
        }

    

        public void ManualUpdate()
        {
            // we update from here if updatetype is set to Late, or in auto mode,
            // if the target does not have a rigidbody, or - does have a rigidbody but is set to kinematic.
            if (m_AutoTargetPlayer && (m_Target == null || !m_Target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            if (m_UpdateType == UpdateType.ManualUpdate)
            {
                FollowTarget(Time.deltaTime);
            }
        }

        protected abstract void FollowTarget(float deltaTime);

    
        public void FindAndTargetPlayer()
        {
            // auto target an object tagged player, if no target has been assigned

            if (PlayerPrefs.GetInt("SelectCharacter").Equals(0)) // unitychan
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/unitychan");
                //targetObj = (GameObject)Instantiate(targetObj, new Vector3(0, 0, 0), Quaternion.identity);
                targetObj = GameObject.FindGameObjectWithTag("unitychan");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(1)) // witch
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/ChaWitch");
                targetObj = GameObject.FindGameObjectWithTag("ChaWitch");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(2)) // maid
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/ToonMaid_Cool");
                targetObj = GameObject.FindGameObjectWithTag("ToonMaid_Cool");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(3)) // jelly
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/JellyFishGirl");
                targetObj = GameObject.FindGameObjectWithTag("JellyFishGirl");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(4)) //misaki
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/Misaki_sum_humanoid");
                targetObj = GameObject.FindGameObjectWithTag("Misaki");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(5)) // kohaku
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/Utc_sum_humanoid");
                targetObj = GameObject.FindGameObjectWithTag("Utc");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(6)) // yuko
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/Yuko_sum_humanoid");
                targetObj = GameObject.FindGameObjectWithTag("Yuko");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(7)) // thief
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/Thief1");
                targetObj = GameObject.FindGameObjectWithTag("Thief");
            }
            else if (PlayerPrefs.GetInt("SelectCharacter").Equals(8)) //dragon
            {
                //targetObj = (GameObject)Resources.Load("Prefabs/ChaDragon");
                targetObj = GameObject.FindGameObjectWithTag("ChaDragon");
            }

            //var targetObj = GameObject.FindGameObjectWithTag("Player");
            if (targetObj)
            {
                SetTarget(targetObj.transform);
            }
        }


        public virtual void SetTarget(Transform newTransform)
        {
            m_Target = newTransform;
        }


        public Transform Target
        {
            get { return m_Target; }
        }

      
    }
}
