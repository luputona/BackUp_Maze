using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
	}
	
	
}
