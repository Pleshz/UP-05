using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class ProjectsList : Page
    {
        private ProjectContext projectContext = new ProjectContext();
        private UserProjectRoleContext roleContext = new UserProjectRoleContext();
        private List<ProjectContext> allUserProjects;

        public ProjectsList()
        {
            InitializeComponent();
            if (MainWindow.CurrentUser == null)
            {
                MessageBox.Show("Требуется авторизация.");
                MainWindow.init.OpenPages(MainWindow.pages.login);
                return;
            }
            // Не вызываем LoadProjects здесь, а переносим в Loaded
            this.Loaded += ProjectsList_Loaded;
        }

        private void ProjectsList_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProjects();
        }

        private void LoadProjects()
        {
            try
            {
                if (MainWindow.CurrentUser == null)
                {
                    MessageBox.Show("Ошибка: пользователь не авторизован. Пожалуйста, войдите в систему.");
                    ProjectsItemsControl.ItemsSource = new List<ProjectContext>();
                    return;
                }

                allUserProjects?.Clear();
                var allProjects = projectContext.AllProjects();
                var allRoles = roleContext.AllUserProjectRoles();
                int currentUserId = MainWindow.CurrentUser.Id;

                allUserProjects = allProjects
                    .Where(p => p.CreatorId == currentUserId || 
                                allRoles.Any(r => r.ProjectId == p.Id && r.UserId == currentUserId) || 
                                (p.IsPublic && !allRoles.Any(r => r.ProjectId == p.Id && r.UserId == currentUserId))) 
                    .ToList();

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки проектов: {ex.Message}\n{ex.StackTrace}");
                allUserProjects = new List<ProjectContext>();
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            var filtered = allUserProjects?.AsEnumerable() ?? new List<ProjectContext>().AsEnumerable();

            string search = SearchBox?.Text?.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    (p.Name?.ToLower().Contains(search) ?? false) ||
                    (p.Description?.ToLower().Contains(search) ?? false));
            }

            if (FilterBox?.SelectedItem is ComboBoxItem selectedFilter)
            {
                switch (selectedFilter.Content?.ToString())
                {
                    case "Мои проекты":
                        filtered = filtered.Where(p => p.CreatorId == MainWindow.CurrentUser?.Id);
                        break;
                    case "Публичные проекты":
                        filtered = filtered.Where(p => p.IsPublic);
                        break;
                }
            }

            if (SortBox?.SelectedItem is ComboBoxItem selectedSort)
            {
                switch (selectedSort.Content?.ToString())
                {
                    case "По дате создания (новые)":
                        filtered = filtered.OrderByDescending(p => p.CreatedAt);
                        break;
                    case "По дате обновления":
                        filtered = filtered.OrderByDescending(p => p.UpdateAt);
                        break;
                    case "По имени (А-Я)":
                        filtered = filtered.OrderBy(p => p.Name);
                        break;
                    case "По имени (Я-А)":
                        filtered = filtered.OrderByDescending(p => p.Name);
                        break;
                }
            }

            ProjectsItemsControl.ItemsSource = filtered.ToList();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) ApplyFilters();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) ApplyFilters();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) ApplyFilters();
        }

        private void CreateProject(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPages(MainWindow.pages.projectAdd);
        }
    }
}
