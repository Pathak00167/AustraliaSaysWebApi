using AustraliaSaysWebApi.DataAccess.Data;
using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using AustraliaSaysWebApi.Utility.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.Repo
{
    public class UserRepository:IUserRepository
    {
        #region Constructror
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHubContext<ChatHub> _hubContext;
        public UserRepository(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager
            ,ApplicationDbContext context,IWebHostEnvironment hostEnvironment, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hostingEnvironment = hostEnvironment;
            _hubContext = hubContext;
        }

        public ApplicationUser GetUserProfile(string userId)
        {
            try
            {
                // Retrieve user by Id from the ApplicationUser table
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                return user;
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw new Exception($"Error retrieving user profile: {ex.Message}", ex);
            }
        }

        public async Task<List<ApplicationUser>> GetAcceptedFriendsAsync(string userId)
        {
            // Get users who have accepted requests (either sent or received by the user)
            return await _context.FriendRequests
                .Where(fr => (fr.SenderId == userId) && fr.Status == "Accepted")
                .Select(fr => fr.SenderId == userId ? fr.Receiver : fr.Sender)
                .Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,  // Assuming you have FirstName property
                    Email = user.Email,
                    ProfilePicture = user.ProfilePicture // Assuming this property exists
                })
                .ToListAsync();
        }


        #endregion

        public async Task<ReturnMessage> UpdateUserProfileAsync(UpdateUserProfile userProfile)
        {
            try
            {
                // Find the user by their ID
                var user = await _userManager.FindByIdAsync(userProfile.UserId);
                if (user == null)
                {
                    return new ReturnMessage { Succeeded = false, Message = "User not found" };
                }

                // Update user fields if provided
                if (!string.IsNullOrEmpty(userProfile.UserName))
                {
                    var usernameExists = await _userManager.FindByNameAsync(userProfile.UserName);
                    if (usernameExists != null)
                    {
                        return new ReturnMessage { Succeeded = false, Message = "Username is already taken" };
                    }
                    user.UserName = userProfile.UserName;
                }

                if (!string.IsNullOrEmpty(userProfile.Name))
                {
                    user.FirstName = userProfile.Name; // Assuming "Name" is part of your ApplicationUser class
                }

                if (userProfile.Dob.HasValue)
                {
                    user.DateOfBirth = userProfile.Dob.Value; // Assuming "Dob" is part of your ApplicationUser class
                }

                // If a profile picture is uploaded, process and save it
                if (userProfile.UserProfilePicture != null &&userProfile !=null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique filename for the image
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + userProfile.UserProfilePicture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the image to the uploads folder
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await userProfile.UserProfilePicture.CopyToAsync(fileStream);
                    }
                    string relativePath = Path.Combine("uploads", uniqueFileName);
                    user.ProfilePicture = relativePath;
                }

                // Update user in database
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new ReturnMessage { Succeeded = false, Message = "Failed to update user" };
                }

                // Save changes to the database if needed
                await _context.SaveChangesAsync();

                return new ReturnMessage { Succeeded = true, Message = "User profile updated successfully" };
            }
            catch (Exception ex)
            {
                return new ReturnMessage { Succeeded = false, Message = "An error occurred while updating the profile" };
            }
        }

        public async Task<ReturnMessage> ChatRoomAsync(ChatRoomDto chatRoom)
        {
            try
            {
                var findFriend = await _context.FriendRequests
     .FirstOrDefaultAsync(f => f.SenderId == chatRoom.SenderId ||f.SenderId ==chatRoom.ReceiverId && f.ReceiverId == chatRoom.ReceiverId
     ||f.ReceiverId==f.SenderId &&f.Status == "Accepted");
                if(findFriend == null)
                {
                     return new ReturnMessage { Succeeded = false, Message = "Sorry, Currently You are not able to send messages" };
                }
                var message = new ChatMessage()
                {
                    SenderId = chatRoom.SenderId,
                    ReceiverId = chatRoom.ReceiverId,
                    Content = chatRoom.Content,
                    IsRead = false,
                    Timestamp = DateTime.Now,
                };
                await _context.ChatMessages.AddAsync(message);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.User(chatRoom.ReceiverId).SendAsync("ReceiveMessage", chatRoom.SenderId, chatRoom.Content);
                return new ReturnMessage { Succeeded = true, Message = "Message Sent" };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
