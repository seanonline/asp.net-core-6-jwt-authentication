namespace asp.net_core_6_jwt_authentication.Models
{
    public class LoginResponse:BaseModel
    { 
        public LoginResponse(string UserId,string Token,String RefreshToken)
        {
            this.Token = Token;
            this.RefreshToken = RefreshToken;
            this.UserId= UserId;
        }

        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        //HttpResponseMessage can not be initialzed by entity framework automatically.

    }

}
