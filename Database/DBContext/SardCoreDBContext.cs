using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Migrations.Entities;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security;

namespace SardCoreAPI.Database.DBContext;

public class SardCoreDBContext : IdentityDbContext<SardCoreAPIUser>
{
    public SardCoreDBContext(DbContextOptions<SardCoreDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new DefaultRoles());
    }

    public DbSet<World> World { get; set; }
}
