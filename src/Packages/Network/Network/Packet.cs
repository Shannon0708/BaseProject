using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using LearnHub.Data;
using LearnHub.Netwrok.Setup;

namespace Network.Packet {

    /// <summary>
    /// 發送封包
    /// </summary>
    public static class Send {

        #region 各類型態 轉換成 Byte[]樣式
        /// <summary>
        /// 創造int封包，把int轉成二進位(byte)丟給BuildPackage打包
        /// </summary>
        /// <param name="user">要發給誰</param>
        /// <param name="packageType">封包型態，發過去要幹嘛</param>
        /// <param name="data">要發送封包的內容</param>
        public static void IntPacket(User user, PackageType packageType, int data) {
            
            //將整數型data 轉換成 字節(二進位)陣列 data_Byte
            byte[] data_Byte = BitConverter.GetBytes(data);

            //封裝封包(封包驗證碼, 封包型態, 封包加密方法, 封包內容)
            byte[] Packet = Build_Package(user.CrcCode, packageType, user.EncryptionType, data_Byte);

            //發送封包(發送目標, 發送內容)
            Send_Packet(user.Socket, Packet);
        }

        /// <summary>
        /// 創造float封包，把float轉成二進位(byte)丟給BuildPackage打包
        /// </summary>
        /// <param name="user">要發給誰</param>
        /// <param name="packageType">封包型態，發過去要幹嘛</param>
        /// <param name="data">要發送封包的內容</param>
        public static void FloatPacket(User user, PackageType packageType, float data) {

            //將浮點數型data 轉換成 字節(二進位)陣列 data_Byte
            byte[] data_Byte = BitConverter.GetBytes(data);

            //封裝封包(封包驗證碼, 封包型態, 封包加密方法, 封包內容)
            byte[] Packet = Build_Package(user.CrcCode, packageType, user.EncryptionType, data_Byte);

            //發送封包(發送目標, 發送內容)
            Send_Packet(user.Socket, Packet);
        }

        /// <summary>
        /// 創建bool封包，把bool轉成二進位(byte)丟給BuildPackage打包
        /// </summary>
        /// <param name="user">要發給誰</param>
        /// <param name="packageType">封包型態，發過去要幹嘛</param>
        /// <param name="data">要發送封包的內容</param>
        public static void BoolPacket(User user, PackageType packageType, bool data) {

            //將布林型data 轉換成 字節(二進位)陣列 data_Byte
            byte[] data_Byte = BitConverter.GetBytes(data);

            //封裝封包(封包驗證碼, 封包型態, 封包加密方法, 封包內容)
            byte[] Packet = Build_Package(user.CrcCode, packageType, user.EncryptionType, data_Byte);

            //發送封包(發送目標, 發送內容)
            Send_Packet(user.Socket, Packet);
        }

        /// <summary>
        /// 創建long封包，把long轉成二進位(byte)丟給BuildPackage打包
        /// </summary>
        /// <param name="user">要發給誰</param>
        /// <param name="packageType">封包型態，發過去要幹嘛</param>
        /// <param name="data">要發送封包的內容</param>
        public static void LongPacket(User user, PackageType packageType, long data) {

            //將長整數型data 轉換成 字節(二進位)陣列 data_Byte
            byte[] data_Byte = BitConverter.GetBytes(data);

            //封裝封包(封包驗證碼, 封包型態, 封包加密方法, 封包內容)
            byte[] Packet = Build_Package(user.CrcCode, packageType, user.EncryptionType, data_Byte);

            //發送封包(發送目標, 發送內容)
            Send_Packet(user.Socket, Packet);
        }

        /// <summary>
        /// 創建short封包，把short轉成二進位(byte)丟給BuildPackage打包
        /// </summary>
        /// <param name="user">要發給誰</param>
        /// <param name="packageType">封包型態，發過去要幹嘛</param>
        /// <param name="data">要發送封包的內容</param>
        public static void ShortPacket(User user, PackageType packageType, short data) {

            //將短整數型data 轉換成 字節(二進位)陣列 data_Byte
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
        private static byte[] Build_Package(int crcCode, PackageType packageType, EncryptionType encryptionType, byte[] data_Byte) {

            //using System有:BitConverter GetBytes
            //using System.Net有:IPAddress HostToNetworkOrder把後面括號內的參數轉換成網路格式
            byte[] crcCode_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(crcCode));
            byte[] packageType_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)packageType));              //(short)強制將後面參數轉成短整數
            byte[] encryptionType_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)encryptionType));
            byte[] bodyLength = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data_Byte.Length));

            byte[] Packet = new byte[ParameterList.HEAD_LEN + data_Byte.Length];    //封包 Head長度 加 Body長度

            //CopyTo:把前面的內容複製到後面陣列的起始位置(陣列,起始位置)
            crcCode_Byte.CopyTo(Packet, ParameterList.CRC_Code_POS);
            packageType_Byte.CopyTo(Packet, ParameterList.PACKAGE_Type_POS);
            encryptionType_Byte.CopyTo(Packet, ParameterList.ENCRYPTION_TYPE_POS);
            bodyLength.CopyTo(Packet, ParameterList.BODY_LENGTH_POS);
            data_Byte.CopyTo(Packet, ParameterList.HEAD_LEN);

            return Packet;
        }

        /// <summary>
        /// 發送封包
        /// </summary>
        /// <param name="target">發送的目標</param>
        /// <param name="Packet">發送的內容</param>
        private static void Send_Packet(Socket target, byte[] Packet) {

            //判斷發送目標是否處於連線狀態
            if (target.Connected) {
                try {
                    //發送資料給目標Send(封包, 長度, 起始位置)
                    target.Send(Packet, Packet.Length, 0);
                } catch (Exception ex) {
                    Console.WriteLine($"封包發送失敗： {ex.Message}");
                }
            } else {
                throw new Exception($"{target.RemoteEndPoint}： 已斷線");
            }
        }
        #endregion
    }

    /// <summary>
    /// 解析封包：收到二進位解封包
    /// </summary>
    public static class Unpack {

        #region Head 封包自身資訊
        /// <summary>
        /// 封包的頭之CrcCode還原
        /// </summary>
        /// <param name="head">創造一個byte[]名為head</param>
        /// <returns>回傳解析後的內容</returns>
        public static int Head_CrcCode(byte[] head) {
            byte[] Int_Byte = new byte[4];          //陣列宣告的方法，宣告一個4byte的陣列
            Array.Copy(head, 0, Int_Byte, 0, 4);    //Array系統自帶的類別，將head從0開始複製至Int_Byte的0~4格
            int CrcCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(Int_Byte, 0));   //Byte->int

            return CrcCode;
        }

        /// <summary>
        /// 封包的頭之Package還原
        /// </summary>
        /// <param name="head">創造一個byte[]名為head</param>
        /// <returns>回傳解析後的內容</returns>
        public static PackageType Head_PackageType(byte[] head) {
            byte[] PackageType_Byte = new byte[2];          //陣列宣告的方法，宣告一個4byte的陣列
            Array.Copy(head, 4, PackageType_Byte, 0, 2);
            short PackageType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(PackageType_Byte, 0));   //Byte->int

            return (PackageType)PackageType;
        }

        /// <summary>
        /// 封包的頭之Encryption還原
        /// </summary>
        /// <param name="head">創造一個byte[]名為head</param>
        /// <returns>回傳解析後的內容</returns>
        public static EncryptionType Head_EncryptionType(byte[] head) {
            byte[] EncryptionType_Byte = new byte[2];      //陣列宣告的方法，宣告一個4byte的陣列
            Array.Copy(head, 6, EncryptionType_Byte, 0, 2);
            short EncryptionType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(EncryptionType_Byte, 0));   //Byte->int

            return (EncryptionType)EncryptionType;
        }

        /// <summary>
        /// 封包的頭之Body長度
        /// /// </summary>
        /// <param name="head">創造一個byte[]名為head</param>
        /// <returns>回傳解析後的內容</returns>
        public static int Head_BodyLength(byte[] head) {
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
        /// <returns>原始資料</returns>
        //BitConverter是System裡的class
        //ToInt是其中的方法(位元組陣列, 起始位置)

        //將原傳送內文為int的資料還原
        public static int Body_IntData(byte[] body) {
            int Data = BitConverter.ToInt32(body, 0);
            return Data;
        }

        //將原傳送內文為float的資料還原
        public static float Body_FloatData(byte[] body) {
            float Data = BitConverter.ToSingle(body, 0);
            return Data;
        }

        //將原傳送內文為bool的資料還原
        public static bool Body_BoolData(byte[] body) {
            bool Data = BitConverter.ToBoolean(body, 0);
            return Data;
        }

        //將原傳送內文為long的資料還原
        public static long Body_LongData(byte[] body) {
            long Data = BitConverter.ToInt64(body, 0);
            return Data;
        }

        //將原傳送內文為short的資料還原
        public static short Body_ShortData(byte[] body) {
            short Data = BitConverter.ToInt16(body, 0);
            return Data;
        }

        #endregion

        #region 測試用途
        //解封head
        public static byte[] Unpack_Head(byte[] Packet) {
            byte[] Temp_Byte = new byte[12];
            Array.Copy(Packet, 0, Temp_Byte, 0, 12);
            return Temp_Byte;
        }

        //解封Body
        public static byte[] Unpack_Body(byte[] Packet, int Length) {
            byte[] Temp_Byte = new byte[Length];
            Array.Copy(Packet, Packet.Length - Length, Temp_Byte, 0, Length);
            return Temp_Byte;
        }
        #endregion
    }



    #region API接口
    /// <summary>
    /// 串流資料
    /// </summary>
    public interface IStreamData {

        /// <summary>
        /// 資料串流接收
        /// </summary>
        /// <param name="user">資料來源對象</param>
        /// <param name="dataLength">資料長度</param>
        /// <returns></returns>
        byte[] Data(User user, int dataLength);
    }

    /// <summary>
    /// 封包Head接口
    /// </summary>
    public interface IHeadPacket : IStreamData {
        //Nothing
    }

    /// <summary>
    /// 封包Body接口
    /// </summary>
    public interface IBodyPacket : IStreamData {
        /// <summary>
        /// 完整性檢查
        /// </summary>
        /// <param name="head">資料明細</param>
        /// <param name="body">資料內容</param>
        void IntegrityCheck(byte[] head, byte[] body);
    }
    #endregion

    /// <summary>
    /// Bug:串流封包接收類
    /// </summary>
    public class ReceivePacket : IStreamData {

        /// <summary>
        /// 等待資料
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public byte[] Data(User user, int dataLength) {                   
            int currentLength = dataLength;     //資料長度

            // 創建一個容器，保存封包頭對封包描述的訊息，大小為描述長度。
            byte[] data_Byte = new byte[currentLength];          

            // 如果當前需要接收的字節數大於0 and 遊戲未退出 則循環接收
            while (currentLength > 0) {

                //緩存陣列
                byte[] recvData_Byte = new byte[currentLength];

                if(!(user.Socket.Available == 0)) {

                    //防沾包：如果當前接收的字節組大於緩存區，則按緩存區大小接收，否則按剩餘需要接收的字節組接收。
                    int recvAlready =   //接收到的字節組
                        (currentLength >= recvData_Byte.Length)
                            ? user.Socket.Receive(recvData_Byte, recvData_Byte.Length, 0)
                            : user.Socket.Receive(recvData_Byte, currentLength, 0);

                    recvData_Byte.CopyTo(data_Byte, data_Byte.Length - currentLength);      //將接收到的字節數保存       
                    currentLength -= recvAlready;                                           //減掉已經接收到的字節數
                } else {
                    Thread.Sleep(50);   //讓出線程
                }

                //#### UnFinshed：currentLength例外處理
            }
            return data_Byte;
        }

    }

}
