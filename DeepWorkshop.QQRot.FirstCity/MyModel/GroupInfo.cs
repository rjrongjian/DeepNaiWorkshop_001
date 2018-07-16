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
    /// 群信息
    /// </summary>
    public class GroupInfo
    {
        public string GroupName;//群名
        public long GroupId;//群号
        public long OwnerNumber;

        //public List<GroupMemberInfoWithBocai> MemberList;//群员信息 迁移至CacheData类中
        public string NickNameShort = "";

        //以下是群组所有会员汇总数据

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

        public GroupInfo()
        {

        }

        public GroupInfo(string groupName,long groupId,long ownerNumber)
        {
            this.GroupName = groupName;
            this.GroupId = groupId;
            this.OwnerNumber = ownerNumber;
        }
    }
}
