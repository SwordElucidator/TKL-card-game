using UnityEngine;
using System.Collections;

//TODODODODODDODODODO
public class MusouFuuinSkill: Skill{
    public MusouFuuinSkill()
    {
        this.name = "MusouFuuin";
        this.chineseName = "梦想封印";
        this.events.Add(TriggerEvent.CardUsed);
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        return false;
    }

    public override bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        return false;
    }
}

//Youmu
public class MiraieigouzanSkill : Skill
{
    public MiraieigouzanSkill()
    {
        this.name = "Miraieigouzan";
        this.chineseName = "未来永劫斩";
        this.events.Add(TriggerEvent.CardAttacked);
        this.events.Add(TriggerEvent.CardPreAttack);
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.CardAttacked)
        {
            AttackStruct attStruct = (AttackStruct)data;
            if (thisCard.hasSkill(this) && attStruct.fromCard == thisCard)
            {
                return true;
            }
        }
        else if (e == TriggerEvent.CardPreAttack)
        {
            if (thisCard.hasSkill(this))
            {
                return true;
            }
        }
        return false;
    }

    public override bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.CardAttacked)
        {
            AttackStruct attStruct = (AttackStruct)data;
            if (thisCard.hasSkill(this) && attStruct.fromCard == thisCard)
            {
                if (attStruct.toCard)
                {
                    attStruct.toCard.setMark("MiraiMark", thisCard.avatorId + "");
                }
                else
                {
                    thisCard.canDirectlyAttackHero = true;
                }
            }
        }
        else if (e == TriggerEvent.CardPreAttack)
        {
            CardAvator enemy = (CardAvator)data;
            if (thisCard.hasSkill(this))
            {
                return enemy.getMark("MiraiMark") != null && enemy.getMark("MiraiMark") == thisCard.avatorId + "";
            }
        }
        return false;
    }
}