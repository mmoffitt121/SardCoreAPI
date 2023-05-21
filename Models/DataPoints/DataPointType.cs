﻿using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<DataPointTypeParameter> TypeParameters { get; set; }
    }
}
