using System.Diagnostics.CodeAnalysis;

namespace market_tracker_webapi.Application.Utils;

[ExcludeFromCodeCoverage]
public readonly struct Optional<T>
{
    public Optional(T? value)
    {
        this.HasValue = true;
        this.Value = value;
    }

    public bool HasValue { get; }
    public T? Value { get; }

    public static implicit operator Optional<T>(T value) => new Optional<T>(value);

    public override string ToString() =>
        this.HasValue ? (this.Value?.ToString() ?? "null") : "unspecified";
}
