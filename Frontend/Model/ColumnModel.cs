using Frontend.Utils;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class ColumnModel : NotifiableObject
    {
        private bool _isExpanded;

        public string Name { get; init; }
        public int Limit { get; set; }
        public ObservableCollection<TaskModel> Tasks { get; init; }
        public bool IsExpanded  { get => _isExpanded; set { _isExpanded = value; RaisePropertyChanged(nameof(IsExpanded)); } }

        public ColumnModel(ColumnSL column)
        {
            Name = column.Name;
            Limit = column.Limit;
            Tasks = new(column.Tasks.Select(t => new TaskModel(t)));
            IsExpanded = false;
        }
    }
}