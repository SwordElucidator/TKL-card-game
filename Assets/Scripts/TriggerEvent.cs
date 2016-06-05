using UnityEngine;
using System.Collections;

public class NextEvent {

    public bool isSkill = false;
    public OneSkill oneSkill;
    public OneCall oneCall;

    public NextEvent(OneSkill oneSkill)
    {
        isSkill = true;
        this.oneSkill = oneSkill;
    }

    public NextEvent(OneCall oneCall)
    {
        this.oneCall = oneCall;
    }
}

public class NextPut
{
    public CardFile card;
    public bool isHero1;

    public NextPut(CardFile card, bool isHero1)
    {
        this.card = card;
        this.isHero1 = isHero1;
    }
}

public delegate void CallEvent(GameObject source, object oj);

public class Calls
{
    public static void CallSetEvent(GameObject source, object oj)
    {
        //对于set而言，oj是gameObject surface
        Areas.Set(source.GetComponent<Card>(), (GameObject)oj);
    }

    public static void CallPutEvent(GameObject source, object oj)
    {
        //对于put而言, source是area object是CardFile card
        Areas.Put((NextPut)oj, source);
    }

    public static void CallMoveEvent(GameObject source, object oj)
    {
        Areas.Move(source.GetComponent<CardAvator>(), (GameObject)oj);
    }

    public static void CallAttackEvent(GameObject source, object oj)
    {
        Areas.Attack(source.GetComponent<CardAvator>(), (GameObject)oj);
    }

    public static void CallAttackBaseEvent(GameObject source, object oj)
    {
        Areas.AttackBase(source.GetComponent<CardAvator>(), (GameObject)oj);
    }
}

public class OneCall
{


    //这个class是用来让nextEvent调用函数的

    private GameObject source;
    private object oj;
    private CallEvent call;


    

    public void Call()
    {
        call(this.source, this.oj);
    }

    public OneCall(GameObject source, object oj, CallEvent call)
    {
        this.source = source;
        this.oj = oj;
        this.call = call;
    }
}

public class OneSkill
{


    //这个class是用来让nextEvent调用函数的

    private Skill skill;
    private CardAvator thisCard;
    private object data;
    private TriggerEvent e;
    
    public bool trigger()
    {
        return skill.OnTrigger(thisCard, data, e);
    }

    public void waitForResult()
    {
        thisCard.StartCoroutine(skill.waitForResult(thisCard, data, e));
    }

    public float getYieldTime()
    {
        return skill.yieldtime;
    }

    public OneSkill(Skill skill, CardAvator thisCard, object data, TriggerEvent e)
    {
        this.skill = skill;
        this.thisCard = thisCard;
        this.data = data;
        this.e = e;
    }
}