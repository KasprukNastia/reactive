using Microsoft.EntityFrameworkCore;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL
{
    public class UsersLivetrackerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

        public UsersLivetrackerContext(DbContextOptions<UsersLivetrackerContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
