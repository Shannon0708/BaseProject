using System.Collections.Concurrent;
using System.Collections.Generic;

using LearnHub.Data;
using LearnHub.Data.Type;
using UnityEngine;

namespace LearnHub.Callback {

    /// <summary>
    /// 封裝回調
    /// </summary>
    public class CallBack {

        private PacketCallbackEventHandler PacketCallBack { get; set; }
        private User User { get; set; }
        private byte[] Head { get; set; }
        private byte[] Body { get; set; }

        //叫clientCallBack去分封包
        public CallBack(User user, byte[] head, byte[] body, PacketCallbackEventHandler PacketCallBack) {
            User = user;       //this用來指這個類別的這個名字
            Head = head;
            Body = body;
            this.PacketCallBack = PacketCallBack;
        }

        /// <summary>
        /// 執行回調
        /// </summary>
        public void Execute() {
            PacketCallBack(User, Head, Body);
        }
    }

    /// <summary>
    /// 封包回調方法 : 方法類別, 註冊接口
    /// </summary>
    public class PacketCallback : PacketCallbackMethods {

        private static int Counter; //計算回調數目

        //資料結構
        public static ConcurrentQueue<CallBack> callbackQueue;                        //創建回調(封包怎麼做)方法列隊(想像成佇列版的郵箱)
        public static Dictionary<PackageType, PacketCallbackEventHandler> callbackDictionary;     //封包類型與回調方法(解釋封包裡面要做什麼)

        /// <summary>
        /// Instance
        /// </summary>
        public PacketCallback() => Initial();

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initial() {
            Counter = 0;
            callbackQueue = new ConcurrentQueue<CallBack>();
            callbackDictionary = new Dictionary<PackageType, PacketCallbackEventHandler>();
        }

        /// <summary>
        /// 將註冊清單內容存入字典中
        /// </summary>
        private void PushRegisterToDictionary(PackageType packageType, PacketCallbackEventHandler callbackMethod) {
            if (!callbackDictionary.ContainsKey(packageType)) {
                callbackDictionary.Add(packageType, callbackMethod);
                Counter++;
            } else {
                Debug.Log($"註冊了相同的回調事件 -> Info : {packageType}");
            }
        }
        

        /// <summary>
        /// 註冊回調方法
        /// </summary>
        public void Register() {
            PushRegisterToDictionary(PackageType.Test, Testing);
            PushRegisterToDictionary(PackageType.None, Test2);

            Debug.Log($"封包回調註冊註冊完成, 共計成功註冊回調: {Counter}");
        }

    }
}
