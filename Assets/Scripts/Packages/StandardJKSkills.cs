using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParttimeJobSkill : Skill
{
    public ParttimeJobSkill()
    {
        this.name = "ParttimeJob";
        this.chineseName = "副业";
        this.events.Add(TriggerEvent.CardIn);
        this.global = true;
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.CardIn)
        {
            return true;
        }
        return false;
    }

    public override bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.CardIn)
        {
            if (thisCard.hasSkill(this) && thisCard.getMark("AkoDoCardIn") == null)
            {
                List<CardAvator> lst = thisCard.getAreas().findAvatorsBySkill(this);
                if (lst != null && lst.Count > 0)
                {
                    lst.Remove(thisCard);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        lst[i].addDamage(thisCard.damage - lst[i].damage);
                        lst[i].playAnimation("majangPeng", 50);
                    }
                }
                

                //TODO
                List<CardAvator> cards = thisCard.getAreas().getAllActiveAvatorsOfBothHerosByType();
                if (cards.Contains(thisCard))
                {
                    cards.Remove(thisCard);
                }
                if (cards.Count == 0)
                {
                    return false;
                }
                    
                thisCard.setMark("AkoDoCardIn", "standby");
                thisCard.getHero().askForAvator(cards);

            }
            else
            {
                List<CardAvator>  lst = thisCard.getAreas().findAvatorsBySkill(this);
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i].addDamage(thisCard.damage - lst[i].damage);
                    lst[i].playAnimation("majangPeng", 50);
                }
            }
        }
        return false;
    }

    public override IEnumerator waitForResult(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (thisCard.getMark("AkoDoCardIn") == "standby")
        {
            thisCard.setMark("AkoDoCardIn", "finished");
            yield return new WaitForSeconds(0.1f);
            while (Areas.IsButtonsOnUse)
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
            int value = thisCard.hp - Areas.ClickedAvator.hp;
            CardAvator.changeHp(new HpChangeStruct(thisCard, -value), true);
            thisCard.playAnimation("changeHp", 30);
            CardAvator.changeHp(new HpChangeStruct(Areas.ClickedAvator, value), true);
            Areas.ClickedAvator.playAnimation("changeHp", 30);
            //每个waitForResult必须定义eventTrigger的结束时间
            GameController.eventTriggering = false;
        }else
        {
            GameController.eventTriggering = false;
        }
    }
}

public class AlastorSkill : Skill
{
    public AlastorSkill()
    {
        this.name = "Alastor";
        this.chineseName = "天壤劫火";
        this.events.Add(TriggerEvent.OnDying);
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.OnDying)
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
        if (e == TriggerEvent.OnDying)
        {
            List<CardAvator> cards = thisCard.getAreas().getNearbyAvators(thisCard);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].changeMaxHp(cards[i].maxHp / 2);
                cards[i].playAnimation("fire", 40, 1.5f);

            }
            if (thisCard.getAreas().isHeroNearby(thisCard, !thisCard.isHero1))
            {
                Hero hero = thisCard.getAreas().getHero(!thisCard.isHero1);
                hero.changeMaxHp(hero.maxHp / 2);
            }
        }
        return false;
    }


}