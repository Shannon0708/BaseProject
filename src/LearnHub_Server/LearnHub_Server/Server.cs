using System;

namespace LearnHub_Server {

    /// <summary>
    /// 服務器類別：服務器啟動方法及規則
    /// </summary>
    public class Server {

        //instance
        public Server() {
            //執行初始化 Initial()
        }

        //服務器初始化方法
        private void Initial() {
            //物件實例化
            //註冊回調方法
            //Others
        }

        //啟動服務器
        public void Start() {
            //TCP/IP連線
            //啟動線程
            Console.WriteLine("Hello");
        }

        #region 線程 : 線程命名規則 ( 以Thread結尾 )

        //監聽Client連線線程：持續監聽來自Clinet的連線消息
        private void AwaitClientThread() {

        }

        //監聽Client封包子線程：持續監聽來自Client的封包
        private void ReceivePackageThread() {

        }

        //封包回調線程：持續檢查佇列中的封包
        private void PacketCallBackThread() {

        }

        //其他線程
        #endregion


    }

}
