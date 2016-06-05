using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MultiplePlayers : MonoBehaviour {
    public string roomName;
    public uint maxPlayerCount = 2;
    public UIInput roomnameInput;
    private string IP = "127.0.0.1";
    public UIInput ipInput;
    private int Port = 7777;
    public UIInput portInput;

    public TweenAlpha settingsContainerTween;
    public TweenAlpha connectionContainerTween;
    public TweenAlpha clientConnectionContainerTween;



    public EvolvedNetworkManager networkManager;
    public static MultiplePlayers _instance;

    private NetworkClient hostClient;

    public MatchInfo info;

    void Awake()
    {
        _instance = this;
    }

    public void OnCreateHostButtonClicked()
    {
        //TODO 暂时不知道写的对不对
        networkManager.matchSize = maxPlayerCount;
        networkManager.matchName = roomnameInput.value;

        //TODO 
        PlayerPrefs.SetInt("isClientHero1", 1);
        PlayerPrefs.SetInt("networkGame", 1);
        PlayerPrefs.SetString("hero1", Hero.HeroNames[0]);
        PlayerPrefs.SetString("hero2", Hero.HeroNames[1]);

        hostClient = networkManager.StartHost();
        
        StartCoroutine(playSettingToConnectionTween());
    }

    public IEnumerator playSettingToConnectionTween()
    {
        settingsContainerTween.PlayForward();
        yield return new WaitForSeconds(0.5f);
        connectionContainerTween.PlayForward();
    }

    public void OnstartClientGameButtonClicked()
    {

        //TODO 
        PlayerPrefs.SetInt("isClientHero1", 0);
        PlayerPrefs.SetInt("networkGame", 1);
        PlayerPrefs.SetString("hero1", Hero.HeroNames[0]);
        PlayerPrefs.SetString("hero2", Hero.HeroNames[1]);



        networkManager.StartClient();
        StartCoroutine(playSettingToClientConnectionTween());
    }

    public IEnumerator playSettingToClientConnectionTween()
    {
        settingsContainerTween.PlayForward();
        yield return new WaitForSeconds(0.5f);
        clientConnectionContainerTween.PlayForward();
    }




    // called when a match is created
    public virtual void OnMatchCreate(CreateMatchResponse matchInfo)
    {

    }

    // called when a list of matches is received
    public virtual void OnMatchList(ListMatchResponse matchList)
    {

    }

    // called when a match is joined
    public void OnMatchJoined(JoinMatchResponse matchInfo)
    {

    }

}
