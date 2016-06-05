using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SettingData
{
    public float voicePercentage;
    public float bgmPercentage;
    public string playerName;
}

public class GameSettings : MonoBehaviour {

    // Use this for initialization


    void Awake () {
        Load();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (!File.Exists(Application.persistentDataPath + "/settings.dat"))
        {
            file = File.Create(Application.persistentDataPath + "/settings.dat");
        }else
        {
            file = File.Open(Application.persistentDataPath + "/settings.dat", FileMode.Open);
        }
            
        SettingData data = new SettingData();
        data.bgmPercentage = transform.Find("bgm_Slider_Container").GetComponent<UISlider>().value;
        data.voicePercentage = transform.Find("voice_Slider_Container").GetComponent<UISlider>().value;
        data.playerName = transform.Find("multiplay_name_input").GetComponent<UIInput>().value;
        bf.Serialize(file, data);
        file.Close();
        ChangeGlobalVolumn();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.dat", FileMode.Open);
            SettingData data = (SettingData)bf.Deserialize(file);
            file.Close();
            try
            {
                transform.Find("voice_Slider_Container").GetComponent<UISlider>().value = data.voicePercentage;
                transform.Find("bgm_Slider_Container").GetComponent<UISlider>().value = data.bgmPercentage;
                transform.Find("multiplay_name_input").GetComponent<UIInput>().value = data.playerName;
                return;
            }catch(System.InvalidCastException e)
            {

            }


        }
        transform.Find("voice_Slider_Container").GetComponent<UISlider>().value = 0.8f;
        transform.Find("bgm_Slider_Container").GetComponent<UISlider>().value = 0.4f;
        transform.Find("multiplay_name_input").GetComponent<UIInput>().value = "SE";

    }
    //能改变BGM音量的地方应只有这里的函数
    
    //用来修改当前播放的bgm声音大小
    public void ChangeLocalBgmVolumn()
    {
        GameObject.Find("BGMScript").GetComponent<BGMLoader>().fxSound.volume = transform.Find("bgm_Slider_Container").GetComponent<UISlider>().value;
    }
    //用来修改试听的voice大小
    public void changeLocalVoiceVolumn()
    {
        this.transform.Find("try_voice").GetComponent<AudioSource>().volume = transform.Find("voice_Slider_Container").GetComponent<UISlider>().value;
    }
    //测试voice
    public void tryVoice()
    {
        string[] testnames = { "Yuudachi", "Ranka", "Sheryl", "Kaga", "Nico", "Akatsuki" };
        string path = "Music/Sounds/" + testnames[Random.Range(0, testnames.Length)] + "_in";
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip)
        {
            this.transform.Find("try_voice").GetComponent<AudioSource>().Stop();
            this.transform.Find("try_voice").GetComponent<AudioSource>().clip = clip;
            this.transform.Find("try_voice").GetComponent<AudioSource>().Play();
        }
        
    }

    public void ChangeGlobalVolumn()
    {
        PlayerPrefs.SetFloat("voicePercentage", transform.Find("voice_Slider_Container").GetComponent<UISlider>().value);
        PlayerPrefs.SetFloat("bgmPercentage", transform.Find("bgm_Slider_Container").GetComponent<UISlider>().value);
    }
}
