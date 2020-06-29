using DomainClassLib.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Models.Bank
{
    public class BankDet
    {
        public string logo { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string bank_address { get; set; }
        public string bank_contact_name { get; set; }
        public string bank_contact_phone { get; set; }
        public string bank_contact_email { get; set; }
           public string status { get; set; }
 }
}
