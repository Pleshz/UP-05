using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP.Models
{
    public class UserProjectRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string Role { get; set; }

        public UserProjectRole(int UserId, int ProjectId, string Role)
        {
            this.UserId = UserId;
            this.ProjectId = ProjectId;
            this.Role = Role;
        }

        public UserProjectRole(int Id, int UserId, int ProjectId, string Role)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.ProjectId = ProjectId;
            this.Role = Role;
        }
    }
}
