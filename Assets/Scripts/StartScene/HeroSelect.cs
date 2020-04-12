using System.Collections;
using UnityEngine;

namespace StartScene
{
    public class HeroSelect : MonoBehaviour {

        public TweenAlpha selectHeroAlpha;

        private UISprite selectHeroSprate;
        private UILabel selectHeroName;

        // Use this for initialization

        private void Awake()
        {
            var heroZero = this.transform.parent.parent.Find("Hero zero");
            selectHeroSprate = heroZero.Find("hero0").GetComponent<UISprite>();
            selectHeroName = heroZero.Find("hero_name").GetComponent<UILabel>();
        }

        void OnClick()
        {
            GameObject.Find("BGMScript").GetComponent<BGMLoader>().playOnSelectHero(this.gameObject.name);
            StartCoroutine(ChangeAnimate());
        }
        
        //hero0的变换animate
        IEnumerator ChangeAnimate()
        {
            selectHeroAlpha.PlayForward();
            yield return new WaitForSeconds(0.3f);

            string heroName = this.gameObject.name;

            //修改hero0的属性
            selectHeroSprate.spriteName = heroName;
            char heroIndexChar = heroName[heroName.Length - 1];
            int heroIndex = heroIndexChar - '0';
            selectHeroName.text = Hero.HeroChineseNames[heroIndex - 1];
            yield return new WaitForSeconds(0.5f);
            selectHeroAlpha.ResetToBeginning();

        }
    }
}
