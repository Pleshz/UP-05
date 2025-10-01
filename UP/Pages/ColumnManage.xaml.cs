using System;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class ColumnManage : Page
    {
        private ColumnContext currentColumn;
        private bool isNewColumn = false;

        public ColumnManage()
        {
            InitializeComponent();
            LoadColumn();
        }

        private void LoadColumn()
        {
            currentColumn = MainWindow.CurrentColumn;

            if (currentColumn != null)
            {
                HeaderText.Text = "Редактирование колонки";
                NameTextBox.Text = currentColumn.Name;
                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                HeaderText.Text = "Создание новой колонки";
                isNewColumn = true;
                currentColumn = new ColumnContext
                {
                    ProjectId = MainWindow.CurrentProject.Id
                };
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            currentColumn.Name = NameTextBox.Text.Trim();

            try
            {
                if (isNewColumn)
                {
                    currentColumn.Save(false);
                    MessageBox.Show("Колонка создана!");
                }
                else
                {
                    currentColumn.Save(true);
                    MessageBox.Show("Колонка обновлена!");
                }

                MainWindow.CurrentColumn = null;
                MainWindow.init.OpenPages(MainWindow.pages.project);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (currentColumn == null) return;

            if (MessageBox.Show("Удалить колонку?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    currentColumn.Delete();
                    MessageBox.Show("Колонка удалена.");
                    MainWindow.CurrentColumn = null;
                    MainWindow.init.OpenPages(MainWindow.pages.project);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CurrentColumn = null;
            MainWindow.init.OpenPages(MainWindow.pages.project);
        }
    }
}
