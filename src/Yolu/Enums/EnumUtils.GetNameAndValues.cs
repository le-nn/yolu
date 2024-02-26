namespace Yolu.Enums;

/// <summary>
/// Utility class for working with enums.
/// </summary>
public static partial class EnumUtils {
    /// <summary>
    /// Gets the names and values of the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>An enumerable of tuples containing the name and value of each enum member.</returns>
    public static IEnumerable<(string Name, TEnum Value)> GetNameAndValues<TEnum>()
        where TEnum : struct, Enum {
        return Enum.GetNames<TEnum>().Zip(Enum.GetValues<TEnum>());
    }

    /// <summary>
    /// Gets the display names and values of the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>An enumerable of tuples containing the display name and value of each enum member.</returns>
    public static IEnumerable<(string? Name, TEnum Value)> GetDisplayNameAndValues<TEnum>()
        where TEnum : struct, Enum {
        return Enum.GetValues<TEnum>().Select(x => x.ToDisplayName()).Zip(Enum.GetValues<TEnum>());
    }
}
