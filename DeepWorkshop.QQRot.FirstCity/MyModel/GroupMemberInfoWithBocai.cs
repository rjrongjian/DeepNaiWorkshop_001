using AI.Bll;
using Newbe.CQP.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyModel
{
    /// <summary>
    /// 扩展会员信息
    /// </summary>
    public class GroupMemberInfoWithBocai
    {
        public GroupMemberInfo GroupMemberBaseInfo;

        public bool sfrj = false;//是否入局


        public string NickNameShort = "";
        //时时彩积分
        public xztj xiazhutongji = new xztj();

        //
        public xztj shangQiXiaZhu = new xztj();

        public int lsxzjf = 0;//临时下注积分
        public int lszjf = 0;//临时现有积分

        public string conter = "";//本期下注指令
        public int benqiyingkui = 0;//本期盈亏
        public int benqixiazhu = 0;//本期下注积分

        public int zongjifen = 0;//现有积分
        public int zongyingkui = 0;//总盈亏
        public int zongxiazhu = 0;//总下注积分

        public string bendibeizhu = "";//本地备注

        public GroupMemberInfoWithBocai(GroupMemberInfo groupMemberBaseInfo)
        {
            GroupMemberBaseInfo = groupMemberBaseInfo;
        }
    }
}
