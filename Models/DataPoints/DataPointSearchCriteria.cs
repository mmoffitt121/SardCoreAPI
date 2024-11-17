using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using System.ComponentModel;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointSearchCriteria : PagedSearchCriteria
    {
        public int? TypeId { get; set; }
        public List<int>? DataPointIds { get; set; }
        public List<int>? TypeIds { get; set; }
        public List<int>? LocationIds { get; set; }
        public List<DataPointParameter>? Parameters { get; set; }
        public List<ParameterReturnOptions>? ParameterReturnOptions { get; set; }
        public List<ParameterSearchOptions>? ParameterSearchOptions { get; set; }
        public DataPointTypeParameter? OrderByTypeParam { get; set; }
        public bool? OrderByTypeParamDesc { get; set; }
        public List<SearchBinCriteria>? SearchBinCriteria { get; set; }
        public int? OrderByBin { get; set; }

        public bool? IncludeTypes { get; set; }
        public bool? IncludeChildDataPoints { get; set; }
        public bool? IncludeRelevantDataPoints { get; set; }
        public bool? IncludeParameters { get; set; } = true;
        public bool? IncludeChildParameters { get; set; }
        public bool? IncludeRelevantLocations { get; set; }

        public override List<string> Validate()
        {
            var errors = new List<string>();
            ParameterSearchOptions?.ForEach(option =>
            {
                var param = Parameters?.FirstOrDefault(p => p.DataPointTypeParameterId == option.DataPointTypeParameterId);
                if (param == null) 
                {
                    errors.Add($"No parameter specified for Search Option with type parameter id {option.DataPointTypeParameterId}.");
                    return;
                }

                if (param.GetType() == typeof(DataPointParameterBoolean)) 
                { 
                    switch (option.FilterMode) {
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.Contains:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.StartsWith:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.EndsWith:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.GreaterThan:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.LessThan:
                            errors.Add($"Filter mode {Enum.GetName(option.FilterMode)} not supported for type paramater {param.DataPointTypeParameterId}");
                            break;
                    }
                }
                else if (param.GetType() == typeof(DataPointParameterDataPoint))
                {
                    switch (option.FilterMode)
                    {
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.Contains:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.StartsWith:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.EndsWith:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.GreaterThan:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.LessThan:
                            errors.Add($"Filter mode {Enum.GetName(option.FilterMode)} not supported for type paramater {param.DataPointTypeParameterId}");
                            break;
                    }
                }
                else if (param.GetType() == typeof(DataPointParameterDocument))
                {
                    errors.Add($"Searching over article parameters is not currently supported.");
                }
                else if (param.GetType() == typeof(DataPointParameterDouble))
                {
                    switch (option.FilterMode)
                    {
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.Equals:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.GreaterThan:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.LessThan:
                            break;
                        default:
                            errors.Add($"Filter mode {Enum.GetName(option.FilterMode)} not supported for type paramater {param.DataPointTypeParameterId}");
                            break;
                    }
                }
                else if (param.GetType() == typeof(DataPointParameterInt))
                {
                    switch (option.FilterMode)
                    {
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.Equals:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.GreaterThan:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.LessThan:
                            break;
                        default:
                            errors.Add($"Filter mode {Enum.GetName(option.FilterMode)} not supported for type paramater {param.DataPointTypeParameterId}");
                            break;
                    }
                }
                else if (param.GetType() == typeof(DataPointParameterString))
                {

                }
                else if (param.GetType() == typeof(DataPointParameterSummary))
                {

                }
                else if (param.GetType() == typeof(DataPointParameterUnit))
                {
                    switch (option.FilterMode)
                    {
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.Equals:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.GreaterThan:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.LessThan:
                            break;
                        default:
                            errors.Add($"Filter mode {Enum.GetName(option.FilterMode)} not supported for type paramater {param.DataPointTypeParameterId}");
                            break;
                    }
                }
                else if (param.GetType() == typeof(DataPointParameterTime))
                {
                    switch (option.FilterMode)
                    {
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.Equals:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.GreaterThan:
                        case DataPoints.ParameterSearchOptions.FilterModeEnum.LessThan:
                            break;
                        default:
                            errors.Add($"Filter mode {Enum.GetName(option.FilterMode)} not supported for type paramater {param.DataPointTypeParameterId}");
                            break;
                    }
                }
                else
                {
                    errors.Add($"Parameter with id {param.DataPointTypeParameterId} was not a valid parameter type.");
                }
            });

            errors.AddRange(base.Validate());
            return errors;
        }
    }
}
