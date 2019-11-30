using System;
using System.Collections.Generic;
using Data;

namespace LearnHub_Server {

    #region 註冊

    //Package   組件
    //Packet    封包

    public delegate void CallBackHandler(User client, byte[] Head, byte[] Body);

    /// <summary>
    /// 註冊類
    /// </summary>
    public class Register {

        private IRegister register;

        /// <summary>
        /// 
        /// </summary>
        public Register(IRegister register) {
            this.register = register;
        }

        /// <summary>
        /// 註冊方法
        /// </summary>
        public void Add<T>(T packetType, CallBackHandler callBackHandler) {
            register.Register(packetType, callBackHandler);
        }

    }




    /// <summary>
    /// 註冊接口
    /// </summary>
    public interface IRegister {
        void Register<T>(T packetType, CallBackHandler callBackHandler);
    }

    /// <summary>
    /// 封包型態註冊
    /// </summary>
    public class PackageTypeRegister : IRegister {

        private static Dictionary<PackageType, CallBackHandler> CallBacksDictionary;

        /// <summary>
        /// Instance
        /// </summary>
        public PackageTypeRegister() {
            CallBacksDictionary = new Dictionary<PackageType, CallBackHandler>();
        }

        /// <summary>
        /// 註冊方法
        /// </summary>
        public void Register<T>(T packetType, CallBackHandler callBackHandler) {

            object Type = packetType;
            var packageType = (PackageType)Type;

            if (!CallBacksDictionary.ContainsKey(packageType)) {
                CallBacksDictionary.Add(packageType, callBackHandler);
                Console.WriteLine("# 註冊成功");
            } else
                Console.WriteLine("# Warning: 封包註冊了相同的回調事件");
        }
    }

    /// <summary>
    /// 資料庫型態註冊
    /// </summary>
    public class DatabaseTypeRegister : IRegister {

        private static Dictionary<DatabaseType, CallBackHandler> CallBacksDictionary;

        /// <summary>
        /// Instance
        /// </summary>
        public DatabaseTypeRegister() {
            CallBacksDictionary = new Dictionary<DatabaseType, CallBackHandler>();
        }

        /// <summary>
        /// 註冊方法
        /// </summary>
        public void Register<T>(T packetType, CallBackHandler callBackHandler) {

            object Type = packetType;
            var databaseType = (DatabaseType)Type;

            if (!CallBacksDictionary.ContainsKey(databaseType))
                CallBacksDictionary.Add(databaseType, callBackHandler);
            else
                Console.WriteLine("# Warning: 資料庫註冊了相同的回調事件");
        }
    }



    #endregion

}
