using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Networking;

public enum GameState
{
    CardGenerating,//随即发牌
    PlayCard,//出牌
    End//游戏结束阶段
}

public class GameController :  MonoBehaviour{

    public GameState gamestate = GameState.CardGenerating;
    public MyCard myCard1;
    public MyCard myCard2;
    public Areas areas;
    public EndButton endButton;
    public Hero hero1;
    public Hero hero2;
    public static CardAvator FAKEAV = new CardAvator();
    public static bool ONCHANGE = false;

    public static Queue<NextEvent> skillEventsQueue = new Queue<NextEvent>();
    
    public static bool eventTriggering = false;

    public bool animateTriggering = false;

    public Dictionary<CardAvator, List<Skill>> skillList = new Dictionary<CardAvator, List<Skill>>();

    public float cycleTime = 60f;

    

    private UISprite wickropeSprite;
    private SoundController soundController;
    private float timer = 0;
    private float wickropeLength;
    public static bool isCurrentTurnHero1 = true;//当前回合英雄

    private CardGenerator cardGenerator;

    public static bool triggerSkillDone = true;

    void Awake()
    {
        if (PlayerPrefs.GetInt("isClientHero1") == 0)
        {
            MyCard mc = myCard1;
            this.myCard1 = myCard2;
            this.myCard2 = mc;
        }

        Client.isClientHero1 = PlayerPrefs.GetInt("isClientHero1") == 1;
        skillList = new Dictionary<CardAvator, List<Skill>>();
        skillList[FAKEAV] = new List<Skill>();
        wickropeSprite = this.transform.Find("wickrope").GetComponent<UISprite>();
        wickropeLength = wickropeSprite.width;
        wickropeSprite.width = 0;
        MonoBehaviour.print(wickropeSprite.width);
        soundController = GameObject.Find("FightCard").GetComponent<SoundController>();

        skillEventsQueue = new Queue<NextEvent>();
        StartCoroutine(generalEventHandler());



        this.cardGenerator = this.GetComponent<CardGenerator>();
        //命令cardGenerator制作卡牌
        this.cardGenerator.generateCardsForCurrentPlayers(PlayerPrefs.GetString("hero1"), PlayerPrefs.GetString("hero2"));
        //给当前回合的英雄发起始牌 默认是hero1
        StartCoroutine(GenerateCardForHero1(3));
        //给另一个英雄发起始牌
        StartCoroutine(GenerateCardForHero2(4));
        //第一回合应当默认为hero1开始，无论玩家是hero1还是hero2
        isCurrentTurnHero1 = true;
        //应当让另外一个人的所有活动停止 TODO 无法顺利停止
        disableAllPlayerMovement(!isCurrentTurnHero1);
        //应当允许当前hero1的所有行动
        enableAllPlayerMovement(isCurrentTurnHero1);

        gamestate = GameState.PlayCard;

        //如果第一个回合时玩家不是hero1，那么应当由hero1的AI来执行。
        if (!Client.isClientHero1)
        {
            endButton.disable();
            StartCoroutine(EnemyAITurn(2));
        }



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

    private IEnumerator generalEventHandler()
    {
        while (true)
        {
            if (eventTriggering)
            {
                //执行当前的event中

            }else
            {
                if (skillEventsQueue.Count > 0)
                {
                    //从当前的queue中获得一个并执行
                    NextEvent ne = skillEventsQueue.Dequeue();
                    eventTriggering = true;
                    if (ne.isSkill)
                    {
                        ne.oneSkill.trigger();
                        ne.oneSkill.waitForResult();

                        yield return new WaitForSeconds(ne.oneSkill.getYieldTime());
                    }
                    else
                    {
                        ne.oneCall.Call();
                    }
                }
                
            }
            yield return new WaitForSeconds(0.1f);
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
        while (eventTriggering || skillEventsQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        triggerTurnSkills(TriggerEvent.OnTurnEnd);
        while ( eventTriggering || skillEventsQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isCurrentTurnHero1 = !isCurrentTurnHero1;
        //应当结算currentHero所有牌的回合开始阶段
        triggerTurnSkills(TriggerEvent.OnTurnStart);
        while (eventTriggering || skillEventsQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        enableAllPlayerMovement(isCurrentTurnHero1);

        //给一张牌

        //AI模式
        if (!isCurrentTurnHero1)
        {
            //Hero2的turn
            StartCoroutine(GenerateCardForHero2());
            //如果用户是hero1，那么因为现在回合是hero2的turn也就是说要搞AI
            if (Client.isClientHero1)
                StartCoroutine(EnemyAITurn());
            else //如果用户是hero2，那么是正常的turn
                endButton.enableButton();
        }
        else
        {
            //hero1的turn，类似的逻辑
            StartCoroutine(GenerateCardForHero1());
            if (Client.isClientHero1)
                endButton.enableButton();
            else
                StartCoroutine(EnemyAITurn());
        }
        ONCHANGE = false;
    }

    public void triggerTurnSkills(TriggerEvent e)
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
                        GameController.skillEventsQueue.Enqueue(new NextEvent(new OneSkill(lst[i], validcas[j], isCurrentTurnHero1, e)));
                    }
                }
            }
        }
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

        if (card && !skillList.ContainsKey(card))
        {
            MonoBehaviour.print("ALERT: " + card.cardName + "is not in skillList for some reason!");
        }
        if (card && skillList.ContainsKey(card) && skillList[card] != null)
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
        //欲关掉所有的player movement的话，  比如要关掉的牌是hero1的话，那么应当关掉所有相应的mycard1的操作，因为mycard1是hero1的mycard
        if (isHero1)
        {
            myCard1.changeAllCardsStatus(false);
        }else
        {
            myCard2.changeAllCardsStatus(false);
        }
    }

    private void enableAllPlayerMovement(bool isHero1 = true)
    {
        areas.changeAllAvatorsStatus(true, isHero1);
        if (isHero1)
        {
            myCard1.changeAllCardsStatus(true);
        }
        else
        {
            myCard2.changeAllCardsStatus(true);
        }
    }

    private IEnumerator GenerateCardForHero1(int num = 1)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject cardGo = cardGenerator.RandomGenerateCard();//调用方法随即生成一个卡牌//要等两秒
            yield return new WaitForSeconds(0.5f);
            //放入mycard  为hero1生成的牌应当放在mycard1中
            myCard1.AddCard(cardGo);

        }
        myCard1.UpdateShow();
    }

    private IEnumerator GenerateCardForHero2(int num = 1)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject cardGo = cardGenerator.RandomGenerateCard(false);//调用方法随即生成一个卡牌//要等两秒
            yield return new WaitForSeconds(0.5f);
            //放入mycard
            myCard2.AddCard(cardGo);
        }
        myCard2.UpdateShow();
    }


    private IEnumerator EnemyAITurn(float extreWait = 0f)
    {
        yield return new WaitForSeconds(extreWait);
        yield return new WaitForSeconds(1f);
        //TODO 测试用，给enemy的牌扔到场上，不一定是hero2
        System.Random rnd = new System.Random();
        var numbers = Enumerable.Range(0, 32).OrderBy(r => rnd.Next()).ToArray();
        //如果玩家是hero1，那么敌人是mycard2
        MyCard m;
        if (Client.isClientHero1)
        {
            m = myCard2;
        }
        else
        {
            m = myCard1;
        }
         
        for (int i = 0; i < 32; i++)
        {
            if (m.hasCardLeft() && Areas.CanSet(m.getRandomCard().GetComponent<Card>(), areas.getArea(numbers[i])))
            {
                Areas.Set(m.getRandomCard().GetComponent<Card>(), areas.getArea(numbers[i]));
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
