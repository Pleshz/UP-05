using System;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class ProjectAdd : Page
    {
        private readonly ProjectContext _project;
        public ProjectAdd()
        {
            InitializeComponent();
            _project = new ProjectContext
            {
                CreatorId = MainWindow.CurrentUser?.Id ?? 0,
                CreatedAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                IsPublic = false 
            };
            DataContext = _project;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.CurrentUser == null)
                {
                    MessageBox.Show("Ошибка: пользователь не авторизован. Пожалуйста, войдите в систему.");
                    MainWindow.init.OpenPages(MainWindow.pages.login);
                    return;
                }

                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Ошибка: название проекта обязательно для заполнения.");
                    NameTextBox.Focus();
                    return;
                }

                _project.Name = NameTextBox.Text.Trim();
                _project.Description = DescriptionTextBox.Text?.Trim();
                _project.IsPublic = IsPublicCheckBox.IsChecked ?? false;
                _project.CreatorId = MainWindow.CurrentUser.Id;
                _project.CreatedAt = DateTime.Now;
                _project.UpdateAt = DateTime.Now;

                _project.Save(false);

                MessageBox.Show("Проект успешно создан!");
                MainWindow.init.OpenPages(MainWindow.pages.projectList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании проекта: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPages(MainWindow.pages.projectList);
        }
    }
}
