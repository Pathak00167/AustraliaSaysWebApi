using AustraliaSaysWebApi.DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.IRepo
{
    public interface IAuthRepository
    {
        Task<ReturnMessage> Register(RegisterData model, string password);
        Task<ReturnMessage> Login(string email, string password);
    }
}
