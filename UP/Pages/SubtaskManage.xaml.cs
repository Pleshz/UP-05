using System;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class SubtaskManage : Page
    {
        private SubtaskContext currentSubtask;
        private bool isNewSubtask = false;

        public SubtaskManage()
        {
            InitializeComponent();
            LoadSubtask();
        }

        private void LoadSubtask()
        {
            var task = MainWindow.CurrentTask;
            if (task == null)
            {
                MessageBox.Show("Задача не выбрана.");
                MainWindow.init.OpenPages(MainWindow.pages.project);
                return;
            }

            currentSubtask = MainWindow.CurrentSubtask;

            var userContext = new UserContext();
            var users = userContext.AllUsers();
            UserComboBox.ItemsSource = users;
            UserComboBox.DisplayMemberPath = "Login";
            UserComboBox.SelectedValuePath = "Id";

            if (currentSubtask != null)
            {
                HeaderText.Text = "Редактирование подзадачи";
                NameTextBox.Text = currentSubtask.Name;
                DescriptionTextBox.Text = currentSubtask.Description;
                DueDatePicker.SelectedDate = currentSubtask.DueDate;
                UserComboBox.SelectedValue = currentSubtask.AssignedUserId;

                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                HeaderText.Text = "Создание подзадачи";
                isNewSubtask = true;
                currentSubtask = new SubtaskContext
                {
                    TaskId = task.Id,
                    ColumnId = task.ColumnId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            currentSubtask.Name = NameTextBox.Text.Trim();
            currentSubtask.Description = DescriptionTextBox.Text.Trim();
            currentSubtask.AssignedUserId = (int)UserComboBox.SelectedValue;
            currentSubtask.DueDate = DueDatePicker.SelectedDate ?? DateTime.Now;
            currentSubtask.UpdatedAt = DateTime.Now;

            try
            {
                if (isNewSubtask)
                {
                    currentSubtask.Save(false);
                    MessageBox.Show("Подзадача создана!");
                }
                else
                {
                    currentSubtask.Save(true);
                    MessageBox.Show("Подзадача обновлена!");
                }

                MainWindow.CurrentSubtask = null;
                MainWindow.init.OpenPages(MainWindow.pages.taskManage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (currentSubtask == null) return;

            if (MessageBox.Show("Удалить подзадачу?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    currentSubtask.Delete();
                    MessageBox.Show("Подзадача удалена.");
                    MainWindow.CurrentSubtask = null;
                    MainWindow.init.OpenPages(MainWindow.pages.taskManage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentSubtask = null;
            MainWindow.init.OpenPages(MainWindow.pages.taskManage);
        }
    }
}
