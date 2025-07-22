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

        public User(int id, string login, string email, string password, string fullName, string bio) 
        {
            this.id = id;
            this.login = login;
            this.email = email;
            this.password = password;
            this.fullName = fullName;
            this.bio = bio;
        }
    }
}
