using AustraliaSaysWebApi.DataAccess.Data;
using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        public UserRepository(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager
            ,ApplicationDbContext context,IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hostingEnvironment = hostEnvironment;
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
                    user.UserName = userProfile.UserName;
                    user.Email = userProfile.UserName; // Assuming username and email are the same
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
                if (userProfile.UserProfilePicture != null)
                {
                   
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
    }
}
