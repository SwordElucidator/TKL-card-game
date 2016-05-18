using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour
{
    public bool isHero1 = true;

    public static string[] HeroNames = {
        "Touhou",
        "Kancolle",
        "Divas",
        "JK",
        };

    public static string[] HeroChineseNames = {
        "东方project",
        "舰娘",
        "歌姬",
        "女子高中生",
        };

    public int maxHp = 30;
    public int minHp = 0;

    protected UISprite sprite;
    private UILabel hpLabel;
    private int hpCount = 30;
    //test function
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            TakeDamage(Random.Range(1, 5));
        }
        if (Input.GetKey(KeyCode.R))
        {
            Heal(Random.Range(1, 5));
        }
    }

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
        hpLabel = this.transform.Find("hp").GetComponent<UILabel>();
    }

    public void TakeDamage(int damage)
    {
        hpCount -= damage;
        hpLabel.text = hpCount + "";
        if (hpCount <= minHp)
        {
            //game over
        }
    }

    public void changeMaxHp(int newMax)
    {
        maxHp = newMax;
        if (hpCount > maxHp)
        {
            hpCount = maxHp;
            hpLabel.text = hpCount + "";
        }
    }

    public void Heal(int hp)
    {
        hpCount += hp;
        if (hpCount >= maxHp)
        {
            hpCount = maxHp;
        }
        hpLabel.text = hpCount + "";
    }

    public Areas getAreas()
    {
        return GameObject.Find("FightCard").GetComponent<Areas>();
    }

    public MyCard getMyCard()
    {
        if (isHero1)
        {
            return GameObject.Find("My Card").GetComponent<MyCard>();
        }else
        {
            return GameObject.Find("Enemy Card").GetComponent<MyCard>();
        }
    }

    public void askForAvator(List<CardAvator> avators)
    {
        if (Areas.IsButtonsOnUse || avators.Count == 0)
        {
            return;
        }
        GameController.triggerSkillDone = false;
        getAreas().enableSpecificButtons(avators);
        getMyCard().changeAllCardsStatus(false);
        StartCoroutine(waitForButtonClicked(avators));

    }

    private IEnumerator waitForButtonClicked(List<CardAvator> avators)
    {
        while (!Areas.IsButtonClicked)
        {
            if (GameController.ONCHANGE)
            {
                Areas.ClickedAvator = avators[Random.Range(0, avators.Count)];
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        Areas.IsButtonClicked = false;
        Areas.IsButtonsOnUse = false;

        getAreas().disableSpecificButtons(avators);
        getMyCard().changeAllCardsStatus(GameObject.Find("GameController").GetComponent<GameController>().isCurrentTurnHero1);
        GameController.triggerSkillDone = true;
    }
}
