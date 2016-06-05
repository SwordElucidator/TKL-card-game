using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour
{
    public bool isHero1 = true;

    public string heroName;

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
    public int hpCount = 30;

    private UISprite animatorSprite;

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
        //如果并非是正确的hero1和hero2的位置的话，应当调整这俩的名字
        if (PlayerPrefs.GetInt("isClientHero1") == 0)
        {
            if (this.gameObject.name == "hero1")
            {
                this.gameObject.name = "hero2";
            }else if (this.gameObject.name == "hero2")
            {
                this.gameObject.name = "hero1";
            }
        }
        sprite = this.GetComponent<UISprite>();
        hpLabel = this.transform.Find("hp").GetComponent<UILabel>();
        animatorSprite = this.transform.Find("animator").GetComponent<UISprite>();
        if (this.gameObject.name == "hero1")
        {
            isHero1 = true;
        }else if (this.gameObject.name == "hero2")
        {
            isHero1 = false;
        }
    }

    

    void Start()
    {
        heroName = PlayerPrefs.GetString(isHero1 ? "hero1" : "hero2");
        sprite.spriteName = "home_" + heroName;
    }

    public void playAnimation(string name, int frameRate)
    {

        animatorSprite.GetComponent<UISpriteAnimation>().enabled = true;
        GameObject atlas = Resources.Load<GameObject>("Effects/" + name);
        animatorSprite.atlas = atlas.GetComponent<UIAtlas>();
        animatorSprite.spriteName = animatorSprite.atlas.spriteList[0].name;
        animatorSprite.GetComponent<UISpriteAnimation>().framesPerSecond = frameRate;
        animatorSprite.GetComponent<UISpriteAnimation>().RebuildSpriteList();
        animatorSprite.GetComponent<UISpriteAnimation>().ResetToBeginning();
        animatorSprite.GetComponent<UISpriteAnimation>().Play();
    }

    public void playGameover()
    {

        StartCoroutine(gameOverAnimation());
    }

    private IEnumerator gameOverAnimation()
    {
        this.shake(2f, 0.04f);
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(this.gameObject);
        //TODO animation
        GameObject.Find("GameController").GetComponent<GameController>().returnToMainPage();
    }

    public void Reset()
    {
        hpLabel.text = hpCount + "";
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
            return GameObject.Find("mycard1").GetComponent<MyCard>();
        }else
        {
            return GameObject.Find("mycard2").GetComponent<MyCard>();
        }
    }

    public void askForAvator(List<CardAvator> avators)
    {
        if (Areas.IsButtonsOnUse || avators.Count == 0)
        {
            return;
        }
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
        getMyCard().changeAllCardsStatus(GameController.isCurrentTurnHero1 == getMyCard().isHero1);
    }


    public void shake(float time, float ratio)
    {
        iTween.ShakePosition(this.gameObject, new Vector3(ratio, ratio, 0), time);
    }
}
