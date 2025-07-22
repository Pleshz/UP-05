namespace UP_05.Data.Models
{
    public class TaskAssign
    {
        private int id { get; set; }
        private Task taskId { get; set; }
        private User userId { get; set; }

        public TaskAssign(int id, Task taskId, User userId) 
        {
            this.id = id;
            this.taskId = taskId;
            this.userId = userId;
        }
    }
}
