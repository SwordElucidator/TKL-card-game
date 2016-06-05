using UnityEngine;
using System.Collections;

public class EndButton : MonoBehaviour {

    UISprite endbutton;
    public bool clicked = false;
    bool can_click = true;

    void Awake()
    {
        endbutton = this.GetComponent<UISprite>();
    }

	public void OnEndButtonClick()
    {
        can_click = false;
        clicked = true;
        this.GetComponent<UIButton>().normalSprite = "enemy_button";
        this.GetComponent<UIButton>().enabled = false;

    }

    public void disable()
    {
        can_click = false;
        this.GetComponent<UIButton>().normalSprite = "enemy_button";
        this.GetComponent<UIButton>().enabled = false;
    }

    public void enemyButtonClick()
    {
        clicked = true;
    }

    public void enableButton()
    {
        can_click = true;
        this.GetComponent<UIButton>().normalSprite = "end_button";
        this.GetComponent<UIButton>().enabled = true;
    }
}
