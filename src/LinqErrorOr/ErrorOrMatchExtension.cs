namespace ErrorOr;

public static class ErrorOrMatchExtension
{
    public static void Match<TA>(this ErrorOr<TA> ma, Action<TA> Succ, Action<List<Error>> Fail) =>
       ma.Switch(
            x => Succ(x),
            err => Fail(err));
    public static void MatchSucc<TA>(this ErrorOr<TA> ma, Action<TA> Succ) {
        if (ma.IsError is false)
        {
            Succ(ma.Value);
        }
    }
    public static void MatchError<TA>(this ErrorOr<TA> ma, Action<List<Error>> Fail)
    {
        if (ma.IsError)
        {
            Fail(ma.Errors);
        }
    }
    public static void MatchFirstError<TA>(this ErrorOr<TA> ma, Action<Error> Fail)
    {
        if (ma.IsError)
        {
            Fail(ma.FirstError);
        }
    }
    public static Task MatchSuccAsync<TA>(this ErrorOr<TA> ma, Func<TA, Task> Succ, Action<List<Error>> Fail)
    {
        if (ma.IsError)
        {
            Fail(ma.Errors);
            return Task.CompletedTask;
        }
        else
        {
            return Succ(ma.Value);
        }
    }
}
