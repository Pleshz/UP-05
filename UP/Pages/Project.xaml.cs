using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;
using UP.Models;

namespace UP.Pages
{
    public partial class Project : Page
    {
        private ProjectContext currentProject;

        public Project()
        {
            InitializeComponent();
            LoadProject();
            AddTaskButton.IsEnabled = HasPermission(ProjectRole.Creator, ProjectRole.Member);
            AddColumnButton.IsEnabled = HasPermission(ProjectRole.Creator, ProjectRole.Member);
        }

        private void LoadProject()
        {
            currentProject = MainWindow.CurrentProject;

            if (currentProject == null)
            {
                MessageBox.Show("Проект не выбран.");
                MainWindow.init.OpenPages(MainWindow.pages.projectList);
                return;
            }

            LoadProjectTasks();
        }

        public void LoadProjectTasks()
        {
            try
            {
                ColumnsItemsControl.ItemsSource = null;
                var columnContext = new ColumnContext();
                var taskContext = new TaskContext();

                var allColumns = columnContext.AllColumns()
                                              .Where(c => c.ProjectId == currentProject.Id)
                                              .ToList();

                var allTasks = taskContext.AllTasks()
                                          .Where(t => t.ProjectId == currentProject.Id)
                                          .ToList();

                foreach (var col in allColumns)
                {
                    col.Tasks = new ObservableCollection<TaskContext>(
                        allTasks.Where(t => t.ColumnId == col.Id)
                    );
                }

                
                ColumnsItemsControl.ItemsSource = allColumns;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке задач: {ex.Message}");
            }
        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentTask = null; 
            MainWindow.init.OpenPages(MainWindow.pages.taskManage);
        }

        private void AddColumn(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentColumn = null; 
            MainWindow.init.OpenPages(MainWindow.pages.columnManage);
        }

        private void BackToProfile(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPages(MainWindow.pages.profile);
        }

        private void TasksPanel_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TaskContext)))
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.Move;

            e.Handled = true;
        }

        private void TasksPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TaskContext)))
            {
                var task = e.Data.GetData(typeof(TaskContext)) as TaskContext;
                if (task == null) return;

                var targetColumn = (sender as FrameworkElement)?.DataContext as ColumnContext;
                if (targetColumn == null) return;

                if (task.ColumnId == targetColumn.Id) return; 

                task.ColumnId = targetColumn.Id;
                task.UpdatedAt = DateTime.Now;
                task.Save(true);

                LoadProjectTasks();
            }
        }

        private bool HasPermission(params ProjectRole[] allowedRoles)
        {
            var roles = new UserProjectRoleContext().AllUserProjectRoles();
            var userRole = roles.FirstOrDefault(r => r.UserId == MainWindow.CurrentUser.Id && r.ProjectId == currentProject.Id);

            if (userRole == null) return false;
            return allowedRoles.Contains(userRole.RoleEnum);
        }

        private void OpenMembers(object sender, RoutedEventArgs e)
        {
            if (HasPermission(ProjectRole.Creator))
                MainWindow.init.OpenPages(MainWindow.pages.projectMembers);
            else
                MessageBox.Show("Только создатель проекта может управлять участниками!");
        }
    }
}
