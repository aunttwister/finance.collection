using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListPropertiesToFile
{
    public static class PropertyListerCustom
    {
        private static readonly HashSet<Type> _primitiveTypes = new HashSet<Type>
        {
            typeof(string),
            typeof(decimal),
            typeof(bool),
            typeof(Boolean),
            typeof(Decimal),
            typeof(MethodBase),
            typeof(Object),
            typeof(IDictionary),
            typeof(ICollection),
            typeof(Exception),
            typeof(Int64),
            typeof(Guid),
            typeof(Type)
        };

        public static void ListPropertiesToFile(Type type, string filePath)
        {
            var lines = GetProperties(type);
            File.WriteAllLines(filePath, lines);
        }

        public static IEnumerable<string> GetProperties(Type type, string prefix = "", HashSet<Type> visited = null)
        {
            // Initialize the set of visited types if not already provided
            visited = visited ?? new HashSet<Type>();

            // Prevent infinite recursion
            if (visited.Contains(type))
            {
                return Enumerable.Empty<string>();
            }
            visited.Add(type);

            // Handle primitive and special types
            if (IsPrimitiveOrSpecialType(type))
            {
                return Enumerable.Empty<string>();
            }

            // Get properties
            PropertyInfo[] properties = type.GetProperties();
            List<string> lines = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Type propType = property.PropertyType;
                string line = $"{prefix}{property.Name} ({propType.Name})";
                lines.Add(line);

                // Correctly identifying generic dictionaries
                if (propType.IsGenericType && propType.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    || propType.Name.Equals("IDictionary`2"))
                {
                    Type[] args = propType.GetGenericArguments();
                    line = $"{prefix}\t{property.Name} (Dictionary of KeyValuePairs<{args[0].Name}, {args[1].Name}>)";
                    lines.Add(line);
                    // Recursively get properties for key and value types if they are complex
                    lines.AddRange(GetProperties(args[0], prefix + "\t\tKey: ", new HashSet<Type>(visited)));
                    lines.AddRange(GetProperties(args[1], prefix + "\t\tValue: ", new HashSet<Type>(visited)));
                }
                else if (propType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
                {
                    Type itemType = propType.GetGenericArguments()[0];
                    line = $"{prefix}\t{property.Name} (Collection of {itemType.Name})";
                    lines.Add(line);
                    if (!IsPrimitiveOrSpecialType(itemType))
                    {
                        lines.AddRange(GetProperties(itemType, prefix + "\t\t", new HashSet<Type>(visited)));
                    }
                }
                else if (!IsPrimitiveOrSpecialType(propType))
                {
                    lines.AddRange(GetProperties(propType, prefix + "\t", new HashSet<Type>(visited)));
                }
            }

            return lines;
        }

        private static bool IsComplexType(Type type)
        {
            return !type.IsValueType && !type.IsPrimitive && type != typeof(string) && !type.IsEnum && type != typeof(DateTime);
        }
        private static bool IsPrimitiveOrSpecialType(Type type)
        {
            return type.IsPrimitive || _primitiveTypes.Contains(type);
        }
    }
}
