using System;
using System.Net.Sockets;

namespace LearnHub_Server {

    //封包註冊接口
    public interface IPacketRegister {
        void PK_Register();
    }

    //資料庫註冊接口
    public interface IDatabaseRegister {
        void DB_Register();
    }

    //在線接口
    public interface IOnline {
        void Offline();
    }

    /// <summary>
    /// 服務器基礎抽象類
    /// </summary>
    public abstract class BaseServer : IPacketRegister, IDatabaseRegister {

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
        protected abstract void Start();

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

        #region 接口實現
        public abstract void PK_Register();
        public abstract void DB_Register();
        #endregion
    }

}
