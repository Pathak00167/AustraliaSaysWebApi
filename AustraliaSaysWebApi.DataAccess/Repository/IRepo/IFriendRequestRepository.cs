using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.IRepo
{
    public interface IFriendRequestRepository
    {
        Task<IActionResult> SendFriendRequestAsync(SendFriendRequest request);
        Task<List<ApplicationUser>> GetAllUsersExceptSenderAsync(string senderId);
    }
}
