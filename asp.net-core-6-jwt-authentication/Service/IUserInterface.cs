using asp.net_core_6_jwt_authentication.Models;

namespace asp.net_core_6_jwt_authentication.Service
{
    public interface IUserInterface
    {
        void RecordUserLogin(LoginResponse user);

        LoginResponse? GetUser(string email);

        public RefreshToken? GetRefreshTokenDetail(string email);

        public void RemoveRefreshToken(string username, string refreshToken);
        
     }
}
