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

        public BoardModel(BackendController controller, BoardSL board)
            : base(controller)
        {
            Name = board.Name;
            Owner = board.Owner;
            Members = new ObservableCollection<string>(board.Members);
            Columns = new ObservableCollection<ColumnModel>(
                board.Columns.Select(c => new ColumnModel(controller, c)));
        }
    }
}