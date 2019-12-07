using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearnHub.Client {
    public abstract class ClientBase {

        /// <summary>
        /// TCP/IP連結&設定
        /// Client資料初始化
        /// 連接伺服器
        /// </summary>
        public abstract void Start(string IP, int Port);

        /// <summary>
        /// 持續接收封包的線程
        /// 並判斷封包的完整性
        /// </summary>
        public abstract void ReceivePacketThread();

        /// <summary>
        /// 回覆並調用=>回調
        /// 拆開封包並執行其的內容
        /// </summary>
        public abstract void CallBackThead();

        /// <summary>
        /// 註冊器
        /// 寫CallBackThead執行的方法
        /// 告訴它要怎麼做
        /// </summary>
        public abstract void Register();

        /// <summary>
        /// 設定封包資料
        /// 跟伺服器協調封包形式
        /// </summary>
        public abstract void SetupInfoPacket();

    }



}

