namespace market_tracker_webapi.Application.Utils;

/*
public interface IEither<out TL, out TR>
{
    public bool IsFailure() => this is Left<TL>;

    public bool IsSuccess() => this is Right<TR>;
}

public class Left<TL>(TL error) : IEither<TL, object>
{
    public TL Error { get; } = error;
}

public class Right<TR>(TR value) : IEither<object, TR>
{
    public TR Value { get; } = value;
}

public static class EitherExtensions
{
    
    // functions to when using Either to create a new instance of Left or Right

    public static IEither<TL, object> Failure<TL>(TL error) => new Left<TL>(error);

    public static IEither<object, TR> Success<TR>(TR value) => new Right<TR>(value);
}
*/


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
