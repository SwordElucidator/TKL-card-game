using UnityEngine;
using System.Collections;

public class Hero1Crystal : MonoBehaviour {

    public int usableNumber = 1;
    public int totalNumber = 1;
    public int maxNumber;//10

    public UISprite[] crystals;

    private UILabel label;

    void Awake()
    {
        maxNumber = crystals.Length;
        label = this.GetComponent<UILabel>();
    }

    void Update()
    {
        UpdateShow();
    }

    void UpdateShow()
    {
        for (int i = totalNumber; i < maxNumber; i++)
        {
            crystals[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < totalNumber; i++)
        {
            crystals[i].gameObject.SetActive(true);
        }

        for (int i = usableNumber; i < totalNumber; i++)
        {
            crystals[i].spriteName = "TextInlineImages_normal";
        }

        for (int i = 0; i < usableNumber; i++)
        {
            if (i == 9)
            {
                crystals[i].spriteName = "TextInlineImages_" + (i+1);
            }
            else
            {
                crystals[i].spriteName = "TextInlineImages_0" + (i+1);
            }
        }
        label.text = usableNumber + "/" + totalNumber;
    }
}
