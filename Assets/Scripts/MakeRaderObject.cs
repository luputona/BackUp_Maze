using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakeRaderObject : MonoBehaviour {

    public UISprite m_sprite;

	// Use this for initialization
	void Start () 
    {
        MapRader.RegisterRaderObject(this.gameObject, m_sprite);        
	}
	
    void OnDestroy()
    {
        MapRader.RemoveMapObject(this.gameObject);
    }
}
