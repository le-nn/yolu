using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Yolu.Identifier;

/// <summary>
/// Represents a Universally Unique Lexicographically Sortable Identifier (ULID).
/// Spec: https://github.com/ulid/spec
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 16)]
[DebuggerDisplay("{ToString(),nq}")]
[TypeConverter(typeof(UlidTypeConverter))]
[JsonConverter(typeof(UlidJsonConverter))]
public readonly struct Ulid : IEquatable<Ulid>, IComparable<Ulid> {
    // https://en.wikipedia.org/wiki/Base32
    private static readonly char[] _base32Text = "0123456789ABCDEFGHJKMNPQRSTVWXYZ".ToCharArray();
    private static readonly byte[] _base32Bytes = Encoding.UTF8.GetBytes(_base32Text);
    private static readonly byte[] _charToBase32 = [
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        255,
        255,
        255,
        255,
        255,
        255,
        255,
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        17,
        255,
        18,
        19,
        255,
        20,
        21,
        255,
        22,
        23,
        24,
        25,
        26,
        255,
        27,
        28,
        29,
        30,
        31,
        255,
        255,
        255,
        255,
        255,
        255,
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        17,
        255,
        18,
        19,
        255,
        20,
        21,
        255,
        22,
        23,
        24,
        25,
        26,
        255,
        27,
        28,
        29,
        30,
        31
];

    private static readonly DateTimeOffset _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static readonly Ulid MinValue = new(_unixEpoch.ToUnixTimeMilliseconds(),
        new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

    public static readonly Ulid MaxValue = new(DateTimeOffset.MaxValue.ToUnixTimeMilliseconds(),
        new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });

    public static readonly Ulid Empty;

    // Timestamp(64bits)
    [FieldOffset(0)] private readonly byte _timestamp0;
    [FieldOffset(1)] private readonly byte _timestamp1;
    [FieldOffset(2)] private readonly byte _timestamp2;
    [FieldOffset(3)] private readonly byte _timestamp3;
    [FieldOffset(4)] private readonly byte _timestamp4;
    [FieldOffset(5)] private readonly byte _timestamp5;

    // Randomness(80bits)
    [FieldOffset(6)] private readonly byte _randomness0;
    [FieldOffset(7)] private readonly byte _randomness1;
    [FieldOffset(8)] private readonly byte _randomness2;
    [FieldOffset(9)] private readonly byte _randomness3;
    [FieldOffset(10)] private readonly byte _randomness4;
    [FieldOffset(11)] private readonly byte _randomness5;
    [FieldOffset(12)] private readonly byte _randomness6;
    [FieldOffset(13)] private readonly byte _randomness7;
    [FieldOffset(14)] private readonly byte _randomness8;
    [FieldOffset(15)] private readonly byte _randomness9;

    [IgnoreDataMember]
    public byte[] Random => [
        _randomness0,
        _randomness1,
        _randomness2,
        _randomness3,
        _randomness4,
        _randomness5,
        _randomness6,
        _randomness7,
        _randomness8,
        _randomness9
    ];

    [IgnoreDataMember]
    public DateTimeOffset Time {
        get {
            Span<byte> buffer = stackalloc byte[8];
            buffer[0] = _timestamp5;
            buffer[1] = _timestamp4;
            buffer[2] = _timestamp3;
            buffer[3] = _timestamp2;
            buffer[4] = _timestamp1;
            buffer[5] = _timestamp0; // [6], [7] = 0

            var timestampMilliseconds = Unsafe.As<byte, long>(ref MemoryMarshal.GetReference(buffer));
            return DateTimeOffset.FromUnixTimeMilliseconds(timestampMilliseconds);
        }
    }

    internal Ulid(long timestampMilliseconds, XorShift64 random)
        : this() {
        // Get memory in stack and copy to ulid(Little->Big reverse order).
        ref var fisrtByte = ref Unsafe.As<long, byte>(ref timestampMilliseconds);
        _timestamp0 = Unsafe.Add(ref fisrtByte, 5);
        _timestamp1 = Unsafe.Add(ref fisrtByte, 4);
        _timestamp2 = Unsafe.Add(ref fisrtByte, 3);
        _timestamp3 = Unsafe.Add(ref fisrtByte, 2);
        _timestamp4 = Unsafe.Add(ref fisrtByte, 1);
        _timestamp5 = Unsafe.Add(ref fisrtByte, 0);

        // Get first byte of randomness from Ulid Struct.
        Unsafe.WriteUnaligned(ref _randomness0, random.Next()); // randomness0~7(but use 0~1 only)
        Unsafe.WriteUnaligned(ref _randomness2, random.Next()); // randomness2~9
    }

    internal Ulid(long timestampMilliseconds, ReadOnlySpan<byte> randomness)
        : this() {
        ref var fisrtByte = ref Unsafe.As<long, byte>(ref timestampMilliseconds);
        _timestamp0 = Unsafe.Add(ref fisrtByte, 5);
        _timestamp1 = Unsafe.Add(ref fisrtByte, 4);
        _timestamp2 = Unsafe.Add(ref fisrtByte, 3);
        _timestamp3 = Unsafe.Add(ref fisrtByte, 2);
        _timestamp4 = Unsafe.Add(ref fisrtByte, 1);
        _timestamp5 = Unsafe.Add(ref fisrtByte, 0);

        ref var src = ref MemoryMarshal.GetReference(randomness); // length = 10
        _randomness0 = randomness[0];
        _randomness1 = randomness[1];
        Unsafe.WriteUnaligned(ref _randomness2,
            Unsafe.As<byte, ulong>(ref Unsafe.Add(ref src, 2))); // randomness2~randomness9
    }

    public Ulid(ReadOnlySpan<byte> bytes)
        : this() {
        if (bytes.Length != 16) {
            throw new ArgumentException("invalid bytes length, length:" + bytes.Length);
        }

        ref var src = ref MemoryMarshal.GetReference(bytes);
        Unsafe.WriteUnaligned(ref _timestamp0, Unsafe.As<byte, ulong>(ref src)); // timestamp0~randomness1
        Unsafe.WriteUnaligned(ref _randomness2,
            Unsafe.As<byte, ulong>(ref Unsafe.Add(ref src, 8))); // randomness2~randomness9
    }

    internal Ulid(ReadOnlySpan<char> base32) {
        // unroll-code is based on NUlid.

        _randomness9 =
            (byte)(_charToBase32[base32[24]] << 5 | _charToBase32[base32[25]]); // eliminate bounds-check of span

        _timestamp0 = (byte)(_charToBase32[base32[0]] << 5 | _charToBase32[base32[1]]);
        _timestamp1 = (byte)(_charToBase32[base32[2]] << 3 | _charToBase32[base32[3]] >> 2);
        _timestamp2 = (byte)(_charToBase32[base32[3]] << 6 | _charToBase32[base32[4]] << 1 |
                            _charToBase32[base32[5]] >> 4);
        _timestamp3 = (byte)(_charToBase32[base32[5]] << 4 | _charToBase32[base32[6]] >> 1);
        _timestamp4 = (byte)(_charToBase32[base32[6]] << 7 | _charToBase32[base32[7]] << 2 |
                            _charToBase32[base32[8]] >> 3);
        _timestamp5 = (byte)(_charToBase32[base32[8]] << 5 | _charToBase32[base32[9]]);

        _randomness0 = (byte)(_charToBase32[base32[10]] << 3 | _charToBase32[base32[11]] >> 2);
        _randomness1 = (byte)(_charToBase32[base32[11]] << 6 | _charToBase32[base32[12]] << 1 |
                             _charToBase32[base32[13]] >> 4);
        _randomness2 = (byte)(_charToBase32[base32[13]] << 4 | _charToBase32[base32[14]] >> 1);
        _randomness3 = (byte)(_charToBase32[base32[14]] << 7 | _charToBase32[base32[15]] << 2 |
                             _charToBase32[base32[16]] >> 3);
        _randomness4 = (byte)(_charToBase32[base32[16]] << 5 | _charToBase32[base32[17]]);
        _randomness5 = (byte)(_charToBase32[base32[18]] << 3 | _charToBase32[base32[19]] >> 2);
        _randomness6 = (byte)(_charToBase32[base32[19]] << 6 | _charToBase32[base32[20]] << 1 |
                             _charToBase32[base32[21]] >> 4);
        _randomness7 = (byte)(_charToBase32[base32[21]] << 4 | _charToBase32[base32[22]] >> 1);
        _randomness8 = (byte)(_charToBase32[base32[22]] << 7 | _charToBase32[base32[23]] << 2 |
                             _charToBase32[base32[24]] >> 3);
    }

    // HACK: We assume the layout of a Guid is the following:
    // Int32, Int16, Int16, Int8, Int8, Int8, Int8, Int8, Int8, Int8, Int8
    // source: https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/Guid.cs
    public Ulid(Guid guid) {
        Span<byte> buf = stackalloc byte[16];
        MemoryMarshal.Write(buf, ref guid);
        if (BitConverter.IsLittleEndian) {
            byte tmp;
            tmp = buf[0];
            buf[0] = buf[3];
            buf[3] = tmp;
            tmp = buf[1];
            buf[1] = buf[2];
            buf[2] = tmp;
            tmp = buf[4];
            buf[4] = buf[5];
            buf[5] = tmp;
            tmp = buf[6];
            buf[6] = buf[7];
            buf[7] = tmp;
        }

        this = MemoryMarshal.Read<Ulid>(buf);
    }

    // Factory

    public static Ulid NewUlid() {
        return new Ulid(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), RandomProvider.GetXorShift64());
    }

    public static Ulid NewUlid(DateTimeOffset timestamp) {
        return new Ulid(timestamp.ToUnixTimeMilliseconds(), RandomProvider.GetXorShift64());
    }

    public static Ulid NewUlid(DateTimeOffset timestamp, ReadOnlySpan<byte> randomness) {
        if (randomness.Length != 10) {
            throw new ArgumentException("invalid randomness length, length:" + randomness.Length);
        }

        return new Ulid(timestamp.ToUnixTimeMilliseconds(), randomness);
    }

    public static Ulid Parse(string base32) {
        return Parse(base32.AsSpan());
    }

    public static Ulid Parse(ReadOnlySpan<char> base32) {
        if (base32.Length != 26) {
            throw new ArgumentException("invalid base32 length, length:" + base32.Length);
        }

        return new Ulid(base32);
    }

    public static Ulid Parse(ReadOnlySpan<byte> base32) {
        if (!TryParse(base32, out var ulid)) {
            throw new ArgumentException("invalid base32 length, length:" + base32.Length);
        }

        return ulid;
    }

    public static bool TryParse(string base32, out Ulid ulid) {
        return TryParse(base32.AsSpan(), out ulid);
    }

    public static bool TryParse(ReadOnlySpan<char> base32, out Ulid ulid) {
        if (base32.Length != 26) {
            ulid = default;
            return false;
        }

        try {
            ulid = new Ulid(base32);
            return true;
        }
        catch {
            ulid = default;
            return false;
        }
    }

    public static bool TryParse(ReadOnlySpan<byte> base32, out Ulid ulid) {
        if (base32.Length != 26) {
            ulid = default;
            return false;
        }

        try {
            ulid = ParseCore(base32);
            return true;
        }
        catch {
            ulid = default;
            return false;
        }
    }

    private static Ulid ParseCore(ReadOnlySpan<byte> base32) {
        if (base32.Length != 26) {
            throw new ArgumentException("invalid base32 length, length:" + base32.Length);
        }

        var ulid = default(Ulid);
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 15) = (byte)(
            _charToBase32[base32[24]] << 5 | _charToBase32[base32[25]]
        );

        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 0) =
            (byte)(_charToBase32[base32[0]] << 5 | _charToBase32[base32[1]]);
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 1) = (byte)(
            _charToBase32[base32[2]] << 3 | _charToBase32[base32[3]] >> 2
        );
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 2) = (byte)(
            _charToBase32[base32[3]] << 6 |
            _charToBase32[base32[4]] << 1 |
            _charToBase32[base32[5]] >> 4
        );
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 3) =
            (byte)(_charToBase32[base32[5]] << 4 | _charToBase32[base32[6]] >> 1);
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 4) = (byte)(
            _charToBase32[base32[6]] << 7 |
            _charToBase32[base32[7]] << 2 |
            _charToBase32[base32[8]] >> 3
        );
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 5) = (byte)(
            _charToBase32[base32[8]] << 5 | _charToBase32[base32[9]]
        );

        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 6) =
            (byte)(_charToBase32[base32[10]] << 3 | _charToBase32[base32[11]] >> 2);
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 7) = (byte)(
            _charToBase32[base32[11]] << 6 |
            _charToBase32[base32[12]] << 1 |
            _charToBase32[base32[13]] >> 4
        );
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 8) =
            (byte)(_charToBase32[base32[13]] << 4 | _charToBase32[base32[14]] >> 1);
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 9) = (byte)(
            _charToBase32[base32[14]] << 7 |
            _charToBase32[base32[15]] << 2 |
            _charToBase32[base32[16]] >> 3
        );
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 10) =
            (byte)(_charToBase32[base32[16]] << 5 | _charToBase32[base32[17]]);
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 11) = (byte)(
            _charToBase32[base32[18]] << 3
            | _charToBase32[base32[19]] >> 2
        );
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 12) = (byte)(
            _charToBase32[base32[19]] << 6 |
            _charToBase32[base32[20]] << 1 |
            _charToBase32[base32[21]] >> 4
        );
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 13) =
            (byte)(_charToBase32[base32[21]] << 4 | _charToBase32[base32[22]] >> 1);
        Unsafe.Add(ref Unsafe.As<Ulid, byte>(ref ulid), 14) = (byte)(
            _charToBase32[base32[22]] << 7 |
            _charToBase32[base32[23]] << 2 |
            _charToBase32[base32[24]] >> 3
        );

        return ulid;
    }

    // Convert

    public byte[] ToByteArray() {
        var bytes = new byte[16];
        Unsafe.WriteUnaligned(ref bytes[0], this);
        return bytes;
    }

    public bool TryWriteBytes(Span<byte> destination) {
        if (destination.Length < 16) {
            return false;
        }

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);
        return true;
    }

    public string ToBase64(Base64FormattingOptions options = Base64FormattingOptions.None) {
        var buffer = ArrayPool<byte>.Shared.Rent(16);
        try {
            TryWriteBytes(buffer);
            return Convert.ToBase64String(buffer, options);
        }
        finally {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public bool TryWriteStringify(Span<byte> span) {
        if (span.Length < 26) {
            return false;
        }

        span[25] = _base32Bytes[_randomness9 & 31]; // eliminate bounds-check of span

        // timestamp
        span[0] = _base32Bytes[(_timestamp0 & 224) >> 5];
        span[1] = _base32Bytes[_timestamp0 & 31];
        span[2] = _base32Bytes[(_timestamp1 & 248) >> 3];
        span[3] = _base32Bytes[(_timestamp1 & 7) << 2 | (_timestamp2 & 192) >> 6];
        span[4] = _base32Bytes[(_timestamp2 & 62) >> 1];
        span[5] = _base32Bytes[(_timestamp2 & 1) << 4 | (_timestamp3 & 240) >> 4];
        span[6] = _base32Bytes[(_timestamp3 & 15) << 1 | (_timestamp4 & 128) >> 7];
        span[7] = _base32Bytes[(_timestamp4 & 124) >> 2];
        span[8] = _base32Bytes[(_timestamp4 & 3) << 3 | (_timestamp5 & 224) >> 5];
        span[9] = _base32Bytes[_timestamp5 & 31];

        // randomness
        span[10] = _base32Bytes[(_randomness0 & 248) >> 3];
        span[11] = _base32Bytes[(_randomness0 & 7) << 2 | (_randomness1 & 192) >> 6];
        span[12] = _base32Bytes[(_randomness1 & 62) >> 1];
        span[13] = _base32Bytes[(_randomness1 & 1) << 4 | (_randomness2 & 240) >> 4];
        span[14] = _base32Bytes[(_randomness2 & 15) << 1 | (_randomness3 & 128) >> 7];
        span[15] = _base32Bytes[(_randomness3 & 124) >> 2];
        span[16] = _base32Bytes[(_randomness3 & 3) << 3 | (_randomness4 & 224) >> 5];
        span[17] = _base32Bytes[_randomness4 & 31];
        span[18] = _base32Bytes[(_randomness5 & 248) >> 3];
        span[19] = _base32Bytes[(_randomness5 & 7) << 2 | (_randomness6 & 192) >> 6];
        span[20] = _base32Bytes[(_randomness6 & 62) >> 1];
        span[21] = _base32Bytes[(_randomness6 & 1) << 4 | (_randomness7 & 240) >> 4];
        span[22] = _base32Bytes[(_randomness7 & 15) << 1 | (_randomness8 & 128) >> 7];
        span[23] = _base32Bytes[(_randomness8 & 124) >> 2];
        span[24] = _base32Bytes[(_randomness8 & 3) << 3 | (_randomness9 & 224) >> 5];

        return true;
    }

    public bool TryWriteStringify(Span<char> span) {
        if (span.Length < 26) {
            return false;
        }

        span[25] = _base32Text[_randomness9 & 31]; // eliminate bounds-check of span

        // timestamp
        span[0] = _base32Text[(_timestamp0 & 224) >> 5];
        span[1] = _base32Text[_timestamp0 & 31];
        span[2] = _base32Text[(_timestamp1 & 248) >> 3];
        span[3] = _base32Text[(_timestamp1 & 7) << 2 | (_timestamp2 & 192) >> 6];
        span[4] = _base32Text[(_timestamp2 & 62) >> 1];
        span[5] = _base32Text[(_timestamp2 & 1) << 4 | (_timestamp3 & 240) >> 4];
        span[6] = _base32Text[(_timestamp3 & 15) << 1 | (_timestamp4 & 128) >> 7];
        span[7] = _base32Text[(_timestamp4 & 124) >> 2];
        span[8] = _base32Text[(_timestamp4 & 3) << 3 | (_timestamp5 & 224) >> 5];
        span[9] = _base32Text[_timestamp5 & 31];

        // randomness
        span[10] = _base32Text[(_randomness0 & 248) >> 3];
        span[11] = _base32Text[(_randomness0 & 7) << 2 | (_randomness1 & 192) >> 6];
        span[12] = _base32Text[(_randomness1 & 62) >> 1];
        span[13] = _base32Text[(_randomness1 & 1) << 4 | (_randomness2 & 240) >> 4];
        span[14] = _base32Text[(_randomness2 & 15) << 1 | (_randomness3 & 128) >> 7];
        span[15] = _base32Text[(_randomness3 & 124) >> 2];
        span[16] = _base32Text[(_randomness3 & 3) << 3 | (_randomness4 & 224) >> 5];
        span[17] = _base32Text[_randomness4 & 31];
        span[18] = _base32Text[(_randomness5 & 248) >> 3];
        span[19] = _base32Text[(_randomness5 & 7) << 2 | (_randomness6 & 192) >> 6];
        span[20] = _base32Text[(_randomness6 & 62) >> 1];
        span[21] = _base32Text[(_randomness6 & 1) << 4 | (_randomness7 & 240) >> 4];
        span[22] = _base32Text[(_randomness7 & 15) << 1 | (_randomness8 & 128) >> 7];
        span[23] = _base32Text[(_randomness8 & 124) >> 2];
        span[24] = _base32Text[(_randomness8 & 3) << 3 | (_randomness9 & 224) >> 5];

        return true;
    }

    public override string ToString() {
        Span<char> span = stackalloc char[26];
        TryWriteStringify(span);
        unsafe {
            return new string((char*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)), 0, 26);
        }
    }

    // Comparable/Equatable

    public override unsafe int GetHashCode() {
        // Simply XOR, same algorithm of Guid.GetHashCode
        fixed (void* p = &_timestamp0) {
            var a = (int*)p;
            return *a ^ *(a + 1) ^ *(a + 2) ^ *(a + 3);
        }
    }

    public unsafe bool Equals(Ulid other) {
        // readonly struct can not use Unsafe.As...
        fixed (byte* a = &_timestamp0) {
            var b = &other._timestamp0;

            {
                var x = *(ulong*)a;
                var y = *(ulong*)b;
                if (x != y) {
                    return false;
                }
            }
            {
                var x = *(ulong*)(a + 8);
                var y = *(ulong*)(b + 8);
                if (x != y) {
                    return false;
                }
            }

            return true;
        }
    }

    public override bool Equals(object? obj) {
        return obj is Ulid other && Equals(other);
    }

    public static bool operator ==(Ulid a, Ulid b) {
        return a.Equals(b);
    }

    public static bool operator !=(Ulid a, Ulid b) {
        return !a.Equals(b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetResult(byte me, byte them) {
        return me < them ? -1 : 1;
    }

    public int CompareTo(Ulid other) {
        if (_timestamp0 != other._timestamp0) {
            return GetResult(_timestamp0, other._timestamp0);
        }

        if (_timestamp1 != other._timestamp1) {
            return GetResult(_timestamp1, other._timestamp1);
        }

        if (_timestamp2 != other._timestamp2) {
            return GetResult(_timestamp2, other._timestamp2);
        }

        if (_timestamp3 != other._timestamp3) {
            return GetResult(_timestamp3, other._timestamp3);
        }

        if (_timestamp4 != other._timestamp4) {
            return GetResult(_timestamp4, other._timestamp4);
        }

        if (_timestamp5 != other._timestamp5) {
            return GetResult(_timestamp5, other._timestamp5);
        }

        if (_randomness0 != other._randomness0) {
            return GetResult(_randomness0, other._randomness0);
        }

        if (_randomness1 != other._randomness1) {
            return GetResult(_randomness1, other._randomness1);
        }

        if (_randomness2 != other._randomness2) {
            return GetResult(_randomness2, other._randomness2);
        }

        if (_randomness3 != other._randomness3) {
            return GetResult(_randomness3, other._randomness3);
        }

        if (_randomness4 != other._randomness4) {
            return GetResult(_randomness4, other._randomness4);
        }

        if (_randomness5 != other._randomness5) {
            return GetResult(_randomness5, other._randomness5);
        }

        if (_randomness6 != other._randomness6) {
            return GetResult(_randomness6, other._randomness6);
        }

        if (_randomness7 != other._randomness7) {
            return GetResult(_randomness7, other._randomness7);
        }

        if (_randomness8 != other._randomness8) {
            return GetResult(_randomness8, other._randomness8);
        }

        if (_randomness9 != other._randomness9) {
            return GetResult(_randomness9, other._randomness9);
        }

        return 0;
    }

    //public static implicit operator Guid(Ulid target) {
    //    return target.ToGuid();
    //}

    //public static implicit operator Ulid(Guid target) {
    //    return new Ulid(target);
    //}

    /// <summary>
    /// Convert this <c>Ulid</c> value to a <c>Guid</c> value with the same comparability.
    /// </summary>
    /// <remarks>
    /// The byte arrangement between Ulid and Guid is not preserved.
    /// </remarks>
    /// <returns>The converted <c>Guid</c> value</returns>
    public Guid ToGuid() {
        Span<byte> buf = stackalloc byte[16];
        MemoryMarshal.Write(buf, ref Unsafe.AsRef(this));
        if (BitConverter.IsLittleEndian) {
            byte tmp;
            tmp = buf[0];
            buf[0] = buf[3];
            buf[3] = tmp;
            tmp = buf[1];
            buf[1] = buf[2];
            buf[2] = tmp;
            tmp = buf[4];
            buf[4] = buf[5];
            buf[5] = tmp;
            tmp = buf[6];
            buf[6] = buf[7];
            buf[7] = tmp;
        }

        return MemoryMarshal.Read<Guid>(buf);
    }
}

internal static class RandomProvider {
    [ThreadStatic] private static Random? _random;
    [ThreadStatic] private static XorShift64? _xorShift;

    // this random is async-unsafe, be careful to use.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Random GetRandom() {
        _random ??= CreateRandom();

        return _random;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Random CreateRandom() {
        using var rng = RandomNumberGenerator.Create();
        // Span<byte> buffer = stackalloc byte[sizeof(int)];
        var buffer = new byte[sizeof(int)];
        rng.GetBytes(buffer);
        var seed = BitConverter.ToInt32(buffer, 0);
        return new Random(seed);
    }

    // this random is async-unsafe, be careful to use.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XorShift64 GetXorShift64() {
        _xorShift ??= CreateXorShift64();

        return _xorShift;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static XorShift64 CreateXorShift64() {
        using var rng = RandomNumberGenerator.Create();
        // Span<byte> buffer = stackalloc byte[sizeof(UInt64)];
        var buffer = new byte[sizeof(ulong)];
        rng.GetBytes(buffer);
        var seed = BitConverter.ToUInt64(buffer, 0);
        return new XorShift64(seed);
    }
}

internal class XorShift64 {
    private ulong _x = 88172645463325252UL;

    public XorShift64(ulong seed) {
        if (seed != 0) {
            _x = seed;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Next() {
        _x ^= _x << 7;
        return _x ^= _x >> 9;
    }
}