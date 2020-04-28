using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using static WorkOrderEMS.Models.LoginI9AuthenticationModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class CommonAPIJsonConvertToClass<T>
    {
        public T ConvertJsonStringToClass(string getOutputData, string APINameval)
        {
            object value = new object();
            if (getOutputData != null && APINameval != null)
            {
                switch (APINameval)
                {
                    case APIName.I9AuthenticationAPI:
                        value = JsonConvert.DeserializeObject<RootObjectLoginI9>(getOutputData);
                        break;
                }
            }
            return  (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
