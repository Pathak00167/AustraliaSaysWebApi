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
using System.Reflection.Metadata.Ecma335;
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

        public async Task<IActionResult> SendFriendRequestAsync(FriendRequestDto request)
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

        public async Task<IActionResult> AcceptFriendRequestAsync(FriendRequestDto request)
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

            if (existingRequest == null)
            {
                return new ConflictObjectResult("User has cancelled his request");
            }
          
            existingRequest.Status = "Accepted";
            existingRequest.RespondedAt= DateTime.UtcNow;
            // Save the friend request
            _context.FriendRequests.Update(existingRequest);
            await _context.SaveChangesAsync();

            // Optionally send a SignalR notification to the receiver
            await _hubContext.Clients.User(request.ReceiverId).SendAsync("ReceiveFriendRequest", request.SenderId);

            return new OkObjectResult($"Your request is accepted by {existingRequest.Receiver.FirstName}");
        }

        public async Task<IActionResult> RejectFriendRequestAsync(FriendRequestDto request)
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

            if (existingRequest == null)
            {
                return new ConflictObjectResult("User has cancelled his request");
            }
            existingRequest.Status = "Rejected";
            existingRequest.RespondedAt = DateTime.UtcNow;
            // Save the friend request
            _context.FriendRequests.Update(existingRequest);
            await _context.SaveChangesAsync();

            // Optionally send a SignalR notification to the receiver
            await _hubContext.Clients.User(request.ReceiverId).SendAsync("ReceiveFriendRequest", request.SenderId);

            return new OkObjectResult($"Your request is Declined by {existingRequest.Receiver.FirstName}");
        }

        public async Task<IActionResult> CancelFriendRequestAsync(FriendRequestDto request)
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

            if (existingRequest == null)
            {
                return new ConflictObjectResult("Request Not Found");
            }
           
            // Save the friend request
            _context.FriendRequests.Remove(existingRequest);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(request.ReceiverId).SendAsync("ReceiveNotification",
        $"Your friend request from {existingRequest.Sender.FirstName} was canceled.");

            return new OkObjectResult($"Your request is canceled by {existingRequest.Receiver.FirstName}");
        }

        public List<ApplicationUser> PendingRequestsAsync(string userId)
        {
            try
            {
                var pendingUsers = _context.FriendRequests
                    .Where(x => x.ReceiverId == userId && x.Status == "Pending")
                    .Join(_context.Users,
                          friendRequest => friendRequest.SenderId,
                          user => user.Id,
                          (friendRequest, user) => user)
                    .ToList();

                return pendingUsers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching pending friend requests.", ex);
            }
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
                    .OrderBy(r => Guid.NewGuid())  
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
