namespace SardCoreAPI
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return "Server=localhost;Port=3306;Uid=root;Pwd=123;Database=LibrariesOfSard;";
        }

        public static string GetConnectionString(string databaseName)
        {
            return "Server=localhost;Port=3306;Uid=root;Pwd=123;Database=" + databaseName + ";";
        }
    }
}
