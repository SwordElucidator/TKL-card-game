﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardAvator : CardBase
{
    public static int totalId = 0;
    public bool isHero1;
    public int avatorId;
    public bool canDoMove = false;
    public bool canDoAttack = false;
    public bool underdoBrainwashing = false;

    public bool canDirectlyAttackHero = false;

    private TweenPosition thisTweenPosition;
    private TweenScale thisTweenScale;

    private TweenPosition attackTweenPosition;
    private TweenScale attackTweenScale;

    private TweenPosition avatorInTweenPosition;
    private TweenScale avatorInTweenScale;

    private UISprite sprite;
    private UILabel hpLabel;
    private UILabel damageLabel;
    private UILabel attackDistanceLabel;
    private SoundController soundController;
    private UISprite animatorSprite;

    void Awake()
    {
        avatorId = totalId;
        totalId += 1;
        sprite = this.GetComponent<UISprite>();
        hpLabel = transform.Find("hp_num").GetComponent<UILabel>();
        damageLabel = transform.Find("damage_num").GetComponent<UILabel>();
        attackDistanceLabel = transform.Find("attackDistance_num").GetComponent<UILabel>();
        soundController = GameObject.Find("FightCard").GetComponent<SoundController>();
        animatorSprite = this.transform.Find("animator").GetComponent<UISprite>();
        attackTweenPosition = GameObject.Find("AvatorAttack").GetComponent<TweenPosition>();
        attackTweenScale = GameObject.Find("AvatorAttack").GetComponent<TweenScale>();
        avatorInTweenPosition = GameObject.Find("AvatorIn").GetComponent<TweenPosition>();
        avatorInTweenScale = GameObject.Find("AvatorIn").GetComponent<TweenScale>();
        thisTweenPosition = this.GetComponent<TweenPosition>();
        thisTweenScale = this.GetComponent<TweenScale>();
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
        this.GetComponent< UIWidget >().depth = 2;
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
        maxHp = card.maxHp;
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

    private static bool doDamage(DamageStruct damage)
    {


        CardAvator from = damage.fromCard;
        CardAvator to = damage.toCard;
        List<Skill> lst;
        if (from)
        {
            //trigger OnDamage
            lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.OnDamage, damage.fromCard);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].canTrigger(damage.fromCard, (object)damage, TriggerEvent.OnDamage))
                    {
                        //这里返回值应该决定了是否要终止这次伤害
                        //OnTrigger应当有修正目标的能力 这个修正可以由对damage的直接修正来实现，因为c#的boxing是很厉害的
                        if (lst[i].OnTrigger(damage.fromCard, (object)damage, TriggerEvent.OnDamage))
                        {
                            //说不定要触发一些效果，比如当damage被终止时可能会比较难看

                            return false;
                        }
                    }
                }
            }
        }


        if (to)
        {
            //trigger OnDamaged
            lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.OnDamaged, damage.toCard);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].canTrigger(damage.fromCard, (object)damage, TriggerEvent.OnDamaged))
                    {
                        //这里返回值应该决定了是否要终止这次伤害
                        //OnTrigger应当有修正目标的能力 这个修正可以由对damage的直接修正来实现，因为c#的boxing是很厉害的
                        if (lst[i].OnTrigger(damage.fromCard, (object)damage, TriggerEvent.OnDamaged))
                        {
                            //说不定要触发一些效果，比如当damage被终止时可能会比较难看

                            return false;
                        }
                    }
                }
            }

            if (changeHp(new HpChangeStruct(to, -damage.damage)))
                return true;
        }
        else
        {
            if (changeHp(new HpChangeStruct(damage.toHero1, -damage.damage)))
            {
                return true;
            }
        }
        if (from)
        {
            //trigger Damage
            lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.Damage, damage.fromCard);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].canTrigger(damage.fromCard, (object)damage, TriggerEvent.Damage))
                    {
                        lst[i].OnTrigger(damage.fromCard, (object)damage, TriggerEvent.Damage);
                    }
                }
            }
        }
            
        if (to)
        {
            //trigger Damaged
            lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.Damaged, damage.toCard);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].canTrigger(damage.fromCard, (object)damage, TriggerEvent.Damaged))
                    {
                        lst[i].OnTrigger(damage.fromCard, (object)damage, TriggerEvent.Damaged);
                    }
                }
            }
        }

        

        return false;
    }

    public static bool changeHp(HpChangeStruct change)
    {
        //return true if hp <= 0


        List<Skill> lst;

        if (change.card)
        {
            //trigger OnHpChange
            lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.OnHpChange, change.card);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].canTrigger(change.card, (object)change, TriggerEvent.OnHpChange))
                    {
                        //这里返回值应该决定了是否要终止这次体力变化
                        //OnTrigger应当有修正目标的能力 这个修正可以由对damage的直接修正来实现，因为c#的boxing是很厉害的
                        if (lst[i].OnTrigger(change.card, (object)change, TriggerEvent.OnHpChange))
                        {
                            //说不定要触发一些效果，比如当被终止时可能会比较难看
                            change.card.ResetShow();
                            return false;
                        }
                    }
                }
            }

            change.card.hp += change.value;

            //trigger HpChanged
            lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.HpChanged, change.card);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].canTrigger(change.card, (object)change, TriggerEvent.HpChanged))
                    {
                        lst[i].OnTrigger(change.card, (object)change, TriggerEvent.HpChanged);
                    }
                }
            }
            change.card.ResetShow();
            if (change.card.hp <= 0)
            {
                return true;
            }
        }else
        {
            Transform toHero;
            if (change.isHero1)
            {
                toHero = GameObject.Find("hero1").transform;
            }
            else
            {
                toHero = GameObject.Find("hero2").transform;
            }
            int newHp = int.Parse(toHero.Find("hp").GetComponent<UILabel>().text) + change.value;
            toHero.Find("hp").GetComponent<UILabel>().text = newHp + "";
            if (newHp <= 0)
            {
                return true;
            }
        }
        return false;
    }

    public static void kill(DeathStruct death)
    {
        //TODO 播放死亡动画之类的

        //trigger HpChanged
        List<Skill> lst = GameObject.Find("GameController").GetComponent<GameController>().getSkillsOn(TriggerEvent.OnDying, death.card);
        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].canTrigger(death.card, (object)death, TriggerEvent.HpChanged))
                {
                    lst[i].OnTrigger(death.card, (object)death, TriggerEvent.HpChanged);
                }
            }
        }

        GameObject.Find("GameController").GetComponent<GameController>().removeSkillsFromCard(death.card);
        death.card.transform.parent.DestroyChildren();
    }

    

    private IEnumerator attackAnime(CardAvator card, bool can_counter = true) {
        //播放攻击动画
        this.GetComponent<UIWidget>().width = 80;
        this.ResetPos();
        setUpTween(attackTweenPosition, attackTweenScale);
        Vector3 toPos = card.transform.parent.localPosition - this.transform.parent.localPosition;
        thisTweenPosition.to = toPos;
        thisTweenPosition.PlayForward();
        thisTweenScale.PlayForward();
        yield return new WaitForSeconds(0.85f);
        DamageStruct damage = new DamageStruct(this, card, this.damage);
        DamageStruct damage2 = null;
        bool cardDead = doDamage(damage);
        //try shake animation;
        card.playAnimation("bump", 30);
        card.shake(0.2f, 0.02f);
        bool thisDead = false;
        if (can_counter)
        {
            damage2 = new DamageStruct(card, this, card.damage);
            thisDead = doDamage(damage2);
        }
        this.ResetShow();
        card.ResetShow();
        yield return new WaitForSeconds(0.75f);
        thisTweenPosition.ResetToBeginning();
        thisTweenScale.ResetToBeginning();
        //结束后归位
        this.transform.parent.parent.GetComponent<Areas>().UpdateShow();

        if (cardDead)
        {
            //TODO 播放死亡动画
            if (this.hp > 0)
            {
                card.PlaySound("out");
            }
            kill(new DeathStruct(card, damage));
        }
        if (thisDead)
        {
            //TODO 播放死亡动画
            this.PlaySound("out");
            kill(new DeathStruct(this, damage2));
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

        setUpTween(attackTweenPosition, attackTweenScale);
        thisTweenPosition.to = toHero.localPosition - this.transform.parent.localPosition;
        thisTweenPosition.PlayForward();
        thisTweenScale.PlayForward();
        yield return new WaitForSeconds(0.85f);

        DamageStruct damage = new DamageStruct(this, null, false, !this.isHero1, this.damage);
        bool heroDead = doDamage(damage);
        if (heroDead)
        {
            //gameover判定

        }
        

        this.ResetShow();
        yield return new WaitForSeconds(0.75f);
        thisTweenPosition.ResetToBeginning();
        thisTweenScale.ResetToBeginning();
        //结束后归位
        this.transform.parent.parent.GetComponent<Areas>().UpdateShow();
    }

    public void doSet()
    {
        StartCoroutine(setAnime());
    }

    public IEnumerator setAnime()
    {
        setUpTween(avatorInTweenPosition, avatorInTweenScale);
        this.GetComponent<UIWidget>().depth = 99;
        thisTweenPosition.PlayForward();
        thisTweenScale.PlayForward();
        yield return new WaitForSeconds(1f);
        thisTweenPosition.ResetToBeginning();
        thisTweenScale.ResetToBeginning();
        this.ResetPos();
        this.ResetShow();

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

    public void doBrainwashing()
    {
        underdoBrainwashing = true;
        playAnimation("brain", 40);
        if (changeHp(new HpChangeStruct(this, -1))){
            kill(new DeathStruct(this, null));
        }
        loseDamage(1);
        ResetShow();
    }

    public void doMotivate(int hp, int damage)
    {
        playAnimation("motivate", 40);
        if (changeHp(new HpChangeStruct(this, hp)))
        {
            kill(new DeathStruct(this, null));
        }
        addDamage(1);
        ResetShow();
    }

    public void addDamage(int num)
    {
        damage += num;
    }

    public void loseDamage(int num)
    {
        damage -= num;
        if (damage < 0)
        {
            damage = 0;
        }
    }

    public List<CardAvator> getEnemyCards(bool all = false)
    {
        return this.transform.parent.parent.GetComponent<Areas>().getAllActiveAvators(!this.isHero1, all);
    }

    public List<CardAvator> getSelfCards(bool all = false)
    {
        return this.transform.parent.parent.GetComponent<Areas>().getAllActiveAvators(this.isHero1, all);
    }

    //UI
    private void shake(float time, float ratio)
    {
        iTween.ShakePosition(this.gameObject, new Vector3(ratio, ratio, 0), time);
    }
    
    //do animation, need to be added on the prefab of avator
    private void playAnimation(string name,int frameRate)
    {

        animatorSprite.GetComponent<UISpriteAnimation>().enabled = true;
        GameObject atlas = Resources.Load<GameObject>("Effects/" + name);
        MonoBehaviour.print(atlas);
        animatorSprite.atlas = atlas.GetComponent<UIAtlas>();
        animatorSprite.spriteName = animatorSprite.atlas.spriteList[0].name;
        animatorSprite.GetComponent<UISpriteAnimation>().framesPerSecond = frameRate;
        animatorSprite.GetComponent<UISpriteAnimation>().RebuildSpriteList();
        animatorSprite.GetComponent<UISpriteAnimation>().ResetToBeginning();
        animatorSprite.GetComponent<UISpriteAnimation>().Play();
    }

    private void setUpTween(TweenPosition tp, TweenScale ts)
    {
        thisTweenPosition.from = tp.from;
        thisTweenPosition.to = tp.to;
        thisTweenPosition.style = tp.style;
        thisTweenPosition.animationCurve = tp.animationCurve;
        thisTweenPosition.duration = tp.duration;
        thisTweenPosition.delay = tp.delay;
        thisTweenPosition.tweenGroup = tp.tweenGroup;
        thisTweenPosition.ignoreTimeScale = tp.ignoreTimeScale;
        thisTweenPosition.onFinished = tp.onFinished;

        thisTweenScale.from = ts.from;
        thisTweenScale.to = ts.to;
        thisTweenScale.style = ts.style;
        thisTweenScale.animationCurve = ts.animationCurve;
        thisTweenScale.duration = ts.duration;
        thisTweenScale.delay = ts.delay;
        thisTweenScale.tweenGroup = ts.tweenGroup;
        thisTweenScale.ignoreTimeScale = ts.ignoreTimeScale;
        thisTweenScale.onFinished = ts.onFinished;
    }
}
