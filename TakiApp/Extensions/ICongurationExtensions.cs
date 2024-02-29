using Microsoft.Extensions.Configuration;

namespace Taki.Extensions
{
    public static class ICongurationExtensions
    {
        public static string GetRequiredValue(this IConfiguration configuration, params string[] keys)
        {
            return GetNestedSection(configuration, keys).Value!;
        }

        public static IConfigurationSection GetNestedSection(this IConfiguration configuration, params string[] keys)
        {
            if (keys.Length == 0)
                throw new ArgumentException("must enter a key");

            if (keys.Length == 1)
                return configuration.GetRequiredSection(keys[0]) ?? 
                    throw new ArgumentException($"the application requires field {keys[0]}");

            return GetNestedSection(configuration.GetRequiredSection(keys[0]), keys.Skip(1).ToArray());
        }

        public static int GetRequiredIntegerValue(this IConfiguration configuration, params string[] keys)
        {
            return int.Parse(GetRequiredValue(configuration, keys));
        }
    }
}
