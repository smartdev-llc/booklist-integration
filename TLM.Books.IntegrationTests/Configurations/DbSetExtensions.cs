using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TLM.Books.IntegrationTests.Configurations;

public static class DbSetExtensions
{
    public static IEnumerable<T> AddRangeFromFile<T>(this DbSet<T> dbSet, string filePath) where T : class
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Cannot found file for seed data", filePath);
        }

        var fileContent = File.ReadAllText(filePath);

        var entityList = JsonConvert.DeserializeObject<List<T>>(fileContent, new JsonSerializerSettings()
        {
            ContractResolver = new PrivateResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        });

        dbSet.AddRange(entityList);

        return entityList;
    }
}

public class PrivateResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);
        if (!prop.Writable)
        {
            var property = member as PropertyInfo;
            if (property?.GetSetMethod(true) != null)
            {
                prop.Writable = true;
            }
        }

        return prop;
    }
}