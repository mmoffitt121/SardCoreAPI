using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.Region;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Services.Maps
{
    public interface ILocationService 
    {
        Task PutLocationType(LocationType type);
        Task DeleteLocationType(int Id);
        Task<List<Location>> GetLocations(LocationSearchCriteria criteria);
        Task<Location> GetLocation(int Id);
        Task<int> CountLocations(LocationSearchCriteria criteria);
        Task<List<Location>> GetLocationHeiarchy(int id, int depth);
        Task PutLocation(Location location);
        Task DeleteLocation(int Id);
        Task<int> PutRegion(Region region);
        Task DeleteRegion(int RegionId);
        Task PutDataPointLocation(DataPointLocation dpl);
    }

    public class LocationService : ILocationService
    {
        private readonly IDataService data;

        public LocationService(IDataService data) 
        {
            this.data = data;
        }

        public async Task PutLocationType(LocationType type)
        {
            LocationType? original = data.Context.LocationType.SingleOrDefault(x => x.Id.Equals(type.Id));
            if (original == null)
            {
                data.Context.LocationType.Add(type);
            }
            else
            {
                original.Name = type.Name;
                original.Summary = type.Summary;
                original.ParentTypeId = type.ParentTypeId;
                original.AnyTypeParent = type.AnyTypeParent;
                original.IconPath = type.IconPath;
                original.ZoomProminenceMin = type.ZoomProminenceMin;
                original.ZoomProminenceMax = type.ZoomProminenceMax;
                original.UsesIcon = type.UsesIcon;
                original.UsesLabel = type.UsesLabel;
                original.IconURL = type.IconURL;
                original.LabelFontSize = type.LabelFontSize;
                original.LabelFontColor = type.LabelFontColor;
                original.IconSize = type.IconSize;
                data.Context.LocationType.Update(original);
            }
            await data.Context.SaveChangesAsync();
        }

        public async Task DeleteLocationType(int id)
        {
            if (data.Context.Location.Where(l => l.LocationTypeId.Equals(id)).Count() > 0)
            {
                throw new Exception("This location type is in use by locations.");
            }
            await data.Context.LocationType.Where(lt => lt.Id.Equals(id)).ExecuteDeleteAsync();
        }

        public async Task<List<Location>> GetLocations(LocationSearchCriteria criteria)
        {
            return await data.Context.Location
                .Include(x => x.LocationType)
                .Where(criteria.GetQuery())
                .Paginate(criteria)
                .Select(location => location.ConsumeTypeData(location.LocationType))
                .ToListAsync();
        }

        public async Task<Location> GetLocation(int id)
        {
            return await data.Context.Location.SingleOrDefaultAsync(l => l.Id.Equals(id));
        }

        public async Task<List<Location>> GetLocationHeiarchy(int id, int depth)
        {
            List<Location> result = new List<Location>();
            LocationDataAccess dataAccess = new LocationDataAccess();
            int? currentId = id;
            for (int i = 0; i < depth; i++)
            {
                Location next = await GetLocation((int)currentId);
                if (next == null) { break; }
                result.Add(next);
                currentId = next.ParentId;
                if (currentId == null) { break; }
            }
            return result;
        }

        public async Task<int> CountLocations(LocationSearchCriteria criteria)
        {
            return await data.Context.Location.Where(criteria.GetQuery()).CountAsync();
        }

        public async Task PutLocation(Location location)
        {
            Location? old = data.Context.Location.SingleOrDefault(l => l.Id.Equals(location.Id));

            if (old == null)
            {
                data.Context.Add(location);
            }
            else
            {
                old.CopyValuesFrom(location);
                data.Context.Update(old);
            }

            await data.Context.SaveChangesAsync();
        }

        public async Task DeleteLocation(int Id)
        {
            await data.Context.Location.Where(l => l.Id.Equals(Id)).ExecuteDeleteAsync();
        }

        public async Task<int> PutRegion(Region region)
        {
            Region? old = data.Context.Region.SingleOrDefault(r => r.Id.Equals(region.Id));

            if (old == null)
            {
                data.Context.Add(region);
            }
            else
            {
                old.CopyValuesFrom(region);
                data.Context.Update(old);
            }

            await data.Context.SaveChangesAsync();
            return (old == null ? region.Id : old.Id) ?? -1;
        }

        public async Task DeleteRegion(int RegionId)
        {
            await data.Context.Region.Where(r => r.Id.Equals(RegionId)).ExecuteDeleteAsync();
        }

        public async Task PutDataPointLocation(DataPointLocation dpl)
        {
            DataPointLocation? old = data.Context.DataPointLocation.SingleOrDefault(x => x.LocationId.Equals(dpl.LocationId) && x.DataPointId.Equals(dpl.DataPointId));

            if (old == null)
            {
                data.Context.Add(dpl);
            }
            else
            {
                if (dpl.IsPrimary)
                {
                    data.Context.DataPointLocation
                        .Where(l => l.LocationId.Equals(dpl.LocationId))
                        .ExecuteUpdate(setters => setters.SetProperty(l => l.IsPrimary, false));
                }
                old.DataPointId = dpl.DataPointId;
                old.IsPrimary = dpl.IsPrimary;
                data.Context.Update(old);
            }

            await data.Context.SaveChangesAsync();
        }
    }
}
