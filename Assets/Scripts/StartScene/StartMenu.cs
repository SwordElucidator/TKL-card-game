﻿using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

    public MovieTexture movTexture;

    //whether movie is being drawn
    public bool isDrawMov = true;

    public bool isShowMessage = false;

    public TweenScale logoTweenScale;
    public TweenRotation logoTweenRotation;
    public TweenPosition logoTweenPosition;

    public TweenPosition selectRoleTween;
    public TweenPosition settingsTween;

    public TweenPosition heroZeroTween;

    public UILabel label;

    public Transform mainButtonContainer;

    public GameSettings gamesettings;

    public UISprite hero1;

    private bool canShowMainButtons = false;
    private bool canShowSelectRole = false; //check whether can show select role screen

	// Use this for initialization
	void Start () {
        gamesettings.ChangeGlobalVolumn();
        //add onFinish to logoTweenScale
        logoTweenScale.AddOnFinished(this.OnLogoTweenFinished);
        if (PlayerPrefs.GetInt("from_scene") == 0)
        {
            movTexture.loop = false;
            movTexture.Play();
            this.GetComponent<AudioSource>().Play();
        }else
        {
            PlayerPrefs.SetInt("from_scene", 0);
            StopMov();
        }
        
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

       if (canShowMainButtons && Input.GetMouseButtonDown(0))
        {
            showMainButtons();
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


    private void showMainButtons()
    {
        label.enabled = false;
        logoTweenRotation.PlayForward();
        logoTweenPosition.PlayForward();
        canShowMainButtons = false;
        for (int i = 0; i < mainButtonContainer.childCount; i++)
        {
            mainButtonContainer.GetChild(i).GetComponent<TweenPosition>().PlayForward();
        }
    }

    private void showMainButtonsAgain()
    {
        for (int i = 0; i < mainButtonContainer.childCount; i++)
        {
            mainButtonContainer.GetChild(i).GetComponent<TweenPosition>().PlayForward();
        }
    }

    public void onStartButtonClicked()
    {
        clearMainButtons();
        showHeroSelect();
    }

    public void onSettingButtonClicked()
    {
        clearMainButtons();
        showSettings();
    }

    public void onExitButtonClicked()
    {
        clearMainButtons();
        StartCoroutine(ExitByExitButton());
    }

    IEnumerator ExitByExitButton()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    private void clearMainButtons()
    {
        for (int i = 0; i < mainButtonContainer.childCount; i++)
        {
            mainButtonContainer.GetChild(i).GetComponent<TweenPosition>().PlayReverse();
        }
    }

    private void showHeroSelect()
    {
        //show select row page
        selectRoleTween.PlayForward();
        heroZeroTween.PlayForward();
    }

    private void showSettings()
    {
        //show select row page
        settingsTween.PlayForward();
    }

    public void OnSelectHeroReturnButtonClick()
    {
        selectRoleTween.PlayReverse();
        heroZeroTween.PlayReverse();
        showMainButtonsAgain();
    }

    public void OnSettingsReturnButtonClick()
    {
        settingsTween.PlayReverse();
        showMainButtonsAgain();
    }


    private void StopMov()
    {
        movTexture.Stop();
        isDrawMov = false;
        //show main scene
        GameObject.Find("BGMScript").GetComponent<BGMLoader>().playRandom();
        logoTweenScale.PlayForward();
    }

    private void OnLogoTweenFinished()
    {
        canShowMainButtons = true;
    }

    public void OnPlayButtonClick()
    {
        BlackMask._instance.Show();

        string hero1Name = hero1.spriteName;
        int herotype1 = int.Parse(hero1Name.Substring(4, hero1Name.Length - 4));
        int herotype2 = Random.Range(1, 4);
        string hero2Name = "hero" + herotype2;

        

        //set playerprefs 传输给scene play
        //应该传送的是hero的name而不是1,2,3,4

        

        int isClientHero1 = Random.Range(0, 2);
        PlayerPrefs.SetInt("isClientHero1", isClientHero1);
        if (isClientHero1 == 1)
        {
            PlayerPrefs.SetString("hero1", Hero.HeroNames[herotype1 - 1]);
            PlayerPrefs.SetString("hero2", Hero.HeroNames[herotype2 - 1]);
            VSShow._instance.Show(hero1Name, hero2Name);
        }
        else
        {
            PlayerPrefs.SetString("hero2", Hero.HeroNames[herotype1 - 1]);
            PlayerPrefs.SetString("hero1", Hero.HeroNames[herotype2 - 1]);
            VSShow._instance.Show(hero2Name, hero1Name);
        }
       //设置卡组



       StartCoroutine(LoadPlayScene());
    }

    

    IEnumerator LoadPlayScene()
    {
        yield return new WaitForSeconds(3f);
        //Application.LoadLevel is obsoleted
        GameObject.Find("BGMScript").GetComponent<BGMLoader>().stopMainPageSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        
    }
}
