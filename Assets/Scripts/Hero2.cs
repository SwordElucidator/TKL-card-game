using UnityEngine;
using System.Collections;

public class Hero2 : Hero {

    public string heroName;

    void Start()
    {
        heroName = PlayerPrefs.GetString("hero2");
        sprite.spriteName = "home_" + heroName;
        isHero1 = false;
    }
}
