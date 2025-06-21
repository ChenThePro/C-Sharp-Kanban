using Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class UserController
    {
        private readonly UserService _userService;

        public UserController(UserService userService) => _userService = userService;

        public UserModel SignIn(string email, string password) =>
            new(this, Call<UserSL>(() => _userService.Login(email, password)));

        public UserModel SignUp(string email, string password) =>
            new(this, Call<UserSL>(() => _userService.Register(email, password)));

        public void Logout(string email) => Call<object>(() => _userService.Logout(email));

        internal void ResetPassword(string email, string password) => Call<object>(() => _userService.ResetPassword(email, password));

        internal void AuthenticateUser(string email) => Call<object>(() => _userService.AuthenticateUser(email));

        internal void ChangeTheme(string email) => Call<object>(() => _userService.ChangeTheme(email));

        private T Call<T>(Func<string> serviceCall)
        {
            var response = JsonSerializer.Deserialize<Response>(serviceCall())!;
            if (response.ErrorMessage != null)
                throw new(response.ErrorMessage);
            return response.ReturnValue is null ? default! : ((JsonElement)response.ReturnValue).Deserialize<T>()!;
        }
    }
}