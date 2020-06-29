namespace CoreLib.Models.Account.Request
{
    public class AddAccount:BaseRequest
    {
        public string account_no { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public string status { get; set; }
    }
}