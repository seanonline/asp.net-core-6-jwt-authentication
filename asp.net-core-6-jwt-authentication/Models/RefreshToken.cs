namespace asp.net_core_6_jwt_authentication.Models
{
    public class RefreshToken:BaseModel
    {
        public RefreshToken(string token, string tokenType, DateTime expiration,string username)
        {
            Token = token;
            TokenType = tokenType;
            Expiration = expiration;
            Username = username;
        }

        public string Token { get; set; }
        public string TokenType { get; set; }
        public DateTime  Expiration { get; set; }
        public string Username { get; set; }
    }
}
