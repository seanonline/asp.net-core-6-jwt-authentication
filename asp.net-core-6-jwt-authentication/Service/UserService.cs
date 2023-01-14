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
            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public LoginResponse? GetUser(string email)
        {
            return this.context.Users.SingleOrDefault(x => x.UserId == email);
        }


    }
}
