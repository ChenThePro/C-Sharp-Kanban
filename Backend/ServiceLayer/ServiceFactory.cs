using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BuisnessLayer;

namespace Backend.ServiceLayer
{
    public class ServiceFactory
    {
        public UserService Us { get; private set; }
        public BoardService Bs { get; private set; }
        public TaskService Ts { get; private set; }

        /// <summary>
        /// Initializes all services and their dependencies.
        /// </summary>
        /// <exception cref="InvalidOperationException">If service dependencies cannot be initialized.</exception>
        public ServiceFactory()
        {
            try
            {
                Us = new UserService(new UserFacade());
                Bs = new BoardService(new BoardFacade());
                Ts = new TaskService(new BoardFacade());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize services.", ex);
            }
        }
    }
}
