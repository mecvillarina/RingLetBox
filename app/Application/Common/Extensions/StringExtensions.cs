using System.Text.RegularExpressions;

namespace Application.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToNormalize(this string s)
        {
            s ??= string.Empty;
            s = s.Replace(" ", string.Empty).ToUpper();
            return s;
        }

        public static string ToSingleSpacing(this string str)
        {
            var options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            str ??= string.Empty;
            str = regex.Replace(str, " ");

            return str;
        }
    }
}