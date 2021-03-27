using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lecture.Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public static class DbConnectionExtensions
    {
        public static List<T> Query<T>(
            this DbConnection connection,
            string query,
            object parameters = null)
        {
            var command = connection.CreateCommand();

            command.CommandText = query;

            if (parameters is not null)
            {
                var pType = parameters.GetType();

                foreach (var property in pType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var parameter = command.CreateParameter();

                    parameter.ParameterName = property.Name;
                    parameter.Value = property.GetValue(parameters);

                    command.Parameters.Add(parameter);
                }
            }

            var reader = command.ExecuteReader();

            var ctor = typeof(T).GetConstructor(Array.Empty<Type>());

            List<T> results = new();

            while (reader.Read())
            {
                var entity = (T)ctor.Invoke(Array.Empty<object>());

                var properties = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    string name;

                    if (property.GetCustomAttribute<ColumnNameAttribute>() is { } attr)
                    {
                        name = attr.Name;
                    }
                    else
                    {
                        name = property.Name;
                    }

                    var value = reader[name];

                    if (value == DBNull.Value)
                    {
                        property.SetValue(entity, null);
                    }
                    else
                    {
                        property.SetValue(entity, value);
                    }
                }

                results.Add(entity);
            }

            return results;
        }
    }
}
