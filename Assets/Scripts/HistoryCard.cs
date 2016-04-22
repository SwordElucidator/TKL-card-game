using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HistoryCard : MonoBehaviour {

    public Transform inCard;
    public Transform outCard;
    public Transform card1;
    public Transform card2;
    public GameObject cardPrefab;

    public static int MAX_COUNT = 7;

    private float yOffset;
    private List<GameObject> cardList = new List<GameObject>();


    void Start()
    {
        yOffset = card2.position.y - card1.position.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(AddCard());
        }
    }
    //position will be set up when generate the object, so need to wait until we can set it
	public IEnumerator AddCard()
    {
        GameObject go = GameObject.Instantiate(cardPrefab, inCard.position, Quaternion.identity) as GameObject;
        NGUITools.SetActive(go, false);
        yield return 0;//wait for 1 zhen
        go.transform.position = inCard.position;
        NGUITools.SetActive(go, true);
        iTween.MoveTo(go, card1.position, 1f);

        cardList.Add(go);
        if (cardList.Count > MAX_COUNT)
        {
            float animate_time = 1f;
            iTween.MoveTo(cardList[0], outCard.position, animate_time);
            Destroy(cardList[0], animate_time + 1f);
            cardList.RemoveAt(0);
        }

        for (int i = 0; i < cardList.Count-1; i++)
        {
            iTween.MoveTo(cardList[i], cardList[i].transform.position + new Vector3(0, yOffset, 0), 0.5f);
        }
    }
}
