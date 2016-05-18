using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



public class Packages {
    public static List<CardFile> allCards = new List<CardFile>();
    public static List<CardFile> TouhouCards = new List<CardFile>();
    public static List<CardFile> KancolleCards = new List<CardFile>();
    public static List<CardFile> DivasCards = new List<CardFile>();
    public static List<CardFile> JKCards = new List<CardFile>();
    public static List<Package> allPackages = new List<Package>();
    private static bool created = false;

    //Alert: 这个方法只应该被call一遍去生成全局的allCards，之后如果需要修正的话不应该再调用这个方法了。
    public static void CreatePackages()
    {
        if (created)
            return;
        StandardPackage standard = new StandardPackage();
        loadPackage(standard);
        distributeCardsToHero();
        created = true;
    }

    private static void distributeCardsToHero()
    {
       for (int i = 0; i < allCards.Count; i++)
        {
            switch (allCards[i].heroName)
            {
                case "Touhou":
                    TouhouCards.Add(allCards[i]);
                    break;
                case "Kancolle":
                    KancolleCards.Add(allCards[i]);
                    break;
                case "Divas":
                    DivasCards.Add(allCards[i]);
                    break;
                case "JK":
                    JKCards.Add(allCards[i]);
                    break;
            }
        } 
    }

    public CardFile findCardFileByName(string name)
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].spriteName == name)
            {
                return allCards[i];
            }
        }
        return null;
    }

    public static List<CardFile> FindCardFilesByNames(List<string> names)
    {
        List<CardFile> output = new List<CardFile>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (names.Contains(allCards[i].spriteName))
            {
                names.Remove(allCards[i].spriteName);
                output.Add(allCards[i]);
            }
        }
        return output;
    }

    public static List<string> GetNamesByCardFiles(List<CardFile> cards)
    {
        List<string> output = new List<string>();
        for (int i = 0; i < cards.Count; i++)
        {
            output.Add(cards[i].spriteName);
        }
        return output;
    }

    public static List<CardFile> getCardsByHeroName(string heroname)
    {
        switch (heroname)
        {
            case "Touhou":
                return TouhouCards;
            case "Kancolle":
                return KancolleCards;
            case "Divas":
                return DivasCards;
            case "JK":
                return JKCards;
        }
        return null;
    }

    public static void loadPackage(Package package)
    {
        //如果全局已经有这个包了那么思考下要不要加了
        if (!Packages.allPackages.Contains(package))
        {
            Packages.allPackages.Add(package);
            for (int i = 0; i < package.getCards().Count; i++)
            {
                Packages.allCards.Add(package.getCards()[i]);
            }
        }
    }
}

//储存一个package下的所有卡牌
//这个是最原始的Package class，所有的package class应当继承于此
public class Package
{
    public string name;
    protected List<CardFile> cards = new List<CardFile>();

    public Package()
    {
        this.name = "Package";
    }

    public List<CardFile> getCards()
    {
        return cards;
    }
}

[System.Serializable]
public class DecFile
{
    public List<string> cardNames;
    public int maxLength;
}

//储存一个dec
public class Dec
{
    private List<CardFile> cards;
    private int maxLength = 30;
    public static Dec makeDec(List<CardFile> cards, int maxLength = 30)
    {
        return new Dec(cards, maxLength);
        
    }

    public Dec(List<CardFile> cards, int maxLength = 30)
    {
        this.cards = cards;
        this.maxLength = maxLength;
    }

    public Dec(string decName)
    {
        loadDec(decName, this);
    }

    public static bool loadDec(string decName, Dec dec)
    {
        if (File.Exists(Application.persistentDataPath + "/Decs/" + decName + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Decs/" + decName + ".dat", FileMode.Open);
            DecFile data = (DecFile)bf.Deserialize(file);
            file.Close();
            dec.maxLength = data.maxLength;
            dec.cards = Packages.FindCardFilesByNames(data.cardNames);
            return true;
        }
        return false;
    }

    public static void saveDec(Dec dec, string decName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (!File.Exists(Application.persistentDataPath + "/Decs/"+ decName+".dat"))
        {
            file = File.Create(Application.persistentDataPath + "/Decs/" + decName + ".dat");
        }
        else
        {
            file = File.Open(Application.persistentDataPath + "/Decs/" + decName + ".dat", FileMode.Open);
        }

        DecFile data = new DecFile();
        data.maxLength = dec.maxLength;

        //TODO 不认为这个方法应该写在Packages下面， 应该写在cardFile之类的下面
        data.cardNames = Packages.GetNamesByCardFiles(dec.cards);
        bf.Serialize(file, data);
        file.Close();
    }


    public bool addCard(CardFile card)
    {
        if (cards.Count >= maxLength)
        {
            return false;
        }
        cards.Add(card);
        return true;
    }

    public bool deleteCard(CardFile card)
    {
        return cards.Remove(card);
    }

    public List<CardFile> currentCards()
    {
        return cards;
    }

    public int getCurrentNum()
    {
        return cards.Count;
    }

}