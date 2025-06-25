using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Controllers
{
    public sealed class ControllerFactory
    {
        private static readonly Lazy<ControllerFactory> _instance = new(() => new());

        public static ControllerFactory Instance => _instance.Value;

        public UserController UserController { get; }
        public BoardController BoardController { get; }

        private ControllerFactory()
        {
            ServiceFactory factory = new();
            factory.GetBoardService().LoadData();
            UserController = new(factory.GetUserService());
            BoardController = new(factory.GetBoardService());
        }
    }
}