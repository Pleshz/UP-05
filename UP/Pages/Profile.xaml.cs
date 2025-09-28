using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class Profile : Page
    {
        private UserContext currentUser;
        public Profile()
        {
            InitializeComponent();
            LoadUserProfile();
        }

        private void LoadUserProfile() 
        {
            currentUser = MainWindow.CurrentUser;

            if (currentUser == null) 
            {
                MessageBox.Show("Ошибка: как вы зашли??");
                return;
            }

            LoginTextBox.Text = currentUser.Login;
            EmailTextBox.Text = currentUser.Email;
            FullNameTextBox.Text = currentUser.FullName;
            BioTextBox.Text = currentUser.Bio;
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            if (currentUser == null) return;

            currentUser.Login = LoginTextBox.Text.Trim();
            currentUser.Email = EmailTextBox.Text.Trim();
            currentUser.FullName = FullNameTextBox.Text.Trim();
            currentUser.Bio = BioTextBox.Text.Trim();
            currentUser.UpdateAt = DateTime.Now;

            string newPassword = PasswordBox.Password.Trim();
            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword.Length < 6 ||
                    !Regex.IsMatch(newPassword, @"[a-zA-Z]") ||
                    !Regex.IsMatch(newPassword, @"\d") ||
                    !Regex.IsMatch(newPassword, @"[!@#$%^&*()_+\-=\[\]{};':\""\\|,.<>\/?]+") ||
                    Regex.IsMatch(newPassword, @"[А-Яа-яЁё]"))
                {
                    MessageBox.Show("Пароль должен содержать минимум 6 символов, латинские буквы, как минимум 1 цифру и 1 спецсимвол, без кириллицы.");
                    return;
                }

                currentUser.Password = newPassword;
            }

            try
            {
                currentUser.Save(true);
                MessageBox.Show("Профиль успешно обновлён!");

                PasswordBox.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        public void GoToProjects(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPages(MainWindow.pages.projectList);
        }
    }
}
