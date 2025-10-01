using System.Windows;
using UP.Classes.Context;
using UP.Pages;

namespace UP
{
    public partial class MainWindow : Window
    {
        public static MainWindow init;
        public static UserContext CurrentUser { get; set; }
        public static ProjectContext CurrentProject { get; set; }
        public static TaskContext CurrentTask { get; set; }
        public static ColumnContext CurrentColumn { get; set; }
        public static SubtaskContext CurrentSubtask { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            init = this;
            OpenPages(pages.login);
        }
        public enum pages 
        {
            login,
            registration,
            profile,
            project,
            projectList,
            projectAdd,
            taskManage,
            columnManage,
            subtaskManage,
            projectMembers
        }

        public void OpenPages(pages page) 
        {
            switch (page) 
            {
                case pages.login:
                    frame.Navigate(new Login());
                    break;
                case pages.registration:
                    frame.Navigate(new Registration());
                    break;
                case pages.profile:
                    frame.Navigate(new Profile());
                    break;
                case pages.project:
                    frame.Navigate(new Project());
                    break;
                case pages.projectList:
                    frame.Navigate(new ProjectsList());
                    break;
                case pages.projectAdd:
                    frame.Navigate(new ProjectAdd());
                    break;
                case pages.taskManage:
                    frame.Navigate(new TaskManage());
                    break;
                case pages.columnManage:
                    frame.Navigate(new ColumnManage());
                    break;
                case pages.subtaskManage:
                    frame.Navigate(new SubtaskManage());
                    break;
                case pages.projectMembers:
                    frame.Navigate(new ProjectMembers());
                    break;
            }
        }
    }
}
