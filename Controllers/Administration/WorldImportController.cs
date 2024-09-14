using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Models.Administration;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.Files;
using System.IO;
using System;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using Newtonsoft.Json;
using SardCoreAPI.Models.Units;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.Region;
using SardCoreAPI.Models.Calendars.CalendarData;
using Microsoft.AspNetCore.Authorization;
using SardCoreAPI.Models.Calendars;
using Microsoft.EntityFrameworkCore;

namespace SardCoreAPI.Controllers.Administration
{
    [ApiController]
    [Route("Map/[controller]/[action]")]
    public class WorldImportController : GenericController
    {
        private IDataService data { get; set; }

        public WorldImportController(IDataService data)
        {
            this.data = data;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Import([FromBody] WorldImportRequest request)
        {
            await data.StartUsingWorldContext(new Models.Hub.Worlds.WorldInfo(request.World));

            data.Context.RemoveRange(data.Context.DataPointType.ToList());
            data.Context.RemoveRange(data.Context.DataPointTypeParameter.ToList());
            data.Context.RemoveRange(data.Context.DataPoint.ToList());
            data.Context.RemoveRange(data.Context.DataPointParameter.ToList());
            data.Context.RemoveRange(data.Context.Map.ToList());
            data.Context.RemoveRange(data.Context.MapLayer.ToList());
            data.Context.RemoveRange(data.Context.LocationType.ToList());
            data.Context.RemoveRange(data.Context.Location.ToList());
            data.Context.RemoveRange(data.Context.Region.ToList());
            data.Context.RemoveRange(data.Context.Measurable.ToList());
            data.Context.RemoveRange(data.Context.Unit.ToList());
            data.Context.RemoveRange(data.Context.Calendar.ToList());

            data.Context.SaveChanges();

            Serialize<DataPointType>("export/Stage1.json");
            Serialize<DataPointTypeParameter>("export/Stage2.json");
            SerializeDataPoints("export/Stage3.json");
            Serialize<Models.Map.Map>("export/Stage4.json");
            Serialize<MapLayer>("export/Stage5.json");
            Serialize<LocationType>("export/Stage6.json");
            Serialize<Location>("export/Stage7.json");
            Serialize<Region>("export/Stage8.json");
            Serialize<Measurable>("export/Stage9.json");
            Serialize<Unit>("export/Stage10.json");
            SerializeCalendar("export/Stage12.json");

            data.Context.SaveChanges();

            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.IconURL, (string?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.UsesIcon, (bool?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.UsesLabel, (bool?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.LabelFontSize, (string?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.LabelFontColor, (string?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.ZoomProminenceMin, (int?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.ZoomProminenceMax, (int?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.ZoomProminenceMax, (int?)null));
            data.Context.Location.ExecuteUpdate(setters => setters.SetProperty(l => l.IconSize, (int?)null));
            data.Context.LocationType.ExecuteUpdate(setters => setters.SetProperty(l => l.IconURL, (string?)null));

            data.Context.SaveChanges();

            await data.EndUsingWorldContext();

            return Ok();
        }

        private void Serialize<T>(string path)
        {
            using (StreamReader reader = new(path))
            {
                string text = reader.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<T>>(text);
                foreach (T item in items)
                {
                    data.Context.Add(item);
                }
            }
        }

        private void SerializeDataPoints(string path)
        {
            string text;
            using (StreamReader reader = new(path))
            {
                text = reader.ReadToEnd();
            }

            var items = JsonConvert.DeserializeObject<List<DataPoint>>(text);
            List<DataPointParameter> parameters = new List<DataPointParameter>();
            foreach (DataPoint item in items)
            {
                item.Parameters = item.Parameters?.Where(p => p != null).ToList();
                item.Parameters?.ForEach(p =>
                {
                    if (p.GetType() == typeof(DataPointParameterUnit))
                    {
                        ((DataPointParameterUnit)p).Unit = null;
                        ((DataPointParameterUnit)p).UnitID = null;
                    }
                });
                parameters.AddRange(item.Parameters ?? new List<DataPointParameter>());
                item.Parameters = null;

                data.Context.Add(item);
            }

            foreach (DataPointParameter item in parameters)
            {
                data.Context.Add(item);
            }
        }

        private void SerializeCalendar(string path)
        {
            string text;
            using (StreamReader reader = new(path))
            {
                text = reader.ReadToEnd();
            }

            var items = JsonConvert.DeserializeObject<List<Calendar>>(text);
            foreach (Calendar item in items)
            {
                data.Context.Add(new CalendarDataAccessWrapper(item));
            }
        }
    }
}
