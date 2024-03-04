using AdsProject.BussinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.DataAccessLogic
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Ad> Ad { get; set; }
        public DbSet<AdImage> AdImage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source = JC-PC; Initial Catalog = AdsProject; Integrated Security = True;
                Encrypt = False; TrustServerCertificate = True");
        }
    }
}
