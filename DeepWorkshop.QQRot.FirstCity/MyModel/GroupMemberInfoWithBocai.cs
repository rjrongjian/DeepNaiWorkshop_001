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
        public string RemarkName = "";

        public bool sfrj = false;//是否入局
        public int ArrIndex;//获取的群员列表在数组中的索引值
        public String Seq;//类似于每个用户在不同表中对应的索引
        public int Id;//此会员在本地数据库的Id 在dgv2中有更新
        public bool IsAutoAddGroupMember = false;//是否是自动加入的群员

        //以下是个人数据

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

        public string bendibeizhu = "";//本地备注（存我们数据库的）

        public GroupMemberInfoWithBocai(GroupMemberInfo groupMemberBaseInfo,int arrIndex)
        {
            GroupMemberBaseInfo = groupMemberBaseInfo;
            ArrIndex = arrIndex;
            //this.Seq = groupMemberBaseInfo.NickName + groupMemberBaseInfo.Number;
            this.Seq = ""+groupMemberBaseInfo.Number;//每一个用户的识别码只用qq号码来标识
        }

        /// <summary>
        /// 自动添加到列表中的用户由于不能获取成员信息，只有一个qq号码，所以当此成员存在本地备注时，用备注名称，没有的话昵称用qq号码显示
        /// </summary>
        /// <returns></returns>
        public String GetNickName()
        {
            if (IsAutoAddGroupMember)
            {
                if (string.IsNullOrWhiteSpace(bendibeizhu))
                {
                    return GroupMemberBaseInfo.NickName;
                }
                else
                {
                    return bendibeizhu;
                }
            }
            else
            {
                return GroupMemberBaseInfo.NickName;
            }
        }

    }
}
