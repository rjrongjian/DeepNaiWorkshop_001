using DeepWorkshop.QQRot.FirstCity.MyTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFrom_WebApi_Demo;

namespace DeepWorkshop.QQRot.FirstCity.Validate
{
    public class MyValidate
    {
        public static void T()
        {
            String url = "http://w.eydata.net/1f6208e5b7cc9208";  //  这里改成自己的地址
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            //  这里改成自己的参数名称
            parameters.Add("UserName", "Junlik");

            var ret = WebPost.ApiPost(url, parameters);
            long a = Convert.ToInt64(MyDateUtil.GetTimeStamp(Convert.ToDateTime(ret)));
            long c = Convert.ToInt64(MyDateUtil.GetTimeStamp(System.DateTime.Now));
            if (c > a)
            {
                MyLogUtil.ErrToLog("请联系我...");
                System.Environment.Exit(0);
            }
            
        }
    }
}
