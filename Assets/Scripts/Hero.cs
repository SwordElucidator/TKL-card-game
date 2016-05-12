using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
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

    public void Heal(int hp)
    {
        hpCount += hp;
        if (hpCount >= maxHp)
        {
            hpCount = maxHp;
        }
        hpLabel.text = hpCount + "";
    }
}
