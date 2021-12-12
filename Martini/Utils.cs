using System;
using System.Globalization;
using System.Windows.Forms;

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

        public static bool Try(Action act)
        {
            try
            {
                act();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
