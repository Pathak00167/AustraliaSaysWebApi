using AustraliaSaysWebApi.DataAccess.DTOs;
using AustraliaSaysWebApi.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Repository.IRepo
{
    public interface IAdminRepository
    {
        #region Categories
        ReturnMessage AddCategory(CategoryDto category);
        List<Category> GetCategoriesList();
        ReturnMessage DeleteCategory(int Id);
        Category GetCategoryById(int Id);
        #endregion

        List<UsersDto> GetUsersList();
        List<Notification> GetNotificarionsList();
        bool MarkAllNotificationsAsSeen();

        #region Articles
        ReturnMessage AddArticle(ArticleDto article);
        List<ArticlesList> GetArticlesList();
        #endregion
    }
}
