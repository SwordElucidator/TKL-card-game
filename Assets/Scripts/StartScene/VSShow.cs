using UnityEngine;

namespace StartScene
{
    public class VSShow : MonoBehaviour {

        public static VSShow instance;

        public TweenScale vsTween;
        public TweenPosition hero1Tween;
        public TweenPosition hero2Tween;

        public void Awake()
        {
            instance = this;
        }

        public void Show(string hero1Name, string hero2Name)
        {

            BlackMask.instance.Show(); 

            hero1Tween.GetComponent<UISprite>().spriteName = hero1Name;
            hero2Tween.GetComponent<UISprite>().spriteName = hero2Name;

            vsTween.PlayForward();
            hero1Tween.PlayForward();
            hero2Tween.PlayForward();
        }
    }
}
