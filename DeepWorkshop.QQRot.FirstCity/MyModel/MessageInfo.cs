using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyModel
{
    public class MessageInfo
    {
        public String MessageContent;
        public int MessageType;
        public MessageInfo(String messageContent,int messageType)
        {
            MessageContent = messageContent;
            MessageType = messageType;
        }
    }
}
