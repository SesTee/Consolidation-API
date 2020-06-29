using CommonLib;
using Newtonsoft.Json;
using System;

namespace LoggerClassLib.Middleware
{
    public class BadRequestException : Exception
    {
        public BadRequestException(GeneralResponse gResp)
           : base(JsonConvert.SerializeObject(gResp))
        {
        }

    }
}
