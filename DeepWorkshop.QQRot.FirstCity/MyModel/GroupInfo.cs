using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyModel
{
    public class GroupInfo
    {
        public string GroupName;//群名
        public long GroupId;//群号
        public long OwnerNumber;
        public GroupInfo(string groupName,long groupId,long ownerNumber)
        {
            this.GroupName = groupName;
            this.GroupId = groupId;
            this.OwnerNumber = ownerNumber;
        }
    }
}
