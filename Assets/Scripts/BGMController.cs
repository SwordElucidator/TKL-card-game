using UnityEngine;
using System.Collections;

public class BGMController : MonoBehaviour {
    AudioSource fxSound;
    private BGMLoader loader;

    private AudioClip[] current_bgms;
    private float current_length = 0f;
    private int current_id = 0;

    // Use this for initialization
    void Start () {
        if (!GameObject.Find("BGMScript") || !GameObject.Find("BGMScript").GetComponent<BGMLoader>())
        {
            MonoBehaviour.print("No BGMLoader Found, it must be from the first scene");
        }
        loader = GameObject.Find("BGMScript").GetComponent<BGMLoader>();
        string heroname = "";
        if (PlayerPrefs.GetInt("isClientHero1") == 1)
        {
            heroname = PlayerPrefs.GetString("hero1"); 
        }
        else
        {
            heroname = PlayerPrefs.GetString("hero2");
        }
        switch (heroname)
        {
            case "Touhou":
                current_bgms = loader.TH_BGMs;
                break;
            case "Kancolle":
                current_bgms = loader.KC_BGMs;
                break;
            case "Divas":
                current_bgms = loader.DV_BGMs;
                break;
            case "JK":
                current_bgms = loader.JK_BGMs;
                break;
        }

        current_id = Random.Range(0, 4);
        // Audio Source responsavel por emitir os sons
        fxSound = GetComponent<AudioSource>();
        fxSound.volume = PlayerPrefs.GetFloat("bgmPercentage");
        fxSound.clip = current_bgms[current_id];
        current_length = current_bgms[current_id].length;
        Invoke("ChangeBGM", current_length);
        fxSound.Play();
    }

    void ChangeBGM()
    {
        fxSound.Stop();
        current_id += 1;
        if (current_id > 3)
        {
            current_id = 0;
        }
        fxSound.clip = current_bgms[current_id];
        current_length = current_bgms[current_id].length;
        Invoke("ChangeBGM", current_length);
        fxSound.Play();
    }

    public void Stop()
    {
        fxSound.Stop();
    }


}
