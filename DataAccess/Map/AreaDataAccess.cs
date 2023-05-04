using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.Area;
using SardCoreAPI.Models.Map.Location;

namespace SardCoreAPI.DataAccess.Map
{
    public class AreaDataAccess
    {
        #region Area
        public List<Area> GetAreas(string? query)
        {
            string sql = @"SELECT * FROM Areas 
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<Area> areas = connection.Query<Area>(sql, new { Name = query }).ToList();
                    return areas;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public Area GetArea(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    a.Id as Id, a.Name as Name, a.Longitude, a.Latitude,
                    sr.Id as SubregionId, sr.Name as SubregionName,
                    r.Id as RegionId, r.Name as RegionName,
                    sc.Id as SubcontinentId, sc.Name as SubcontinentName,
                    c.Id as ContinentId, c.Name as ContinentName,
                    cb.Id as CelestialObjectId, cb.Name as CelestialObjectName,
                    cs.Id as CelestialSystemId, cs.Name as CelestialSystemName,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM Areas a
                    LEFT JOIN Subregions sr on sr.Id = a.SubregionId
                    LEFT JOIN Regions r on r.id = sr.RegionId
                    LEFT JOIN Subcontinents sc on sc.id = r.SubcontinentId
                    LEFT JOIN Continents c on c.id = sc.ContinentId
                    LEFT JOIN CelestialObjects cb on cb.id = c.CelestialObjectId
                    LEFT JOIN CelestialSystems cs on cs.id = cb.CelestialSystemId
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("a.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<Area>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostArea(Area area)
        {
            string sql = @"INSERT INTO Areas (Name, SubregionId, Longitude, Latitude) 
                VALUES (@Name, @SubregionId, @Longitude, @Latitude)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, area) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion

        #region Subregion
        public List<Subregion> GetSubregions(string? query)
        {
            string sql = @"SELECT * FROM Subregions 
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<Subregion> result = connection.Query<Subregion>(sql, new { Name = query }).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public Subregion GetSubregion(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    sr.Id as Id, sr.Name as Name, sr.Longitude, sr.Latitude,
                    r.Id as RegionId, r.Name as RegionName,
                    sc.Id as SubcontinentId, sc.Name as SubcontinentName,
                    c.Id as ContinentId, c.Name as ContinentName,
                    cb.Id as CelestialObjectId, cb.Name as CelestialObjectName,
                    cs.Id as CelestialSystemId, cs.Name as CelestialSystemName,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM Subregions sr
                    LEFT JOIN Regions r on r.id = sr.RegionId
                    LEFT JOIN Subcontinents sc on sc.id = r.SubcontinentId
                    LEFT JOIN Continents c on c.id = sc.ContinentId
                    LEFT JOIN CelestialObjects cb on cb.id = c.CelestialObjectId
                    LEFT JOIN CelestialSystems cs on cs.id = cb.CelestialSystemId
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("sr.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<Subregion>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostSubregion(Subregion data)
        {
            string sql = @"INSERT INTO Subregions (Name, RegionId, Longitude, Latitude) 
                VALUES (@Name, @RegionId, @Longitude, @Latitude)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion

        #region Region
        public List<Region> GetRegions(string? query)
        {
            string sql = @"SELECT * FROM Regions 
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<Region> result = connection.Query<Region>(sql, new { Name = query }).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public Region GetRegion(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    r.Id as Id, r.Name as Name, r.Longitude, r.Latitude,
                    sc.Id as SubcontinentId, sc.Name as SubcontinentName,
                    c.Id as ContinentId, c.Name as ContinentName,
                    cb.Id as CelestialObjectId, cb.Name as CelestialObjectName,
                    cs.Id as CelestialSystemId, cs.Name as CelestialSystemName,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM Regions r
                    LEFT JOIN Subcontinents sc on sc.id = r.SubcontinentId
                    LEFT JOIN Continents c on c.id = sc.ContinentId
                    LEFT JOIN CelestialObjects cb on cb.id = c.CelestialObjectId
                    LEFT JOIN CelestialSystems cs on cs.id = cb.CelestialSystemId
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("r.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<Region>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostRegion(Region data)
        {
            string sql = @"INSERT INTO Regions (Name, SubcontinentId, Longitude, Latitude) 
                VALUES (@Name, @SubcontinentId, @Longitude, @Latitude)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion

        #region Subcontinent
        public List<Subcontinent> GetSubcontinents(string? query)
        {
            string sql = @"SELECT * FROM Subcontinents 
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<Subcontinent> result = connection.Query<Subcontinent>(sql, new { Name = query }).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public Subcontinent GetSubcontinent(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    sc.Id as Id, sc.Name as Name, sc.Longitude, sc.Latitude,
                    c.Id as ContinentId, c.Name as ContinentName,
                    cb.Id as CelestialObjectId, cb.Name as CelestialObjectName,
                    cs.Id as CelestialSystemId, cs.Name as CelestialSystemName,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM Subcontinents sc
                    LEFT JOIN Continents c on c.id = sc.ContinentId
                    LEFT JOIN CelestialObjects cb on cb.id = c.CelestialObjectId
                    LEFT JOIN CelestialSystems cs on cs.id = cb.CelestialSystemId
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("sc.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<Subcontinent>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostSubcontinent(Subcontinent data)
        {
            string sql = @"INSERT INTO Subcontinents (Name, ContinentId, Longitude, Latitude) 
                VALUES (@Name, @ContinentId, @Longitude, @Latitude)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion

        #region Continent
        public List<Continent> GetContinents(string? query)
        {
            string sql = @"SELECT * FROM Continents 
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<Continent> result = connection.Query<Continent>(sql, new { Name = query }).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public Continent GetContinent(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    c.Id as Id, c.Name as Name, c.Longitude, c.Latitude,
                    cb.Id as CelestialObjectId, cb.Name as CelestialObjectName,
                    cs.Id as CelestialSystemId, cs.Name as CelestialSystemName,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM Continents c
                    LEFT JOIN CelestialObjects cb on cb.id = c.CelestialObjectId
                    LEFT JOIN CelestialSystems cs on cs.id = cb.CelestialSystemId
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("c.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<Continent>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostContinent(Continent data)
        {
            string sql = @"INSERT INTO Continents (Name, CelestialObjectId, Longitude, Latitude, Aquatic) 
                VALUES (@Name, @CelestialObjectId, @Longitude, @Latitude, @Aquatic)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion

        #region Celestial Object
        public List<CelestialObject> GetCelestialObjects(string? query)
        {
            string sql = @"SELECT * FROM CelestialObjects
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<CelestialObject> result = connection.Query<CelestialObject>(sql, new { Name = query }).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public CelestialObject GetCelestialObject(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    cb.Id as CelestialObjectId, cb.Name as Name,
                    cs.Id as CelestialSystemId, cs.Name as CelestialSystemName,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM CelestialObjects cb
                    LEFT JOIN CelestialSystems cs on cs.id = cb.CelestialSystemId
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("cb.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<CelestialObject>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostCelestialObject(CelestialObject data)
        {
            string sql = @"INSERT INTO CelestialObjects (Name, CelestialSystemId) 
                VALUES (@Name, @CelestialSystemId)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion

        #region Celestial System
        public List<CelestialSystem> GetCelestialSystems(string? query)
        {
            string sql = @"SELECT * FROM CelestialSystems
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<CelestialSystem> result = connection.Query<CelestialSystem>(sql, new { Name = query }).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public CelestialSystem GetCelestialSystem(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    cs.Id as CelestialSystemId, cs.Name as Name,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM CelestialSystems cs
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("cs.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<CelestialSystem>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostCelestialSystem(CelestialSystem data)
        {
            string sql = @"INSERT INTO CelestialSystems (Name, ManifoldId) 
                VALUES (@Name, @ManifoldId)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion

        #region Manifold
        public List<Manifold> GetManifolds(string? query)
        {
            string sql = @"SELECT * FROM Manifolds
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<Manifold> result = connection.Query<Manifold>(sql, new { Name = query }).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public Manifold GetManifold(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                    m.Id as ManifoldId, m.Name as Name
                FROM Manifolds m
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("m.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        var data = connection.QueryFirst<Manifold>(template.RawSql, new { Id });
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostManifold(Manifold data)
        {
            string sql = @"INSERT INTO Manifolds (Name, Number) 
                VALUES (@Name, @Number)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion
    }
}
