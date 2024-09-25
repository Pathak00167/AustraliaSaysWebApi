using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class ArticlesList
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string Description { get; set; }
        public string ArticleImage { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
