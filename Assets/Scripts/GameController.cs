using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public enum GameState
{
    CardGenerating,//随即发牌
    PlayCard,//出牌
    End//游戏结束阶段
}

public class GameController : MonoBehaviour {
    public GameState gamestate = GameState.CardGenerating;
    public MyCard myCard;
    public MyCard enemyCard;
    public Areas areas;
    public EndButton endButton;
    public Hero1 hero1;
    public Hero2 hero2;
    public static CardAvator FAKEAV = new CardAvator();
    public static bool ONCHANGE = false;

    public Dictionary<CardAvator, List<Skill>> skillList = new Dictionary<CardAvator, List<Skill>>();

    public float cycleTime = 60f;

    

    private UISprite wickropeSprite;
    private SoundController soundController;
    private float timer = 0;
    private float wickropeLength;
    public bool isCurrentTurnHero1 = true;//当前回合英雄
    private CardGenerator cardGenerator;

    public static bool triggerSkillDone = true;

    void Awake()
    {
        skillList = new Dictionary<CardAvator, List<Skill>>();
        skillList[FAKEAV] = new List<Skill>();
        wickropeSprite = this.transform.Find("wickrope").GetComponent<UISprite>();
        wickropeLength = wickropeSprite.width;
        wickropeSprite.width = 0;
        soundController = GameObject.Find("FightCard").GetComponent<SoundController>();
        this.cardGenerator = this.GetComponent<CardGenerator>();
        //命令cardGenerator制作卡牌
        this.cardGenerator.generateCardsForCurrentPlayers(PlayerPrefs.GetString("hero1"), PlayerPrefs.GetString("hero2"));
        //给当前回合的英雄发起始牌 默认是hero1
        StartCoroutine(GenerateCardForHero1(3));
        //给另一个英雄发起始牌
        StartCoroutine(GenerateCardForHero2(3));
        gamestate = GameState.PlayCard;
    }


    void Update()
    {
        //当游戏处于出牌阶段，才会开始计时
        if (gamestate == GameState.PlayCard)
        {

            timer += Time.deltaTime;
            if (timer > cycleTime || endButton.clicked)
            {

                //结束回合并进行下一个回合
                StartCoroutine(doTurnChange());
            }
            else if (cycleTime - timer <= 15f)//如果当前时间小于15s
            {
                //显示绳子动画
                wickropeSprite.width = (int)((cycleTime - timer) / 15f * wickropeLength);
            }
        }
    }

    private IEnumerator doTurnChange()
    {
        //结束回合并进行下一个回合
        ONCHANGE = true;
        soundController.clearWaitList();
        endButton.clicked = false;
        timer = 0;
        wickropeSprite.width = 0;
        disableAllPlayerMovement(isCurrentTurnHero1);


        //应当结算currentHero所有牌的回合结算阶段
        while (!triggerSkillDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        triggerSkillDone = false;
        StartCoroutine(triggerTurnSkills(TriggerEvent.OnTurnEnd));
        while (!triggerSkillDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isCurrentTurnHero1 = !isCurrentTurnHero1;
        triggerSkillDone = false;
        //应当结算currentHero所有牌的回合开始阶段
        StartCoroutine(triggerTurnSkills(TriggerEvent.OnTurnStart));
        while (!triggerSkillDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        enableAllPlayerMovement(isCurrentTurnHero1);

        //给一张牌

        //AI模式
        if (!isCurrentTurnHero1)
        {
            StartCoroutine(GenerateCardForHero2());
            StartCoroutine(EnemyAITurn());
        }
        else
        {
            StartCoroutine(GenerateCardForHero1());
            endButton.enableButton();
        }
        ONCHANGE = false;
    }

    public IEnumerator triggerTurnSkills(TriggerEvent e)
    {
        CardAvator[] validcas = skillList.Keys.ToArray();
        for (int j = 0; j < validcas.Length; j++)
        {
            List<Skill> lst = getSkillsOn(e, validcas[j]);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].canTrigger(validcas[j], (object)isCurrentTurnHero1, e))
                    {
                        lst[i].OnTrigger(validcas[j], (object)isCurrentTurnHero1, e);
                        yield return new WaitForSeconds(lst[i].yieldtime);
                    }
                }
            }
        }
        triggerSkillDone = true;
    }


    public void addSkillsFromCard(CardAvator card)
    {
        skillList.Add(card, card.skills);
        if (card.skills == null)
        {
            return;
        }
        for (int i = 0; i < card.skills.Count; i++)
        {
            if (card.skills[i].global)
            {
                skillList[FAKEAV].Add(card.skills[i]);
            }
        }
    }

    public bool removeSkillsFromCard(CardAvator card)
    {
        if (card.skills != null)
        {
            for (int i = 0; i < card.skills.Count; i++)
            {
                if (card.skills[i].global)
                {
                    skillList[FAKEAV].Remove(card.skills[i]);
                }
            }
        }
        
        if (skillList.ContainsKey(card))
        {
            return skillList.Remove(card);
        }else
        {
            MonoBehaviour.print("Alert: did not find skill to delete for avator " + card.spriteName + card.avatorId);
            return false;
        }
        
    }

    //每一个相同的技能应当只被调用一次，也就是如果多个牌需要调用相同函数的时候，这个函数应当可以被应用于所有的技能拥有者
    public List<Skill> getSkillsOn(TriggerEvent e, CardAvator card = null)
    {
        List<Skill> list = new List<Skill>();
        if (card && skillList[card] != null)
        {
            for (int i =0; i < skillList[card].Count; i++)
            {
                if (skillList[card][i].events.Contains(e))
                    list.Add(skillList[card][i]);
            }
        }
        //null means global checking skills  TODO
        for (int i = 0; i < skillList[FAKEAV].Count; i++)
        {
            if (skillList[FAKEAV][i].events.Contains(e))
                list.Add(skillList[FAKEAV][i]);
        }
        
        return list.Distinct().ToList();
    }

    private void disableAllPlayerMovement(bool isHero1 = true)
    {
        areas.changeAllAvatorsStatus(false, isHero1);
        if (isHero1)
        {
            myCard.changeAllCardsStatus(false);
        }else
        {
            enemyCard.changeAllCardsStatus(false);
        }
    }

    private void enableAllPlayerMovement(bool isHero1 = true)
    {
        areas.changeAllAvatorsStatus(true, isHero1);
        if (isHero1)
        {
            myCard.changeAllCardsStatus(true);
        }
        else
        {
            enemyCard.changeAllCardsStatus(true);
        }
    }

    private IEnumerator GenerateCardForHero1(int num = 1)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject cardGo = cardGenerator.RandomGenerateCard();//调用方法随即生成一个卡牌//要等两秒
            yield return new WaitForSeconds(0.5f);
            //放入mycard
            myCard.AddCard(cardGo);
        }
        myCard.UpdateShow();
    }

    private IEnumerator GenerateCardForHero2(int num = 1)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject cardGo = cardGenerator.RandomGenerateCard(false);//调用方法随即生成一个卡牌//要等两秒
            yield return new WaitForSeconds(0.5f);
            //放入mycard
            enemyCard.AddCard(cardGo);
        }
        myCard.UpdateShow();
    }


    private IEnumerator EnemyAITurn()
    {
        yield return new WaitForSeconds(1f);
        //TODO 测试用，给hero2的牌扔到场上
        System.Random rnd = new System.Random();
        var numbers = Enumerable.Range(0, 16).OrderBy(r => rnd.Next()).ToArray();
        for (int i = 0; i < 16; i++)
        {
            if (enemyCard.hasCardLeft() && Areas.CanSet(enemyCard.getRandomCard().GetComponent<Card>(), areas.getArea(numbers[i]), false))
            {
                Areas.Set(enemyCard.getRandomCard().GetComponent<Card>(), areas.getArea(numbers[i]));
                yield return new WaitForSeconds(1f);
            }
        }
        endButton.enemyButtonClick();
    }

    public void returnToMainPage()
    {
        this.GetComponent<BGMController>().Stop();
        soundController.Stop();
        PlayerPrefs.SetInt("from_scene", 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
