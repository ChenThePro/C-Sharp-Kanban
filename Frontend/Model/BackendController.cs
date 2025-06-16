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

        public void Logout(string email)
        {
            ExecuteServiceCall<UserSL>(() => _serviceFactory.GetUserService().Logout(email));
        }

        public BoardModel CreateBoard(string email, string newBoardName)
        {
            BoardSL boardSl = ExecuteServiceCall<BoardSL>(() => _serviceFactory.GetBoardService().CreateBoard(email, newBoardName));
            return new(this, boardSl);
        }

        public void DeleteBoard(string email, string name)
        {
            ExecuteServiceCall<BoardSL>(() => _serviceFactory.GetBoardService().DeleteBoard(email, name));
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