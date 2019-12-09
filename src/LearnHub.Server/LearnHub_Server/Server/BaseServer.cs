using System;
using System.Collections.Generic;
using System.Net.Sockets;

using LearnHub.Data;

namespace LearnHub.Server {

    /// <summary>
    /// 服務器基礎抽象類
    /// </summary>
    public abstract class BaseServer {

        /// <summary>
        /// 服務器初始化方法
        /// 物件實例化
        /// 註冊回調方法
        /// Others
        /// </summary>
        protected abstract void Initial();

        /// <summary>
        /// 啟動服務器
        /// TCP/IP連線
        /// 啟動線程
        /// </summary>
        public abstract void Start();

        #region 線程 : 線程命名規則 ( 以Thread結尾 )

        /// <summary>
        /// 監聽Client封包子線程：持續監聽來自Client的封包
        /// </summary>
        protected abstract void ReceivePackageThread(object clientSocket);

        /// <summary>
        /// 監聽Client連線線程：持續監聽來自Clinet的連線消息
        /// </summary>
        protected abstract void AwaitClientThread();

        /// <summary>
        /// 封包回調線程：持續檢查佇列中的封包
        /// </summary>
        protected abstract void PacketCallBackThread();

        //其他線程
        #endregion
    }

}