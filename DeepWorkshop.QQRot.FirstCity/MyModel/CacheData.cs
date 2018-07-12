using Newbe.CQP.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication4;

namespace DeepWorkshop.QQRot.FirstCity.MyModel
{
    public class CacheData
    {
        public static ICoolQApi CoolQApi;
        public static List<GroupInfo> CurrentGroupList;//当前加载的群列表
        public static int SelectedGroupIndex;//当前被选中的群的索引
        public static String Seq;//当前选中的数据库
        public static Form1 MainFrom;//主窗口，只会出现一个
        public static bool IsJianTing = false;//是否启动监听指定群中群员发送的消息

        public static long LoginQQ;//当前登录的qq号
        public static string LoginNick;//当前登录的qq号的昵称
    }
}
