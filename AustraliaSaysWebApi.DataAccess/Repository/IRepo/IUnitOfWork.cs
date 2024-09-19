using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.IRepo
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        IAdminRepository Admin { get; }
        void Save();
    }
}
