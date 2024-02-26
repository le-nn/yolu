namespace Yolu.Identifier;

public abstract record PrefixedUlid {
    public abstract string PrefixWithSeparator { get; }

    public Ulid Value { get; private set; }

    public string FullValue => $"{PrefixWithSeparator}{Value}";

    public PrefixedUlid(string id) {
        CheckIfValidAndSetUlid(id);
    }

    public PrefixedUlid() {
    }

    private void CheckIfValidAndSetUlid(string id) {
        if (id.StartsWith(PrefixWithSeparator) is false) {
            throw Error.WithDisplayMessage($"Id must start with '{PrefixWithSeparator}'");
        }

        var ulidText = id.Replace(PrefixWithSeparator, "");
        if (Ulid.TryParse(ulidText, out var ulid)) {
            Value = ulid;
        }
        else {
            throw Error.WithDisplayMessage($"Ulid '{ulidText}' is invalid ");
        }
    }

    public static T Parse<T>(string rawId) where T : PrefixedUlid, new() {
        var pid = new T();
        pid.CheckIfValidAndSetUlid(rawId);
        return pid;
    }

    public static T NewPrefixedUlid<T>() where T : PrefixedUlid, new() {
        var pid = new T();
        pid.Value = Ulid.NewUlid();
        return pid;
    }

    public sealed override string ToString() => FullValue;
}