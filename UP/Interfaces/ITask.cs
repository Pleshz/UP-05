using System.Collections.Generic;
using UP.Classes.Context;

namespace UP.Interfaces
{
    public interface ITask
    {
        void Save(bool Update = false);
        List<TaskContext> AllTasks();
        void Delete();
    }
}
