using Backend.BuisnessLayer.BoardPackage;

namespace Backend.ServiceLayer
{
    public class BoardSL
    {
        public string Name { get; set; }
        internal BoardSL(BoardBL board)
        {
            Name = board.name;
        }
    }
}
