using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Akatsuki
public class ReconnaissanceSkill : Skill
{
    public ReconnaissanceSkill()
    {
        this.name = "Reconnaissance";
        this.chineseName = "强行侦查";
        this.events.Add(TriggerEvent.CardPreAttack);
        this.global = true;
    }

    public override bool canTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.CardPreAttack)
        {
            return true;
        }
        return false;
    }

    public override bool OnTrigger(CardAvator thisCard, object data, TriggerEvent e)
    {
        if (e == TriggerEvent.CardPreAttack)
        {
            GameObject toAttack = (GameObject)data;

            CardAvator ava = toAttack.GetComponent<CardAvator>();
            if (ava)
            {
                if (ava.isHero1 == thisCard.isHero1)
                {
                    return false;
                }
                if (ava.hasSkill(this))
                {
                    //目标是晓
                    if (ava.getCardOn(Direct.Left) != null && ava.getCardOn(Direct.Left).isHero1 != ava.isHero1)
                    {
                        return true;
                    }
                    if (ava.getCardOn(Direct.Right) != null && ava.getCardOn(Direct.Right).isHero1 != ava.isHero1)
                    {
                        return true;
                    }
                    if (ava.getCardOn(Direct.Bottom) != null && ava.getCardOn(Direct.Bottom).isHero1 != ava.isHero1)
                    {
                        return true;
                    }
                    if (ava.getCardOn(Direct.Top) != null && ava.getCardOn(Direct.Top).isHero1 != ava.isHero1)
                    {
                        return true;
                    }
                }
                else
                {
                    List<CardAvator> lst = thisCard.getAreas().findAvatorsBySkill(this);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (Areas.getAbsoluteDistance(lst[i].transform.parent.gameObject, toAttack.transform.parent.gameObject) == 1 && lst[i].isHero1 != ava.isHero1)
                        {
                            return true;
                        }
                    }
                }
            }else
            {
                Hero hero = toAttack.GetComponent<Hero>();
                if (hero)
                {
                    List<CardAvator> lst = thisCard.getAreas().findAvatorsBySkill(this);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].getAreas().isHeroNearby(lst[i], hero.isHero1))
                        {
                            return true;
                        }
                    }
                }
            }
            
        }
        return false;
    }
}