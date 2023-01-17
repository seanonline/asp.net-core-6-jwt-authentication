using asp.net_core_6_jwt_authentication.Models;

namespace asp.net_core_6_jwt_authentication.Service
{
    public interface IUserInterface
    {
        // record the user login+accesstoken+refresh token
        void RecordUserLogin(LoginResponse user);

        // get User detail
        LoginResponse? GetUser(string email);

        // Return a refresh token detail for a user
        public RefreshToken? GetRefreshTokenDetail(string email);

        // Remove refresh token from the database
        public void RemoveRefreshToken(string username, string refreshToken);
        
     }
}
