using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer.BoardPackage;


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
            this.Name = name;
            this.Owmer = owner;
            this.Id = id;
            Columns = new List<ColumnDAL> { new(0,id), new(1, id), new(2, id) };
        }

        public void AddTask(string title, string description, DateTime due, string email, int id, int column)
        {
            TaskDAL task = new TaskDAL(title, due, description, id, Id);
            Columns[column].AddTask(task, email);
            return task;
        }
    }
}
