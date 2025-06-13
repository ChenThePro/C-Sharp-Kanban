using IntroSE.Kanban.Backend.ServiceLayer;

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
        public string SignIn(string email, string password) =>
            _serviceFactory.GetUserService().Login(email, password);
        public string SignUp(string email, string password) =>
            _serviceFactory.GetUserService().Register(email, password);
        public string Logout(string email) =>
            _serviceFactory.GetUserService().Logout(email);
    }
}
