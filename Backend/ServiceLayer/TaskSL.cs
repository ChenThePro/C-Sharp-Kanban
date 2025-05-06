namespace Backend.ServiceLayer
{
    public class TaskSL
    {
        public string Title { get; set; }
        public DateTime Due { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; init; }
        public int Id { get; set; }
        public TaskSL(string title, DateTime due, string description, DateTime creationTime, int id)
        {
            Title = title;
            Due = due;
            Description = description;
            CreationTime = creationTime;
            Id = id;
        }
    }
}
