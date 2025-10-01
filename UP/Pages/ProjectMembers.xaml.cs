using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;
using UP.Models;

namespace UP.Pages
{
    public partial class ProjectMembers : Page
    {
        private ProjectContext currentProject;
        private List<UserProjectRoleContext> projectRoles;
        private List<UserContext> allUsers;

        public ProjectMembers()
        {
            InitializeComponent();
            LoadMembers();
        }

        private void LoadMembers()
        {
            currentProject = MainWindow.CurrentProject;
            if (currentProject == null)
            {
                MessageBox.Show("Проект не выбран");
                MainWindow.init.OpenPages(MainWindow.pages.projectList);
                return;
            }

            var roleContext = new UserProjectRoleContext();
            projectRoles = roleContext.AllUserProjectRoles().Where(r => r.ProjectId == currentProject.Id).ToList();

            var userContext = new UserContext();
            allUsers = userContext.AllUsers();

            var members = projectRoles.Select(r => new MemberViewModel
            {
                Id = r.Id,
                UserLogin = allUsers.FirstOrDefault(u => u.Id == r.UserId)?.Login ?? "Неизвестно",
                UserEmail = allUsers.FirstOrDefault(u => u.Id == r.UserId)?.Email ?? "-",
                Role = r.Role
            }).ToList();

            MembersGrid.ItemsSource = members;
        }

        private void SaveRoles(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (dynamic item in MembersGrid.ItemsSource)
                {
                    var role = projectRoles.FirstOrDefault(r => r.Id == item.Id);
                    if (role != null && role.Role != item.Role)
                    {
                        role.Role = item.Role;
                        role.Save(true);
                    }
                }

                MessageBox.Show("Роли обновлены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении ролей: {ex.Message}");
            }
        }

        private void AddUserToProject(object sender, RoutedEventArgs e)
        {
            string login = UserLoginBox.Text.Trim();
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин пользователя");
                return;
            }

            var user = allUsers.FirstOrDefault(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                MessageBox.Show("Пользователь с таким логином не найден");
                return;
            }

            if (projectRoles.Any(r => r.UserId == user.Id))
            {
                MessageBox.Show("Этот пользователь уже состоит в проекте");
                return;
            }

            var newRole = new UserProjectRoleContext
            {
                UserId = user.Id,
                ProjectId = currentProject.Id,
                RoleEnum = ProjectRole.Viewer 
            };
            newRole.Save(false);

            MessageBox.Show($"Пользователь {user.Login} добавлен в проект");

            LoadMembers();
        }

        private void RemoveUserFromProject(object sender, RoutedEventArgs e)
        {
            if (MembersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для удаления");
                return;
            }

            dynamic selected = MembersGrid.SelectedItem;
            var role = projectRoles.FirstOrDefault(r => r.Id == selected.Id);

            if (role == null)
                return;

            if (role.RoleEnum == ProjectRole.Creator)
            {
                MessageBox.Show("Нельзя удалить создателя проекта!");
                return;
            }

            try
            {
                role.Delete();
                MessageBox.Show($"Пользователь {selected.UserLogin} удалён из проекта");
                LoadMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        private void BackToProject(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPages(MainWindow.pages.project);
        }
    }
}
