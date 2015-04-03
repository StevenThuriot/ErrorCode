using System.Globalization;
using System.Linq;

namespace ErrorCode.Extensions
{
    static class StringExtensions
    {
        public static string AsReadable(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value.Skip(1)
                        .Aggregate(value[0].ToString(CultureInfo.InvariantCulture),
                                   (current, next) => current + ParseCharacter(next));
        }

        private static string ParseCharacter(char next)
        {
            if (char.IsPunctuation(next)) return "";
            return (char.IsUpper(next) ? " " : "") + next;
        }
    }
}