using System.Collections.Generic;
using UP.Classes.Context;

namespace UP.Interfaces
{
    public interface IUserProjectRole
    {
        void Save(bool Update = false);
        List<UserProjectRoleContext> AllUserProjectRoles();
        void Delete();
    }
}
