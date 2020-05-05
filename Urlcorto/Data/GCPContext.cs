using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urlcorto.Data
{
    public class GCPContext : DbContext
    {
        public GCPContext(DbContextOptions<GCPContext> options)
            : base(options)
        {

        }

        public DbSet<Urlcorto.Models.Url> Urls { get; set; }
    }
}
