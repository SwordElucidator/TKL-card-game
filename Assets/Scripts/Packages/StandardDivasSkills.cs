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
            List<CardAvator> cards = thisCard.transform.parent.parent.GetComponent<Areas>().getAllActiveAvators(!thisCard.isHero1);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].doBrainwashing();
            }
            //舰载机，等会写
        }
        return false;
    }

}
