namespace UP_05.Data.Models
{
    public class Task
    {
        private int id { get; set; }
        private string name { get; set; }
        private string description { get; set; }
        private DateTime dueDate { get; set; }
        private Column columnId { get; set; }

        public Task(int id, string name, string description, DateTime dueDate, Column columnId) 
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.dueDate = dueDate;
            this.columnId = columnId;
        }
    }
}
