﻿using UnityEngine;
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
    //这前面的这些event一般不需要加入到skillList中，除非是全局的。


    //结算新的移动格子  DONE
    CardPreMove,
    //准备移动时
    CardOnMove,
    //移动完成时
    CardMoved,
    //结算新的可攻击格子  DONE
    CardPreAttack,
    //准备攻击时   结算出手时的技能（比如精准射击之类的，能够破除对方的烟之类的）  DONE    接受一个gameObject data，可以代表卡牌或者英雄
    CardOnAttack,
    //造成伤害前  结算造成或受到攻击前的技能（比如拉烟，无敌，减伤之类的）  DONE
    OnDamage,
    //受到伤害前  结算造成或受到攻击前的技能（比如拉烟，无敌，减伤之类的）   true意味着终止伤害  DONE
    OnDamaged,
    //造成伤害后  结算造成或受到攻击时的技能（比如激怒之类的）   true意味着终止伤害  DONE
    Damage,
    //受到伤害后  结算造成或受到攻击时的技能（比如激怒之类的）  DONE
    Damaged,
    //主动攻击完成时   DONE
    CardAttacked,
    //HP变化时  DONE
    OnHpChange,
    //HP变化后  DONE
    HpChanged,
    //回合结束时  DONE
    OnTurnEnd,
    //回合开始时  DONE
    OnTurnStart,
    //死亡时  DONE
    OnDying,
    
}

public class AttackStruct
{
    public CardAvator fromCard;
    public bool fromHero1 = false; //如果没有fromCard则根据这个判断是谁打的
    public CardAvator toCard;
    public bool toHero1 = false; //同上

    public bool canCounter;

    public AttackStruct(CardAvator fromCard, CardAvator toCard, bool fromHero1, bool toHero1, bool canCounter = false)
    {
        this.fromCard = fromCard;
        this.toCard = toCard;
        this.fromHero1 = fromHero1;
        this.toHero1 = toHero1;
        this.canCounter = canCounter;
    }

    public AttackStruct(CardAvator fromCard, CardAvator toCard, bool canCounter)
    {
        this.fromCard = fromCard;
        this.toCard = toCard;
        this.canCounter = canCounter;
    }
}

public class DamageStruct
{
    public CardAvator fromCard;
    public bool fromHero1 = false; //如果没有fromCard则根据这个判断是谁打的
    public CardAvator toCard;
    public bool toHero1 = false; //同上
    public int damage;
    
    public DamageStruct(CardAvator fromCard, CardAvator toCard, bool fromHero1, bool toHero1, int damage)
    {
        this.fromCard = fromCard;
        this.fromHero1 = fromHero1;
        this.toCard = toCard;
        this.toHero1 = toHero1;
        this.damage = damage;
        
    }

    public DamageStruct(CardAvator fromCard, CardAvator toCard, int damage)
    {
        this.fromCard = fromCard;
        this.toCard = toCard;
        this.damage = damage;
    }
}

public class HpChangeStruct
{
    public CardAvator card;
    public bool isHero1 = false;
    public int value = 0;

    public HpChangeStruct(CardAvator card, int value)
    {
        this.card = card;
        this.value = value;
    }

    public HpChangeStruct(bool isHero1, int value)
    {
        this.isHero1 = isHero1;
        this.value = value;
    }
}

public class DeathStruct
{
    public CardAvator card;
    public DamageStruct damage;

    public DeathStruct(CardAvator card, DamageStruct damage)
    {
        this.card = card;
        this.damage = damage;
    }
}

public class Skill  {
    public string name;
    public string chineseName;
    public float yieldtime = 0f;
    public List<TriggerEvent> events = new List<TriggerEvent>();
    public bool global = false;

    public virtual bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        return false;
    }

    public virtual bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        return false;
    }

    public virtual IEnumerator waitForResult(CardAvator thisCard, object data, TriggerEvent e)
    {
        yield return new WaitForSeconds(yieldtime);
        GameController.eventTriggering = false;
    }


}
