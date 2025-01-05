using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using ChatBox.Constant;
using ChatBox.Models;
using ChatBox.ViewModels;
using CommunityToolkit.Mvvm.Collections;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ChatBox.DataAccess;

public class DbContext : IDisposable
{
    private readonly SqliteConnection _connection;

    [DapperAot]
    public DbContext()
    {
        var path = new FileInfo(Path.Combine(ConstantPath.ChatDbPath));

        if (path.Directory?.Exists == false)
        {
            path.Directory.Create();
        }

        _connection = new SqliteConnection($"Data Source={path.FullName}");
        _connection.Open();

        EnsureTableIsCreated();
    }

    public SqliteConnection GetConnection()
    {
        return _connection;
    }

    public async Task<int> InsertAsync<T>(T entity)
    {
        var type = typeof(T);
        var properties = type.GetProperties().Where(p => p.CanWrite);
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        await using var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO {type.Name}  ({columns}) VALUES ({values});";

        foreach (var property in properties)
        {
            // 如果是ObservableGroupedCollection
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableGroupedCollection<,>))
            {
                command.Parameters.AddWithValue($"@{property.Name}",
                    JsonSerializer.Serialize(property.GetValue(entity), AppJsonSerialize.SerializerOptions));
                continue;
            }

            // 如果类型是class
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string) &&
                property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                command.Parameters.AddWithValue($"@{property.Name}",
                    JsonSerializer.Serialize(property.GetValue(entity), AppJsonSerialize.SerializerOptions));
                continue;
            }

            // DateTime 类型
            if (property.PropertyType == typeof(DateTime))
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity).ToString());
                continue;
            }

            if (property.PropertyType == typeof(DateTime?))
            {
                command.Parameters.AddWithValue($"@{property.Name}",
                    string.IsNullOrEmpty(property.GetValue(entity)?.ToString())
                        ? ""
                        : property.GetValue(entity).ToString());
                continue;
            }

            command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
        }

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> UpdateAsync<T>(T entity)
    {
        var type = typeof(T);
        var properties = type.GetProperties().Where(p => p.CanWrite);
        var columns = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        await using var command = _connection.CreateCommand();
        command.CommandText = $"UPDATE {type.Name} SET {columns} WHERE Id = @Id;";

        foreach (var property in properties)
        {
            // 如果是ObservableGroupedCollection
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableGroupedCollection<,>))
            {
                command.Parameters.AddWithValue($"@{property.Name}",
                    JsonSerializer.Serialize(property.GetValue(entity), AppJsonSerialize.SerializerOptions));
                continue;
            }

            // 如果类型是class
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string) &&
                property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                command.Parameters.AddWithValue($"@{property.Name}",
                    JsonSerializer.Serialize(property.GetValue(entity), AppJsonSerialize.SerializerOptions));
                continue;
            }

            // DateTime 类型
            if (property.PropertyType == typeof(DateTime))
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity).ToString());
                continue;
            }

            if (property.PropertyType == typeof(DateTime?))
            {
                command.Parameters.AddWithValue($"@{property.Name}",
                    string.IsNullOrEmpty(property.GetValue(entity)?.ToString())
                        ? ""
                        : property.GetValue(entity).ToString());
                continue;
            }

            command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
        }

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> DeleteAsync<T>(string id)
    {
        await using var command = _connection.CreateCommand();
        command.CommandText = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id;";
        command.Parameters.AddWithValue("@Id", id);

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
    {
        var type = typeof(T);

        var properties = type.GetProperties().Where(p => p.CanWrite);

        await using var command = _connection.CreateCommand();
        command.CommandText = sql;
        var result = new List<T>();
        await using var reader = await command.ExecuteReaderAsync();


        while (await reader.ReadAsync())
        {
            var ctor = typeof(T).GetConstructors().FirstOrDefault();
            if (ctor == null)
            {
                throw new InvalidOperationException($"{typeof(T).Name} 未找到构造函数！");
            }

            var parameters = ctor.GetParameters().Select(p =>
            {
                var dbValue = reader[p.Name];
                return dbValue == DBNull.Value ? null : Convert.ChangeType(dbValue, p.ParameterType);
            }).ToArray();

            var entity = (T)ctor.Invoke(parameters);

            // 填充属性
            // 填充属性
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableGroupedCollection<,>))
                {
                    property.SetValue(entity,
                        JsonSerializer.Deserialize(reader[property.Name].ToString(), property.PropertyType,
                            AppJsonSerialize.SerializerOptions));
                    continue;
                }

                if (property.PropertyType.IsClass && property.PropertyType != typeof(string) &&
                    property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
                {
                    property.SetValue(entity,
                        JsonSerializer.Deserialize(reader[property.Name].ToString(), property.PropertyType,
                            AppJsonSerialize.SerializerOptions));
                    continue;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    property.SetValue(entity, DateTime.Parse(reader[property.Name].ToString()));
                    continue;
                }

                if (property.PropertyType == typeof(DateTime?))
                {
                    property.SetValue(entity, string.IsNullOrEmpty(reader[property.Name].ToString())
                        ? null
                        : DateTime.Parse(reader[property.Name].ToString()));
                    continue;
                }

                property.SetValue(entity, reader[property.Name]);
            }

            result.Add(entity);
        }

        return result;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(Expression<Func<T, bool>> predicate)
    {
        var type = typeof(T);
        var properties = type.GetProperties().Where(p => p.CanWrite);
        var columns = string.Join(", ", properties.Select(p => p.Name));

        await using var command = _connection.CreateCommand();
        command.CommandText =
            $"SELECT {columns} FROM {type.Name} as {predicate.Parameters.First().Name} WHERE {predicate.Body.ToString().TrimStart('(').TrimEnd(')')};";

        var result = new List<T>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var ctor = typeof(T).GetConstructors().FirstOrDefault();
            if (ctor == null)
            {
                throw new InvalidOperationException($"{typeof(T).Name} 未找到构造函数！");
            }

            var parameters = ctor.GetParameters().Select(p =>
            {
                var dbValue = reader[p.Name];
                return dbValue == DBNull.Value ? null : Convert.ChangeType(dbValue, p.ParameterType);
            }).ToArray();

            var entity = (T)ctor.Invoke(parameters);

            // 填充属性
            // 填充属性
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableGroupedCollection<,>))
                {
                    property.SetValue(entity,
                        JsonSerializer.Deserialize(reader[property.Name].ToString(), property.PropertyType,
                            AppJsonSerialize.SerializerOptions));
                    continue;
                }

                if (property.PropertyType.IsClass && property.PropertyType != typeof(string) &&
                    property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
                {
                    property.SetValue(entity,
                        JsonSerializer.Deserialize(reader[property.Name].ToString(), property.PropertyType,
                            AppJsonSerialize.SerializerOptions));
                    continue;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    property.SetValue(entity, DateTime.Parse(reader[property.Name].ToString()));
                    continue;
                }

                if (property.PropertyType == typeof(DateTime?))
                {
                    property.SetValue(entity, string.IsNullOrEmpty(reader[property.Name].ToString())
                        ? null
                        : DateTime.Parse(reader[property.Name].ToString()));
                    continue;
                }
                
                if (property.PropertyType == typeof(long))
                {
                    property.SetValue(entity, long.Parse(reader[property.Name].ToString()));
                    continue;
                }
                
                property.SetValue(entity, reader[property.Name]);
            }

            result.Add(entity);
        }

        return result;
    }

    public bool EnsureTableIsCreated()
    {
        // 创建 ChatMessage 表
        using var command = _connection.CreateCommand();

        command.CommandText = $@"
            CREATE TABLE IF NOT EXISTS {nameof(ChatMessage)} (
                {GetColumns<ChatMessage>()}
            );

            CREATE TABLE IF NOT EXISTS {nameof(Session)} (
                {GetColumns<Session>()}
            )    
        ";
        command.ExecuteNonQuery();

        return true;
    }

    /// <summary>
    /// 将一个泛型的所有属性转换为一个字符串，并且根据属性的类型进行转换sqlite的类型
    /// </summary>
    /// <returns></returns>
    public string GetColumns<T>()
    {
        var type = typeof(T);
        var properties = type.GetProperties().Where(p => p.CanWrite);
        var columns = string.Join(", " + Environment.NewLine, properties.Select(p =>
        {
            if (p.Name == "Id")
            {
                return $"{p.Name} TEXT PRIMARY KEY";
            }

            return $"{p.Name} {p.PropertyType.Name switch
            {
                "String" => "TEXT",
                "Int32" => "INTEGER",
                "DateTime" => "TEXT",
                _ => "TEXT"
            }}";
        }));
        return columns;
    }


    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}