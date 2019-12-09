//系統自帶
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

//自定義 命名空間
using LearnHub.Server.Setup;
using LearnHub.Data;

namespace LearnHub.Server {

    public class BuildServer : BaseServer {

        public static List<User> Users;                                     //玩家集合宣告

        public static bool IsQuit { get; set; }
        protected static Socket ServerSocket { get; private set; }


        //Instance
        public BuildServer() {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //創建Socket
        }

        /// <summary>
        /// 啟動服務器
        /// </summary>
        public override void Start() {

            #region 啟動服務器
            //建立連線
            IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Parse(ParameterList.SERVER_HOST), ParameterList.SERVER_PORT);

            //初始化服務器ip地址與端口
            ServerSocket.Bind(IPEndPoint);

            //啟動監聽(最大監聽量為 SERVER_MAXMEMBER)
            ServerSocket.Listen(ParameterList.SERVER_MAXMEMBER);           
            Console.WriteLine($"服務器已啟動\t Info [Gate {ParameterList.SERVER_HOST}:{ParameterList.SERVER_PORT} ]");

            #endregion


        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Initial() {

        }


        protected override void AwaitClientThread() {

            Socket ClientSocket;

            while (!IsQuit) {
                try {

                    //同步等待,程序会阻塞在这里（設置超時等待,線程休息）
                    ClientSocket = ServerSocket.Accept();                       //为新的客户端连接创建一个Socket对象，接收並返回一個新的Socket
                    string endPoint = ClientSocket.RemoteEndPoint.ToString();   //獲取客戶端唯一鍵

                    //設置用戶連線資訊
                    var user = new User(ClientSocket);                          //把PlayerSocket保存到Player資料中
                    Users.Add(user);                                            //新增用戶到List集合中
                    Console.WriteLine($"連接消息: {user.Socket.RemoteEndPoint} 連接成功！");

                    #region 子線程
                    //開啟持續接收封包線程
                    var ReceiveMethod = new ParameterizedThreadStart(ReceivePackageThread);   //開啟參數化線程，將obj傳入
                    var Listener = new Thread(ReceiveMethod) { IsBackground = true };         //創建新線程，用來監聽客戶端發送的消息
                    Listener.Start(user);                                                     //開始監聽客戶端發送的消息

                    ////開啟心跳確認線程(確認Client是否在線)
                    //var AliveChecking = new ParameterizedThreadStart(AliveThread);            //開啟參數化線程，將obj傳入
                    //var Thread_HandleAlive = new Thread(AliveChecking) { IsBackground = true };                  //啟動後台線程
                    //Thread_HandleAlive.Start(player);                               //啟動線程
                    #endregion


                } catch (Exception ex) {
                    Console.WriteLine($"#  Thread Close.\t Info [Thread Name : AwaitClient_Thread()]");
                    Console.WriteLine(ex.Message);
                }
            }


        }

        protected override void ReceivePackageThread(object clientSocket) {




        }

        protected override void PacketCallBackThread() {
            throw new NotImplementedException();
        }




        //測試

        private static void Test(User client, byte[] Head, byte[] Body) {
            Console.WriteLine("Packet Register Test method");
        }

        private static void DatabaseTest(User client, byte[] Head, byte[] Body) {
            Console.WriteLine("Database Register Test method");
        }
    }

}
