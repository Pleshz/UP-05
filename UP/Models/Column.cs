using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }

        public Column(string Name, int ProjectId)
        {
            this.Name = Name;
            this.ProjectId = ProjectId;
        }

        public Column(int Id, string Name, int ProjectId)
        {
            this.Id = Id;
            this.Name = Name;
            this.ProjectId = ProjectId;
        }
    }
}
