using AustraliaSaysWebApi.DataAccess.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.IRepo
{
    public interface IUserRepository
    {
        #region ProfileSection Interfaces
        Task<ReturnMessage> UpdateUserProfileAsync(UpdateUserProfile userProfile);
        #endregion

        #region ChatEnd Interfaces

        #endregion

    }
}
