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
        Task<IActionResult> SendFriendRequestAsync(FriendRequestDto request);
        Task<IActionResult> AcceptFriendRequestAsync(FriendRequestDto request);
        Task<IActionResult> RejectFriendRequestAsync(FriendRequestDto request);
        Task<IActionResult> CancelFriendRequestAsync(FriendRequestDto request);
        List<ApplicationUser> PendingRequestsAsync(string userId);
        Task<List<ApplicationUser>> GetAllUsersExceptSenderAsync(string senderId);
    }
}
