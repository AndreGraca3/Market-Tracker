namespace market_tracker_webapi.Application.Utils;

public class Either<TL, TR>
{
    public TL Error { get; }
    public TR Value { get; }

    public bool IsSuccessful() => this.Value != null;

    public Either(TL error)
    {
        this.Error = error;
    }

    public Either(TR value)
    {
        this.Value = value;
    }
}

public static class EitherExtensions
{
    public static Either<TL, TR> Failure<TL, TR>(TL error) => new(error);

    public static Either<TL, TR> Success<TL, TR>(TR value) => new(value);
}
