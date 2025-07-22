namespace UP_05.Data.Models
{
    public class Project
    {
        private int id { get; set; }
        private string name { get; set; }
        private string description { get; set; }
        private bool isPublic { get; set; }
        public Project(int id, string name, string description, bool isPublic) 
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.isPublic = isPublic;
        }
    }
}
