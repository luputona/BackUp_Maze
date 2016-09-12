using UnityEngine;
using System.Collections;

public class ItemColliderCheck : MonoBehaviour
{
    private string m_colltype;
    private string m_getCheckItem;

    public UISprite m_getItemUI;
    public UILabel m_getItemUI_Label;

    public bool m_getUI;
    public static bool m_getKeyCheck = false;

	void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.GetComponent<ItemContainer>())
        {
            m_colltype = coll.gameObject.GetComponent<ItemContainer>().GetItemType();            
        }
        else
        {
            DisableDisplayGetItemUI();
        }
        EnableDisplayGetItemUI();       
    }

    void OnTriggerExit(Collider coll)
    {
        DisableDisplayGetItemUI();
    }

    void EnableDisplayGetItemUI()
    {
        m_getItemUI.gameObject.SetActive(true);
        m_getItemUI_Label.gameObject.SetActive(true);      
    }
    void DisableDisplayGetItemUI()
    {
        m_getItemUI.gameObject.SetActive(false);
        m_getItemUI_Label.gameObject.SetActive(false);
    }

    public string GetItem()
    {
        switch(m_colltype)
        {
            case "Minimap":
                m_getCheckItem = m_colltype;
                break;
            case "Key":
                m_getCheckItem = m_colltype;
                m_getKeyCheck = true;
                break;
        }
        return m_getCheckItem;
    }
	
}
