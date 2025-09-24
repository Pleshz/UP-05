using System.Collections.Generic;
using UP.Classes.Context;

namespace UP.Interfaces
{
    public interface ISubtask
    {
        void Save(bool Update = false);
        List<SubtaskContext> AllSubtasks();
        void Delete();
    }
}
