using CourseWork.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Web.Context
{
    public class RailwayContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public RailwayContext(DbContextOptions options) : base(options)
        {
        }
    }
}
