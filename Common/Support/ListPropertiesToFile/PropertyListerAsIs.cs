using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListPropertiesToFile
{
    public static class PropertyListerAsIs
    {
        public static void ListPropertiesToFile(Type type, string filePath)
        {
            var lines = GetProperties(type);
            File.WriteAllLines(filePath, lines);
        }

        private static IEnumerable<string> GetProperties(Type type, string prefix = "")
        {
            // Check if the type should be excluded from further introspection
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type.IsEnum)
                return Enumerable.Empty<string>();

            PropertyInfo[] properties = type.GetProperties();
            List<string> lines = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Type propType = property.PropertyType;
                string line = $"{prefix}{property.Name} ({property.PropertyType.Name})";
                lines.Add(line);

                // Special handling for dictionaries
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    Type[] args = propType.GetGenericArguments();
                    lines.Add($"{prefix}\t{property.Name} (Dictionary of {args[0].Name} to {args[1].Name})");
                    lines.AddRange(GetProperties(args[1], prefix + "\t\t"));
                }
                // Special handling for collections
                else if (propType.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(propType.GetGenericTypeDefinition()))
                {
                    Type itemType = propType.GetGenericArguments()[0];
                    if (itemType.IsPrimitive || itemType == typeof(string) || itemType.IsEnum)
                    {
                        lines.Add($"{prefix}\t{property.Name} (Collection of {itemType.Name})");
                    }
                    else
                    {
                        lines.Add($"{prefix}\t{property.Name} Items:");
                        lines.AddRange(GetProperties(itemType, prefix + "\t\t"));
                    }
                }
                // Recurse into complex types
                else if (IsComplexType(propType))
                {
                    lines.AddRange(GetProperties(propType, prefix + "\t"));
                }
            }
            return lines;
        }

        private static bool IsComplexType(Type type)
        {
            // Adjust this as needed for your definition of "complex type"
            return !type.IsValueType && !type.IsPrimitive && type != typeof(string) && !type.IsEnum && !type.IsGenericType;
        }
    }
}
