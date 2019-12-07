namespace LearnHub.Netwrok.Setup {

    //縮寫定義：
    //  POS     Position
    //  LEN     Length

    /// <summary>
    /// 封包參數設定
    /// </summary>
    public static class ParameterList {

        // head data position
        public const int CRC_Code_POS = 0;      
        public const int PACKAGE_Type_POS = 4;
        public const int ENCRYPTION_TYPE_POS = 6;     
        //public const int DATABASE_Type_POS = 8;
        public const int BODY_LENGTH_POS = 8;

        // data length
        public const int HEAD_LEN = 12;   //定義封包頭長度
        

    }

}
