using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum Direct
{
    Left,
    Right,
    Top,
    Bottom
}

public class IntVector2
{
    public int x;
    public int y;
    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static int getAbsoluteDistance(IntVector2 a1, IntVector2 a2)
    {
        IntVector2 dis = getDisplacement(a1, a2);
        return Math.Abs(dis.x) + Math.Abs(dis.y);
    }

    public int getAbsoluteDistance()
    {
        return Math.Abs(x) + Math.Abs(y);
    }

    public static bool onSameLine(IntVector2 a1, IntVector2 a2)
    {
        return a1.x == a2.x || a1.y == a2.y;
    }

    public bool onSameLine()
    {
        return x == 0 || y == 0;
    }

    public static IntVector2 getDisplacement(IntVector2 a1, IntVector2 a2)
    {
        return new IntVector2(a2.x - a1.x, a2.y - a1.y);
    }

    public IntVector2(GameObject area)
    {
        x = Areas.getID(area) % 8;
        y = Areas.getID(area) / 8;
    }
}

public class Areas : MonoBehaviour {

    public GameObject areaPrefeb;
    public GameObject avatorPrefeb;

    public static IntVector2 Hero2vec1 = new IntVector2(3, -1);
    public static IntVector2 Hero2vec2 = new IntVector2(4, -1);
    public static IntVector2 Hero1vec1 = new IntVector2(3, 4);
    public static IntVector2 Hero1vec2 = new IntVector2(4, 4);


    public static CardAvator ClickedAvator;
    public static bool IsButtonClicked = false;
    public static bool IsButtonsOnUse = false;

    private GameObject[] areas;

    void Awake()
    {

        areas = new GameObject[32];
        Transform area1_transform = this.transform.Find("area1_try").GetComponent<Transform>();
        Transform area2_transform = this.transform.Find("area2_try").GetComponent<Transform>();
        Transform area3_transform = this.transform.Find("area3_try").GetComponent<Transform>();
        Transform area4_transform = this.transform.Find("area4_try").GetComponent<Transform>();
        Vector3 width = area2_transform.position - area1_transform.position;
        Vector3 height = area3_transform.position - area1_transform.position;

        if (PlayerPrefs.GetInt("isClientHero1") == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area1_transform.position + i * width;
                area.name = "area" + i;
                area.tag = presetTag(i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i] = area;
            }

            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area3_transform.position + i * width;
                area.name = "area" + (i + 8);
                area.tag = presetTag(i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i + 8] = area;
            }

            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area4_transform.position + i * width;
                area.name = "area" + (i + 16);
                area.tag = presetTag(i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i + 16] = area;
            }

            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area4_transform.position + height + i * width;
                area.name = "area" + (i + 24);
                area.tag = presetTag(i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i + 24] = area;
            }
        }else
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area1_transform.position + i * width;
                area.name = "area" + (31 - i);
                area.tag = presetTag(31 - i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i] = area;
            }

            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area3_transform.position + i * width;
                area.name = "area" + (23 - i);
                area.tag = presetTag(23 - i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i + 8] = area;
            }

            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area4_transform.position + i * width;
                area.name = "area" + (15 - i);
                area.tag = presetTag(15 - i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i + 16] = area;
            }

            for (int i = 0; i < 8; i++)
            {
                GameObject area = NGUITools.AddChild(this.gameObject, areaPrefeb);
                area.GetComponent<Transform>().position = area4_transform.position + height + i * width;
                area.name = "area" + (7 - i);
                area.tag = presetTag(7 - i);
                area.GetComponent<UIWidget>().depth = 1;
                areas[i + 24] = area;
            }
        }
        

    }


    public GameObject getArea(int id)
    {
        return areas[id];
    }

    private string presetTag(int area_id)
    {
        if ((area_id % 8) > 1 && (area_id % 8) < 6)
        {
            return "CharacterAvators";
        }
        else if ((area_id % 8) == 0 || (area_id % 8) == 7)
        {
            return "TacticAvators";
        }
        else
        {
            return "BaseAvators";
        }
    }

    public static void Set(Card card, GameObject area)
    {
        GameObject avator = NGUITools.AddChild(area, area.transform.parent.GetComponent<Areas>().avatorPrefeb);
        avator.GetComponent<CardAvator>().InheritFromCard(card.GetComponent<Card>());
        //doSet会handleupdateShow的职责
        avator.GetComponent<CardAvator>().doSet();
        avator.GetComponent<CardAvator>().PlaySound("in");
        //加入到技能列表中
        GameObject.Find("GameController").GetComponent<GameController>().addSkillsFromCard(avator.GetComponent<CardAvator>());

        

        //进去的时候call的方法
        List<Skill> lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardIn, avator.GetComponent<CardAvator>());
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(avator.GetComponent<CardAvator>(), null, TriggerEvent.CardIn))
                {
                    GameController.skillEventsQueue.Enqueue(new NextEvent(new OneSkill(lst[i], avator.GetComponent<CardAvator>(), null, TriggerEvent.CardIn)));
                    //lst[i].OnTrigger(avator.GetComponent<CardAvator>(), null, TriggerEvent.CardIn);
                    //area.transform.parent.GetComponent<Areas>().StartCoroutine(lst[i].waitForResult(avator.GetComponent<CardAvator>(), null, TriggerEvent.CardIn));
                }
            }
        }

        //结束set的全部工作 因为之上的doSet之类的并不改变各项属性，可以直接令方法被调用了。
        GameController.eventTriggering = false;

        card.transform.parent.GetComponent<MyCard>().LoseCard(card.gameObject);
        //area.transform.parent.GetComponent<Areas>().UpdateShow();
        
    }

    public static bool CanSet(Card card, GameObject area)
    {
        //TODO
        //保证gameobject是个area
        if (!area.transform.parent)
            return false;
        if (area.transform.parent.name != "FightCard")
        {
            return false;
        }
        
        if (area.transform.childCount > 0)
        {
            return false;
        }
        if (card.isHero1 && getID(area) < 16)
            return false;
        if (!card.isHero1 && getID(area) > 15)
            return false;
        //如果类型是CharacterCard那么就要自己这边的两排
        if (card.cardType == CardType.CharacterCard)
        {
            return area.tag == "CharacterAvators";
        }else if (card.cardType == CardType.BaseCard)
        {
            return area.tag == "BaseAvators";
        }
        else if (card.cardType == CardType.TacticCard)
        {
            return area.tag == "TacticAvators";
        }
        return false;
    }

    public static void Move(CardAvator card, GameObject area)
    {
        //如果是步行到一个有怪的区域，则将其拿起
        card.GetComponent<UIWidget>().depth = 99;
        Transform old = null;
        if (area.transform.childCount > 0)
        {
            old = area.transform.GetChild(0);
            old.parent = null;
        }
        //拖拽倒了合理的某个区域
        Transform newParent = card.transform.parent;
        card.transform.parent = area.transform;
        if (old != null)
        {
            old.parent = newParent;
        }

        card.canDoMove = false;
        card.moved = true;
        if (!card.hasRush)
        {
            card.canDoAttack = false;
        }
        card.ResetArea();
        card.ResetShow();
        card.ResetPos();
        if (old)
        {
            old.GetComponent<CardAvator>().ResetArea();
            old.GetComponent<CardAvator>().ResetShow();
            old.GetComponent<CardAvator>().ResetPos();
        }
        
        card.moveWait();

        //area.transform.parent.GetComponent<Areas>().UpdateShow();

    }

    public static bool CanMove(CardAvator card, GameObject area)
    {
        //TODO
        GameObject current_area = card.GetComponentInParent<Transform>().parent.gameObject;

        //保证gameobject是个area
        if (!area.transform.parent)
            return false;
        if (area.transform.parent.name != "FightCard" && area.transform.parent.parent.name == "FightCard")
        {
            area = area.transform.parent.gameObject;
        }

        
        if (!card.GetComponent<CardAvator>().canDoMove)
        {
            return false;
        }

        //移动范围类技能的触发点
        //如果有技能的情况
        //Trigger CardPreAttack Skills
        List<Skill> lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardPreMove, card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(card, (object)area, TriggerEvent.CardPreMove))
                {
                    //返回true说明这个技能允许特殊移动
                    if (lst[i].OnTrigger(card, (object)area, TriggerEvent.CardPreMove))
                    {
                        return true;
                    }
                }
            }
        }


        //如果类型是CharacterCard那么就要自己这边的两排
        if (card.cardType == CardType.CharacterCard)
        {
            if (area.tag != "CharacterAvators")
            {
                return false;
            }
        }

        //移动位置应当符合card的移动范围
        switch (card.typeMove)
        {
            case TypeMove.Fly:
                //不能交换
                if (area.transform.childCount > 0 && area.transform.GetChild(0).GetComponent<CardAvator>().hp > 0)
                {
                    return false;
                }
                //任意方向两个格
                IntVector2 dis = getDisplacement(current_area, area);
                return dis.getAbsoluteDistance() <= 2 && dis.getAbsoluteDistance() > 0;
            case TypeMove.Sail:
                //不能交换
                if (area.transform.childCount > 0 && area.transform.GetChild(0).GetComponent<CardAvator>().hp > 0)
                {
                    return false;
                }
                IntVector2 dis2 = getDisplacement(current_area, area);
                if (!dis2.onSameLine())
                    return false;
                if (card.isHero1)
                    return Math.Abs(dis2.x) == 1 || (dis2.y >= -3 && dis2.y < 0);
                return Math.Abs(dis2.x) == 1 || (dis2.y <= 3 && dis2.y > 0);
            case TypeMove.Walk:
                //不能交换敌方
                if (area.transform.childCount > 0 && area.transform.GetChild(0).GetComponent<CardAvator>().hp > 0)
                {
                    if (area.transform.GetChild(0).GetComponent<CardAvator>().isHero1 != card.isHero1)
                    {
                        return false;
                    }
                }
                //任意方向1个格，允许交换
                IntVector2 dis3 = getDisplacement(current_area, area);
                if (!dis3.onSameLine())
                    return false;
                return Math.Abs(dis3.x) == 1 || Math.Abs(dis3.y) == 1;
            case TypeMove.Stop:
                return false;
        }

        return false;
    }

    public static void Attack(CardAvator card, GameObject area)
    {
        //TODO
        GameObject current_area = card.GetComponentInParent<Transform>().parent.gameObject;

        //保证gameobject是个area
        if (!area.transform.parent)
            return;
        if (area.transform.parent.name != "FightCard" && area.transform.parent.parent.name == "FightCard")
        {
            area = area.transform.parent.gameObject;
        }



        IntVector2 dis3 = getDisplacement(current_area, area);

        CardAvator card2 = area.transform.GetChild(0).GetComponent<CardAvator>();
        AttackStruct attStruct = new AttackStruct(card, card2, dis3.getAbsoluteDistance() <= card2.attackDistance && dis3.onSameLine());

        //这里应该结算正要attack的情形，这里如果被终止的话有时会触发attack结算完毕的事件，需要技能本身来定义
        //如果有技能的情况
        //Trigger CardOnAttack Skills
        List<Skill> lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardOnAttack, card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(card, (object)attStruct, TriggerEvent.CardOnAttack))
                {
                    //这里返回值应该决定了是否要终止这次攻击
                    //OnTrigger应当有修正目标的能力 这个修正可以由对attStruct的直接修正来实现，因为c#的boxing是很厉害的
                    if (lst[i].OnTrigger(card, (object)attStruct, TriggerEvent.CardOnAttack))
                    {
                        //说不定要触发一些效果，比如当attack被终止时可能会比较难看
                        
                        return;
                    }
                }
            }
        }

        card.Attack(attStruct.toCard, attStruct.canCounter);

        //这里应该结算attack完成后的情况
        //如果有技能的情况
        //Trigger CardAttacked Skills
        lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardAttacked, card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(card, (object)attStruct, TriggerEvent.CardAttacked))
                {
                        //这里返回值没想好做什么用
                    lst[i].OnTrigger(card, (object)attStruct, TriggerEvent.CardAttacked);
                }
            }
        }

        card.canDoAttack = false;
        card.attacked = true;
        if (!card.hasRush)
        {
            card.canDoMove = false;
        }
        //这个update应该在动画结束后再做
        //area.transform.parent.GetComponent<Areas>().UpdateShow();

    }

    public static bool CanAttack(CardAvator card, GameObject area)
    {
        //TODO
        GameObject current_area = card.GetComponentInParent<Transform>().parent.gameObject;

        //保证gameobject是个area
        if (!area.transform.parent)
            return false;
        if (area.transform.parent.name != "FightCard" && area.transform.parent.parent.name == "FightCard")
        {
            area = area.transform.parent.gameObject;
        }

        if (!card.GetComponent<CardAvator>().canDoAttack)
        {
            return false;
        }

        if (area.transform.childCount == 0 || area.transform.GetChild(0).GetComponent<CardAvator>().isHero1 == card.isHero1 || area.transform.GetChild(0).GetComponent<CardAvator>().hp <= 0) {
            return false;
        }

        //如果有技能的情况
        //Trigger CardPreAttack Skills
        List<Skill> lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardPreAttack, card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(card, (object)area.transform.GetChild(0).gameObject, TriggerEvent.CardPreAttack))
                {
                    //返回true说明这个技能允许跨距离打击
                    if (lst[i].OnTrigger(card, (object)area.transform.GetChild(0).gameObject, TriggerEvent.CardPreAttack))
                    {
                        return true;
                    }
                }
            }
        }

        //普通情况
        IntVector2 dis3 = getDisplacement(current_area, area);
        if (!dis3.onSameLine())
            return false;
        return Math.Abs(dis3.x) <= card.attackDistance && Math.Abs(dis3.y) <= card.attackDistance;
    }

    public Hero getHero(bool isHero1)
    {
        if (isHero1)
        {
            return GameObject.Find("hero1").GetComponent<Hero>();
        }
        return GameObject.Find("hero2").GetComponent<Hero>();
    }

    public List<CardAvator> getNearbyAvators(CardAvator card)
    {
        List<CardAvator> outs = new List<CardAvator>();
        List < CardAvator > cards = getAllActiveAvatorsOfBothHerosByType();
        for (int i = 0; i < cards.Count; i++)
        {
            IntVector2 dis3 = getDisplacement(card.transform.parent.gameObject, cards[i].transform.parent.gameObject);
            if (dis3.onSameLine() && Math.Abs(dis3.x) <= 1 && Math.Abs(dis3.y) <= 1)
            {
                outs.Add(cards[i]);
            }
        }
        return outs;
    }

    public bool isHeroNearby(CardAvator card, bool isHero1)
    {
        if (!isHero1)
        {
            return getID(card.transform.parent.gameObject) == 3 || getID(card.transform.parent.gameObject) == 4;
        }
        else
        {
            return getID(card.transform.parent.gameObject) == 27 || getID(card.transform.parent.gameObject) == 28;
        }
    }

    public static void AttackBase(CardAvator card, GameObject area)
    {
        //TODO
        GameObject current_area = card.GetComponentInParent<Transform>().parent.gameObject;

        if (area.name != "hero1" && area.name != "hero2")
            return;

        if (area.GetComponent<Hero>().isHero1 == card.isHero1)
        {
            return;
        }
        if (!card.GetComponent<CardAvator>().canDoAttack)
        {
            return;
        }

        AttackStruct attStruct = new AttackStruct(card, null, card.isHero1, area.GetComponent<Hero>().isHero1);

        //这里应该结算正要attack的情形，这里如果被终止的话有时会触发attack结算完毕的事件，需要技能本身来定义
        //如果有技能的情况
        //Trigger CardOnAttack Skills
        List<Skill> lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardOnAttack, card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(card, (object)attStruct, TriggerEvent.CardOnAttack))
                {
                    //这里返回值应该决定了是否要终止这次攻击
                    //OnTrigger应当有修正目标的能力 这个修正可以由对attStruct的直接修正来实现，因为c#的boxing是很厉害的
                    if (lst[i].OnTrigger(card, (object)attStruct, TriggerEvent.CardOnAttack))
                    {
                        //说不定要触发一些效果，比如当attack被终止时可能会比较难看

                        return;
                    }
                }
            }
        }

        //不再测试了，所以一定要先call canAttackBase啊！
        card.AttackBase();//TODODODODO

        //这里应该结算attack完成后的情况
        //如果有技能的情况
        //Trigger CardAttacked Skills
        lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardAttacked, card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(card, (object)attStruct, TriggerEvent.CardAttacked))
                {
                    //这里返回值没想好做什么用
                    lst[i].OnTrigger(card, (object)attStruct, TriggerEvent.CardAttacked);
                }
            }
        }

        card.canDoAttack = false;
        card.attacked = true;
        if (!card.hasRush)
        {
            card.canDoMove = false;
        }
        //这个update应该在动画结束后再做
        //area.transform.parent.GetComponent<Areas>().UpdateShow();

    }

    public static bool CanAttackBase(CardAvator card, GameObject area)
    {
        if (area.name != "hero1" && area.name != "hero2")
            return false;

        if ((area.GetComponent<Hero>().isHero1) == card.isHero1)
        {
            return false;
        }
        GameObject current_area = card.GetComponentInParent<Transform>().parent.gameObject;
        if (!card.GetComponent<CardAvator>().canDoAttack)
        {
            return false;
        }
        if (card.canDirectlyAttackHero)
        {
            return true;
        }

        //如果有技能的情况
        //Trigger CardPreAttack Skills
        List<Skill> lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.CardPreAttack, card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(card, (object)area, TriggerEvent.CardPreAttack))
                {
                    //返回true说明这个技能允许跨距离打击
                    if (lst[i].OnTrigger(card, (object)area, TriggerEvent.CardPreAttack))
                    {
                        return true;
                    }
                }
            }
        }

        if (area.name == "hero1")
        {
            IntVector2 dis3 = IntVector2.getDisplacement(new IntVector2(current_area), Hero1vec1);
            IntVector2 dis3_2 = IntVector2.getDisplacement(new IntVector2(current_area), Hero1vec2);
            return (dis3.onSameLine() && Math.Abs(dis3.x) <= card.attackDistance && Math.Abs(dis3.y) <= card.attackDistance) || (dis3_2.onSameLine() && Math.Abs(dis3_2.x) <= card.attackDistance && Math.Abs(dis3_2.y) <= card.attackDistance);
        }
        else
        {
            if (area.name == "hero2")
            {
                IntVector2 dis3 = IntVector2.getDisplacement(new IntVector2(current_area), Hero2vec1);
                IntVector2 dis3_2 = IntVector2.getDisplacement(new IntVector2(current_area), Hero2vec2);
                return (dis3.onSameLine() && Math.Abs(dis3.x) <= card.attackDistance && Math.Abs(dis3.y) <= card.attackDistance) || (dis3_2.onSameLine() && Math.Abs(dis3_2.x) <= card.attackDistance && Math.Abs(dis3_2.y) <= card.attackDistance);
            }
        }

        return false;
    }

    //area2 - area1, 得到  (2, -2) etc.
    public static IntVector2 getDisplacement(GameObject area1, GameObject area2)
    {
        return IntVector2.getDisplacement(new IntVector2(area1), new IntVector2(area2));
    }

    public static int getAbsoluteDistance(GameObject area1, GameObject area2)
    {
        return IntVector2.getAbsoluteDistance(new IntVector2(area1), new IntVector2(area2));
    }

    public CardAvator getCardById(int id)
    {
        Transform area = this.transform.Find("area" + id);
        if (area && area.childCount > 0)
        {
            return area.GetChild(0).GetComponent<CardAvator>();
        }
        else
        {
            return null;
        }
    }

    public CardAvator cardOn(CardAvator card, Direct dir)
    {
        int id = getID(card.transform.parent.gameObject);
        switch (dir)
        {
            case Direct.Top:
                if (id < 8)
                {
                    return null;
                }
                return getCardById(id - 8);
            case Direct.Bottom:
                if (id > 23)
                {
                    return null;
                }
                return getCardById(id + 8);
            case Direct.Left:
                if (id % 8 == 0)
                {
                    return null;
                }
                return getCardById(id - 1);
            case Direct.Right:
                if (id % 8 == 7)
                {
                    return null;
                }
                return getCardById(id + 1);
        }
        return null;
    }

    //获得area的ID
    public static int getID(GameObject area)
    {
        return int.Parse(area.name.Substring(4));
    }

    public void UpdateShow()
    {
        for(int i = 0; i < areas.Length; i++)
        {
            //若有卡牌被移动，则回复到原来的位置
            if (areas[i].transform.childCount > 0)
            {
                //Vector3 toPosition = areas[i].transform.position;
                //iTween.MoveTo(areas[i].transform.GetChild(0).gameObject, toPosition, 0.5f);
                areas[i].transform.GetChild(0).GetComponent<CardAvator>().ResetArea();
                areas[i].transform.GetChild(0).GetComponent<CardAvator>().ResetPos();
                areas[i].transform.GetChild(0).GetComponent<CardAvator>().ResetShow();

                //评定可否移动
                /*
                if (!areas[i].transform.GetChild(0).GetComponent<CardAvator>().isHero1 || (!areas[i].transform.GetChild(0).GetComponent<CardAvator>().canDoAttack && !areas[i].transform.GetChild(0).GetComponent<CardAvator>().canDoMove))
                {
                    areas[i].transform.GetChild(0).GetComponent<DraggableAvator>().enabled = false;
                }
                else
                {
                    areas[i].transform.GetChild(0).GetComponent<DraggableAvator>().enabled = true;
                }
                */
            }
        }
    }

    //用于回合初
    public void changeAllAvatorsStatus(bool isEnable,bool isHero1 = true)
    {
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].transform.childCount > 0)
            {
                if (areas[i].transform.GetChild(0).GetComponent<CardAvator>().isHero1 == isHero1)
                {
                    //洗脑判定
                    if (isEnable && areas[i].transform.GetChild(0).GetComponent<CardAvator>().underdoBrainwashing)
                    {
                        areas[i].transform.GetChild(0).GetComponent<CardAvator>().underdoBrainwashing = false;
                        areas[i].transform.GetChild(0).GetComponent<CardAvator>().canDoAttack = false;
                        areas[i].transform.GetChild(0).GetComponent<CardAvator>().canDoMove = false;
                    }
                    else
                    {
                        areas[i].transform.GetChild(0).GetComponent<CardAvator>().canDoAttack = isEnable;
                        areas[i].transform.GetChild(0).GetComponent<CardAvator>().canDoMove = isEnable;
                    }
                    areas[i].transform.GetChild(0).GetComponent<CardAvator>().moved = false;
                    areas[i].transform.GetChild(0).GetComponent<CardAvator>().attacked = false;
                    areas[i].transform.GetChild(0).GetComponent<CardAvator>().ResetShow();
                }
            }

        }
        UpdateShow();
    }

    //关掉所有的可拖拽
    public void stopAllAvatorDraggable()
    {
        UpdateShow();
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].transform.childCount > 0)
            {
                areas[i].transform.GetChild(0).GetComponent<UISprite>().color = new Color(0.5F, 0.5F, 0.5F);
                areas[i].transform.GetChild(0).GetComponent<DraggableAvator>().enabled = false;
            }

        }
        
    }
    //打开特定的avators
    public void enableSpecificButtons(List<CardAvator> avators)
    {
        stopAllAvatorDraggable();
        for (int i =0; i < avators.Count; i++)
        {
            avators[i].enableButton();
        }
        IsButtonsOnUse = true;
        IsButtonClicked = false;
    }

    public void disableSpecificButtons(List<CardAvator> avators)
    {
        for (int i = 0; i < avators.Count; i++)
        {
            avators[i].disableButton();
        }
        UpdateShow();
    }

    public void lightenMap(CardAvator card)
    {
        if (CanAttackBase(card, this.transform.parent.Find("hero1").gameObject))
        {
            this.transform.parent.Find("hero1").GetComponent<UISprite>().color = new Color(1F, 0.5F, 0.5F);
        }
        if (CanAttackBase(card, this.transform.parent.Find("hero2").gameObject))
        {
            this.transform.parent.Find("hero2").GetComponent<UISprite>().color = new Color(1F, 0.5F, 0.5F);
        }
        for (int i = 0; i < areas.Length; i++)
        {
            if (CanAttack(card, areas[i]))
            {
                areas[i].GetComponent<UISprite>().spriteName = "100x125 white";
                areas[i].GetComponent<UISprite>().color = new Color(1F, 0.5F, 0.5F);
            }
            else if(CanMove(card, areas[i]))
            {
                areas[i].GetComponent<UISprite>().spriteName = "100x125 white";
                areas[i].GetComponent<UISprite>().color = new Color(0.5F, 1F, 1F);
            }

        }
    }

    public void lightenSetMap(Card card)
    {
        for (int i = 0; i < areas.Length; i++)
        {
            if (CanSet(card, areas[i]))
            {
                areas[i].GetComponent<UISprite>().spriteName = "100x125 white";
                areas[i].GetComponent<UISprite>().color = new Color(0.5F, 1F, 0.5F);
            }

        }
    }

    public void inlightenMap()
    {
        this.transform.parent.Find("hero1").GetComponent<UISprite>().color = new Color(1F, 1F, 1F);
        this.transform.parent.Find("hero2").GetComponent<UISprite>().color = new Color(1F, 1F, 1F);
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].GetComponent<UISprite>().spriteName != "blank")
            {
                areas[i].GetComponent<UISprite>().spriteName = "blank";
            }
           

        }
    }

    public List<CardAvator> getAllActiveAvators(bool isHero1, bool all = false)
    {
        List<CardAvator> outs = new List<CardAvator>();
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].transform.childCount > 0 && areas[i].transform.GetChild(0).GetComponent<CardAvator>().isHero1 == isHero1)
            {
                if (all || areas[i].transform.GetChild(0).GetComponent<CardAvator>().cardType == CardType.CharacterCard)
                    outs.Add(areas[i].transform.GetChild(0).GetComponent<CardAvator>());
            }

        }
        return outs;
    }

    public List<CardAvator> getAllActiveAvatorsOfBothHerosByType(CardType type = CardType.CharacterCard)
    {
        List<CardAvator> outs = new List<CardAvator>();
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].transform.childCount > 0)
            {
                if (areas[i].transform.GetChild(0).GetComponent<CardAvator>().cardType == type)
                    outs.Add(areas[i].transform.GetChild(0).GetComponent<CardAvator>());
            }

        }
        return outs;
    }

    public List<CardAvator> findAvatorsBySkill(Skill skill)
    {
        List<CardAvator> outs = new List<CardAvator>();
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].transform.childCount > 0)
            {
                if (areas[i].transform.GetChild(0).GetComponent<CardAvator>().hasSkill(skill))
                    outs.Add(areas[i].transform.GetChild(0).GetComponent<CardAvator>());
            }

        }
        return outs;
    }
}
