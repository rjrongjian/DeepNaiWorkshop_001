using DeepWorkshop.QQRot.FirstCity.MyModel;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using Newbe.CQP.Framework;
using Newbe.CQP.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication4;
using 新一城娱乐系统;

namespace DeepWorkshop.QQRot.FirstCity
{
   /// <summary>
   /// 各类消息事件处理入口
   /// </summary>
    public class MainPlugin : PluginBase
    {
        
        public static Form1 frmMain = null;//软件主窗口
        private bool IsSupportedRuntimeVersion = false;
        private static readonly object Obj = new object();
        public MainPlugin(ICoolQApi coolQApi) : base(coolQApi)
        {
            try
            {
                //这里调用的话，会报authCode错误,
                /*
                CacheData.LoginQQ = coolQApi.GetLoginQQ();
                CacheData.LoginNick = coolQApi.GetLoginNick();
                MyLogUtil.ToLogFotTest(CacheData.LoginQQ + "_" + CacheData.LoginNick);
                */
                //先判断当前电脑是否有合适的.net版本
                if (!MyDotNetFrameworkUtil.IsSupportedRuntimeVersion())
                {
                    IsSupportedRuntimeVersion = false;

                    new RuntimeVerForm().Show();

                    return;
                }
                else
                {
                    IsSupportedRuntimeVersion = true;
                    CacheData.CoolQApi = coolQApi;
                    //酷q登录成功后，进入软件登录页面
                    new login().Show();
                }

                
                
            }
            catch (Exception ex)
            {
                MyLogUtil.ErrToLog("不可预知的异常，原因："+ex);
                MessageBox.Show("不可预知的异常，请查看错误日志");
            }
            


        }
        /// <summary>
        /// AppId需要与程序集名称相同
        /// </summary>
        public override string AppId => "DeepWorkshop.QQRot.FirstCity";
        
        /// <summary>
        /// 监听私聊事件
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="sendTime"></param>
        /// <param name="fromQQ"></param>
        /// <param name="msg"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public override int ProcessPrivateMessage(int subType, int sendTime, long fromQQ, string msg, int font)
        {
            //使用CoolQApi将信息回发给发送者
            //CoolQApi.SendPrivateMsg(fromQQ, msg);
            return base.ProcessPrivateMessage(subType, sendTime, fromQQ, msg, font);
        }

        /// <summary>
        /// 处理群聊消息
        /// </summary>
        /// <param name="subType">消息类型，目前固定为1</param>
        /// <param name="sendTime">消息发送时间的时间戳</param>
        /// <param name="fromGroup">消息来源群号</param>
        /// <param name="fromQq">发送此消息的QQ号码</param>
        /// <param name="fromAnonymous">发送此消息的匿名用户</param>
        /// <param name="msg">消息内容</param>
        /// <param name="font">消息所使用字体</param>
        /// <returns></returns>
        public override int ProcessGroupMessage(int subType, int sendTime, long fromGroup, long fromQq, string fromAnonymous, string msg, int font)
        {
            //在进入主界面容易崩溃，是不是因为软件环境还没油准备好，就去处理消息了
            if (IsSupportedRuntimeVersion&&CacheData.IsInitComplete)//只有当主界面完全加载完成，CacheData.IsInitComplete才会变为true
            {

                frmMain.MessageArrival(fromGroup, fromQq, msg);
            }
            return base.ProcessGroupMessage(subType, sendTime, fromGroup, fromQq, fromAnonymous, msg, font);
        }


        /// <summary>
        /// 处理群成员添加事件
        /// </summary>
        /// <param name="subType">事件类型。1为管理员已同意；2为管理员邀请</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQq, long target)
        {
            //MyLogUtil.ToLogFotTest("群员增加事件：" + fromGroup + "_fromQQ;" + target + "__"+ CacheData.IsAutoAddGroupMemberJifen+"__"+ CacheData.IsInitComplete);
            if (CacheData.IsAutoAddGroupMemberJifen && CacheData.IsInitComplete&& fromGroup==CacheData.CurrentSelectedGroupId)
            {
                lock (Obj)
                {

                    //MyLogUtil.ToLogFotTest("1111111：");
                    try
                    {
                        //先查询是否已经有他的数据了
                        GroupMemberInfoWithBocai g =  CacheData.SearchMemberInfo.GetValue(CacheData.GroupMemberInfoDic, target);
                        if (g == null)//说明第一次建立此用户信息
                        {
                            //win7下，Cool​Api​Extensions.GetGroupMemberInfoV2也会崩
                            /*
                            ModelWithSourceString<GroupMemberInfo> model = Cool​Api​Extensions.GetGroupMemberInfoV2(CacheData.CoolQApi, fromGroup, target, true);
                            //MyLogUtil.ToLogFotTest("获取的群会员信息1：" + model);
                            //MyLogUtil.ToLogFotTest("获取的群会员信息12：" + model.Model.NickName);
                            if (model != null)
                            {
                                GroupMemberInfo groupMemberInfo = model.Model;
                                //MyLogUtil.ToLogFotTest("获取的群会员信息："+ groupMemberInfo.NickName);
                                GroupMemberInfoWithBocai temp = new GroupMemberInfoWithBocai(groupMemberInfo, CacheData.GroupMemberInfoList.Count);
                                CacheData.GroupMemberInfoList.Add(temp);
                                CacheData.GroupMemberInfoDic.Add(target, temp);

                                //将数据展示在软件列表中，并添加数据到数据库

                                    CacheData.MainFrom.dgv2(temp);
                             }
                            
                            */
                            //方案二
                            GroupMemberInfo groupMemberInfo = new GroupMemberInfo();
                            groupMemberInfo.NickName = "" + target;
                            groupMemberInfo.Number = target;
                            groupMemberInfo.GroupId = fromGroup;

                            GroupMemberInfoWithBocai temp = new GroupMemberInfoWithBocai(groupMemberInfo, CacheData.GroupMemberInfoList.Count);
                            temp.IsAutoAddGroupMember = true;
                            CacheData.GroupMemberInfoList.Add(temp);
                            CacheData.GroupMemberInfoDic.Add(target, temp);

                            //将数据展示在软件列表中，并添加数据到数据库

                            CacheData.MainFrom.dgv2(temp);
                        }
                        else
                        {
                            MyLogUtil.ToLogFotTest("5555555555,此qq信息已经缓存：" + target);
                        }
                    
                    }catch(Exception ex)
                    {
                        MyLogUtil.ToLogFotTest("当群员入群后，自动添加用户时出现异常，原因：" + ex);
                        //CacheData.CoolQApi.AddLog(40,CoolQLogLevel.Debug, "当群员入群后，自动添加用户时出现异常，原因："+ex);
                    }
                }
                //MyLogUtil.ToLogFotTest("3333333：");

            }

            return base.ProcessGroupMemberIncrease(subType, sendTime, fromGroup, fromQq, target);
        }

        /// <summary>
        /// 处理群成员数量减少事件
        /// </summary>
        /// <param name="subType">事件类型。1为群员离开；2为群员被踢为；3为自己(即登录号)被踢</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQq, long target)
        {

            MyLogUtil.ToLogFotTest("处理群员减少"+fromGroup+"_"+fromQq+"_"+target);
            
            if (CacheData.IsAutoAddGroupMemberJifen && CacheData.IsInitComplete && fromGroup == CacheData.CurrentSelectedGroupId)
            {
                try
                {
                    lock (Obj)
                    {
                        MyLogUtil.ToLogFotTest("处理群员减少1");
                        //先查询是否已经有他的数据了
                        GroupMemberInfoWithBocai g = CacheData.SearchMemberInfo.GetValue(CacheData.GroupMemberInfoDic, target);
                        if (g != null)//移除该qq会员在列表中的显示
                        {
                        
                            MyLogUtil.ToLogFotTest("处理群员减少2,选中的用户的索引：" + g.ArrIndex+"_昵称"+ CacheData.GroupMemberInfoList[g.ArrIndex].GroupMemberBaseInfo.NickName+"_target:"+target);
                            List<GroupMemberInfoWithBocai> list = new List<GroupMemberInfoWithBocai>();
                            for(int i = 0;i< CacheData.GroupMemberInfoList.Count; i++)
                            {
                                GroupMemberInfoWithBocai groupMember = CacheData.GroupMemberInfoList[i];
                                if (groupMember.GroupMemberBaseInfo.Number != target)
                                {
                                    groupMember.ArrIndex = list.Count;
                                    list.Add(groupMember);
                                }
                            }
                            CacheData.GroupMemberInfoList = list;
                            CacheData.SearchMemberInfo.Remove(CacheData.GroupMemberInfoDic, target);
                            CacheData.MainFrom.RefreshGroupMemberList();
                            /*
                            CacheData.GroupMemberInfoList[g.ArrIndex] = null;//由于每个成员对象中存了在当前列表中的索引（以免每次查找都要遍历列表），为了避免重建每个对象的索引，直接置为空
                            CacheData.SearchMemberInfo.Remove(CacheData.GroupMemberInfoDic, target);
                            CacheData.MainFrom.RefreshGroupMemberList();
                            */

                        }
                        else
                        {
                            MyLogUtil.ToLogFotTest("处理群员减少2");
                        }
                    }
                }
                catch(Exception ex)
                {
                    MyLogUtil.ToLogFotTest("当群员退群后，自动删减用户时出现异常，原因：" + ex);
                    //CacheData.CoolQApi.AddLog(40, CoolQLogLevel.Debug, "当群员退群后，自动删减用户时出现异常，原因" + ex);
                }
                
            }
            

            return base.ProcessGroupMemberDecrease(subType, sendTime, fromGroup, fromQq, target);
        }
        /*

        /// <summary>
        /// 好友添加请求
        /// </summary>
        /// <param name="subType">事件类型。固定为1。</param>
        /// <param name="sendTime">	事件发生时间的时间戳。</param>
        /// <param name="fromQq">事件来源QQ。</param>
        /// <param name="msg">附言内容。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)</param>
        /// <returns></returns>
        public override int ProcessAddFriendRequest(int subType, int sendTime, long fromQq, string msg, string responseFlag)
        {
            
            return base.ProcessAddFriendRequest(subType, sendTime, fromQq, msg, responseFlag);
        }

        /// <summary>
        /// 处理好友已添加事件（此事件监听不到）
        /// </summary>
        /// <param name="subType">事件类型。固定为1</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <returns></returns>
        public override int ProcessFriendsAdded(int subType, int sendTime, long fromQq)
        {

            //给用户回复的信息日志
            //MyLogUtil.WriteQQDialogueLogOfMe(fromQq, SystemConfig.MsgWhenFriendsAdded);
            //CoolQApi.SendPrivateMsg(fromQq, SystemConfig.MsgWhenFriendsAdded);

            return base.ProcessFriendsAdded(subType, sendTime, fromQq);
        }

        /// <summary>
        /// 处理讨论组消息
        /// </summary>
        /// <param name="subType">消息类型，目前固定为1</param>
        /// <param name="sendTime">消息发送时间的时间戳</param>
        /// <param name="fromDiscuss">消息来源讨论组号</param>
        /// <param name="fromQq">发送此消息的QQ号码</param>
        /// <param name="msg">消息内容</param>
        /// <param name="font">消息所使用字体</param>
        /// <returns></returns>
        public override int ProcessDiscussGroupMessage(int subType, int sendTime, long fromDiscuss, long fromQq, string msg, int font)
        {

            //mainForm.displayMsg2("处理讨论组消息：" + subType + "," + sendTime + "," + fromDiscuss + "," + fromQq + "," + msg + "," + font);
            return base.ProcessDiscussGroupMessage(subType, sendTime, fromDiscuss, fromQq, msg, font);
        }

        /// <summary>
        /// 处理群管理员变动事件
        /// </summary>
        /// <param name="subType">事件类型。1为被取消管理员，2为被设置管理员</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupAdminChange(int subType, int sendTime, long fromGroup, long target)
        {
            //mainForm.displayMsg2("处理群管理员变动事件：" + subType + "," + sendTime + "," + fromGroup + "," + target);

            return base.ProcessGroupAdminChange(subType, sendTime, fromGroup, target);
        }
        /// <summary>
        /// 处理群成员数量减少事件
        /// </summary>
        /// <param name="subType">事件类型。1为群员离开；2为群员被踢为；3为自己(即登录号)被踢</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQq, long target)
        {
            //mainForm.displayMsg2("处理群成员数量减少事件：" + subType + "," + sendTime + "," + fromGroup + "," + fromQq + "," + target);

            return base.ProcessGroupMemberDecrease(subType, sendTime, fromGroup, fromQq, target);
        }
        /// <summary>
        /// 处理群成员添加事件
        /// </summary>
        /// <param name="subType">事件类型。1为管理员已同意；2为管理员邀请</param>
        /// <param name="sendTime">事件发生时间的时间戳</param>
        /// <param name="fromGroup">事件来源群号</param>
        /// <param name="fromQq">事件来源QQ</param>
        /// <param name="target">被操作的QQ</param>
        /// <returns></returns>
        public override int ProcessGroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQq, long target)
        {
            

            return base.ProcessGroupMemberIncrease(subType, sendTime, fromGroup, fromQq, target);
        }
        
        /// <summary>
        /// 处理群文件上传事件
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="sendTime"></param>
        /// <param name="fromGroup"></param>
        /// <param name="fromQq"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public override int ProcessGroupUpload(int subType, int sendTime, long fromGroup, long fromQq, string file)
        {
            //mainForm.displayMsg2("处理群文件上传事件：" + subType + "," + sendTime + "," + fromGroup + "," + fromQq + "," + file);

            return base.ProcessGroupUpload(subType, sendTime, fromGroup, fromQq, file);
        }
        /// <summary>
        /// 处理加群请求（有加群请求）
        /// </summary>
        /// <param name="subType">请求类型。1为他人申请入群；2为自己(即登录号)受邀入群</param>
        /// <param name="sendTime">请求发送时间戳</param>
        /// <param name="fromGroup">要加入的群的群号</param>
        /// <param name="fromQq">发送此请求的QQ号码</param>
        /// <param name="msg">附言内容</param>
        /// <param name="responseMark">用于处理请求的标识</param>
        /// <returns></returns>
        public override int ProcessJoinGroupRequest(int subType, int sendTime, long fromGroup, long fromQq, string msg, string responseMark)
        {

            return base.ProcessJoinGroupRequest(subType, sendTime, fromGroup, fromQq, msg, responseMark);
        }
        */
    }

}
