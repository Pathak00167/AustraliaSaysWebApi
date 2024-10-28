using AustraliaSaysWebApi.DataAccess.Data;
using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using AustraliaSaysWebApi.Utility.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.Repo
{
    public class FriendRequestRepository:IFriendRequestRepository
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public FriendRequestRepository(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> SendFriendRequestAsync(SendFriendRequest request)
        {
            // Check if both the sender and receiver exist
            var senderExists = await _context.Users.AnyAsync(u => u.Id == request.SenderId);
            var receiverExists = await _context.Users.AnyAsync(u => u.Id == request.ReceiverId);

            if (!senderExists)
            {
                return new NotFoundObjectResult("Sender does not exist.");
            }

            if (!receiverExists)
            {
                return new NotFoundObjectResult("Receiver does not exist.");
            }

            // Check if a friend request already exists between these users
            var existingRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(fr => fr.SenderId == request.SenderId && fr.ReceiverId == request.ReceiverId);

            if (existingRequest != null)
            {
                 return new ConflictObjectResult("Friend request already exists.");
            }
            var sendrequest = new FriendRequest()
            {
               SenderId = request.SenderId,
               ReceiverId = request.ReceiverId,
                Status = "Pending"
                
            };
            // Save the friend request
            _context.FriendRequests.Add(sendrequest);
            await _context.SaveChangesAsync();

            // Optionally send a SignalR notification to the receiver
            await _hubContext.Clients.User(request.ReceiverId).SendAsync("ReceiveFriendRequest", request.SenderId);

            return new OkObjectResult("Friend request sent successfully.");
        }


        public async Task<List<ApplicationUser>> GetAllUsersExceptSenderAsync(string senderId)
        {
            try
            {
                // Get all users excluding the sender
                var excludedUserIds = await _context.FriendRequests
                .Where(fr => (fr.SenderId == senderId || fr.ReceiverId == senderId)
                             && (fr.Status == "Pending" || fr.Status == "Accepted"))
                .Select(fr => fr.SenderId == senderId ? fr.ReceiverId : fr.SenderId)
                .ToListAsync();

                // Fetch all users except the sender and those involved in a friend request
                var allUsers = await _context.Users
                    .Where(u => u.Id != senderId && !excludedUserIds.Contains(u.Id))
                    .OrderBy(r => Guid.NewGuid())  // Optional: shuffle the result set
                    .ToListAsync();

                return allUsers;
            }
            catch (Exception ex)
            {
                // Log or handle the exception accordingly
                throw new Exception("An error occurred while fetching users.", ex);
            }
        }


        #endregion
    }
}
