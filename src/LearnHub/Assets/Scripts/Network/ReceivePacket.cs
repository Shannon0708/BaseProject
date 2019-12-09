using LearnHub.Callback;
using LearnHub.Client;
using LearnHub.Client.Setup;
using LearnHub.Data;
using LearnHub.Data.Type;

using System.Net.Sockets;
using System.Threading;
using UnityEngine;              //Debug.Log

namespace LearnHub.Network.Packet {

    /// <summary>
    /// 等待及接收封包
    /// </summary>
    public class ReceivePacket {

        private readonly Unpack unpack; //解析類

        #region Instance
        /// <summary>
        /// Instance : 無參數
        /// </summary>
        public ReceivePacket() {
            unpack = new Unpack();
        }

        /// <summary>
        /// Instance : 帶解析類參數
        /// </summary>
        /// <param name="unpack"></param>
        public ReceivePacket(Unpack unpack) {
            this.unpack = unpack;
        }
        #endregion

        public byte[] Head (Socket UserSocket) {
            int RecvAlready;
            int HeadLength = Setup.PACKET_HEADLENGTH;
            byte[] Head_Byte = new byte[HeadLength];            //把資料抓過來

            while (HeadLength > 0) {
                byte[] RecvHead_Bytes = new byte[HeadLength];   //RecvHead_Bytes保存現在有幾個byte

                //檢查緩存區是否有資料需要讀取; True為有資料, False為緩存區沒有資料
                if (!(UserSocket.Available == 0)) {                //判斷有沒有東西進來，沒有就不要一直在那邊乾等
                    if (HeadLength >= RecvHead_Bytes.Length) {
                        RecvAlready = UserSocket.Receive(RecvHead_Bytes, RecvHead_Bytes.Length, 0);     //Socket型態裡面有
                    } else {
                        RecvAlready = UserSocket.Receive(RecvHead_Bytes, HeadLength, 0);
                    }

                    RecvHead_Bytes.CopyTo(Head_Byte, Head_Byte.Length - HeadLength);    //RecvHead_Bytes複製到Head_Byte
                    HeadLength -= RecvAlready;
                } else {
                    Thread.Sleep(50);       //如果沒收到東西，睡50毫秒 => 讓出線程
                    if (LearnHubClient.IsQuit)
                        break;
                }
            }
            return Head_Byte;
        }

        public byte[] Body (Socket UserScoket, byte[] Head_Byte) {
            int RecvAlready;
            int Bodylength = unpack.Head_BodyLength(Head_Byte);    //因為不像Head一樣知道長度，所以在Unpack的時候先算BodyLength
            byte[] Body_Byte = new byte[Bodylength];

            while (Bodylength > 0) {
                byte[] RecvBody_Bytes = new byte[Bodylength < 1024 ? Bodylength : 1024];    //[if(Bodylength <1024) 回傳Bodylength，else回傳1024]

                if (Bodylength >= RecvBody_Bytes.Length) {
                    RecvAlready = UserScoket.Receive(RecvBody_Bytes, RecvBody_Bytes.Length, 0);
                } else {
                    RecvAlready = UserScoket.Receive(RecvBody_Bytes, Bodylength, 0);
                }

                RecvBody_Bytes.CopyTo(Body_Byte, Body_Byte.Length - Bodylength);
                Bodylength -= RecvAlready;

            }
            return Body_Byte;
        }

        /// <summary>
        /// 封包完整性檢查
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Head_Byte"></param>
        /// <param name="Body_Byte"></param>
        public void CheckPacket (User user, byte[] Head_Byte, byte[] Body_Byte) {
            int crcCode = unpack.Head_CrcCode(Head_Byte);
            PackageType packageType = unpack.Head_PackageType(Head_Byte);

            //clientCallBack拿封包過來檢查
            if (user.CrcCode == crcCode) {         //比對 封包驗證碼 及 用戶身份Key
                if (PacketCallback.callbackDictionary.ContainsKey(packageType)) {   //確認 合格封包的類別是否存在
                    CallBack callBack = new CallBack(user, Head_Byte, Body_Byte, PacketCallback.callbackDictionary[packageType]);      //將封包打包成列隊格式        
                    PacketCallback.callbackQueue.Enqueue(callBack);                 //將合格的封包丟進列隊中(回調線程會在列隊中抓取封包解讀，並且根據封包類別去執行不同的方法)
                } else {
                    Debug.Log($"錯誤 未知的封包型態:{(int)packageType} 註冊表中未發現此類型態的描述!");
                }
            } else {
                    Debug.Log($"錯誤 封包驗證碼: {crcCode}");
            }
        }
    }
}


