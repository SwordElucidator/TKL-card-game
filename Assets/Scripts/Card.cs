using UnityEngine;
using System.Collections;

public class Card : CardBase
{

    public bool isHero1;
    public bool canDoSet = true;

    private UISprite sprite;
    private UILabel hpLabel;
    private UILabel damageLabel;
    private UILabel attackDistanceLabel;
    private UILabel costLabel;

    private string CardName
    {
        get
        {
            if (sprite == null)
            {
                sprite = this.GetComponent<UISprite>();
            }
            return sprite.spriteName;
        }
    }

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
        hpLabel = transform.Find("hp_num").GetComponent<UILabel>();
        damageLabel = transform.Find("damage_num").GetComponent<UILabel>();
        attackDistanceLabel = transform.Find("attackDistance_num").GetComponent<UILabel>();
        costLabel = transform.Find("cost_num").GetComponent<UILabel>();
    }

	void OnHover(bool isHovered)
    {
        if (isHovered)
        {
            DesCard._instance.ShowCard(CardName);
        }else
        {
            DesCard._instance.HideCard();
        }
    }


    public void SetDepth(int depth)
    {
        sprite.depth = depth;
        hpLabel.depth = depth + 1;
        damageLabel.depth = depth + 1;
        costLabel.depth = depth + 1;
        attackDistanceLabel.depth = depth + 1;
    }

    public void ResetPos()//更新血量伤害的位置
    {
        damageLabel.GetComponent<UIAnchor>().enabled = true;
        hpLabel.GetComponent<UIAnchor>().enabled = true;
        attackDistanceLabel.GetComponent<UIAnchor>().enabled = true;
        costLabel.GetComponent<UIAnchor>().enabled = true;
    }

    public void ResetShow()
    {//更新血量伤害的显示
        damageLabel.text = damage + "";
        hpLabel.text = hp + "";
        attackDistanceLabel.text = attackDistance + "";
        costLabel.text = cost + "";
    }

    public void InheritFromCardFile(CardFile card, bool isHero1 = true)
    {
        package = card.package;
        cost = card.cost;
        damage = card.damage;
        damageFly = card.damageFly;
        damageWalk = card.damageWalk;
        damageSail = card.damageSail;
        damageStop = card.damageStop;
        hp = card.hp;
        maxHp = card.hp;
        attackDistance = card.attackDistance;
        cardName = card.cardName;
        heroName = card.heroName;
        spriteName = card.spriteName;
        typeAge = card.typeAge;
        typeCharacter = card.typeCharacter;
        typeMove = card.typeMove;
        cardType = card.cardType;
        hasCharge = card.hasCharge;
        hasRush = card.hasRush;
        //TODO 关于费用之后写
        canDoSet = true;
        this.isHero1 = isHero1;
        skills = card.skills;
        this.GetComponent<UISprite>().spriteName = card.spriteName;
        ResetShow();
    }
}
