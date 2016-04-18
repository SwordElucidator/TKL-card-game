using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

    public MovieTexture movTexture;

    //whether movie is being drawn
    public bool isDrawMov = true;

    public bool isShowMessage = false;

    public TweenScale logoTweenScale;

	// Use this for initialization
	void Start () {
        movTexture.loop = false;
        movTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
	    if (isDrawMov)
        {
            if (Input.GetMouseButtonDown(0) && isShowMessage == false)
            {
                isShowMessage = true;
            }else if (Input.GetMouseButtonDown(0) && isShowMessage == true)
            {
                StopMov();
            }
        }
       if (isDrawMov != movTexture.isPlaying)
        {
            StopMov();
        }
	}

    void OnGUI()
    {
        if (isDrawMov) { 
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), movTexture);
            if (isShowMessage)
            {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 40), "再次点击屏幕退出动画的播放");
            }
        }
    }

    private void StopMov()
    {
        movTexture.Stop();
        isDrawMov = false;
        //show main scene
        logoTweenScale.PlayForward();
    }
}
