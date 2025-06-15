using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace Frontend.Model
{
    public class BackendController
    {
        private readonly ServiceFactory _serviceFactory;

        public BackendController()
        {
            _serviceFactory = new();
            _serviceFactory.GetBoardService().LoadData();
        }

        public UserModel SignIn(string email, string password)
        {
            UserSL userSl = ExecuteServiceCall<UserSL>(() => _serviceFactory.GetUserService().Login(email, password));
            return new(this, userSl);
        }

        public UserModel SignUp(string email, string password)
        {
            UserSL userSl = ExecuteServiceCall<UserSL>(() => _serviceFactory.GetUserService().Register(email, password));
            return new(this, userSl);
        }

        public bool Logout(string email)
        {
            ExecuteServiceCall<object>(() => _serviceFactory.GetUserService().Logout(email));
            return true;
        }

        private T ExecuteServiceCall<T>(Func<string> serviceCall)
        {
            string json = serviceCall();
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            if (response.ReturnValue == null)
                return default!;
            JsonElement jsonElement = (JsonElement)response.ReturnValue;
            return jsonElement.Deserialize<T>()!;
        }
    }
}