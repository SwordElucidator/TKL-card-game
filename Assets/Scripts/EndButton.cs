using UnityEngine;
using System.Collections;

public class EndButton : MonoBehaviour {

    private UILabel label;

    void Awake()
    {
        label = transform.Find("Label").GetComponent<UILabel>();
    }

	public void OnEndButtonClick()
    {
        label.text = "对方回合";
    }
}
