using UnityEngine;

namespace StartScene
{
    public class BlackMask : MonoBehaviour {

        public static BlackMask instance;
        private void Awake()
        {
            instance = this;
            this.gameObject.SetActive(false);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

    }
}
