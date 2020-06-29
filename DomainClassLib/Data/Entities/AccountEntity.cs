namespace DomainClassLib.Data.Entities
{
    public class AccountEntity
    {
        public string account_no { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public decimal current_balance { get; set; }
        public decimal previous_balance { get; set; }
        public string status { get; set; }
}
}