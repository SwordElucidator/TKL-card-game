using UnityEngine;
using System.Collections;

public class CardAvator : CardBase
{
    public bool isHero1;

    public bool canDoMove = false;
    public bool canDoAttack = false;

    public TweenPosition attackTween;

    private UISprite sprite;
    private UILabel hpLabel;
    private UILabel damageLabel;
    private UILabel attackDistanceLabel;
    private SoundController soundController;

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
        hpLabel = transform.Find("hp_num").GetComponent<UILabel>();
        damageLabel = transform.Find("damage_num").GetComponent<UILabel>();
        attackDistanceLabel = transform.Find("attackDistance_num").GetComponent<UILabel>();
        soundController = GameObject.Find("FightCard").GetComponent<SoundController>();
    }

    void OnHover(bool isHovered)
    {
        if (isHovered)
        {
            DesCard._instance.ShowCard(spriteName);
        }
        else
        {
            DesCard._instance.HideCard();
        }
    }

    public void ResetPos()//更新血量伤害的位置
    {
        damageLabel.GetComponent<UIAnchor>().enabled = true;
        hpLabel.GetComponent<UIAnchor>().enabled = true;
        attackDistanceLabel.GetComponent<UIAnchor>().enabled = true;
    }

    public void ResetShow()
    {//更新血量伤害的显示 更新sprite显示
        damageLabel.text = damage + "";
        hpLabel.text = hp + "";
        attackDistanceLabel.text = attackDistance + "";
        this.GetComponent<UISprite>().spriteName = spriteName + "_avator";
        if (!canDoAttack && !canDoMove)
        {
            this.GetComponent<UISprite>().color = new Color(0.5F, 0.5F, 0.5F);
        }
        else if (!canDoMove)
        {
            this.GetComponent<UISprite>().color = new Color(1F, 0.5F, 0.5F);
        }else if (!canDoAttack)
        {
            this.GetComponent<UISprite>().color = new Color(0.5F, 1F, 1F);
        }else
        {
            this.GetComponent<UISprite>().color = new Color(1F, 1F, 1F);
        }
    }


    public void InheritFromCard(Card card)
    {
        cost = card.cost;
        damage = card.damage;
        hp = card.hp;
        attackDistance = card.attackDistance;
        cardName = card.cardName;
        heroName = card.heroName;
        spriteName = card.spriteName;
        typeAge = card.typeAge;
        typeCharacter = card.typeCharacter;
        typeMove = card.typeMove;
        cardType = card.cardType;
        skills = card.skills;
        hasCharge = card.hasCharge;
        if (hasCharge)
        {
            canDoMove = true;
            canDoAttack = true;
        }else
        {
            canDoMove = false;
            canDoAttack = false;
        }
        hasRush = card.hasRush;
        isHero1 = card.isHero1;
        if (!isHero1)
        {
            this.GetComponent<UISprite>().flip = UIBasicSprite.Flip.Both;
        }
        this.GetComponent<UISprite>().spriteName = card.spriteName + "_avator";
        ResetShow();
    }

    public void MoveTo(int area_id)
    {
        //this.transform.parent
    }

    public void Attack(CardAvator card, bool can_counter = true)
    {
        StartCoroutine(attackAnime(card, can_counter));
        
    }

    

    private IEnumerator attackAnime(CardAvator card, bool can_counter = true) {
        //播放攻击动画
        Vector3 toPos = card.transform.parent.localPosition - this.transform.parent.localPosition;
        attackTween.to = toPos;
        attackTween.PlayForward();
        yield return new WaitForSeconds(0.62f);

        card.hp -= this.damage;
        if (can_counter)
        {
            this.hp -= card.damage;
        }
        this.ResetShow();
        card.ResetShow();
        yield return new WaitForSeconds(1f);
        attackTween.ResetToBeginning();
        //结束后归位
        this.transform.parent.parent.GetComponent<Areas>().UpdateShow();

        if (card.hp <= 0)
        {
            //TODO 播放死亡动画
            if (this.hp > 0)
            {
                card.PlaySound("out");
            }
            card.transform.parent.DestroyChildren();
        }
        if (this.hp <= 0)
        {
            //TODO 播放死亡动画
            this.PlaySound("out");
            this.transform.parent.DestroyChildren();
        }
    }

    public void AttackBase()
    {
        StartCoroutine(attackBaseAnime());

    }

    private IEnumerator attackBaseAnime()
    {
        //播放攻击动画
        Transform toHero;
        if (this.isHero1)
        {
            toHero = this.transform.parent.parent.parent.Find("hero2");
        }else
        {
            toHero = this.transform.parent.parent.parent.Find("hero1");
        }
        attackTween.to = toHero.localPosition - this.transform.parent.localPosition;
        attackTween.PlayForward();
        yield return new WaitForSeconds(0.62f);

        int newHp = int.Parse(toHero.Find("hp").GetComponent<UILabel>().text) - this.damage;
        toHero.Find("hp").GetComponent<UILabel>().text = newHp + "";
        if (newHp <= 0)
        {
            //gameover判定

        }
        

        this.ResetShow();
        yield return new WaitForSeconds(1f);
        attackTween.ResetToBeginning();
        //结束后归位
        this.transform.parent.parent.GetComponent<Areas>().UpdateShow();
    }

    public void PlaySound(string type)
    {
        if (soundController.getWaitLength() > 0)
        {
            return;
        }
        if (type == "attack" && soundController.isPlaying())
        {
            return;
        }

        string path = "Music/Sounds/" + this.spriteName + "_" + type;
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip)
        {
            soundController.playSound(clip);
        }
    }
}
