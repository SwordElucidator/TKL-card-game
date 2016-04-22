using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject cardPrefeb;
    public Transform fromCard;
    public Transform toCard;
    public string[] cardNames;
    public float transformTime = 2f;
    public int tranformSpeed = 20;

    private bool isTransforming = false;
    private float timer = 0;
    private UISprite nowGenerateCard;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomGenerateCard();
        }

        if (isTransforming)
        {
            timer += Time.deltaTime;
            int index =  (int)(timer / (1f / tranformSpeed));
            index %= cardNames.Length;
            nowGenerateCard.spriteName = cardNames[index];
            if (timer > transformTime)
                //变换结束
            {
                isTransforming = false;
                timer = 0;
            }
        }
    }

	public void RandomGenerateCard()
    {

        GameObject go = NGUITools.AddChild(this.gameObject, cardPrefeb);
        go.transform.position = fromCard.position;
        nowGenerateCard = go.GetComponent<UISprite>();
        iTween.MoveTo(go, toCard.position, 1f);
        isTransforming = true;
    }
}
