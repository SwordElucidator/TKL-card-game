using UnityEngine;
using System.Collections;

public class BGMLoader : MonoBehaviour {

    public AudioClip[] TH_BGMs;
    public AudioClip[] KC_BGMs;
    public AudioClip[] DV_BGMs;
    public AudioClip[] JK_BGMs;

    public AudioSource fxSound;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            AudioSource fx = this.fxSound;
            Destroy(gameObject);
            GameObject.Find("BGMScript").GetComponent<BGMLoader>().fxSound = fx;
        }
    }

    public void playOnSelectHero(string heroName)
    {
        fxSound.Stop();
        switch (heroName)
        {
            case "hero1":
                fxSound.clip = TH_BGMs[3];
                break;
            case "hero2":
                fxSound.clip = KC_BGMs[0];
                break;
            case "hero3":
                fxSound.clip = DV_BGMs[1];
                break;
            case "hero4":
                fxSound.clip = JK_BGMs[0];
                break;
        }
        fxSound.Play();
    }

    public void playRandom()
    {
        fxSound.Stop();
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                fxSound.clip = TH_BGMs[Random.Range(0, 4)];
                break;
            case 1:
                fxSound.clip = KC_BGMs[Random.Range(0, 4)];
                break;
            case 2:
                fxSound.clip = DV_BGMs[Random.Range(0, 4)];
                break;
            case 3:
                fxSound.clip = JK_BGMs[Random.Range(0, 4)];
                break;
        }
        fxSound.Play();
    }

    public void stopMainPageSound()
    {
        fxSound.Stop();
    }
}
