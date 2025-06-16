using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardController
    {
        private readonly ServiceFactory _serviceFactory;

        public BoardController (ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
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
