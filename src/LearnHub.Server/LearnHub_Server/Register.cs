using System;
using System.Collections.Generic;
using Data;

namespace LearnHub_Server.Register {

    #region 註冊

    //Package   組件
    //Packet    封包

    public delegate void PacketCallBackHandler(User client, byte[] Head, byte[] Body);

    /// <summary>
    /// 註冊類
    /// </summary>
    public class Register {

        

        public Register() {
            Dictionary<PackageType, PacketCallBackHandler> CallBacksDictionary = new Dictionary<PackageType, PacketCallBackHandler>();
        }




        /// <summary>
        /// 註冊 封包回調方法(靜態)
        /// </summary>
        /// <param name="packageType">封包型態</param>
        /// <param name="PK_CallBackMethod">封包回調方法</param>
        public static void Register(PackageType packageType, ServerCallBack PK_CallBackMethod) {
            if (!PK_CallBacksDictionary.ContainsKey(packageType))
                PK_CallBacksDictionary.Add(packageType, PK_CallBackMethod);
            else
                Console.WriteLine("# Warning: 封包註冊了相同的回調事件");
        }

        /// <summary>
        /// 註冊 數據庫回調方法(靜態)
        /// </summary>
        /// <param name="dataBaseType">封包中關於數據庫行為型態</param>
        /// <param name="DB_DataBaseCallBack">資料庫回調方法</param>
        public static void Register(DataBaseType dataBaseType, DataBaseCallBack DB_DataBaseCallBack) {
            if (!DB_CallBacksDictionary.ContainsKey(dataBaseType))
                DB_CallBacksDictionary.Add(dataBaseType, DB_DataBaseCallBack);
            else
                Console.WriteLine("# Warning: 數據庫註冊了相同的回調事件");
        }

    }




    /// <summary>
    /// 註冊接口
    /// </summary>
    public interface IRegister {
        void Register<T>(T packetType, CallBackEventHandler callBackEventHandler);
    }

    /// <summary>
    /// 封包型態註冊
    /// </summary>
    public class PackageTypeRegister : IRegister {

        private static Dictionary<PackageType, CallBackEventHandler> CallBacksDictionary;

        /// <summary>
        /// Instance
        /// </summary>
        public PackageTypeRegister() {
            CallBacksDictionary = new Dictionary<PackageType, CallBackEventHandler>();
        }

        /// <summary>
        /// 註冊方法
        /// </summary>
        public void Register<T>(T packetType, CallBackEventHandler callBackEventHandler) {

            object Type = packetType;
            var packageType = (PackageType)Type;

            if (!CallBacksDictionary.ContainsKey(packageType)) {
                CallBacksDictionary.Add(packageType, callBackEventHandler);
                Console.WriteLine("# 註冊成功");
            } else
                Console.WriteLine("# Warning: 封包註冊了相同的回調事件");
        }
    }

    /// <summary>
    /// 資料庫型態註冊
    /// </summary>
    public class DatabaseTypeRegister : IRegister {

        private static Dictionary<DatabaseType, CallBackEventHandler> CallBacksDictionary;

        /// <summary>
        /// Instance
        /// </summary>
        public DatabaseTypeRegister() {
            CallBacksDictionary = new Dictionary<DatabaseType, CallBackEventHandler>();
        }

        /// <summary>
        /// 註冊方法
        /// </summary>
        public void Register<T>(T packetType, CallBackEventHandler callBackEventHandler) {

            object Type = packetType;
            var databaseType = (DatabaseType)Type;

            if (!CallBacksDictionary.ContainsKey(databaseType))
                CallBacksDictionary.Add(databaseType, callBackEventHandler);
            else
                Console.WriteLine("# Warning: 資料庫註冊了相同的回調事件");
        }
    }



    #endregion

}
