using System;
using System.Net;
using Network.Packet;

namespace LearnHub_Server.Tests {

    public class Test {

        //封包測試
        public void Start() {

            Send send = new Send();
            Unpack unPack = new Unpack();

            User user = new User();
            user.CrcCode = 234625;
            user.EncryptionType = EncryptionType.None;

            byte[] Data = send.IntPacket(user, PackageType.Test, 23849);

            byte[] head = unPack.Unpack_Head(Data);

            int CrcCode = unPack.Head_CrcCode(head);
            EncryptionType encryption = unPack.Head_EncryptionType(head);
            PackageType packetType = unPack.Head_PackageType(head);

            int Length = unPack.Head_BodyLength(head);
            byte[] Test = unPack.Unpack_Body(Data, Length);

            int IntData = unPack.Body_IntData(Test);


            Console.WriteLine($"Crc: {CrcCode}\tEn:{encryption}\tPack:{packetType}\tIntdata:{IntData}");
        }

    }
}
