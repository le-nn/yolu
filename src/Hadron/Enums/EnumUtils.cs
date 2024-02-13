using System.ComponentModel;
using System.Reflection;

namespace Hadron.Enums;

/// <summary>
/// Utility class for working with enums.
/// </summary>
public static partial class EnumUtils {
    /// <summary>
    /// Gets the display name of the specified enum value.
    /// </summary>
    /// <param name="e">The enum value.</param>
    /// <returns>The display name of the enum value.</returns>
    public static string? ToDisplayName(this Enum e) {
        return e.GetType().GetField(e.ToString())?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
    }
}
