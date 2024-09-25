using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.DTOs
{
    public class ArticleDto
    {
        public string ArticleTitle { get; set; }
        public string Description { get; set; }
        public IFormFile ArticleImage {  get; set; }
        public int Categoryid { get; set; }
    }
}
