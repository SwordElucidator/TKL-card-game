using UnityEngine;
using System.Collections;

public class VSShow : MonoBehaviour {

    public static VSShow _instance;

    public TweenScale vsTween;
    public TweenPosition hero1Tween;
    public TweenPosition hero2Tween;

    public void Awake()
    {
        _instance = this;
        //this.gameObject.SetActive(false);
    }

    public void Start()
    {
        //Show("hero2", "hero4");
    }

    public void Show(string hero1Name, string hero2Name)
    {
        PlayerPrefs.SetString("hero1", hero1Name);
        PlayerPrefs.SetString("hero2", hero2Name);

        BlackMask._instance.Show(); 

        hero1Tween.GetComponent<UISprite>().spriteName = hero1Name;
        hero2Tween.GetComponent<UISprite>().spriteName = hero2Name;

        vsTween.PlayForward();
        hero1Tween.PlayForward();
        hero2Tween.PlayForward();
    }
}
