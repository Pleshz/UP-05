namespace UP.Models
{
    public class UserProjectRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string Role { get; set; }

    }
}
