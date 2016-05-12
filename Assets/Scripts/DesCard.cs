using UnityEngine;
using System.Collections;

public class DesCard : MonoBehaviour {

    public static DesCard _instance;
    private UISprite sprite;

    public float hideTime = 0.6f;
    private float timer = 0;
    private bool isGoingHide = false;
    
    void Awake()
    {
        _instance = this;
        sprite = this.GetComponent<UISprite>();
        //this.gameObject.SetActive(false);
        sprite.alpha = 0;
    }

    void Update()
    {
        if (isGoingHide)
        {
            timer += Time.deltaTime;
            if (timer > hideTime)
            {
                sprite.alpha = 0;
                timer = 0;
                isGoingHide = false;
            }else
            {
                sprite.alpha = (hideTime - timer) / hideTime;
            }
        }
    }

    public void ShowCard(string cardname)
    {
        if (isGoingHide)
        {
            isGoingHide = false;
            timer = 0;
        }
        sprite.spriteName = cardname + "_original";
        this.gameObject.SetActive(true);
        sprite.alpha = 1;
    }

    public void HideCard()
    {
        //iTween.FadeTo(this.gameObject, 0, 3f);  ->not working
        isGoingHide = true;
        timer = 0;
    }
}
