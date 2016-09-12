using UnityEngine;
using System.Collections;


public class FPSDisplay : MonoBehaviour {

    public UILabel label;
    float deltaTime = 0.0f;
    float msec;
    float fps;
	 //Update is called once per frame


    void Update () 
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        label.text = string.Format("{0:0.0} ms / ({1:0} fps)", msec, fps);
    }


}
