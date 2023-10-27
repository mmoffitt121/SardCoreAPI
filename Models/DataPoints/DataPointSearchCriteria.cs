using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointSearchCriteria : PagedSearchCriteria
    {
        public int? TypeId { get; set; }
        public List<int>? TypeIds { get; set; }
        public List<DataPointParameter>? Parameters { get; set; }

        internal void SetParam(int index, string value)
        {
            switch (index)
            {
                case 0:
                    Param0 = value;
                    break;
                case 1:
                    Param1 = value;
                    break;
                case 2:
                    Param2 = value;
                    break;
                case 3:
                    Param3 = value;
                    break;
                case 4:
                    Param4 = value;
                    break;
                case 5: 
                    Param5 = value;
                    break;
                case 6:
                    Param6 = value;
                    break;
                case 7:
                    Param7 = value;
                    break;
                case 8:
                    Param8 = value;
                    break;
                case 9:
                    Param9 = value;
                    break;
                case 10:
                    Param10 = value;
                    break;
                case 11:
                    Param11 = value;
                    break;
                case 12:
                    Param12 = value;
                    break;
                case 13:
                    Param13 = value;
                    break;
                case 14:
                    Param14 = value;
                    break;
                case 15:
                    Param15 = value;
                    break;
                case 16:
                    Param16 = value;
                    break;
                case 17:
                    Param17 = value;
                    break;
                case 18:
                    Param18 = value;
                    break;
                case 19:
                    Param19 = value;
                    break;
                default:
                    break;
            }
        }

        #region Parameters for SQL
        [JsonIgnore]
        public string? Param0 { get; set; }
        [JsonIgnore]
        public string? Param1 { get; set; }
        [JsonIgnore]
        public string? Param2 { get; set; }
        [JsonIgnore]
        internal string? Param3 { get; set; }
        [JsonIgnore]
        internal string? Param4 { get; set; }
        [JsonIgnore]
        internal string? Param5 { get; set; }
        [JsonIgnore]
        internal string? Param6 { get; set; }
        [JsonIgnore]
        internal string? Param7 { get; set; }
        [JsonIgnore]
        internal string? Param8 { get; set; }
        [JsonIgnore]
        internal string? Param9 { get; set; }
        [JsonIgnore]
        internal string? Param10 { get; set; }
        [JsonIgnore]
        internal string? Param11 { get; set; }
        [JsonIgnore]
        internal string? Param12 { get; set; }
        [JsonIgnore]
        internal string? Param13 { get; set; }
        [JsonIgnore]
        internal string? Param14 { get; set; }
        [JsonIgnore]
        internal string? Param15 { get; set; }
        [JsonIgnore]
        internal string? Param16 { get; set; }
        [JsonIgnore]
        internal string? Param17 { get; set; }
        [JsonIgnore]
        internal string? Param18 { get; set; }
        [JsonIgnore]
        internal string Param19 { get; set; }
        #endregion
    }
}
