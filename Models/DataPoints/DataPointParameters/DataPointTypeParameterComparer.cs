namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointTypeParameterComparer : IEqualityComparer<DataPointTypeParameter>
    {
        public bool Equals(DataPointTypeParameter x, DataPointTypeParameter y)
        {
            if (x.Id == null || y.Id == null) { return false; }

            return x.Id == y.Id;
        }

        public int GetHashCode(DataPointTypeParameter obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
