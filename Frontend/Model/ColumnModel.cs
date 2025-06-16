using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class ColumnModel : NotifiableModelObject<BoardController>
    {
        private bool _isExpanded;

        public string Name { get; init; }
        public int Limit { get; set; }

        public ObservableCollection<TaskModel> Tasks { get; init; }
        
        public bool IsExpanded  { get => _isExpanded; set { _isExpanded = value; RaisePropertyChanged(); } }


        public ColumnModel(BoardController controller, ColumnSL column) : base(controller)
        {
            Name = column.Name;
            Limit = column.Limit;
            Tasks = new(column.Tasks.Select(t => new TaskModel(controller, t)));
            IsExpanded = false;
        }
    }
}