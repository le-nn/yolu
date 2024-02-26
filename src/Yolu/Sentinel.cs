namespace Yolu;

internal sealed class Sentinel : ISingleton<Sentinel> {
    public static Sentinel Instance { get; } = new();

    private Sentinel() {
    }
}