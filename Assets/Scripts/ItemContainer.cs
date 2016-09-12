using UnityEngine;
using System.Collections;

public class ItemContainer : MonoBehaviour {

    public enum ITEM
    {
        E_MINIMAP,
        E_KEY
    }

    public ITEM m_itemType;
	
    public string GetItemType()
    {
        string itemType = "";

        switch (m_itemType)
        {
            case ITEM.E_MINIMAP:
                itemType = "Minimap";
                break;
            case ITEM.E_KEY:
                itemType = "Key";
                break;
        }
        return itemType;
    }
	
}
