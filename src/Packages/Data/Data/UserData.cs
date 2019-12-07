using System.Net.Sockets;

namespace LearnHub.Data {

    public interface INetworkPacketType {
        PackageType PackageType { get; set; }
        EncryptionType EncryptionType { get; set; }
    }

    public class User : INetworkPacketType {

        public User(Socket socket) {
            Socket = socket;
        }

        //用戶資料
        public Socket Socket { get; private set; }

        //玩家連線狀態
        public bool IsConnected { get; set; }                                   //是否還在線
        public bool Responese { get; set; }                                     //響應回應

        public int CrcCode { get; set; }

        public PackageType PackageType { get; set; }
        public EncryptionType EncryptionType { get; set; }

    }

}