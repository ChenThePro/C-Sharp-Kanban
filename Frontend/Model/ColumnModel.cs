using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public string Name { get; init; }
        public int Limit { get; set; }
        public ObservableCollection<TaskModel> Tasks { get; init; }

        public ColumnModel(BackendController controller, ColumnSL column)
            : base(controller)
        {
            Name = column.Name;
            Limit = column.Limit;
            Tasks = new ObservableCollection<TaskModel>(
                column.Tasks.Select(t => new TaskModel(controller, t)));
        }
    }
}