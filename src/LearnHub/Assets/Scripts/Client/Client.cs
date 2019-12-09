
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

//自定義(下方補充)
using LearnHub.Data;
using LearnHub.Data.Type;
using LearnHub.Network.Packet;
using LearnHub.Callback;

namespace LearnHub.Client {

    public class Client : ClientBase {

        #region 全域宣告
        //屬性
        public User User { get; private set; }
        public Socket ClientSocket { get; private set; }

        public static volatile bool IsQuit;     //volatile:要讓每種記憶體都同步資料; IsQuit判斷是否離開 

        //類別or接口
        private ReceivePacket receivePacket;    //接收封包消息類
        #endregion

        /// <summary>
        /// 構造器
        /// </summary>
        public Client() => Initail();   //Lambada表達式
        
        #region 初始化方法
        /// <summary>
        /// 初始化方法
        /// </summary>
        private void Initail() {
            receivePacket = new ReceivePacket();
        }
        #endregion

        #region 一般方法
        /// <summary>
        /// 啟動服務器
        /// </summary>
        /// <param name="IP">IP位址</param>
        /// <param name="Port">端口</param>
        public override void Start(string IP, int Port) {

            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);

            //嘗試連線
            try {
                ClientSocket.Connect(IPEndPoint);     //把一個身分socket透過Connect連結IPEndPoint
                Debug.Log("成功連結伺服器！");
                User = new User(ClientSocket);
                IsQuit = false;
            } catch(Exception e) {
                Debug.Log($"連結失敗！{e.Message}");
                IsQuit = true;
                return;
            }

            #region 線程
            //監聽連線線程
            Thread Listener = new Thread(ReceivePacketThread) { IsBackground = true };  //IsBackground:把線程變成持續進行的線程，在背景工作
            Listener.Start();

            //接收封包線程
            //###
            #endregion

        }
        #endregion

        #region 線程方法(抽象)
        /// <summary>
        /// 持續接收封包
        /// </summary>
        protected override void ReceivePacketThread () {
            while (true) {
                byte[] Head_Byte = receivePacket.Head(User.Socket);
                if (IsQuit) break;
                byte[] Body_Byte = receivePacket.Body(User.Socket, Head_Byte);
                receivePacket.CheckPacket(User, Head_Byte, Body_Byte);
            }
            Debug.Log($"# Thread Close.\t Info [Thread Name] : ReceivePackage_Thread()]");
        }

        /// <summary>
        /// 回調線程
        /// </summary>
        protected override void CallBackThead() {

        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 註冊方法：Bug 靜態調用無法重載
        /// </summary>
        public override void Register() {

        }

        /// <summary>
        /// 封包資訊設定
        /// </summary>
        public override void SetupInfoPacket() {

        }
        #endregion

    }

}
