using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LearnHub.Client;
using LearnHub.Client.Setup;

public class StartConnection : MonoBehaviour {

    public static LearnHubClient Client { get; private set; }

    //Awake is called before the start function
    private void Awake() {

        Initial();          //初始化
        Client = new LearnHubClient();   //new一個構造器給我

        //Client.Start(Setup.CLIENT_HOST, Setup.CLIENT_PORT);
        //Debug.Log("StartConnection的Awake執行完了= 3=");

    }

    private void Initial() {
        LearnHub.Callback.PacketCallback registerCallback = new LearnHub.Callback.PacketCallback();
        registerCallback.Register();
    }
}
