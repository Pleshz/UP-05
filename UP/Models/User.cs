using System;

namespace UP.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }

        public User(string Login, string Email, string Password, DateTime CreatedAt, DateTime UpdateAt, string FullName = null, string Bio = null) {
            this.Login = Login;
            this.Email = Email;
            this.Password = Password;
            this.CreatedAt = CreatedAt;
            this.UpdateAt = UpdateAt;
            this.FullName = FullName;
            this.Bio = Bio;
        }

        public User(int Id, string Login, string Email, string Password, DateTime CreatedAt, DateTime UpdateAt, string FullName, string Bio)
        {
            this.Id = Id;
            this.Login = Login;
            this.Email = Email;
            this.Password = Password;
            this.CreatedAt = CreatedAt;
            this.UpdateAt = UpdateAt;
            this.FullName = FullName;
            this.Bio = Bio;
        }
    }
}
