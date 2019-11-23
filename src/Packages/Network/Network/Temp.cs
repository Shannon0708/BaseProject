using System.Net.Sockets;

namespace Network.Packet {

    /// <summary>
    /// 封包型態
    /// </summary>
    public enum PackageType {
        None,
        Test
    }

    /// <summary>
    /// 加密方法
    /// </summary>
    public enum EncryptionType {
        None,
        RES256
    }

    /// <summary>
    /// 用戶資料列表
    /// </summary>
	public class User {

        //User的屬性{get; set;}
        public int CrcCode { get; set; }
        public EncryptionType EncryptionType { get; set; }
        public Socket Socket { get; set; }

	}

}
