namespace SardCoreAPI
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return @"Server=localhost\SQLEXPRESS;Database=LibrariesOfSard;Trusted_Connection=True;Encrypt=False;";
        }
    }
}
