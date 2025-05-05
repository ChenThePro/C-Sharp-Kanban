using Backend.BuisnessLayer.BoardPackage;

namespace Backend.ServiceLayer
{
    public class BoardSL
    {
        public string Name { get; set; }
        public List<TaskSL> BackLog { get; set; }
        public List<TaskSL> InProgress { get; set; }
        public List<TaskSL> Done { get; set; }
        internal BoardSL(BoardBL board)
        {
            Name = board.name;
            BackLog = new List<TaskSL>();
            InProgress = new List<TaskSL>();
            Done = new List<TaskSL>();
        }
    }
}
