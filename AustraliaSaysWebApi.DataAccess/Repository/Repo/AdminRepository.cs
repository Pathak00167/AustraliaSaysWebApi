using AustraliaSaysWebApi.DataAccess.Data;
using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using AustraliaSaysWebApi.DataAccess.Repository.IRepo;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.Repo
{
    public class AdminRepository:IAdminRepository
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #endregion

        #region Category
        public ReturnMessage AddCategory(CategoryDto category)
        {
            if (category.Id == null && category.CategoryName == null)
            {
                return new ReturnMessage { Succeeded = false, Message = "Nothing Has Been selected" };

            }
            if (category.Id == null)
            {
                var newCategory = new Category()
                {
                    CategoryName = category.CategoryName
                };
                _context.Category.Add(newCategory);
                _context.SaveChanges();
                return new ReturnMessage { Succeeded = true, Message = "Category Added Successfully", Token = null };
            };
            var findid = _context.Category.Find(category.Id);
            if (findid == null)
            {
                return new ReturnMessage { Succeeded = false, Message = "Id Not Found", Token = null };
            }
            findid.CategoryName = category.CategoryName;
            _context.Category.Update(findid);
            _context.SaveChanges();
            return new ReturnMessage { Succeeded = true, Message = "Category Updated Successfully", Token = null };
        }

        public ReturnMessage DeleteCategory(int Id)
        {
            var findCategory = _context.Category.Find(Id);
            if (findCategory == null)
            {
                return new ReturnMessage { Succeeded = false, Message = "Unable to delete at this moment" };
            }
            _context.Category.Remove(findCategory);
            _context.SaveChanges();
            return new ReturnMessage { Succeeded = true, Message = "Category Deleted Successfully" };
        }

        public List<Category> GetCategoriesList()
        {
            var list = _context.Category.ToList();
            return list;
        }

        



        public ReturnMessage Remove(int id)
        {
            var find = _context.Category.Find(id);
            if (find == null)
            {
                return new ReturnMessage { Succeeded = false, Message = "Sorry Category Not Found", Token = null };
            }
            _context.Category.Remove(find);
            _context.SaveChanges();
            return new ReturnMessage { Succeeded = true, Message = "Category Removed Successfully", Token = null };
        }

        #endregion

        #region UsersList
        public List<UsersDto> GetUsersList()
        {
            var users = _userManager.Users.ToList();

           
            var usersDto = users.Select(user => new UsersDto
            {
                Email = user.Email,
                Phonenumber = user.PhoneNumber,
                Name = user.UserName, 
                ImageUrl = user.ProfilePicUrl 
            }).ToList();

            return usersDto;
        }


        #endregion

        #region Notifications
        public List<Notification> GetNotificarionsList()
        {
            // Fetch all unseen notifications
            var notifications = _context.Notification
                .Where(notification => notification.Seen == false) // Filter unseen notifications
                .OrderByDescending(notification => notification.Time) // Order by the most recent notification
                .ToList();

            return notifications;
        }


        public bool MarkAllNotificationsAsSeen()
        {
            var notifications = _context.Notification.Where(n => n.Seen == false).ToList();
            if (notifications.Count == 0) return false;

            notifications.ForEach(n => n.Seen = true);
            _context.SaveChanges();
            return true;
        }


        #endregion
    }
}
