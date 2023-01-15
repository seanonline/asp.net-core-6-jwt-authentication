using asp.net_core_6_jwt_authentication.Models;

namespace asp.net_core_6_jwt_authentication.Service
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext context;


        public UserService(AppDbContext context) 
        { 
            this.context= context;        
        }

        public void RecordUserLogin(LoginResponse user)
        {
            var existingUser = GetUser(user.UserId);
            var refreshtoken = GetRefreshTokenDetail(user.UserId);

             if (existingUser == null) 
            { 
                this.context.Users.Add(user);               
            }
            else
            { 
                this.context.Users.Remove(existingUser);
                this.context.Users.Add(user);                
            }

            if (refreshtoken == null)
            {
                this.context.RefreshToken.Add(user.RefreshToken);
               
            }

            this.context.SaveChanges();
        }

        public LoginResponse? GetUser(string email)
        {
            return this.context.Users.SingleOrDefault(x => x.UserId == email);
        }

        public RefreshToken? GetRefreshTokenDetail(string email)
        {
            return this.context.RefreshToken.SingleOrDefault(x => x.Username == email);
        }

        public void RemoveRefreshToken(string username,string refreshToken) {

            var entity = GetRefreshTokenDetail(username);

            if (entity != null)
            {
                this.context.RefreshToken.Remove(entity);
                this.context.SaveChanges();
            }
        }
    }
}
