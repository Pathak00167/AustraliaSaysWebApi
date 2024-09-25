using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AustraliaSaysWebApi.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Constructor
        private readonly IUnitOfWork _unitofwork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        #endregion
    }
}
