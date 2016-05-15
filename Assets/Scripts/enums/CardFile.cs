﻿using UnityEngine;
using System.Collections;

public enum TypeAge
{
    Loli,
    Otome,
    One,
}

public enum TypeCharacter
{
    Kuchiku,
    Hero,
    Wife,
    Genki,
    Musessou,
    Tekketsu,
    Obasan,
    BlackBelly,
    Queen,
    Enkou,
    Yandere,
    Tsundere,
}

public enum TypeMove
{
    Fly,
    Walk,
    Sail,
    Stop,
}

public enum CardType
{
    CharacterCard,
    BaseCard,
    TacticCard,
    SkillCard,
    Torpedo,
    Fighter
}

//实战卡
public class CardBase : MonoBehaviour
{

    public int cost;
    public int damage;
    public int hp;
    //这个应该只有实战卡才有
    public int maxHp;
    public int attackDistance;
    public string cardName;
    public string heroName;
    public string spriteName;
    public TypeAge typeAge;
    public TypeCharacter typeCharacter;
    public TypeMove typeMove;
    public CardType cardType;
    public Skill[] skills;
    public bool hasCharge = false;
    public bool hasRush = false;
    public Package package;
}

//原始卡
public class CardFile
{

    public int cost;
    public int damage;
    public int hp;
    public int attackDistance;
    public string cardName;
    public string heroName;
    public string spriteName;
    public TypeAge typeAge;
    public TypeCharacter typeCharacter;
    public TypeMove typeMove;
    public CardType cardType;
    public Skill[] skills;
    public bool hasCharge = false;
    public bool hasRush = false;
    public Package package;

    public static CardFile makeCardFile(Package package, int cost, int damage, int hp, int attackDistance, string cardName, string heroName, string spriteName, TypeAge typeAge, TypeCharacter typeCharacter, TypeMove typeMove, bool hasRush = false, bool hasCharge = false, CardType cardType = CardType.CharacterCard, Skill[] skills = null)
    {
        CardFile card = new CardFile();
        card.package = package;
        card.cost = cost;
        card.damage = damage;
        card.hp = hp;
        card.attackDistance = attackDistance;
        card.cardName = cardName;
        card.heroName = heroName;
        card.spriteName = spriteName;
        card.typeAge = typeAge;
        card.typeCharacter = typeCharacter;
        card.typeMove = typeMove;
        card.cardType = cardType;
        card.hasCharge = hasCharge;
        card.hasRush = hasRush;
        card.skills = skills;
        return card;
    }
}
