using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SardCoreAPI.Migrations.Entities;
using SardCoreAPI.Models.Calendars;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Models.Map.Region;
using SardCoreAPI.Models.Pages.Pages;
using SardCoreAPI.Models.Pages.Views;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Models.Units;
using System.Numerics;

namespace SardCoreAPI.Database.DBContext
{
    public class SardLibraryDBContext : DbContext
    {
        public SardLibraryDBContext(DbContextOptions<SardLibraryDBContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Handle big integers with time
            var converter = new ValueConverter<BigInteger, long>(
                model => (long)model,
                provider => new BigInteger(provider));
            builder.Entity<DataPointParameterTime>().Property(p => p.TimeValue).HasConversion(converter);

            builder.Entity<World>().HasIndex(w => w.Location).IsUnique();
        }

        public DbSet<CalendarDataAccessWrapper> Calendar { get; set; }

        public DbSet<DataPointLocation> DataPointLocation { get; set; }

        public DbSet<DataPointParameter> DataPointParameter { get; set; }

        public DbSet<DataPointParameterBoolean> DataPointParameterBoolean { get; set; }

        public DbSet<DataPointParameterDataPoint> DataPointParameterDataPoint { get; set; }

        public DbSet<DataPointParameterDocument> DataPointParameterDocument { get; set; }

        public DbSet<DataPointParameterDouble> DataPointParameterDouble { get; set; }

        public DbSet<DataPointParameterInt> DataPointParameterInt { get; set; }

        public DbSet<DataPointParameterString> DataPointParameterString { get; set; }

        public DbSet<DataPointParameterSummary> DataPointParameterSummary { get; set; }

        public DbSet<DataPointParameterTime> DataPointParameterTime { get; set; }

        public DbSet<DataPointParameterUnit> DataPointParameterUnit { get; set; }

        public DbSet<DataPoint> DataPoint { get; set; }

        public DbSet<DataPointTypeParameter> DataPointTypeParameter { get; set; }

        public DbSet<DataPointType> DataPointType { get; set; }

        public DbSet<

        public DbSet<Location> Location { get; set; }

        public DbSet<LocationType> LocationType { get; set; }

        public DbSet<Map> Map { get; set; }

        public DbSet<MapLayer> MapLayer { get; set; }

        public DbSet<MapTile> MapTile { get; set; }

        public DbSet<Measurable> Measurable { get; set; }

        public DbSet<Page> Page { get; set; }

        public DbSet<PersistentZoomLevel> PersistentZoomLevel { get; set; }

        public DbSet<Region> Region { get; set; }

        public DbSet<RolePermission> RolePermission { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<SettingJSON> SettingJSON { get; set; }

        public DbSet<Unit> Unit { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<ViewWrapper> View { get; set; }
    }
}
