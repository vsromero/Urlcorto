using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Urlcorto.Models
{
    public class Url
    {
        [Key]
        public int Id { get; set; }

        public string UrlToShorten { get; set; }

        public string ShortenedUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
    }
}
