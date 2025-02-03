using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
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
        ApplicationUser GetUserProfile(string userId);
        Task<List<ApplicationUser>> GetAcceptedFriendsAsync(string userId);
        #endregion

        #region ChatEnd Interfaces
        Task<ReturnMessage> ChatRoomAsync(ChatRoomDto chatRoom);
        #endregion

    }
}
