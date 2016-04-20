using UnityEngine;
using System.Collections;

public class Hero2 : MonoBehaviour {
    private UISprite sprite;

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
        string heroName = PlayerPrefs.GetString("hero2");
        sprite.spriteName = heroName;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
