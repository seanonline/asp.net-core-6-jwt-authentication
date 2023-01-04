namespace asp.net_core_6_jwt_authentication.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            this.Token = String.Empty;
            this.responseMsg =
            new HttpResponseMessage()
            {
                StatusCode =
               System.Net.HttpStatusCode.Unauthorized
            };
        }

        public string Token { get; set; }
        public HttpResponseMessage responseMsg
        {
            get; set;
        }

    }

}
