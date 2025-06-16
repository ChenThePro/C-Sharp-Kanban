using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        public string Name { get; init; }
        public string Owner { get; set; }
        public ObservableCollection<string> Members { get; init; }
        public ObservableCollection<ColumnModel> Columns { get; init; }
        public bool IsExpanded { get; set; }

        public BoardModel(BackendController controller, BoardSL board) : base(controller)
        {
            Name = board.Name;
            Owner = board.Owner;
            Members = new(board.Members);
            Columns = new(board.Columns.Select(c => new ColumnModel(controller, c)));
            IsExpanded = false;
        }
    }
}