using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UP.Classes.Context;

namespace UP.Elements
{
    public partial class ColumnItem : UserControl
    {
        public event Action<TaskContext, ColumnContext> TaskDropped;

        public ColumnItem()
        {
            InitializeComponent();
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
            if (e.Data.GetDataPresent(typeof(TaskContext)) && DataContext is ColumnContext column)
            {
                var task = e.Data.GetData(typeof(TaskContext)) as TaskContext;
                if (task == null) return;

                if (task.ColumnId == column.Id) return;

                task.ColumnId = column.Id;
                task.UpdatedAt = DateTime.Now;
                task.Save(true);

                var projectPage = MainWindow.init.frame.Content as Pages.Project;
                projectPage?.LoadProjectTasks();
            }
        }
    }
}
