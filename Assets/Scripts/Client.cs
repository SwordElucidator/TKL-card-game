using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

    public static bool isClientHero1 = true;
	// Use this for initialization
	void Start () {
        isClientHero1 = PlayerPrefs.GetInt("isClientHero1") == 1;

    }
}
