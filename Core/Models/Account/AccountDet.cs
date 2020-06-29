namespace CoreLib.Models.Account
{
    public class AccountDet
    {
        public string account_no { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public decimal current_balance { get; set; }
        public decimal previous_balance { get; set; }
        public string status { get; set; }
    }
}
