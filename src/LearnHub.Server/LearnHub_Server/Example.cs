
//系統自帶(下方補充)
using System;


//自定義(下方補充)
using LearnHub.Server.Setup;


//命名空間
namespace LearnHub_Server {

    //接口宣告處

    //類別、此命名空間中不可以再重複定義Example這個名稱
    public class Example {

        //成員變量、全域變數
        private int intData;
        private static readonly bool IsWork = false;    //靜態唯讀

        //實例 Instance
        public Example(int intData) {
            //初始化物件
            Initail();

            //初始化參數(下方補充)
            this.intData = intData; //當此類成員變量與參數同名，則會用this表示該類的成員變量

        }

        //初始化方法(僅初始化物件)
        private void Initail() {
            //內容
        }

        //接口方法

        //抽象方法

        //一般方法

        //其他方法

        //錯誤處理方法
    }

}
