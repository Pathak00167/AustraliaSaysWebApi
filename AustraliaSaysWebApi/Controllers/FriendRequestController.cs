using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AustraliaSaysWebApi.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        #region Constructor
        private readonly IUnitOfWork _unitOfWork;
        public FriendRequestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        [HttpPost("send")]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendFriendRequest request)
        {
            try
            {
                // Validate sender and receiver
                if (request == null || string.IsNullOrEmpty(request.SenderId) || string.IsNullOrEmpty(request.ReceiverId))
                {
                    return BadRequest("Invalid friend request.");
                }
                var result = await _unitOfWork.FriendRequest.SendFriendRequestAsync(request);
                if (result == null)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("random-users/{senderId}")]
        public async Task<IActionResult> GetRandomUsers(string senderId)
        {
            try
            {
                if (string.IsNullOrEmpty(senderId))
                {
                    return BadRequest();
                }
                var result =await _unitOfWork.FriendRequest.GetAllUsersExceptSenderAsync(senderId);
                if (result == null)
                {

                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
