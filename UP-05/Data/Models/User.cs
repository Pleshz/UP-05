namespace UP_05.Data.Models
{
    public class User
    {
        private int id { get; set; }
        private string login { get; set; }
        private string email { get; set; }
        private string password { get; set; }
        private string fullName { get; set; }
        private string bio { get; set; }

        public User(User user = null) 
        {
            if (user != null) 
            {
                this.id = user.id;
                this.login = user.login;
                this.email = user.email;
                this.password = user.password;
                this.fullName = user.fullName;
                this.bio = user.bio;
            }
        }
        public User() { }
    }
}
