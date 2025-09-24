using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UP.Classes.Context;

namespace UP.Interfaces
{
    public interface IUser
    {
        void Save(bool Update = false);
        List<UserContext> AllUsers();
        void Delete();
    }
}
