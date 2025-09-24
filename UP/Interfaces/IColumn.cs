using System.Collections.Generic;
using UP.Classes.Context;

namespace UP.Interfaces
{
    public interface IColumn
    {
        void Save(bool Update = false);
        List<ColumnContext> AllColumns();
        void Delete();
    }
}
