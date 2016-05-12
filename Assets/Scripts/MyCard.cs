using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCard : MonoBehaviour {

    public GameObject cardPrefeb;
    public Transform card01;
    public Transform card02;
    public bool isHero1 = true;

    private int startDepth = 9;
    private float xOffset;
    private List<GameObject> cards = new List<GameObject>();


    void Start()
    {
        xOffset = card02.position.x - card01.position.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            LoseCard();
        }
    }

    //test
    public string[] cardNames;

    public void AddCard( GameObject cardGo)
    {
        GameObject go = null;
        
        go = cardGo;
        if (!cardGo)
        {
            return;
        }
        //设置父类
        go.transform.parent = this.transform;
        
        
        go.GetComponent<UISprite>().width = 130;
        go.GetComponent<Card>().ResetPos();


        Vector3 toPosition = card01.position + new Vector3(xOffset, 0, 0) * cards.Count;

        //关掉labels
        //go.transform.Find("damage_num").gameObject.SetActive(false);
        //go.transform.Find("hp_num").gameObject.SetActive(false);

        iTween.MoveTo(go, toPosition, 1f);

        cards.Add(go);

        go.GetComponent<Card>().SetDepth(startDepth + 2*(cards.Count - 1));
    }

    public void LoseCard(GameObject cardGo = null)
    {
        if (cardGo == null)
        {
            int index = Random.Range(0, cards.Count); //TODO
            Destroy(cards[index]);
            cards.RemoveAt(index);
            UpdateShow();
        }else
        {
            if (cards.Contains(cardGo))
            {
                cards.Remove(cardGo);
                Destroy(cardGo);
                UpdateShow();
            }
        }
        
    }

    public GameObject getRandomCard()
    {
        return cards[Random.Range(0, cards.Count)];
    }

    public void UpdateShow()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 toPosition = card01.position + new Vector3(xOffset, 0, 0) * i;
            iTween.MoveTo(cards[i], toPosition, 0.5f);
            cards[i].GetComponent<UISprite>().width = 130;
            cards[i].GetComponent<Card>().ResetPos();
            cards[i].GetComponent<Card>().ResetShow();
            cards[i].GetComponent<Card>().SetDepth(startDepth + 2*i);
            if (!cards[i].GetComponent<Card>().canDoSet)
            {
                cards[i].GetComponent<DragableCard>().enabled = false;
            }else
            {
                cards[i].GetComponent<DragableCard>().enabled = true;
            }
        }
    }

    public void changeAllCardsStatus(bool isEnable)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].GetComponent<Card>().canDoSet = isEnable;
        }
        UpdateShow();
    }

    public bool hasCardLeft()
    {
        return cards.Count > 0;
    }
}
