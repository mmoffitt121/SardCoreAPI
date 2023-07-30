using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SardCoreAPI.Migrations.Entities
{
    public class DefaultRoles : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Name = "Viewer",
                NormalizedName = "VIEWER"
            },
            new IdentityRole
            {
                Name = "Editor",
                NormalizedName = "EDITOR"
            },
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
        );
        }
    }
}
