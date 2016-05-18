using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeikanhikouSkill : Skill
{
    public SeikanhikouSkill()
    {
        this.name = "Seikanhikou";
        this.chineseName = "星间飞行";
        this.events.Add(TriggerEvent.CardIn);
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.CardIn)
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
        if (e == TriggerEvent.CardIn)
        {
            List<CardAvator> cards = thisCard.getEnemyCards();
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].typeCharacter != TypeCharacter.Tekketsu)
                    cards[i].doBrainwashing();
            }
            //舰载机，等会写
        }
        return false;
    }

}

public class DiamondCrevasseSkill : Skill
{
    public DiamondCrevasseSkill()
    {
        this.name = "DiamondCrevasse";
        this.chineseName = "钻石裂痕";
        this.events.Add(TriggerEvent.OnTurnStart);
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.OnTurnStart)
        {
            if (thisCard.hasSkill(this) && (bool)data == thisCard.isHero1)
            {
                return true;
            }
        }
        return false;
    }

    public override bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.OnTurnStart)
        {
            List<CardAvator> cards = thisCard.getSelfCards();
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].doMotivate(0, 1);
            }
            //舰载机，等会写
        }
        return false;
    }

}

public class NiconiconiSkill : Skill
{
    public NiconiconiSkill()
    {
        this.name = "Niconiconi";
        this.chineseName = "妮可妮可妮";
        this.events.Add(TriggerEvent.OnTurnEnd);
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.OnTurnEnd)
        {
            if (thisCard.hasSkill(this) && (bool)data == thisCard.isHero1 && (!thisCard.attacked && !thisCard.moved) && thisCard.getRandomEnemyCard())
            {
                return true;
            }
        }
        return false;
    }

    public override bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.OnTurnEnd)
        {
            CardAvator enemy = thisCard.getRandomEnemyCard();
            if (enemy)
                thisCard.getRandomEnemyCard().doBrainwashing();
            
        }
        return false;
    }

}