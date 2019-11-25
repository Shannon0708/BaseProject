using System;
using System.Net;
using System.Net.Sockets;

namespace Network.Packet {

    /// <summary>
    /// 發送封包
    /// </summary>
    public class Send {

        #region 各類型態 轉換成 Byte[]樣式
        /// <summary>
        /// 創造int封包，把int變成二進位(byte)丟給BuildPackage打包
        /// </summary>
        /// <param name="user">要發給誰</param>
        /// <param name="packageType">封包型態，發過去要幹嘛</param>
        /// <param name="data">要發送封包的內容</param>
        public void IntPacket(User user, PackageType packageType, int data) {

            //將整數型data 轉換成 字節陣列 data_Byte
            byte[] data_Byte = BitConverter.GetBytes(data);

            //封裝封包(封包驗證碼, 封包型態, 封包加密方法, 封包內容)
            byte[] Packet = Build_Package(user.CrcCode, packageType, user.EncryptionType, data_Byte);

            //發送封包(發送目標, 發送內容)
            Send_Packet(user.Socket, Packet);
        }

        #endregion

        #region 封裝 及 發送封包
        /// <summary>
        /// 封裝 成二進位
        /// </summary>
        /// <param name="crcCode">封包驗證碼</param>
        /// <param name="packageType">封包型態</param>
        /// <param name="encryptionType">定義加密的方法</param>
        /// <param name="data_Byte">封包資料主體內容</param>
        /// <returns>回傳封裝完畢的封包</returns>
        private byte[] Build_Package(int crcCode, PackageType packageType, EncryptionType encryptionType, byte[] data_Byte) {
            
            //using System有:BitConverter GetBytes
            //using System.Net有:IPAddress HostToNetworkOrder把後面括號內的參數轉換成網路格式
            byte[] crcCode_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(crcCode));             
            byte[] packageType_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)packageType));              //(short)強制將後面參數轉成短整數
            byte[] encryptionType_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)encryptionType));       
            byte[] bodyLength = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data_Byte.Length));

            byte[] Packet = new byte[12 + data_Byte.Length];    //封包 Head長度 加 Body長度

            //CopyTo:把前面的內容複製到後面陣列的起始位置(陣列,起始位置)
            crcCode_Byte.CopyTo(Packet, 0);
            packageType_Byte.CopyTo(Packet, 4);
            encryptionType_Byte.CopyTo(Packet, 6);
            bodyLength.CopyTo(Packet, 8);
            data_Byte.CopyTo(Packet, 12);

            return Packet;
        }

        /// <summary>
        /// 發送封包
        /// </summary>
        /// <param name="target">發送的目標</param>
        /// <param name="Packet">發送的內容</param>
        private void Send_Packet(Socket target, byte[] Packet) {

            //判斷發送目標是否處於連線狀態
            if (target.Connected) {
                try {
                    //發送資料給目標Send(封包, 長度, 起始位置)
                    target.Send(Packet, Packet.Length, 0);
                } catch(Exception ex) {
                    Console.WriteLine($"封包發送失敗： {ex.Message}");
                }
            } else {
                throw new Exception($"{target.RemoteEndPoint}： 已斷線");
            }
        }
        #endregion
    }

    /// <summary>
    /// 收到二進位解封包
    /// </summary>
    public class Unpack {

        #region Head 封包自身資訊
        /// <summary>
        /// 封包的頭之CrcCode還原
        /// </summary>
        /// <param name="head">創造一個byte[]名為head</param>
        /// <returns>回傳解析後的內容</returns>
        public int Head_CrcCode(byte[] head) {
            byte[] Int_Byte = new byte[4];          //陣列宣告的方法，宣告一個4byte的陣列
            Array.Copy(head, 0, Int_Byte, 0, 4);    //Array系統自帶的類別，將head從0開始複製至Int_Byte的0~4格
            int CrcCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(Int_Byte, 0));   //Byte->int

            return CrcCode;
        }

        /// <summary>
        /// 封包的頭之Package還原
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public PackageType Head_PackageType(byte[] head) {
            byte[] PackageType_Byte = new byte[2];          //陣列宣告的方法，宣告一個4byte的陣列
            Array.Copy(head, 4, PackageType_Byte, 0, 2);
            short PackageType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(PackageType_Byte, 0));   //Byte->int

            return (PackageType)PackageType;
        }

        /// <summary>
        /// 封包的頭之Encryption還原
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public EncryptionType Head_EncryptionType(byte[] head) {
            byte[] EncryptionType_Byte = new byte[2];      //陣列宣告的方法，宣告一個4byte的陣列
            Array.Copy(head, 6, EncryptionType_Byte, 0, 2);
            short EncryptionType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(EncryptionType_Byte, 0));   //Byte->int

            return (EncryptionType)EncryptionType;
        }

        /// <summary>
        /// 封包的頭之Body長度
        /// /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public int Head_BodyLength(byte[] head) {
            byte[] BodyLength_Byte = new byte[4];
            Array.Copy(head, 8, BodyLength_Byte, 0, 4);
            int BodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(BodyLength_Byte, 0));

            return BodyLength;
        }
        #endregion


        #region Body 封包實際內容
        /// <summary>
        /// Body內容的資料轉成原有的型態
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public int Body_IntData(byte[] body) {
            int Data = BitConverter.ToInt32(body, 0);
            return Data;
        }

        #endregion


        #region 測試用途
        //解封head
        public byte[] Unpack_Head(byte[] Packet) {
            byte[] Temp_Byte = new byte[12];
            Array.Copy(Packet, 0, Temp_Byte, 0, 12);
            return Temp_Byte;
        }

        //解封Body
        public byte[] Unpack_Body(byte[] Packet, int Length) {
            byte[] Temp_Byte = new byte[Length];
            Array.Copy(Packet, Packet.Length - Length, Temp_Byte, 0, Length);
            return Temp_Byte;
        }
        #endregion

    }


    /// <summary>
    /// Dll測試類
    /// </summary>
    public class Dll {

        public void Test() {
            Console.WriteLine("Dll Test!");
        }

    }

}
