using DeepWorkshop.QQRot.FirstCity.MyTool;
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
        public static long CurrentSelectedGroupId;//当前选中的群号
        public static int SelectedGroupIndex;//当前被选中的群的索引
        public static String Seq;//当前选中的数据库
        public static Form1 MainFrom;//主窗口，只会出现一个
        public static bool IsJianTing = false;//是否启动监听指定群中群员发送的消息

        public static long LoginQQ;//当前登录的qq号
        public static string LoginNick;//当前登录的qq号的昵称

        public static List<GroupMemberInfoWithBocai> GroupMemberInfoList;//当前选择的qq群的群员list
        public static Dictionary<long, GroupMemberInfoWithBocai> GroupMemberInfoDic;////当前选择的qq群的群员Dictionary，方便找群员信息
        public static MyDictionaryUtil<long, GroupMemberInfoWithBocai> SearchMemberInfo = new MyDictionaryUtil<long, GroupMemberInfoWithBocai>();//用于查询群成员信息的查询器
        public static bool IsInitComplete = false;
        public static bool IsAutoAddGroupMemberJifen = false;

        public static MainPlugin MainPluginForTest { get; internal set; }
    }
}
