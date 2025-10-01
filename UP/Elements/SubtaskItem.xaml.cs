using System.Windows.Controls;
using System.Windows.Input;
using UP.Classes.Context;

namespace UP.Elements
{
    public partial class SubtaskItem : UserControl
    {
        public SubtaskItem()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is SubtaskContext subtask)
            {
                MainWindow.CurrentSubtask = subtask;
                MainWindow.init.OpenPages(MainWindow.pages.subtaskManage);
            }
        }
    }
}
