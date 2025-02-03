using AustraliaSaysWebApi.DataAccess.Data;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using AustraliaSaysWebApi.Utility.Services;
using EcomWeb.Utility.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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
        private readonly EmailService _emailService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHubContext<ChatHub> _hubContext;



        public UnitOfWork(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context,JwtService _jwtService, 
            IWebHostEnvironment webHostEnvironment,EmailService emailService,IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbcontext = context;
            jwtService = _jwtService;
            _emailService = emailService;
            _hostingEnvironment = webHostEnvironment;
            _hubContext = hubContext;
            Auth = new AuthRepository(_userManager, _signInManager, _hostingEnvironment, _jwtService,_dbcontext,_emailService);
            Admin = new AdminRepository(_dbcontext,_userManager,_hostingEnvironment);
            User = new UserRepository(_userManager, _signInManager, _dbcontext, _hostingEnvironment, _hubContext);
            FriendRequest=new FriendRequestRepository(_dbcontext,_hubContext);


        }

        #endregion
        public IAuthRepository Auth { get; private set; }

        public IAdminRepository Admin { get; private set; }

        public IUserRepository User {  get; private set; }

        public IFriendRequestRepository FriendRequest { get; private set; }

        public void Save()
        {
            _dbcontext.SaveChanges();
        }
    }
}
