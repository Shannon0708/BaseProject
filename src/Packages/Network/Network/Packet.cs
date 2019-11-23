using System;
using System.Net;

namespace Network.Packet {

    public class Send {
        
        /// <summary>
        /// 封裝
        /// </summary>
        /// <param name="crcCode">封包驗證碼</param>
        /// <param name="packageType">封包型態</param>
        /// <param name="encryptionType">定義加密的方法</param>
        /// <param name="data_Byte">封包資料主體內容</param>
        /// <returns>回傳封裝完畢的封包</returns>
        private byte[] BuildPackage(int crcCode, PackageType packageType, EncryptionType encryptionType, byte[] data_Byte) {
            
            //using System有:BitConverter GetBytes
            //using System.Net有:IPAddress HostToNetworkOrder把後面括號內的參數轉換成網路格式
            byte[] crcCode_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(crcCode));             
            byte[] packageType_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)packageType));          //(short)強制將後面參數轉成短整數
            byte[] encryptionType_Byte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)encryptionType));       
            byte[] bodyLength = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data_Byte.Length));

            byte[] Packet = new byte[8 + data_Byte.Length];

            //CopyTo:把前面的內容複製到後面陣列的起始位置(陣列,起始位置)
            crcCode_Byte.CopyTo(Packet, 0);
            packageType_Byte.CopyTo(Packet, 4);
            encryptionType_Byte.CopyTo(Packet, 6);
            bodyLength.CopyTo(Packet, 8);

            return Packet;

        }

    }

}
