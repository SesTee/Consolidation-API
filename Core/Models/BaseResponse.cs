using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Models
{
    public abstract class BaseResponse
    {
        public string RequestID { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}
