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
        public static void ListPropertiesToFile(Type type, string filePath)
        {
            var lines = GetProperties(type);
            File.WriteAllLines(filePath, lines);
        }

        private static IEnumerable<string> GetProperties(Type type, string prefix = "")
        {
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type.IsEnum)
                return Enumerable.Empty<string>();

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
                    lines.AddRange(GetProperties(args[0], prefix + "\t\tKey: "));
                    lines.AddRange(GetProperties(args[1], prefix + "\t\tValue: "));
                }
                else if (propType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
                {
                    Type itemType = propType.GetGenericArguments()[0];
                    line = $"{prefix}\t{property.Name} (Collection of {itemType.Name})";
                    lines.Add(line);
                    if (IsComplexType(itemType))
                    {
                        lines.AddRange(GetProperties(itemType, prefix + "\t\t"));
                    }
                }
                else if (IsComplexType(propType))
                {
                    lines.AddRange(GetProperties(propType, prefix + "\t"));
                }
            }
            return lines;
        }

        private static bool IsComplexType(Type type)
        {
            return !type.IsValueType && !type.IsPrimitive && type != typeof(string) && !type.IsEnum && type != typeof(DateTime);
        }
    }
}
