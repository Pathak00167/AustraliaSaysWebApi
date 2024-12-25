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
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion


        #region UserProfileSection
        [HttpPatch("Update-UserProfile")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _unitOfWork.User.UpdateUserProfileAsync(userProfile);

            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { result.Message });
        }

        [HttpGet("Get-UserProfile")]
        public  IActionResult GetUserById(string userId)
        {
            try
            {
                if(string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FriendRequestEndpoints

        [HttpPost("send")]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto request)
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
                var result = await _unitOfWork.FriendRequest.GetAllUsersExceptSenderAsync(senderId);
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

        [HttpPost("accept-Request")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] FriendRequestDto acceptRequest)
        {
            try
            {
                if (acceptRequest == null || string.IsNullOrEmpty(acceptRequest.SenderId) || string.IsNullOrEmpty(acceptRequest.ReceiverId))
                {
                    return BadRequest("Invalid request.Please Verify Before Send");
                }
                var result = await _unitOfWork.FriendRequest.AcceptFriendRequestAsync(acceptRequest);
                if (result == null)
                {
                    return BadRequest(new { result });
                }
                return Ok(new { result });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("reject-Request")]
        public async Task<IActionResult> RejectFriendRequest([FromBody] FriendRequestDto rejectRequest)
        {
            try
            {
                if (rejectRequest == null || string.IsNullOrEmpty(rejectRequest.SenderId) || string.IsNullOrEmpty(rejectRequest.ReceiverId))
                {
                    return BadRequest("Invalid request.Please Verify Before Send");
                }
                var result = await _unitOfWork.FriendRequest.RejectFriendRequestAsync(rejectRequest);
                if (result == null)
                {
                    return BadRequest(new { result });
                }
                return Ok(new { result });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Cancel-Request")]
        public async Task<IActionResult> CancelFriendRequest([FromBody] FriendRequestDto cancelRequest)
        {
            try
            {
                if (cancelRequest == null || string.IsNullOrEmpty(cancelRequest.SenderId) || string.IsNullOrEmpty(cancelRequest.ReceiverId))
                {
                    return BadRequest("Invalid request.Please Verify Before Send");
                }
                var result = await _unitOfWork.FriendRequest.CancelFriendRequestAsync(cancelRequest);
                if (result == null)
                {
                    return BadRequest(new { result });
                }
                return Ok(new { result });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("Pending-Request/{userId}")]
        public  IActionResult PendingRequests(string userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request.Please Verify Before Send");
                }
                var findrequest=_unitOfWork.FriendRequest.PendingRequestsAsync(userId);
                if (findrequest ==  null)
                {
                    return BadRequest();
                }
                return Ok(findrequest);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Chat Section

        #endregion

    }
}
