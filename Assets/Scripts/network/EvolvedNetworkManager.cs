using UnityEngine;
using System.Collections;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;

public class EvolvedNetworkManager : NetworkManager {

    public UILabel statusLabel;
    public UILabel Label1;
    public UILabel Label2;
    public UILabel clientStatusLabel;


    //Server Part


    /// <summary>
    /// called when a client connects 
    /// </summary>
    /// <param name="conn"></param>
    // 
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        MonoBehaviour.print("Server: a client connects, " + conn.ToString());
        statusLabel.text = "进入成功，当前人数为" + (this.numPlayers + 1);
        if (this.numPlayers == 0)
        {
            Label1.text = conn.ToString();
            


        }
        else
        {
            Label2.text = conn.ToString();
            StartCoroutine(startGame());
        }
    }

    private IEnumerator startGame()
    {
        //TODO  分配各种设置

        GameObject.Find("BGMScript").GetComponent<BGMLoader>().stopMainPageSound();
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    // called when a client disconnects
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        MonoBehaviour.print("Server: a client disconnects, " + conn.ToString());
        statusLabel.text = "玩家断线，当前人数为" + this.numPlayers;
        if (this.numPlayers == 0)
        {
            Label2.text = "";
        }
    }

    // called when a client is ready
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        MonoBehaviour.print("Server: a client is ready, " + conn.ToString());
    }

   

    // called when a network error occurs
    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
        MonoBehaviour.print("Server: network error happens, error code is" + errorCode);
    }


    //Client Side
    // called when connected to a server
    public override void OnClientConnect(NetworkConnection conn)
    {
       
        base.OnClientConnect(conn);
        MonoBehaviour.print("Client: connected");
        clientStatusLabel.text = "连接成功！";
        if (this.numPlayers == 0)
        {
            StartCoroutine(startGame());
        }


    }

    // called when disconnected from a server
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Label1.text = "断开连接";
        
        MonoBehaviour.print("Client: disconnected");
    }

    // called when a network error occurs
    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        Label1.text = "网络连接错误";
        MonoBehaviour.print("Client:  network error happens, error code is " + errorCode);
    }

    // called when told to be not-ready by a server
    public override void OnClientNotReady(NetworkConnection conn)
    {
        base.OnClientNotReady(conn);
        MonoBehaviour.print("Client:  told to be not-ready by a server ");
    }
}
