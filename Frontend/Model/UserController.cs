using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace Frontend.Model
{
    public class UserController
    {
        private readonly ServiceFactory _serviceFactory;

        public UserController()
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

        public void Logout(string email)
        {
            ExecuteServiceCall<UserSL>(() => _serviceFactory.GetUserService().Logout(email));
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

        internal ServiceFactory GetFactory()
        {
            return _serviceFactory;
        }

    }
}