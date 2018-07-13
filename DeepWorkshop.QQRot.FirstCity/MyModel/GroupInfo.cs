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

        public List<GroupMemberInfoWithBocai> MemberList;//群员信息

        

        public GroupInfo(string groupName,long groupId,long ownerNumber)
        {
            this.GroupName = groupName;
            this.GroupId = groupId;
            this.OwnerNumber = ownerNumber;
        }
    }
}
