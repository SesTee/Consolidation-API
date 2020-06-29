using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerClassLib
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public Dictionary<string, List<string>> UserClaims { get; set; }
    }
}
