using Dapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using SardCoreAPI.Attributes.Easy;
using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Easy;
using System.Reflection;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SardCoreAPI.Services.Easy
{
    public interface IEasyQueryService
    {
        public string BuildGet<T>(object? query, QueryOptions? queryOptions = null, bool count = false);
        public string BuildPost<T>();
        public string BuildPut<T>(bool insert);
        public string BuildDelete<T>(object? key);
    }

    public class MySQLEasyQueryService : IEasyQueryService
    {
        public string BuildGet<T>(object? query, QueryOptions? queryOptions = null, bool count = false)
        {
            PropertyInfo[] queryProperties = query?.GetType().GetProperties() ?? Array.Empty<PropertyInfo>();
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T));

            // Get Table
            string tableName = "";
            foreach (Attribute attribute in attributes)
            {
                if (attribute is TableAttribute)
                {
                    tableName = ((TableAttribute)attribute).Name;
                }
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new InvalidOperationException($@"Table does not exist on type {typeof(T).Name}");
            }

            string pageSettings = "";
            if (queryOptions?.PageNumber != null && queryOptions.PageSize != null)
            {
                pageSettings = $"LIMIT {queryOptions.PageSize} OFFSET {queryOptions.PageNumber * queryOptions.PageSize}";
            }

            string orderBy = "";
            if (queryOptions?.OrderBy != null)
            {
                PropertyInfo? orderByProperty = typeof(T).GetProperty(queryOptions.OrderBy);
                if (orderByProperty != null)
                {
                    orderBy = @$"ORDER BY {GetColumnName(orderByProperty)} {(queryOptions.Descending ?? false ? "DESC" : "ASC")}";
                }
            }

            // Build Query
            string sql = @$"SELECT {(count ? "COUNT(*)" : "*")} FROM {tableName} /**where**/
                {orderBy}
                {pageSettings}";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            foreach (PropertyInfo p in queryProperties)
            {
                if (p.GetValue(query) != null)
                {
                    if (typeof(List<string>).Equals(p.PropertyType) || typeof(List<int>).Equals(p.PropertyType))
                    {
                        builder.Where($"{GetColumnName(p)} IN @{p.Name}");
                    }
                    else if (typeof(string).Equals(p.PropertyType))
                    {
                        builder.Where($"{GetColumnName(p)} LIKE @{p.Name}");
                    }
                    else
                    {
                        builder.Where($"{GetColumnName(p)} = @{p.Name}");
                    }
                }
                
            }

            return template.RawSql;
        }

        public string BuildPost<T>()
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T));

            // Get Table
            string tableName = "";
            foreach (Attribute attribute in attributes)
            {
                if (attribute is TableAttribute)
                {
                    tableName = ((TableAttribute)attribute).Name;
                }
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new InvalidOperationException($@"Table does not exist on type {typeof(T).Name}");
            }

            // Build Insert
            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            // Get Columns from Properties
            foreach (PropertyInfo prop in typeProperties)
            {
                ColumnAttribute? attribute = prop.GetCustomAttribute<ColumnAttribute>();
                if (attribute != null)
                {
                    columns.Add($"{GetColumnName(prop)}");
                    values.Add($"@{prop.Name}");
                }
            }

            return @$"INSERT INTO {tableName} ({string.Join(",", columns.ToArray())}) VALUES ({string.Join(",", values.ToArray())}); SELECT LAST_INSERT_ID();";
        }

        public string BuildPut<T>(bool insert)
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T));

            // Get Table
            string tableName = "";
            foreach (Attribute attribute in attributes)
            {
                if (attribute is TableAttribute)
                {
                    tableName = ((TableAttribute)attribute).Name;
                }
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new InvalidOperationException($@"Table does not exist on type {typeof(T).Name}");
            }

            if (insert)
            {
                // Build Insert
                List<string> columns = new List<string>();
                List<string> values = new List<string>();

                // Get Columns from Properties
                foreach (PropertyInfo prop in typeProperties)
                {
                    ColumnAttribute? attribute = prop.GetCustomAttribute<ColumnAttribute>();
                    if (attribute != null)
                    {
                        columns.Add($"{(!string.IsNullOrEmpty(attribute.Name) ? attribute.Name : prop.Name)}");
                        values.Add($"@{prop.Name}");
                    }
                }

                // Build Update
                List<string> updates = new List<string>();

                // Get Columns from Properties
                foreach (PropertyInfo prop in typeProperties)
                {
                    ColumnAttribute? attribute = prop.GetCustomAttribute<ColumnAttribute>();
                    if (attribute != null)
                    {
                        updates.Add($"{GetColumnName(prop)} = @{prop.Name}");
                    }
                }

                return @$"INSERT INTO {tableName} ({string.Join(",", columns)}) VALUES ({string.Join(",", values)})
                                ON DUPLICATE KEY UPDATE {string.Join(",", updates)}";
            }
            else
            {
                // Build Update
                List<string> updates = new List<string>();
                List<string> primaryKeys = new List<string>();

                // Get Columns from Properties
                foreach (PropertyInfo prop in typeProperties)
                {
                    ColumnAttribute? attribute = prop.GetCustomAttribute<ColumnAttribute>();
                    if (attribute != null)
                    {
                        if (attribute.PrimaryKey)
                        {
                            primaryKeys.Add($"{GetColumnName(prop)} = @{prop.Name}");
                        }
                        else
                        {
                            updates.Add($"{GetColumnName(prop)} = @{prop.Name}");
                        }
                    }
                }

                return @$"UPDATE {tableName} SET {string.Join(",", updates)} {(primaryKeys.Count() > 0 ? "WHERE" : "")} {string.Join(" AND ", primaryKeys)}";
            }
        }

        public string BuildDelete<T>(object? key)
        {
            PropertyInfo[] queryProperties = key?.GetType().GetProperties() ?? Array.Empty<PropertyInfo>();
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T));

            // Get Table
            string tableName = "";
            foreach (Attribute attribute in attributes)
            {
                if (attribute is TableAttribute)
                {
                    tableName = ((TableAttribute)attribute).Name;
                }
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new InvalidOperationException($@"Table does not exist on type {typeof(T).Name}");
            }

            // Build Query
            string sql = @$"DELETE FROM {tableName} /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            foreach (PropertyInfo p in queryProperties)
            {
                if (p.GetValue(key) != null)
                {
                    if (typeof(string).Equals(p.PropertyType))
                    {
                        builder.Where($"{GetColumnName(p)} LIKE @{p.Name}");
                    }
                    else
                    {
                        builder.Where($"{GetColumnName(p)} = @{p.Name}");
                    }
                }

            }

            return template.RawSql;
        }

        private string GetColumnName(PropertyInfo prop)
        {
            ColumnAttribute? attribute = prop.GetCustomAttribute<ColumnAttribute>();
            if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
            {
                return attribute.Name;
            }
            else
            {
                return prop.Name;
            }
        }
    }
}
