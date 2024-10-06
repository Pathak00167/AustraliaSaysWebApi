using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using AustraliaSaysWebApi.DataAccess.Repository.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

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

        [HttpPatch("Update-UserProfile")]
        public async Task<IActionResult> UpdateUser([FromForm]UpdateUserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _unitofwork.User.UpdateUserProfileAsync(userProfile);

            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { result.Message});
        }


        #endregion
    }
}
