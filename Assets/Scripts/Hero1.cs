using UnityEngine;
using System.Collections;

public class Hero1 : MonoBehaviour {

    private UISprite sprite;

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
        string heroName = PlayerPrefs.GetString("hero1");
        sprite.spriteName = heroName;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
