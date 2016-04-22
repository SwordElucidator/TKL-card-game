using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCard : MonoBehaviour {

    public GameObject cardPrefeb;
    public Transform card01;
    public Transform card02;

    private float xOffset;
    private List<GameObject> cards = new List<GameObject>();


    void Start()
    {
        xOffset = card02.position.x - card01.position.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetCard();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            LoseCard();
        }
    }

    public void GetCard()
    {
        GameObject go = NGUITools.AddChild(this.gameObject, cardPrefeb);

        Vector3 toPosition = card01.position + new Vector3(xOffset, 0, 0) * cards.Count;

        iTween.MoveTo(go, toPosition, 1f);

        cards.Add(go);
    }

    public void LoseCard()
    {
        int index = Random.Range(0, cards.Count); //TODO
        Destroy(cards[index]);
        cards.RemoveAt(index);
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 toPosition = card01.position + new Vector3(xOffset, 0, 0) * i;
            iTween.MoveTo(cards[i], toPosition, 0.5f);
        }
    }
}
