using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using AustraliaSaysWebApi.DataAccess.Repository.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AustraliaSaysWebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Constructor
        private readonly IUnitOfWork _unitofwork;
        public AuthController(IUnitOfWork unitOfWork)
        {
                _unitofwork = unitOfWork;
        }
        #endregion

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var loginTry = await _unitofwork.Auth.Login(model.Email, model.Password);
                if(loginTry.Succeeded==false)
                {
                    return BadRequest(loginTry.Message);
                }
                return Ok(loginTry);
            }
            catch (Exception)
            {
                return BadRequest("Internal server error");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterData model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _unitofwork.Auth.Register(model, model.Password);
                if(response.Succeeded==false)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Register-User")]
        public async Task<IActionResult> RegisterUser( RegisterUser model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _unitofwork.Auth.RegisterUser(model.Email, model.Password);
                if (response.Succeeded == false)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
