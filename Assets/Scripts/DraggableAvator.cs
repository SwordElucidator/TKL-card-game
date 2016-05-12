﻿using UnityEngine;
using System.Collections;

public class DraggableAvator : UIDragDropItem
{
    
    protected override void OnDragDropRelease(GameObject surface)
    {
        bool updated = false;
        if (this.transform.parent.parent.name != "FightCard")
        {
            return;
        }
        if (this.transform.parent.parent.parent.FindChild("My Card").GetComponent<MyCard>().isHero1 != this.GetComponent<CardAvator>().isHero1)
        {
            return;
        }

        base.OnDragDropRelease(surface);
        this.transform.parent.parent.GetComponent<Areas>().inlightenMap();

        if (surface.name == "hero1" || surface.name == "hero2")
        {
            if (Areas.CanAttackBase(this.GetComponent<CardAvator>(), surface))
            {
                Areas.AttackBase(this.GetComponent<CardAvator>(), surface);
                updated = true;
            }
        }else
        {
            if (surface.transform.parent && surface.transform.parent.name != "FightCard" &&
            surface.transform.parent.parent && surface.transform.parent.parent.name == "FightCard")
            {
                surface = surface.transform.parent.gameObject;
            }

            if (surface != null && Areas.CanAttack(this.GetComponent<CardAvator>(), surface))
            {
                Areas.Attack(this.GetComponent<CardAvator>(), surface);
                updated = true;
            }
            else if (surface != null && Areas.CanMove(this.GetComponent<CardAvator>(), surface))
            {
                Areas.Move(this.GetComponent<CardAvator>(), surface);
            }
        }
        //attack这种需要播放动画的类型会自行updateshow
        if (updated)
        {
            return;
        }

        //当父类有fightcard，意味着不是瞎找的地方
        if (surface != null && surface.transform.parent && surface.transform.parent.name == "FightCard")
        {
            surface.transform.parent.GetComponent<Areas>().UpdateShow();
        }
        else if (this.transform.parent)
        {
            //当本牌死亡时这个会没有，所以才需要第一个判定
            this.transform.parent.parent.GetComponent<Areas>().UpdateShow();
        }
        else
        {
            //special bug case 一般没有既是瞎找又造成本牌死亡的情况。
        }
    }

    protected override void OnDragDropMove(Vector2 delta)
    {

    }


    protected override void OnDragDropStart()
    {
        if (this.transform.parent.parent.name != "FightCard")
        {
            return;
        }
        

        base.OnDragDropStart();
        this.GetComponent<CardAvator>().PlaySound("attack");
        this.GetComponent<UIWidget>().width = 100;
        this.GetComponent<UIWidget>().depth = 100;
        this.GetComponent<CardAvator>().ResetPos();
        this.transform.parent.parent.GetComponent<Areas>().lightenMap(this.GetComponent<CardAvator>());
    }
}
