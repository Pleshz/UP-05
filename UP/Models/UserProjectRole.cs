using System;

namespace UP.Models
{
    public class UserProjectRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string Role { get; set; }

        public ProjectRole RoleEnum
        {
            get => Enum.TryParse(Role, out ProjectRole parsed) ? parsed : ProjectRole.Viewer;
            set => Role = value.ToString();
        }

    }
    public enum ProjectRole
    {
        Creator,
        Member,
        Viewer
    }
}
