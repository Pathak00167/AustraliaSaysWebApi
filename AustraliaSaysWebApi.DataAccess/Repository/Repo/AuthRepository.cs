﻿using AustraliaSaysWebApi.DataAccess.Data;
using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Models;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using EcomWeb.Utility.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.Repo
{
    public class AuthRepository : IAuthRepository
    {
        #region Constructor

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;


        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment, JwtService jwtService,ApplicationDbContext context,EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = webHostEnvironment;
            _jwtService = jwtService;   
         _emailService = emailService;
            _context = context;
        }
        #endregion


        public async Task<ReturnMessage> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ReturnMessage { Succeeded = false, Message = "Invalid email provided. Please ensure that you have entered a valid email address." };
            }
            //if(user.PhoneNumberConfirmed== false)
            //{
            //    return new ReturnMessage { Succeeded = false, Message = "Please Confirm Your Account First" };

            //}

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Retrieve the user's roles
                var roles = await _userManager.GetRolesAsync(user);

                // Assuming the user has one role, you can use the first role
                var userRole = roles.FirstOrDefault();

                // Generate the JWT token
                var token = _jwtService.GenerateToken(user.Id, userRole, user.UserName);

                // Return success message with token and role
                return new ReturnMessage
                {
                    Succeeded = true,
                    Message = "Login Successful",
                    Token = token,
                    Role = userRole,
                    Data= new UserInfo
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Bio= user.Bio,
                        ProfilePicture= user.ProfilePicture,
                        FirstName= user.FirstName,
                        LastName= user.LastName,
                        PhoneNumber= user.PhoneNumber,
                    }
                };
            }
            else
            {
                return new ReturnMessage
                {
                    Succeeded = false,
                    Message = "Invalid password provided. Please check your password and try again."
                };
            }
        }

        public async Task<ReturnMessage> Register(RegisterData model, string password)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.Name,
                };

                if (model.UserImage != null)
                {

                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");


                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }


                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UserImage.FileName;


                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UserImage.CopyToAsync(stream);
                    }


                    string relativePath = Path.Combine("uploads", uniqueFileName);
                    user.ProfilePicture = relativePath;
                }

                var findUser = await _userManager.FindByEmailAsync(user.Email);
                if (findUser != null)
                {
                    return new ReturnMessage { Succeeded = false, Message = "User with this email already exists", Token = null };
                }


                var createUser = await _userManager.CreateAsync(user, password);
                if (!createUser.Succeeded)
                {
                    return new ReturnMessage { Succeeded = false, Message = "User creation failed. Please try again later.", Token = null };
                }

                var addToRole = await _userManager.AddToRoleAsync(user, "User");
                if (!addToRole.Succeeded)
                {
                    return new ReturnMessage { Succeeded = false, Message = "Failed to add user to role. Please try again later.", Token = null };
                }
                var notificatiodata = new Notification
                {
                    UserId = user.Id,
                    Message = $" {model.Name} has created a new account",
                    Type ="User",
                    Time=DateTime.Now,
                    Seen=false

                };
               var addNotification= _context.Notification.Add(notificatiodata);
                _context.SaveChanges();

                return new ReturnMessage { Succeeded = true, Message = "Account Created SuccessFully Please Login" };
                }
            catch (Exception ex)
            {
                return new ReturnMessage { Succeeded = false, Message = "An error occurred. Please try again later.", Token = null };
            }
        }

        public async Task<ReturnMessage> RegisterUser(string email, string password)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,

                };

                var findUser = await _userManager.FindByEmailAsync(user.Email);
                if (findUser != null)
                {
                    return new ReturnMessage { Succeeded = false, Message = "User with this email already exists", Token = null };
                }


                var createUser = await _userManager.CreateAsync(user, password);
                if (!createUser.Succeeded)
                {
                    return new ReturnMessage { Succeeded = false, Message = "User creation failed. Please try again later.", Token = null };
                }

                var addToRole = await _userManager.AddToRoleAsync(user, "User");
                if (!addToRole.Succeeded)
                {
                    return new ReturnMessage { Succeeded = false, Message = "Failed to add user to role. Please try again later.", Token = null };
                }
                var otp = new Random().Next(100000, 999999).ToString();
                var sendMail = _emailService.SendEmailAsync(email,"Account Registration", $"Your OTP is: {otp}");
                
                var setToken = await _userManager.SetAuthenticationTokenAsync(user, "Default", "OTP", otp);
                if (!setToken.Succeeded)
                {
                    return new ReturnMessage { Succeeded = false, Message = "Failed to set OTP. Please try again later.", Token = null };
                }

                //await _emailService.SendEmailAsync(user.Email,"ChatApp Account Verification", $"Your OTP is: {otp}");
                var notificatiodata = new Notification
                {
                    UserId = user.Id,
                    Message = $" {user.Email} has created a new account",
                    Type = "User",
                    Time = DateTime.Now,
                    Seen = false

                };
                var addNotification = _context.Notification.Add(notificatiodata);
                _context.SaveChanges();
                return new ReturnMessage { Succeeded = true, Message = "OTP sent to your  Mail Address please Verify.", Token = null,Role=addToRole.ToString() };
            }

            catch (Exception ex)
            {
                return new ReturnMessage { Succeeded = false, Message = "An error occurred. Please try again later.", Token = null };
            }
        }

        // Method to verify OTP
        public async Task<ReturnMessage> VerifyOtp(string email, string otp)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ReturnMessage { Succeeded = false, Message = "User not found", Token = null };
                }

                var savedOtp = await _userManager.GetAuthenticationTokenAsync(user, "Default", "OTP");
                if (savedOtp == null || savedOtp != otp)
                {
                    return new ReturnMessage { Succeeded = false, Message = "Invalid OTP", Token = null };
                }

                // OTP is valid, you can mark the user as verified, remove the token, etc.
                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "OTP");
                user.PhoneNumberConfirmed=true;
             await   _userManager.UpdateAsync(user);
                // Retrieve the user's roles
                var roles = await _userManager.GetRolesAsync(user);

                // Assuming the user has one role, you can use the first role
                var userRole = roles.FirstOrDefault();
                var token = _jwtService.GenerateToken(user.Id, userRole, user.UserName);


                return new ReturnMessage { Succeeded = true, Message = "OTP verified successfully", Token = token,Role=userRole  };
            }
            catch (Exception ex)
            {
                return new ReturnMessage { Succeeded = false, Message = "An error occurred. Please try again later.", Token = null };
            }
        }
    }
}
