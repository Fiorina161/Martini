using System.Globalization;

namespace Martini
{
    internal static class Utils
    {
        public static string ToPascalCase(this string s)
        {
            s = s.ToLower().Replace("_", " ");
            var info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(s).Replace(" ", string.Empty);
        }
    }
}
