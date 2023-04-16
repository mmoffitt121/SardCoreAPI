namespace SardCoreAPI
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return "Server=localhost;Port=3306;Database=LibrariesOfSard;Uid=root;Pwd=123;";
            // MSSQL Connection String
            //return @"Server=localhost\SQLEXPRESS;Database=LibrariesOfSard;Trusted_Connection=True;Encrypt=False;";
        }
    }
}
