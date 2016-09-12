using UnityEngine;
using System.Collections;

public class MakeRaderPlayer : MonoBehaviour {

    public UISprite m_sprite;

    // Use this for initialization

    void Start()
    {
        MapRader.RegisterRaderObject(FieldManager.m_targetObj.gameObject, m_sprite);
    }
    void OnDestroy()
    {
        MapRader.RemoveMapObject(FieldManager.m_targetObj.gameObject);
    }
}
