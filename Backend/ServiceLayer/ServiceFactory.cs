using log4net;
using log4net.Config;
using System.Reflection;
using Backend.BuisnessLayer.UserPackage;
using Backend.BuisnessLayer.BoardPackage;


namespace Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private readonly BoardFacade _boardFacade;
        private readonly UserFacade _userFacade;
        private TaskService? _taskService;
        private BoardService? _boardService;
        private UserService? _userService;

        internal ServiceFactory()
        {
            _boardFacade = new BoardFacade();
            _userFacade = new UserFacade();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public TaskService GetTaskService()
        {
            return _taskService ??= new TaskService(_boardFacade);
        }

        public BoardService GetBoardService()
        {
            return _boardService ??= new BoardService(_boardFacade);
        }

        public UserService GetUserService()
        {
            return _userService ??= new UserService(_userFacade);
        }
    }
}