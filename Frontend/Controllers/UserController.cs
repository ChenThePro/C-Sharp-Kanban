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

        private T Call<T>(Func<string> serviceCall)
        {
            var response = JsonSerializer.Deserialize<Response>(serviceCall())!;
            if (!string.IsNullOrEmpty(response.ErrorMessage))
                throw new(response.ErrorMessage);
            return response.ReturnValue is null ? default! : ((JsonElement)response.ReturnValue).Deserialize<T>()!;
        }
    }
}