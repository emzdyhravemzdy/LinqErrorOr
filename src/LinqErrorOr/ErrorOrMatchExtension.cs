namespace ErrorOr;

public static class ErrorOrMatchExtension
{
    public static void Match<TA>(this ErrorOr<TA> ma, Action<TA> Succ, Action<List<Error>> Fail) =>
       ma.Switch(
            x => Succ(x),
            err => Fail(err));
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
