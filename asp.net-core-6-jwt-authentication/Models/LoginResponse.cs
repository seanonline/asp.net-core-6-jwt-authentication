namespace asp.net_core_6_jwt_authentication.Models
{
    public class LoginResponse : BaseModel
    { 
        public LoginResponse(string UserId, string Token, DateTime accessTokenExpiry)
        {
            this.Token = Token;
            this.UserId = UserId;
            AccessTokenExpiry = accessTokenExpiry;  
        }

        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
        public RefreshToken RefreshToken { get; set; }     

    }

}
