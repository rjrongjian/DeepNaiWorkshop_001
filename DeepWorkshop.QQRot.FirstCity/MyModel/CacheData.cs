using Newbe.CQP.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyModel
{
    public class CacheData
    {
        public static ICoolQApi CoolQApi;
        public static List<GroupInfo> CurrentGroupList;//当前加载的群列表
        public static int SelectedGroupIndex;//当前被选中的群的索引
        public static String Seq;//当前选中的数据库
    }
}
