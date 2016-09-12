using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapRaderObject
{
    public UISprite m_icon { get; set; }
    public GameObject owner { get; set; }
}

public class MapRader : MonoBehaviour {
    
    private float m_mapScale = 2.0f;
    public Transform m_playerPos;
    public Camera m_mapCamera;
    public static List<MapRaderObject> m_mapRaderObjectList = new List<MapRaderObject>();

    public UITexture uiTexture;
    public RectTransform rt;
    public Vector3[] corners = new Vector3[4];

    private Vector3 m_object_Scale;
    
    public static void RegisterRaderObject(GameObject _obj, UISprite _image)
    {
        UISprite image = Instantiate(_image);
        m_mapRaderObjectList.Add(new MapRaderObject() {owner = _obj, m_icon = image });
    }

    public static void RemoveMapObject(GameObject obj)
    {
        List<MapRaderObject> newList = new List<MapRaderObject>();
        for(int i = 0; i< m_mapRaderObjectList.Count; i++)
        {
            if(m_mapRaderObjectList[i].owner == obj)
            {
                Destroy(m_mapRaderObjectList[i].m_icon);
                continue;
            }
            else
            {
                newList.Add(m_mapRaderObjectList[i]);
            }
        }
        m_mapRaderObjectList.RemoveRange(0, m_mapRaderObjectList.Count);
        m_mapRaderObjectList.AddRange(newList);
    }
    void Start()
    {
        m_playerPos = FieldManager.m_targetObj;
        rt = this.GetComponent<RectTransform>();
        uiTexture = this.GetComponent<UITexture>();
        
        m_object_Scale.x = 1.0f;
        m_object_Scale.y = 1.0f;
        m_object_Scale.z = 1.0f;
    }
   

	public void DrawMapIcons()
    {        
        for(int i = 0; i<m_mapRaderObjectList.Count; i++)
        {
            Vector3 screenPos = m_mapCamera.WorldToViewportPoint(m_mapRaderObjectList[i].owner.transform.position);
       
            //rt.GetWorldCorners(corners);
            corners = uiTexture.worldCorners;
            corners[0].z = 0.0f;
            screenPos.x = screenPos.x * uiTexture.width * 0.00185f + corners[0].x;
            screenPos.y = screenPos.y * uiTexture.height * 0.00185f + corners[0].y;
          
            //screenPos.x = screenPos.x * rt.rect.width + corners[0].x ;
            //screenPos.y = screenPos.y * rt.rect.height + corners[0].y;
            
            screenPos.z = 0;
            m_mapRaderObjectList[i].m_icon.transform.position = screenPos;
            m_mapRaderObjectList[i].m_icon.transform.localScale = m_object_Scale;
            m_mapRaderObjectList[i].m_icon.transform.SetParent(this.transform);
            
            
        }

    }

    public void DrawDotsInRader()
    {
        for(int i = 0; i<m_mapRaderObjectList.Count; i++)
        {
            Vector3 raderPos = (m_mapRaderObjectList[i].owner.transform.position - m_playerPos.transform.position);
            float distToObject = Vector3.Distance(m_playerPos.transform.position, m_mapRaderObjectList[i].owner.transform.position) * m_mapScale;
            float deltay = Mathf.Atan2(raderPos.x, raderPos.z) * Mathf.Rad2Deg - 270.0f - m_playerPos.transform.eulerAngles.y;
            raderPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1.0f;
            raderPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            m_mapRaderObjectList[i].m_icon.transform.SetParent(this.transform);
            m_mapRaderObjectList[i].m_icon.transform.position = new Vector3(raderPos.x, raderPos.z, 0) + this.transform.position;
        }
    }
    

}
