using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class TaskManage : Page
    {
        private TaskContext currentTask;
        private bool isNewTask = false;

        public TaskManage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var project = MainWindow.CurrentProject;
            if (project == null)
            {
                MessageBox.Show("Проект не выбран.");
                return;
            }

            var columnContext = new ColumnContext();
            var columns = columnContext.AllColumns()
                                       .Where(c => c.ProjectId == project.Id)
                                       .ToList();
            ColumnComboBox.ItemsSource = columns;
            ColumnComboBox.DisplayMemberPath = "Name";
            ColumnComboBox.SelectedValuePath = "Id";

            var userContext = new UserContext();
            var users = userContext.AllUsers();
            UserComboBox.ItemsSource = users;
            UserComboBox.DisplayMemberPath = "Login";
            UserComboBox.SelectedValuePath = "Id";

            currentTask = MainWindow.CurrentTask;
            if (currentTask != null)
            {
                TitleTextBox.Text = currentTask.Title;
                DescriptionTextBox.Text = currentTask.Description;
                ColumnComboBox.SelectedValue = currentTask.ColumnId;
                DueDatePicker.SelectedDate = currentTask.DueDate;
                UserComboBox.SelectedValue = currentTask.CreatorId;
            }
            else
            {
                isNewTask = true;
                currentTask = new TaskContext
                {
                    ProjectId = project.Id,
                    CreatorId = MainWindow.CurrentUser.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }
            LoadSubtasks();
        }

        private void LoadSubtasks()
        {
            var subtaskContext = new SubtaskContext();
            var subtasks = subtaskContext.AllSubtasks()
                                         .Where(s => s.TaskId == currentTask.Id)
                                         .ToList();

            SubtasksItemsControl.ItemsSource = subtasks;
        }

        private void Subtask_Click(object sender, MouseButtonEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is SubtaskContext subtask)
            {
                MainWindow.CurrentSubtask = subtask;
                MainWindow.init.OpenPages(MainWindow.pages.subtaskManage);
            }
        }

        private void AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentSubtask = null;
            MainWindow.init.OpenPages(MainWindow.pages.subtaskManage);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            currentTask.Title = TitleTextBox.Text.Trim();
            currentTask.Description = DescriptionTextBox.Text.Trim();
            currentTask.ColumnId = (int)ColumnComboBox.SelectedValue;
            currentTask.UpdatedAt = DateTime.Now;
            currentTask.DueDate = DueDatePicker.SelectedDate;

            try
            {
                if (isNewTask)
                {
                    currentTask.Save(false);
                    MessageBox.Show("Задача создана!");
                }
                else
                {
                    currentTask.Save(true);
                    MessageBox.Show("Задача обновлена!");
                }

                MainWindow.CurrentTask = null;
                MainWindow.init.OpenPages(MainWindow.pages.project);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentTask = null;
            MainWindow.init.OpenPages(MainWindow.pages.project);
        }
    }
}
