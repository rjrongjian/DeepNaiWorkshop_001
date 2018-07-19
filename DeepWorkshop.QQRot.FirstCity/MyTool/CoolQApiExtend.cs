using AI.Bll;
using DeepWorkshop.QQRot.FirstCity.MyModel;
using Newbe.CQP.Framework;
using Newbe.CQP.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    public class CoolQApiExtend
    {
        /// <summary>
        /// 解析群列表
        /// </summary>
        /// <param name="groupInfo">密文</param>
        /// <returns></returns>
        public static List<MyModel.GroupInfo> ParseGroupList(string groupInfo)
        {
            //MyLogUtil.ToLog("获取的群列表："+ groupInfo);
            byte[] bt = Convert.FromBase64String(groupInfo);
            int weizhi = 0;
            double groups = 0;
            string groupname3 = "";
            List<MyModel.GroupInfo> groupInfoList = new List<MyModel.GroupInfo>();
            for (int i = 0; i < 4; i++)
            {
                groups += bt[i] * Math.Pow(256, 3 - i);
                weizhi++;
            }
            for (int j = 0; j < Convert.ToInt32(groups); j++)
            {
                double chang = 0;
                for (int i = 4; i < 6; i++)
                {
                    chang += bt[weizhi] * Math.Pow(256, 5 - i);
                    weizhi++;
                }
                double qunhao = 0;
                for (int i = 6; i < 14; i++)
                {
                    qunhao += bt[weizhi] * Math.Pow(256, 13 - i);
                    weizhi++;
                }
                long groupId = Convert.ToInt64(qunhao.ToString());
                //MyLogUtil.ToLog("群号：" + qunhao.ToString());
                weizhi++;
                weizhi++;
                int namechang = Convert.ToInt32(chang) - 10;
                List<byte> listname = new List<byte>();
                for (int i = 0; i < namechang; i++)
                {
                    listname.Add(bt[weizhi]);
                    weizhi++;
                }
                byte[] byt = listname.ToArray();
                groupname3 = Encoding.Default.GetString(byt);
                //MyLogUtil.ToLog("群名：" + groupname3.ToString());
                groupInfoList.Add(new MyModel.GroupInfo(groupname3,groupId,0));
            }

            return groupInfoList;

        }
        /// <summary>
        /// 获取qq群列表
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public static List<MyModel.GroupInfo> GetGroupList(ICoolQApi api)
        {
            string content = api.CQ_getGroupList();
            List<MyModel.GroupInfo> list = ParseGroupList(content);
            //缓存当前加载的群列表
            CacheData.CurrentGroupList = list;
            return list;
        } 
        /// <summary>
        /// 获取群成员，并缓存
        /// </summary>
        /// <param name="api"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static List<GroupMemberInfoWithBocai> GetGroupMemberListAndCache(ICoolQApi api, long groupId)
        {
            List<GroupMemberInfoWithBocai> list = new List<GroupMemberInfoWithBocai>();
            Dictionary<long, GroupMemberInfoWithBocai> keyVal = new Dictionary<long, GroupMemberInfoWithBocai>();
            ModelWithSourceString<IEnumerable<GroupMemberInfo>> result = CoolApiExtensions.GetGroupMemberList(api, groupId);
            IEnumerable<GroupMemberInfo> iterator = result.Model;
            if (iterator != null)
            {
                foreach(GroupMemberInfo memberInfo in iterator)
                {
                    GroupMemberInfoWithBocai groupMemberInfoWithBocai = new GroupMemberInfoWithBocai(memberInfo, list.Count);

                    //增加短名字
                    //===================2018-02===================
                    string shortName = "";// Regex.Replace(g.NickName, @"\[[^\]]*?\]", "");                           
                    shortName = function.filtetStingSpecial(groupMemberInfoWithBocai.GroupMemberBaseInfo.NickName);
                    //===================2018-02===================


                    shortName = shortName.Replace("'", "").Replace("/", "").Replace("\\", "").Replace("\"", "").Replace(".", "");
                    bool isHanzi = false;
                    for (int ii = 0; ii < shortName.Length; ii++)
                    {
                        if ((int)shortName[ii] > 127)
                        {
                            //是汉字
                            isHanzi = true;
                            break;
                        }
                    }
                    if (isHanzi)
                    {
                        shortName = shortName.Length > 2 ? shortName.Substring(0, 2) : shortName;
                    }
                    else
                    {
                        shortName = shortName.Length > 4 ? shortName.Substring(0, 4) : shortName;
                    }

                    groupMemberInfoWithBocai.NickNameShort = shortName;

                    list.Add(groupMemberInfoWithBocai);
                    keyVal.Add(memberInfo.Number, groupMemberInfoWithBocai);
                }
            }

            CacheData.GroupMemberInfoList = list;
            CacheData.GroupMemberInfoDic = keyVal;
            return list;
        }




    }
}
