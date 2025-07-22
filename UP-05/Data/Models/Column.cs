namespace UP_05.Data.Models
{
    public class Column
    {
        private int id { get; set; }
        private string name { get; set; }
        private Project projectId { get; set; }

        public Column(int id, string name, Project projectId)
        {
            this.id = id;
            this.name = name;
            this.projectId = projectId;
        }
    }
}
