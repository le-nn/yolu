using System.Text;

namespace Hadron.Text;

public static partial class StringUtils {
    /// <summary>
    /// Repeat string.
    /// </summary>
    /// <param name="str">The target string.</param>
    /// <param name="repeat"></param>
    /// <returns>The value.</returns>
    public static string Repeat(this string str, int repeat) {
        var builder = new StringBuilder();

        for (var i = 0; i < repeat; i++) {
            builder.Append(str);
        }

        return builder.ToString();
    }
}
