using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGenerator : MonoBehaviour {

    public GameObject cardPrefeb;
    public Transform fromCard;
    private List<CardFile> cardfiles1;
    private List<CardFile> cardfiles2;
    public List<CardFile> allCardfiles;

    public CardGenerator(): base()
    {
        
        //构造需要的卡牌
        cardfiles1 = new List<CardFile>();
        cardfiles2 = new List<CardFile>();
        //currently只有加入allPackages了  调用这个只是因为这个方法应该在主页面调用，但是当前scene可能没有这玩意。
        Packages.CreatePackages();
        allCardfiles = new List<CardFile>(Packages.allCards);
        
       

    }
    //TODO 会被dec取代
    public void generateCardsForCurrentPlayers(string name1, string name2)
    {
        for (int i = 0; i < allCardfiles.Count; i++)
        {
            
            if (allCardfiles[i].heroName == name1)
            {
                cardfiles1.Add((allCardfiles[i]));
            }
            if (allCardfiles[i].heroName == name2)
            {
                cardfiles2.Add((allCardfiles[i]));
            }
        }
    }




    public GameObject RandomGenerateCard(bool toHero1=true, string heroName = null)
    {
        
        if (heroName != null)
        {
            //TODO 可能用来generate一些乱七八糟的随机牌
        }
        CardFile card = toHero1 ? cardfiles1[Random.Range(0, cardfiles1.Count)] : cardfiles2[Random.Range(0, cardfiles2.Count)];
        GameObject go = NGUITools.AddChild(this.gameObject, cardPrefeb);
        go.GetComponent<Card>().InheritFromCardFile(card, toHero1);
        go.transform.position = fromCard.position;
        return go;
    }
}
