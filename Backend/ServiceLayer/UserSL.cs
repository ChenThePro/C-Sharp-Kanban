using Backend.BuisnessLayer.UserPackage;

namespace Backend.ServiceLayer
{
    public class UserSL
    {
        private readonly string _password;
        public string Email { get; set; }

         internal UserSL(UserBL user)
         {
            _password = user.password;
            Email = user.email;
         }
    }
}
