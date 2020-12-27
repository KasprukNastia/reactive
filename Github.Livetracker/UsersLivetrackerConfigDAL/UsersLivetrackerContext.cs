using Microsoft.EntityFrameworkCore;
using System.Linq;
using UsersLivetrackerConfigDAL.Models;

namespace UsersLivetrackerConfigDAL
{
    public class UsersLivetrackerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<KeywordInfo> KeywordInfos { get; set; }

        public UsersLivetrackerContext(DbContextOptions<UsersLivetrackerContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }
        }
    }
}
