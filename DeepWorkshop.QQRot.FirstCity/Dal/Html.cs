using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AI.Bll;
namespace AI.Dal
{
    class Html
    {
        public string Type { get; set; }
        public string WU_file { get; set; }
        public string Webwx_data_ticket { get; set; }
        public string Pass_ticket { get; set; }
        public string Uploadmediarequest { get; set; }
        public string typ { get; set; }
        //string imglen;
        public Html(string type, string webwx_data_ticket, string uploadmediarequest, string pass_ticket)
        {
            Type = type;
            Webwx_data_ticket = webwx_data_ticket;
            Uploadmediarequest = uploadmediarequest;
            Pass_ticket = pass_ticket;
            if (Type == "GIF")
            {
                typ = "doc";
            }
            else
            {
                typ = "pic";
            }
        }
        public string getdate()
        {
            string dt = @"-----------------------------153073255032032
Content-Disposition: form-data; name=""id""

WU_FILE_0
-----------------------------153073255032032
Content-Disposition: form-data; name=""name""

temp." + Type + @"
-----------------------------153073255032032
Content-Disposition: form-data; name=""type""

image/" + Type + @"
-----------------------------153073255032032
Content-Disposition: form-data; name=""lastModifiedDate""

Thu Nov 12 2015 16:53:29 GMT+0800
-----------------------------153073255032032
Content-Disposition: form-data; name=""size""

" + function.middlestring(Uploadmediarequest, "DataLen\":", ",") + @"
-----------------------------153073255032032
Content-Disposition: form-data; name=""mediatype""

" + typ + @"
-----------------------------153073255032032
Content-Disposition: form-data; name=""uploadmediarequest""

" + Uploadmediarequest + @"
-----------------------------153073255032032
Content-Disposition: form-data; name=""webwx_data_ticket""

" + Webwx_data_ticket.Replace("webwx_data_ticket=", "").Replace(";", "") + @"
-----------------------------153073255032032
Content-Disposition: form-data; name=""pass_ticket""

" + Pass_ticket + @"
-----------------------------153073255032032
Content-Disposition: form-data; name=""filename""; filename=""temp." + Type + @"""
Content-Type: image/" + Type + @"

";
            return dt;
        }
        public static string ixx()
        {
            string xii = @"-----------------------------85081679315338
Content-Disposition: form-data; name=""id""

WU_FILE_0
-----------------------------85081679315338
Content-Disposition: form-data; name=""name""

aaaaa.gif
-----------------------------85081679315338
Content-Disposition: form-data; name=""type""

image/gif
-----------------------------85081679315338
Content-Disposition: form-data; name=""lastModifiedDate""

Mon Nov 09 2015 13:24:18 GMT+0800
-----------------------------85081679315338
Content-Disposition: form-data; name=""size""

690136
-----------------------------85081679315338
Content-Disposition: form-data; name=""mediatype""

doc
-----------------------------85081679315338
Content-Disposition: form-data; name=""uploadmediarequest""

{""BaseRequest"":{""Uin"":610558101,""Sid"":""nwE7YZyhcegaBmOY"",""Skey"":""@crypt_f9fe938f_4bae8f3d7917b31059e39614e890e9d5"",""DeviceID"":""e394931717233123""},""ClientMediaId"":1447131821779,""TotalLen"":690136,""StartPos"":0,""DataLen"":690136,""MediaType"":4}
-----------------------------85081679315338
Content-Disposition: form-data; name=""webwx_data_ticket""

AQYw09sJMzQ+Q84NiBNzKDa9
-----------------------------85081679315338
Content-Disposition: form-data; name=""pass_ticket""

6fwaBn4HibQYfa9SOX4xNsgWPQ39qazg4JP1hEhK5BSYrG9FlQ+TiFMBJ5/2uD+4
-----------------------------85081679315338
Content-Disposition: form-data; name=""filename""; filename=""aaaaa.gif""
Content-Type: image/gif

";
            return xii;
        }


    }

}
