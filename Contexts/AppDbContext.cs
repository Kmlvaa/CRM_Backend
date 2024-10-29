using CRM.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> AppUsers {  get; set; }
        public DbSet<Customer> Customers { get; set; } 
    }
}
