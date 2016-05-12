using UnityEngine;
using System.Collections;


public class Hero1 : Hero {
    public string heroName;

    void Start()
    {
        heroName = PlayerPrefs.GetString("hero1");
        sprite.spriteName = "home_" + heroName;
    }
}
