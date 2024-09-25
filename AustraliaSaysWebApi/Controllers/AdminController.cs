using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using AustraliaSaysWebApi.DataAccess.Repository.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AustraliaSaysWebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Constructor
        private readonly IUnitOfWork _unitofwork;
        public AdminController(IUnitOfWork unitOfWork)
        {
                _unitofwork = unitOfWork;
        }
        #endregion

        #region Categories
        [HttpPost("AddorUpdate-Category")]
        public IActionResult AddorUpdateCategory([FromBody]CategoryDto category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Please Ensure Your Model State");
                }
                var response = _unitofwork.Admin.AddCategory(category);
                if (response.Succeeded)
                {
                    return Ok(new { response.Message });
                }
                return BadRequest(response.Message);
            }
            catch (Exception)
            {

                return BadRequest("Some Thing Went Wrong Please Try Again later");
            }
        }

        [HttpGet("Categories-List")]
        public IActionResult CategoryList()
        {
            try
            {
                var response = _unitofwork.Admin.GetCategoriesList();
                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest("Some Thing Went Wrong Please Try Again later");
            }
        }

        [HttpDelete("Delete-Category/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Please Ensure Your Model State");
                }
                var response = _unitofwork.Admin.DeleteCategory(id);
                if (response.Succeeded)
                {
                    return Ok(new { message = response.Message });
                }
                return Ok(new { message = response.Message });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UsersList
        [HttpGet("Users-List")]
        public IActionResult UsersList()
        {
            try
            {
                var response = _unitofwork.Admin.GetUsersList();
                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest("Some Thing Went Wrong Please Try Again later");
            }
        }
        #endregion

        #region Notifications
        [HttpGet("Notifications-List")]
        public IActionResult NotificationsList()
        {
            try
            {
                var response = _unitofwork.Admin.GetNotificarionsList();
                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest("Some Thing Went Wrong Please Try Again later");
            }
        }


        [HttpPost("MarkAllNotificationsAsSeen")]
        public IActionResult MarkAllNotificationsAsSeen()
        {
            try
            {
                var result = _unitofwork.Admin.MarkAllNotificationsAsSeen();
                if (result)
                {
                    return Ok(new { Message = "All notifications marked as seen." });
                }
                else
                {
                    return BadRequest(new { Message = "No notifications found to mark as seen." });
                }
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went wrong. Please try again later." });
            }
        }


        #endregion


        #region Articles
        [HttpPost("AddArticle")]
        public IActionResult Article([FromForm]ArticleDto dto)
        {
            try
            {
                if (dto == null || dto.Categoryid == 0)
                {
                    return BadRequest("Invalid article data");
                }

                var category = _unitofwork.Admin.GetCategoryById(dto.Categoryid);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                var addedArticle = _unitofwork.Admin.AddArticle(dto);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("ArticlesList")]
        public IActionResult ArticlesList()
        {
            try
            {
                var response = _unitofwork.Admin.GetArticlesList();
                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest("Some Thing Went Wrong Please Try Again later");
            }
        }
        #endregion


    }
}
