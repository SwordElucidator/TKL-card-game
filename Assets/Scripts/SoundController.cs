using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {
    AudioSource fxSound;
    Queue<AudioClip> waitlist;
    // Use this for initialization
    void Start () {
        waitlist = new Queue<AudioClip>();
        fxSound = GetComponent<AudioSource>();
        fxSound.volume = PlayerPrefs.GetFloat("voicePercentage");
    }

    public void clearWaitList()
    {
        waitlist.Clear();
    }

    public bool isPlaying()
    {
        return fxSound.isPlaying;
    }

    public int getWaitLength()
    {
        return waitlist.Count;
    }
    
    public void playSound(AudioClip clip)
    {
        if (fxSound.isPlaying)
        {
            waitlist.Enqueue(clip);
        }
        else
        {
            fxSound.clip = clip;
            Invoke("checkPlay", clip.length);
            fxSound.Play();
        }
    }

    private void checkPlay()
    {
        fxSound.Stop();
        if (waitlist.Count > 0)
        {
            playSound(waitlist.Dequeue());
        }
    }

    public void Stop()
    {
        clearWaitList();
        fxSound.Stop();
    }
}
