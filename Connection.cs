using System.Configuration;

namespace SardCoreAPI
{
    public class Connection
    {
        private static string? _connectionString;
        private static string? _globalConnectionString;
        public static string GetConnectionString()
        {
            return _connectionString + "Database=LibrariesOfSard;";
        }

        public static string GetConnectionString(string? databaseName)
        {
            if (databaseName == null)
            {
                throw new ArgumentNullException("World Location");
            }
            return _connectionString + "Database=" + databaseName + ";";
        }

        public static string GetGlobalConnectionString()
        {
            return _globalConnectionString;
        }

        public static string GetBaseConnectionString()
        {
            return _connectionString;
        }

        public static void SetGlobalConnectionString(string toSet)
        {
            if (_globalConnectionString != null) { return;  }
            _globalConnectionString = toSet;
        }

        public static void SetConnectionString(string toSet)
        {
            if (_connectionString != null) { return; }
            _connectionString = toSet;
        }
    }
}
