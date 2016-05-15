using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TriggerEvent
{
    //进入手牌时
    CardInMyCard,
    //正要使用卡的时候
    CardOnUse,
    //离开手牌时
    CardLeaveMyCard,
    //结算新的入场可放置范围
    CardPreUse,
    //卡入场时
    CardIn,
    //使用卡结束时
    CardUsed,
    //结算新的移动格子
    CardPreMove,
    //准备移动时
    CardOnMove,
    //移动完成时
    CardMoved,
    //结算新的可攻击格子
    CardPreAttack,
    //准备攻击时   结算出手时的技能（比如精准射击之类的，能够破除对方的烟之类的）
    CardOnAttack,
    //结算伤害前  结算造成或受到攻击前的技能（比如拉烟，无敌，减伤之类的）
    OnDamage,
    //结算伤害后  结算造成或受到攻击时的技能（比如激怒之类的）
    Damaged,
    //攻击完成时  结算攻击完成时的技能，比如未来永劫斩给人上标记
    CardAttacked,
    //失去HP时
    HpLost,
    //回复Hp时
    HpRecovered,
    //回合结束时
    OnTurnEnd,
    //回合开始时
    OnTurnStart,
    //死亡时
    OnDying,
    //死亡后
    Dead,
    //离开场时
    CardOut,
    
}

public class AttackStruct
{
    public CardAvator fromCard;
    public bool fromHero1; //如果没有fromCard则根据这个判断是谁打的
    public CardAvator toCard;
    public bool toHero1; //同上
    public AttackStruct(CardAvator fromCard, CardAvator toCard, bool fromHero1 = false, bool toHero1 = false)
    {
        this.fromCard = fromCard;
        this.toCard = toCard;
        this.fromHero1 = fromHero1;
        this.toHero1 = toHero1;
    }
}

public class Skill  {
    public string name;
    public string chineseName;
    public List<TriggerEvent> events = new List<TriggerEvent>();

    public virtual bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        return false;
    }

    public virtual bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        return false;
    }
	
}
