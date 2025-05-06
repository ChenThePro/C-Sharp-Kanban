using Backend.BuisnessLayer.BoardPackage;

namespace Backend.ServiceLayer
{
    public class BoardSL
    {
        public string Name { get; set; }
        public BoardSL(string name)
        {
            Name = name;
        }
    }
}
