using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Areas.Identity.Data;
using SardCoreAPI.Migrations.Entities;

namespace SardCoreAPI.Data;

public class SardCoreAPIContext : IdentityDbContext<SardCoreAPIUser>
{
    public SardCoreAPIContext(DbContextOptions<SardCoreAPIContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new DefaultRoles());
    }
}
