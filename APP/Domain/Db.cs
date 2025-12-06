using Microsoft.EntityFrameworkCore;

namespace APP.Domain
{
    public class Db : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<GameTag> GameTags { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }
    }
}
