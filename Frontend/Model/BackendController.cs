using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace Frontend.Model
{
    public class BackendController
    {
        private readonly ServiceFactory _serviceFactory;

        public BackendController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public BackendController()
        {
            _serviceFactory = new();
            _serviceFactory.GetBoardService().LoadData();
        }

        public UserModel SignIn(string email, string password)
        {
            string json = _serviceFactory.GetUserService().Login(email, password);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return new UserModel(this, email, password);
        }

        public UserModel SignUp(string email, string password)
        {
            string json = _serviceFactory.GetUserService().Register(email, password);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return new UserModel(this, email, password);
        }

        public bool Logout(string email)
        {
            string json = _serviceFactory.GetUserService().Logout(email);
            Response response = JsonSerializer.Deserialize<Response>(json)!;
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);
            return true;
        }
    }
}
