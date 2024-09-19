﻿using AustraliaSaysWebApi.DataAccess.Data;
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
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructor
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbcontext;
        private readonly JwtService jwtService;
        private readonly IWebHostEnvironment _hostingEnvironment;
      


        public UnitOfWork(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context,JwtService _jwtService, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbcontext = context;
           
            jwtService = _jwtService;
            _hostingEnvironment = webHostEnvironment;
            Auth = new AuthRepository(_userManager, _signInManager, _hostingEnvironment, _jwtService,_dbcontext);
            Admin = new AdminRepository(_dbcontext,_userManager);


        }

        #endregion
        public IAuthRepository Auth { get; private set; }

        public IAdminRepository Admin { get; private set; }

        public void Save()
        {
            _dbcontext.SaveChanges();
        }
    }
}
