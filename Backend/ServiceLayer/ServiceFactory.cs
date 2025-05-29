using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;
using IntroSE.Kanban.Backend.BuisnessLayer.UserPackage;
using System.Text.Json;
using System;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private TaskService _taskService;
        private BoardService _boardService;
        private UserService _userService;
        private readonly BoardFacade _boardFacade;
        private readonly UserFacade _userFacade;

        public ServiceFactory()
        {
            _taskService = null;
            _boardService = null;
            _userService = null;
            _userFacade = new UserFacade();
            _boardFacade = new BoardFacade(_userFacade);
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