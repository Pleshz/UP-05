using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        public void Register(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) 
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }

            if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры.");
                return;
            }

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.");
                return;
            }

            if (password.Length < 6 ||
                !Regex.IsMatch(password, @"[a-zA-Z]") ||
                !Regex.IsMatch(password, @"\d") ||
                !Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':\""\\|,.<>\/?]+") ||
                Regex.IsMatch(password, @"[А-Яа-яЁё]"))
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов, латинские буквы, как минимум 1 цифру и 1 спецсимвол, без кириллицы.");
                return;
            }

            try
            {
                UserContext userContext = new UserContext();

                var allUsers = userContext.AllUsers();
                bool loginExists = allUsers.Exists(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
                if (loginExists)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }

                bool emailExists = allUsers.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (emailExists)
                {
                    MessageBox.Show("Пользователь с такой электронной почтой уже существует.");
                    return;
                }

                UserContext newUser = new UserContext
                {
                    Login = login,
                    Email = email,
                    Password = password,
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    FullName = "",
                    Bio = ""
                };
                newUser.Save(false);

                MessageBox.Show("Регистрация успешна!");
                MainWindow.init.OpenPages(MainWindow.pages.login);
            }

            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка при регистрации: {ex}");
            }
        }
    }
}
