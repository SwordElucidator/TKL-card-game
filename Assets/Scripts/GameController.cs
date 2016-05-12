using UnityEngine;
using System.Collections;
using System.Linq;

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

    public float cycleTime = 60f;

    

    private UISprite wickropeSprite;
    private SoundController soundController;
    private float timer = 0;
    private float wickropeLength;
    public bool isCurrentTurnHero1 = true;//当前回合英雄
    private CardGenerator cardGenerator;

    void Awake()
    {
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
                soundController.clearWaitList();
                endButton.clicked = false;
                timer = 0;
                wickropeSprite.width = 0;
                disableAllPlayerMovement(isCurrentTurnHero1);
                isCurrentTurnHero1 = !isCurrentTurnHero1;
                enableAllPlayerMovement(isCurrentTurnHero1);
                //给一张牌

                //AI模式
                if (!isCurrentTurnHero1)
                {
                    StartCoroutine(GenerateCardForHero2());
                    StartCoroutine(EnemyAITurn());
                }else
                {
                    StartCoroutine(GenerateCardForHero1());
                    endButton.enableButton();
                }
            }
            else if (cycleTime - timer <= 15f)//如果当前时间小于15s
            {
                //显示绳子动画
                wickropeSprite.width = (int)((cycleTime - timer) / 15f * wickropeLength);
            }
        }
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
