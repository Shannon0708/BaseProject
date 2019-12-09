using System.Net.Sockets;
using LearnHub.Data.Type;

namespace LearnHub.Data {

    public class User {

        public User (Socket socket) {
            Socket = socket;
        }

        public Socket Socket { get; set; }

        public int CrcCode { get; set; }

        public PackageType PackageType { get; set; }
        public EncryptionType EncryptionType { get; set; }
    }

}
