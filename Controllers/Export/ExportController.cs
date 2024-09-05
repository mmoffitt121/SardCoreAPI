using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SardCoreAPI.DataAccess.Calendars;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.DataAccess.Units;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Units;
using SardCoreAPI.Utility.Files;
using SardCoreAPI.Utility.Validation;

namespace SardCoreAPI.Controllers.Export
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class ExportController
        : GenericController
    {

        [HttpPost]
        [Validate]
        public async Task<IActionResult> Export()
        {
            await Save(
                JsonConvert.SerializeObject(await new DataPointTypeDataAccess().GetDataPointTypes(new DataPointTypeSearchCriteria(), WorldInfo)),
                "stage1.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(null, WorldInfo)),
                "stage2.json"
                );

            List<DataPoint> dps = await new DataPointDataAccess().GetDataPoints(new DataPointSearchCriteria(), WorldInfo);
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (DataPoint dp in dps)
            {
                dataPoints.Add(await GetDataPoint(dp.Id));
            }
            await Save(
                JsonConvert.SerializeObject(dataPoints),
                "stage3.json"
                );

            await Save(
                JsonConvert.SerializeObject(await new MapDataAccess().GetMaps(new Models.Map.MapSearchCriteria(), WorldInfo)),
                "stage4.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new MapLayerDataAccess().GetMapLayers(new Models.Map.MapLayer.MapLayerSearchCriteria(), WorldInfo)),
                "stage5.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new LocationTypeDataAccess().GetLocationTypes(new Models.Common.PagedSearchCriteria(), WorldInfo)),
                "stage6.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new LocationDataAccess().GetLocations(new Models.Map.Location.LocationSearchCriteria(), WorldInfo)),
                "stage7.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new RegionDataAccess().GetRegions(new Models.Map.Region.RegionSearchCriteria(), WorldInfo)),
                "stage8.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new MeasurableDataAccess().GetMeasurables(new Models.Map.Region.RegionSearchCriteria(), WorldInfo)),
                "stage9.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new UnitDataAccess().GetUnits(new UnitSearchCriteria(), WorldInfo)),
                "stage10.json"
                );
            await Save(
                JsonConvert.SerializeObject(await new CalendarDataAccess().Get(new UnitSearchCriteria(), WorldInfo)),
                "stage11.json"
                );

            return Ok();
        }

        public async Task<DataPoint> GetDataPoint([FromQuery] int? Id)
        {

            // Get Data Point
            DataPoint? result = (await new DataPointDataAccess().GetDataPoints(new DataPointSearchCriteria() { Id = Id }, WorldInfo)).FirstOrDefault();


            // Attach Type
            DataPointType? type = (await new DataPointTypeDataAccess().GetDataPointTypes(new DataPointTypeSearchCriteria() { Id = result.TypeId }, WorldInfo)).FirstOrDefault();


            // Attach Type Parameters
            List<DataPointTypeParameter> parameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(type.Id, WorldInfo)).ToList();
            type.TypeParameters = parameters;


            // Attach Parameters
            result.Parameters = new List<DataPointParameter>();
            DataPointParameterDataAccess dataAccess = new DataPointParameterDataAccess();
            foreach (DataPointTypeParameter tp in type.TypeParameters)
            {
                if (tp.Id == null || result.Id == null) { continue; }
                DataPointParameter p;

                switch (tp.TypeValue)
                {
                    case "int":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterInt>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "dub":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDouble>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "str":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterString>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "sum":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterSummary>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "doc":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDocument>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "dat":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDataPoint>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "bit":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterBoolean>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "uni":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterUnit>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        UnitSearchCriteria unitSearch = new UnitSearchCriteria() { Id = tp.DataPointTypeReferenceId };
                        List<Unit> unitList = await new UnitDataAccess().GetUnits(unitSearch, WorldInfo);
                        if (unitList.Count() < 1 || result.Parameters.Last() == null) break;
                        ((DataPointParameterUnit)result.Parameters.Last()).Unit = unitList?.First();

                        break;
                    case "tim":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterTime>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    default:
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameter>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                }
            }

            return result;
        }

        private async Task Save(string content, string file)
        {

            string docPath = "./export";

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, file)))
            {
                outputFile.WriteLine(content);
            }
        }
    }
}
