using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWorkshop.QQRot.FirstCity.MyTool
{
    public class MyDictionaryUtil<T,U>
    {
        /// <summary>
        /// 使用的时候不要和默认值重复
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public U GetValue(Dictionary<T,U> dictionary, T key)
        {
            try
            {
                U u = dictionary[key];
                return u;
            }catch (KeyNotFoundException ke)
            {
                return default(U);
            }

        }
    }
}
