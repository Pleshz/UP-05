using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UP.Classes.Common;
using UP.Interfaces;
using UP.Models;

namespace UP.Classes.Context
{
    public class ColumnContext : Column, IColumn
    {
        public ObservableCollection<TaskContext> Tasks { get; set; } = new ObservableCollection<TaskContext>();
        public List<ColumnContext> AllColumns()
        {
            List<ColumnContext> allColumns = new List<ColumnContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `Columns`", connection);

            while (data.Read())
            {
                ColumnContext column = new ColumnContext
                {
                    Id = data.GetInt32(0),
                    Name = data.GetString(1),
                    ProjectId = data.GetInt32(2)
                };
                allColumns.Add(column);
            }
            Connection.CloseConnection(connection);
            return allColumns;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Columns` " +
                                    "SET " +
                                        $"`Name` = '{this.Name}', " +
                                        $"`ProjectId` = {this.ProjectId} " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `Columns` " +
                                    "(`Name`, `ProjectId`) " +
                                 $"VALUES ('{this.Name}', {this.ProjectId})", connection);
            }
            Connection.CloseConnection(connection);
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();

            try
            {
                var taskContext = new TaskContext();
                var tasks = taskContext.AllTasks().Where(t => t.ColumnId == this.Id).ToList();

                foreach (var task in tasks)
                {
                    task.Delete();
                }

                Connection.Query($"DELETE FROM `Columns` WHERE `Id` = {this.Id}", connection);
            }
            finally
            {
                Connection.CloseConnection(connection);
            }
        }
    }
}
