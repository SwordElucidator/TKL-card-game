using System.Collections;
using System.Linq;
using UnityEngine;

namespace StartScene
{
    public class HeroSelect : MonoBehaviour {

        public TweenAlpha selectHeroAlpha;

        private UISprite selectHeroSprite;
        private UILabel selectHeroName;

        // Use this for initialization

        private void Awake()
        {
            var heroZero = this.transform.parent.parent.Find("Hero Zero");
            selectHeroSprite = heroZero.Find("Hero 0").GetComponent<UISprite>();
            selectHeroName = heroZero.Find("Hero Name").GetComponent<UILabel>();
        }

        void OnClick()
        {
            GameObject.Find("BGM Script").GetComponent<BGMLoader>().playOnSelectHero(this.gameObject.name);
            StartCoroutine(ChangeAnimate());
        }
        
        //hero0的变换animate
        IEnumerator ChangeAnimate()
        {
            selectHeroAlpha.PlayForward();
            yield return new WaitForSeconds(0.3f);

            var heroName = this.gameObject.name;

            //修改hero0的属性
            var heroIndexChar = heroName[heroName.Length - 1];
            var heroIndex = heroIndexChar - '0';
            selectHeroSprite.spriteName = "hero" + heroIndexChar;
            selectHeroName.text = Hero.HeroChineseNames[heroIndex - 1];
            yield return new WaitForSeconds(0.5f);
            selectHeroAlpha.ResetToBeginning();

        }
    }
}
