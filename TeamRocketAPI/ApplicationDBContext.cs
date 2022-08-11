using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamRocketAPI.Entities;

namespace TeamRocketAPI
{
    //inherits from IdentityDbContext to include identity tables
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        public ApplicationDBContext() : base()
        {

        }

        public DbSet<Pokemon> Pokemon { get; set; }
    }
}
