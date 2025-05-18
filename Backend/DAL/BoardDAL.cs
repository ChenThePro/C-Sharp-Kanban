using System;
using System.Collections.Generic;


namespace IntroSE.Kanban.Backend.DAL
{
    internal class BoardDAL
    {
        public string Name;
        public string Owmer;
        public int Id;
        public List<ColumnDAL> Columns;

        public BoardDAL(string name, string owner, int id)
        {
            Name = name;
            Owmer = owner;
            Id = id;
            Columns = new List<ColumnDAL> { new(0, id), new(1, id), new(2, id) };
        }

        public void AddTask(string title, string description, DateTime due, string email, int id, int column)
        {
            TaskDAL task = new TaskDAL(title, due, description, DateTime.Today, id, Id);
            Columns[column].AddTask(task, email);
        }
    }
}
