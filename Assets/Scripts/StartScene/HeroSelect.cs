using UnityEngine;
using System.Collections;

public class HeroSelect : MonoBehaviour {

    public TweenAlpha selectHeroAlpha;

    public BGMLoader bgmloader;

    private UISprite selectHeroSprate;
    private UILabel selectHeroName;

	// Use this for initialization

    void Awake()
    {
        selectHeroSprate = this.transform.parent.parent.Find("Hero zero").Find("hero0").GetComponent<UISprite>();
        selectHeroName = this.transform.parent.parent.Find("Hero zero").Find("hero_name").GetComponent<UILabel>();
    }

	void OnClick()
    {
        GameObject.Find("BGMScript").GetComponent<BGMLoader>().playOnSelectHero(this.gameObject.name);
        StartCoroutine(changeAnimate());
    }
    //hero0的变换animate
    IEnumerator changeAnimate()
    {
        selectHeroAlpha.PlayForward();
        yield return new WaitForSeconds(0.3f);

        string heroname = this.gameObject.name;

        //修改hero0的属性
        selectHeroSprate.spriteName = heroname;
        char heroIndexChar = heroname[heroname.Length - 1];
        int heroIndex = heroIndexChar - '0';
        selectHeroName.text = Hero.HeroChineseNames[heroIndex - 1];
        yield return new WaitForSeconds(0.5f);
        selectHeroAlpha.ResetToBeginning();

    }
}
