using System;
using System.Net;
using Network.Packet;

namespace LearnHub_Server.Tests {

    public class Test {

        //封包測試
        public void Start() {

            Send send = new Send();
            UnPack unPack = new UnPack();

            User user = new User();
            user.CrcCode = 234625;
            user.EncryptionType = EncryptionType.None;

            byte[] Data = send.IntPacket(user, PackageType.Test, 23849);

            byte[] head = unPack.UpPackHead(Data);

            int CrcCode = unPack.HeadCrcCode(head);
            EncryptionType encryption = unPack.HeadEncryptionType(head);
            PackageType packetType = unPack.HeadPackageType(head);

            int Length = unPack.HeadBodyLength(head);
            byte[] Test = unPack.UpPackBody(Data, Length);

            int IntData = unPack.BodyIntData(Test);


            Console.WriteLine($"Crc: {CrcCode}\tEn:{encryption}\tPack:{packetType}\tIntdata:{IntData}");
        }

    }
}
