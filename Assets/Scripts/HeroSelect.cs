using UnityEngine;
using System.Collections;

public class HeroSelect : MonoBehaviour {

    private UISprite selectHeroSprate;
    private UILabel selectHeroName;
    private string[] HeroNames = { "吉安娜·普罗德摩尔",
        "雷克萨",
        "乌瑟尔•光明使者",
        "加尔鲁什•地狱咆哮",
        "玛法里奥•怒风",
        "古尔丹",
        "萨尔",
        "安度因•乌瑞恩",
        "瓦莉拉•萨古纳尔" };

	// Use this for initialization

    void Awake()
    {
        selectHeroSprate = this.transform.parent.Find("hero0").GetComponent<UISprite>();
        selectHeroName = this.transform.parent.Find("hero_name").GetComponent<UILabel>();
    }

	void OnClick()
    {
        string heroname = this.gameObject.name;
        selectHeroSprate.spriteName = heroname;
        char heroIndexChar = heroname[heroname.Length - 1];
        int heroIndex = heroIndexChar - '0';
        selectHeroName.text = HeroNames[heroIndex - 1];
    }
}
