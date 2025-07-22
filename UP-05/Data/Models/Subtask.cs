namespace UP_05.Data.Models
{
    public class Subtask
    {
        private int id { get; set; }
        private Task taskId { get; set; }
        private string name { get; set; }
        private string description { get; set; }
        private DateTime dueDate { get; set; }
        private Column columnId { get; set; }
        private TaskAssign taskAssignId { get; set; }

        public Subtask(int id, Task taskId, string name, string description, DateTime dueDate, Column columnId, TaskAssign taskAssignId) 
        {
            this.id = id;
            this.taskId = taskId;
            this.name = name;
            this.description = description;
            this.dueDate = dueDate;
            this.columnId = columnId;
            this.taskAssignId = taskAssignId;
        }
    }
}
