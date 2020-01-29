using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Urlcorto.Models;

namespace Urlcorto.Data
{
    public class UrlcortoContext : DbContext
    {
        public UrlcortoContext (DbContextOptions<UrlcortoContext> options)
            : base(options)
        {
        }

        public DbSet<Urlcorto.Models.Url> Urls { get; set; }
    }
}
