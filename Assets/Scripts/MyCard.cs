using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCard : MonoBehaviour {

    public GameObject cardPrefeb;
    public Transform card01;
    public Transform card02;
    public bool isHero1 = true;

    public bool locked = false;

    private int startDepth = 9;
    private float xOffset;
    private List<GameObject> cards = new List<GameObject>();

    void Awake()
    {
        //如果并非是正确的hero1和hero2的位置的话，应当调整这俩的名字
        if (PlayerPrefs.GetInt("isClientHero1") == 0)
        {
            if (this.gameObject.name == "mycard1")
            {
                this.gameObject.name = "mycard2";
            }
            else if (this.gameObject.name == "mycard2")
            {
                this.gameObject.name = "mycard1";
            }
        }
    }

    void Start()
    {
        if (this.gameObject.name == "mycard1")
        {
            isHero1 = true;
        }
        else if (this.gameObject.name == "mycard2")
        {
            isHero1 = false;
        }
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

        if (locked)
        {
            go.GetComponent<Card>().canDoSet = false;
            go.GetComponent<DragableCard>().enabled = false;
        }
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
        locked = !isEnable;
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
