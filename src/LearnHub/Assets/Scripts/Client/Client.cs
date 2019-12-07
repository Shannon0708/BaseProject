using LearnHub.Data;
using LearnHub.Data.Type;
using LearnHub.Receive;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms;

namespace LearnHub.Client {

    public delegate void ClientCallBack (User user, byte[] Head, byte[] Body);      //封包回調

    public class Client : ClientBase {

        public Socket socket;
        public static volatile bool IsQuit;     //volatile:要讓每種記憶體都同步資料; IsQuit判斷是否離開 

        public static ConcurrentQueue<CallBack> callBackQueue;                         //創建回調(封包怎麼做)方法列隊(想像成佇列版的郵箱)
        public static Dictionary<PackageType, ClientCallBack> CallBacksDictionary;     //封包類型與回調方法(解釋封包裡面要做什麼)

        private readonly ReceivePacket receivePacket;                                   //接收封包消息類

        public User user { get; private set; }

        /// <summary>
        /// 構造器
        /// </summary>
        public Client() {
            callBackQueue = new ConcurrentQueue<CallBack>();
            CallBacksDictionary = new Dictionary<PackageType, ClientCallBack>();
            receivePacket = new ReceivePacket();
        }


        public override void Start(string IP, int Port) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
            try {
                socket.Connect(IPEndPoint);     //把一個身分socket透過Connect連結IPEndPoint
                Debug.Log("成功連結伺服器！");
                user = new User(socket);
                IsQuit = false;

            } catch(Exception e) {
                Debug.Log($"連結失敗！{e.Message}");
                IsQuit = true;
                return;
            }

            #region 線程
            Thread Listener = new Thread(ReceivePacketThread) { IsBackground = true };  //IsBackground:把線程變成持續進行的線程，在背景工作
            Listener.Start();

            #endregion



        }
        /// <summary>
        /// 持續接收封包
        /// </summary>
        public override void ReceivePacketThread () {
            while (true) {
                byte[] Head_Byte = receivePacket.Head(user.Socket);
                if (IsQuit) break;
                byte[] Body_Byte = receivePacket.Body(user.Socket, Head_Byte);
                receivePacket.CheckPacket(user, Head_Byte, Body_Byte);
            }
            Debug.Log($"# Thread Close.\t Info [Thread Name] : ReceivePackage_Thread()]");
        }

        public override void CallBackThead() {
            throw new System.NotImplementedException();

        }

        public override void Register() {
            throw new System.NotImplementedException();
        }

        public override void SetupInfoPacket() {
            throw new System.NotImplementedException();
        }


    }

    //CallBack:回調
    public class CallBack {
        private ClientCallBack ClientCallBack { get; set; }
        private User user { get; set; }
        private byte[] Head { get; set; }
        private byte[] Body { get; set; }

        //叫clientCallBack去分封包
        public CallBack (User user, byte[] head, byte[] body, ClientCallBack clientCallBack) {
            this.user = user;       //this用來指這個類別的這個名字
            Head = head;
            Body = body;
            ClientCallBack = clientCallBack;
        }

        public void Execute() {
            ClientCallBack(user, Head, Body);  
        }

    }


}

