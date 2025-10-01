using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;
using Excel = Microsoft.Office.Interop.Excel;

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

        public void GenerateReport(object sender, RoutedEventArgs e)
        {
            try
            {
                var projectContext = new ProjectContext();
                var taskContext = new TaskContext();
                var subtaskContext = new SubtaskContext();
                var roleContext = new UserProjectRoleContext();
                var userContext = new UserContext();

                int currentUserId = MainWindow.CurrentUser.Id;

                var allProjects = projectContext.AllProjects();
                var allRoles = roleContext.AllUserProjectRoles();
                var allUsers = userContext.AllUsers();

                var userProjects = allProjects
                    .Where(p =>
                        p.CreatorId == currentUserId ||
                        allRoles.Any(r => r.ProjectId == p.Id && r.UserId == currentUserId) ||
                        p.IsPublic)
                    .ToList();

                var allTasks = taskContext.AllTasks();
                var allSubtasks = subtaskContext.AllSubtasks();

                var excelApp = new Excel.Application();
                excelApp.Visible = false;

                var workbook = excelApp.Workbooks.Add();
                Excel._Worksheet worksheet = workbook.ActiveSheet;

                worksheet.Cells[1, 1] = "Проект";
                worksheet.Cells[1, 2] = "Описание";
                worksheet.Cells[1, 3] = "Публичный";
                worksheet.Cells[1, 4] = "Задача";
                worksheet.Cells[1, 5] = "Описание задачи";
                worksheet.Cells[1, 6] = "Подзадача";
                worksheet.Cells[1, 7] = "Описание подзадачи";
                worksheet.Cells[1, 8] = "Ответственный";

                Excel.Range headerRange = worksheet.Range["A1", "H1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                int row = 1;

                foreach (var project in userProjects)
                {
                    var projectTasks = allTasks.Where(t => t.ProjectId == project.Id).ToList();

                    if (projectTasks.Count == 0)
                    {
                        row++;
                        worksheet.Cells[row, 1] = project.Name;
                        worksheet.Cells[row, 2] = project.Description;
                        worksheet.Cells[row, 3] = project.IsPublic ? "Да" : "Нет";
                    }

                    foreach (var task in projectTasks)
                    {
                        var subtasks = allSubtasks.Where(s => s.TaskId == task.Id).ToList();

                        if (subtasks.Count == 0)
                        {
                            row++;
                            worksheet.Cells[row, 1] = project.Name;
                            worksheet.Cells[row, 2] = project.Description;
                            worksheet.Cells[row, 3] = project.IsPublic ? "Да" : "Нет";
                            worksheet.Cells[row, 4] = task.Title;
                            worksheet.Cells[row, 5] = task.Description;
                        }

                        foreach (var sub in subtasks)
                        {
                            row++;
                            worksheet.Cells[row, 1] = project.Name;
                            worksheet.Cells[row, 2] = project.Description;
                            worksheet.Cells[row, 3] = project.IsPublic ? "Да" : "Нет";
                            worksheet.Cells[row, 4] = task.Title;
                            worksheet.Cells[row, 5] = task.Description;
                            worksheet.Cells[row, 6] = sub.Name;
                            worksheet.Cells[row, 7] = sub.Description;

                            var assignedUser = allUsers.FirstOrDefault(u => u.Id == sub.AssignedUserId);
                            worksheet.Cells[row, 8] = assignedUser != null ? assignedUser.Login : "-";
                        }
                    }
                }

                worksheet.Columns.AutoFit();

                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel файлы (*.xlsx)|*.xlsx",
                    FileName = $"Report_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                };

                if (sfd.ShowDialog() == true)
                {
                    workbook.SaveAs(sfd.FileName);
                    MessageBox.Show("Отчёт успешно сохранён!");
                }

                workbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчёта: {ex.Message}");
            }
        }

        public void GoToProjects(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPages(MainWindow.pages.projectList);
        }
    }
}
