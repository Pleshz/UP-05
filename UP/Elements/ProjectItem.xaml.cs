using System.Windows.Controls;
using System.Windows.Input;
using UP.Classes.Context;

namespace UP.Elements
{
    public partial class ProjectItem : UserControl
    {
        public ProjectItem()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ProjectContext project)
            {
                MainWindow.CurrentProject = project;
                MainWindow.init.OpenPages(MainWindow.pages.project);
            }
        }
    }
}
