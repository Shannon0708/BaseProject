using System;
using Network.Packet;

//版本1.1.03
namespace LearnHub_Server {

    /// <summary>
    /// 項目入口
    /// </summary>
    public class Start {

        #region 參考
        public static Server Server;    //服務器物件參考
        #endregion

        /// <summary>
        /// 主程式
        /// </summary>
        public static void Main() {

            Initial();  //主程式初始化

            //設定服務器
            //Server.Start(); //啟動服務器
            //Console.WriteLine("正在啟動服務器...\n");

            Console.ReadKey();  //避免閃退
            //Console.WriteLine("#  Server is Close");

        }

        /// <summary>
        /// 初始化
        /// </summary>
        private static void Initial() {
            //Server = new Server();
            //Server

            //Dll測試
            var test = new Test();
            test.Dll();

        }
    }
}
