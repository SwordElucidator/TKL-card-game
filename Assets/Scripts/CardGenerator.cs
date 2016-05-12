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
        allCardfiles = new List<CardFile>();
        
        CardFile Reimu = CardFile.makeCardFile(6, 3, 6, 2, "博丽灵梦", "Touhou", "Reimu", TypeAge.Otome, TypeCharacter.Musessou, TypeMove.Fly);
        CardFile Youmu = CardFile.makeCardFile(5, 3, 6, 1, "魂魄妖梦", "Touhou", "Youmu", TypeAge.Loli, TypeCharacter.Tekketsu, TypeMove.Walk, true);
        CardFile Yukari = CardFile.makeCardFile(9, 5, 9, 2, "八云紫", "Touhou", "Yukari", TypeAge.One, TypeCharacter.Obasan, TypeMove.Fly, true);
        allCardfiles.Add(Reimu);
        allCardfiles.Add(Youmu);
        allCardfiles.Add(Yukari);
        CardFile Yuudachi = CardFile.makeCardFile(4, 3, 5, 1, "夕立", "Kancolle", "Yuudachi", TypeAge.Loli, TypeCharacter.Kuchiku, TypeMove.Sail);
        CardFile Kaga = CardFile.makeCardFile(7, 0, 9, 0, "加贺", "Kancolle", "Kaga", TypeAge.One, TypeCharacter.Wife, TypeMove.Sail);
        CardFile Akatsuki = CardFile.makeCardFile(3, 2, 4, 1, "晓", "Kancolle", "Akatsuki", TypeAge.Loli, TypeCharacter.Genki, TypeMove.Sail);
        allCardfiles.Add(Yuudachi);
        allCardfiles.Add(Kaga);
        allCardfiles.Add(Akatsuki);
        CardFile Nico = CardFile.makeCardFile(6, 2, 8, 1, "矢泽妮可", "Divas", "Nico", TypeAge.Otome, TypeCharacter.BlackBelly, TypeMove.Walk);
        CardFile Ranka = CardFile.makeCardFile(7, 1, 9, 1, "兰花·李", "Divas", "Ranka", TypeAge.One, TypeCharacter.Hero, TypeMove.Walk);
        CardFile Sheryl = CardFile.makeCardFile(7, 2, 5, 1, "雪露·诺姆", "Divas", "Sheryl", TypeAge.Loli, TypeCharacter.Queen, TypeMove.Walk);
        allCardfiles.Add(Nico);
        allCardfiles.Add(Ranka);
        allCardfiles.Add(Sheryl);
        CardFile Ako = CardFile.makeCardFile(3, 0, 4, 1, "新子憧", "JK", "Ako", TypeAge.Otome, TypeCharacter.Enkou, TypeMove.Walk);
        CardFile Kotonoha = CardFile.makeCardFile(4, 4, 4, 1, "桂言叶", "JK", "Kotonoha", TypeAge.Otome, TypeCharacter.Yandere, TypeMove.Walk, true, true);
        CardFile Shana = CardFile.makeCardFile(5, 3, 6, 1, "夏娜", "JK", "Shana", TypeAge.Loli, TypeCharacter.Tsundere, TypeMove.Fly, true);
        allCardfiles.Add(Ako);
        allCardfiles.Add(Kotonoha);
        allCardfiles.Add(Shana);

    }

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
