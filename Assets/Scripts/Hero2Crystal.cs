using UnityEngine;
using System.Collections;

public class Hero2Crystal : MonoBehaviour {

    public int usableNumber = 1;
    public int totalNumber = 1;

    private UILabel label;

    void Awake()
    {
        label = this.GetComponent<UILabel>();
    }

    public void UpdateShow()
    {
        label.text = usableNumber + "/" + totalNumber;
    }
}
