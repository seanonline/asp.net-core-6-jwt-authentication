namespace asp.net_core_6_jwt_authentication.Models
{
    public class LoginRequest
    {
        public LoginRequest()
        {
            this.UserName = String.Empty;
            this.Password = String.Empty;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }

}
