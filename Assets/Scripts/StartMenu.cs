using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

    public MovieTexture movTexture;

    //whether movie is being drawn
    public bool isDrawMov = true;

    public bool isShowMessage = false;

    public TweenScale logoTweenScale;

    public TweenPosition selectRoleTween;

    public UISprite hero1;

    private bool canShowSelectRole = false; //check whether can show select role screen

	// Use this for initialization
	void Start () {
        movTexture.loop = false;
        movTexture.Play();
        //add onFinish to logoTweenScale
        logoTweenScale.AddOnFinished(this.OnLogoTweenFinished);
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

       if (canShowSelectRole && Input.GetMouseButtonDown(0))
        {
            //show seect row page
            selectRoleTween.PlayForward();
            canShowSelectRole = false;
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

    private void OnLogoTweenFinished()
    {
        canShowSelectRole = true;
    }

    public void OnPlayButtonClick()
    {
        BlackMask._instance.Show();
        VSShow._instance.Show(hero1.spriteName, "hero" + Random.Range(1, 10));
        StartCoroutine(LoadPlayScene());
    }

    IEnumerator LoadPlayScene()
    {
        yield return new WaitForSeconds(3f);
        //Application.LoadLevel is obsoleted
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        
    }
}
