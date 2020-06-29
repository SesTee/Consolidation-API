using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Models
{
    public abstract class BaseRequest
    {
        public string RequestID { get;} = Guid.NewGuid().ToString();


    }
}
