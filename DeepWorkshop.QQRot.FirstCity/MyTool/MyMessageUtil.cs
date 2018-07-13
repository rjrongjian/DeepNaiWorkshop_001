using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeepWorkshop.QQRot.FirstCity.MyModel;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    /// <summary>
    /// qq中收到的消息处理类
    /// </summary>
    public class MyMessageUtil
    {
        public static MessageInfo ConvertMessage(string msg)
        {
            String message = msg;
            bool isTxtMsg = true;
            if (message.Contains("[CQ:face,id="))//包含 [CQ:face,id={1}] - QQ表情
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:face,id=.+?\\]", "[QQ表情]");
            }
            if (message.Contains("[CQ:emoji,id="))//包含 [CQ:emoji,id={1}] - emoji表情
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:emoji,id=.+?\\]", "[emoji表情]");
            }

            if (message.Contains("[CQ:bface,id="))//包含 [CQ:bface,id={1}] - 原创表情
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:bface,id=.+?\\]", "[原创表情]");
            }

            if (message.Contains("[CQ:sface,id="))//包含 [CQ:sface,id={1}] - 小表情 
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:sface,id=.+?\\]", "[小表情]");
            }
            if (message.Contains("[CQ:image,file="))//包含 [CQ:image,file={1}] - 发送自定义图片
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:image,file=.+?\\]", "[自定义图片]");
            }
            if (message.Contains("[CQ:record,file="))//包含 [CQ:record,file={1},magic={2}] - 发送语音
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:record,file=.+?,magic=.+?\\]", "[语音]");
            }
            if (message.Contains("[CQ:at,qq="))//包含 [CQ:at,qq={1}] - @某人
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:at,qq=.+?\\]", "[@某人]");
            }
            if (message.Contains("[CQ:rps,type="))//包含 [CQ:rps,type={1}] 发送猜拳魔法表情
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:rps,type=.+?\\]", "[猜拳魔法表情]");
            }
            if (message.Contains("[CQ:dice,type="))//包含 [CQ:dice,type={1}]  发送掷骰子魔法表情
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:dice,type=.+?\\]", "[掷骰子魔法表情]");
            }
            if (message.Contains("[CQ:anonymous,ignore="))//包含 [CQ:anonymous,ignore={1}] - 匿名发消息（仅支持群消息使用）
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:anonymous,ignore=.+?\\]", "[匿名消息]");
            }
            if (message.Contains("[CQ:music,type="))//包含 [CQ:music,type={1},id={2}] - 发送音乐
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:music,type=.+?,id=.+?\\]", "[音乐]");
            }
            if (message.Contains("[CQ:music,type=custom,url="))//包含 [CQ:music,type=custom,url={1},audio={2},title={3},content={4},image={5}] - 发送音乐自定义分享
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:music,type=custom,url=.+?,audio=.+?,title=.+?,content=.+?,image=.+?\\]", "[音乐]");
            }
            if (message.Contains("[CQ:share,url="))//包含 [CQ:share,url={1},title={2},content={3},image={4}] - 发送链接分享
            {
                isTxtMsg = false;
                message = Regex.Replace(message, "\\[CQ:share,url=.+?,title=.+?,content=.+?,image=.+?\\]", "[链接分享]");
            }
            if (message.Contains("[CQ:anonymous]"))
            {
                isTxtMsg = false;
                message = message.Replace("[CQ:anonymous]", "[匿名消息]");
            }

            return new MessageInfo(message, isTxtMsg ? 1 : 2);
            

        }
    }
}
