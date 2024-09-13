namespace TestWeb.Models
{
    public class Account
    {
        public int Key_no { get; set; }
        public string? User_name { get; set; }
        public string? User_password { get; set; }
        public string? User_email { get; set; }
        public string? User_group { get; set; }
        public string? User_office { get; set; }
        public string? Privilege { get; set; }
        public string? Is_active { get; set; }
        public DateTime Created_date { get; set; }
        public DateTime Updated_date { get; set; }
        public string? Creator { get; set; }
        public string? Remarks { get; set; }
    }
}
