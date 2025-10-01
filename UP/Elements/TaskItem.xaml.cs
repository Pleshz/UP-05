using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UP.Classes.Context;
using UP.Models;

namespace UP.Elements
{
    public partial class TaskItem : UserControl
    {
        private Point _dragStart;

        public TaskItem()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is TaskContext task)
            {
                MainWindow.CurrentTask = task;
                MainWindow.init.OpenPages(MainWindow.pages.taskManage);
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DataContext is TaskContext task)
            {
                DragDrop.DoDragDrop(this, task, DragDropEffects.Move);
            }
        }
    }
}
