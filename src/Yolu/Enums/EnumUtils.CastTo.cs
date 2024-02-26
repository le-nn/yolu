namespace Yolu.Enums;

public static partial class EnumUtils {
    /// <summary>
    /// Casts the specified <see cref="Enum"/> value to the specified <typeparamref name="TEnum"/> type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum to cast to.</typeparam>
    /// <param name="e">The <see cref="Enum"/> value to cast.</param>
    /// <param name="ignoreCase">A boolean value indicating whether to ignore case when parsing the enum value.</param>
    /// <returns>The casted value of type <typeparamref name="TEnum"/>.</returns>
    /// <exception cref="InvalidCastException">Thrown when the cast is not possible.</exception>
    public static TEnum CastTo<TEnum>(this Enum e, bool ignoreCase = true)
        where TEnum : struct, Enum =>
            Enum.TryParse<TEnum>(e.ToString(), ignoreCase, out var result)
                ? result
                : throw new InvalidCastException($"Cannot cast {e.GetType()} to {typeof(TEnum)}");
}
