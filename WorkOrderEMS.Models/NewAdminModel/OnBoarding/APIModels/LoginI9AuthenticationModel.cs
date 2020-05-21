using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class LoginI9AuthenticationModel
    {
        public class UserInfo
        {
            public string logon_id { get; set; }
            public string user_access_method_code { get; set; }
            public string user_type_code { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string phone_number { get; set; }
            public object phone_number_ext { get; set; }
            public string email_address { get; set; }
            public List<object> roles { get; set; }
            public object last_login_date { get; set; }
            public int agency_employer_id { get; set; }
            public int account_id { get; set; }
            public string account_name { get; set; }
            public string account_status { get; set; }
            public string ica_version_nbr { get; set; }
        }

        public class RootObjectLoginI9
        {
            public string access_token { get; set; }
            public int token_expiration { get; set; }
            public UserInfo user_info { get; set; }
        }


        public class RootObjectRefreshToken
        {
            public string access_token { get; set; }
            public UserInfo user_info { get; set; }
        }
    }
}
