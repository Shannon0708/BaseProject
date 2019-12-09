using LearnHub.Data;
using UnityEngine;

namespace LearnHub.Callback {

    public delegate void PacketCallbackEventHandler(User user, byte[] head, byte[] body);
    
    /// <summary>
    /// 封包回調方法
    /// </summary>
    public class PacketCallbackMethods {

        //註冊請不要寫到此類別,如果需要添加註冊,請到此類別所繼承的父類中添加(即：RegisterCallback)

        //測試
        protected void Testing(User user, byte[] head, byte[] body) {
            Debug.Log("Testing");
        }

        protected void Test2(User user, byte[] head, byte[] body) {
            Debug.Log("T2");
        }

    }
}
