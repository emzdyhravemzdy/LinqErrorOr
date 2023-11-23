namespace ErrorOr;

public static class EnumerableErrorOrExtension
{
    // Functor UnWrap
    public static IReadOnlyList<ErrorOr<TA>> UnWrap<TA>(this ErrorOr<IReadOnlyList<TA>> ma)
    {
        return ma.Match(
           onError: err => MakeErrorOr<TA>.MapErrors(err),
           onValue: res => MakeErrorOr<TA>.MapValues(res));
    }
    public static IReadOnlyList<ErrorOr<TA>> UnWrap<TA>(this ErrorOr<IReadOnlyList<ErrorOr<TA>>> ma)
    {
        return ma.Match(
           onError: err => MakeErrorOr<TA>.MapErrors(err),
           onValue: res => res);
    }
    public static IEnumerable<TA> SuccessList<TA>(this IEnumerable<ErrorOr<TA>> ma) where TA : class
    {
        foreach (ErrorOr<TA> item in ma)
        {
            if (item.IsError == false)
            {
                yield return item.Value;
            }
        }
    }
    public static IEnumerable<Error> FailuresList<TA>(this IEnumerable<ErrorOr<TA>> ma) where TA : class
    {
        foreach (ErrorOr<TA> item in ma)
        {
            if (item.IsError)
            {
                yield return item.FirstError;
            }
        }
    }
    public static IEnumerable<Error> ErrorsList<TA>(this IEnumerable<ErrorOr<TA>> ma) where TA : class
    {
        foreach (ErrorOr<TA> item in ma)
        {
            if (item.IsError)
            {
                foreach (Error err in item.Errors)
                {
                    yield return err;
                }
            }
        }
    }
    // Functor aggregate
    public static ErrorOr<TAccumulate> AggregateErrorOr<TSource, TAccumulate>(this IEnumerable<TSource> source,
        TAccumulate seed, Func<ErrorOr<TAccumulate>, TSource, ErrorOr<TAccumulate>> func)
    {
        return source.Aggregate(MakeErrorOr<TAccumulate>.Success(seed), func);
    }
    // Functor map
    public static ErrorOr<IEnumerable<TA>> SelectErrorOr<TA>(this IEnumerable<ErrorOr<TA>> ma)
    {
        var errors = ma.Where(a => a.IsError).SelectMany(b => b.Errors).ToList();
        if (errors.Count != 0)
        {
            return ErrorOr<IEnumerable<TA>>.From(errors);
        }
        return ma.Where(a => a.IsError == false).Select(b => b.Value).ToList();
    }
    public static ErrorOr<IEnumerable<TB>> SelectErrorOr<TA, TB>(this IEnumerable<ErrorOr<TA>> ma, Func<TA, TB> f)
    {
        var errors = ma.Where(a => a.IsError).SelectMany(b => b.Errors).ToList();
        if (errors.Count != 0)
        {
            return ErrorOr<IEnumerable<TB>>.From(errors);
        }
        return ma.Where(a => a.IsError == false).Select(x => f(x.Value)).ToList();
    }
    // Functor map
    public static ErrorOr<IDictionary<TK, IEnumerable<TA>>> SelectErrorOr<TK, TA>(this IDictionary<TK, ErrorOr<IEnumerable<TA>>> ma)
        where TK : notnull
    {
        var errors = ma.Where(a => a.Value.IsError).SelectMany(b => b.Value.Errors).ToList();
        if (errors.Count != 0)
        {
            return ErrorOr<IDictionary<TK, IEnumerable<TA>>>.From(errors);
        }
        return ma.Where(a => a.Value.IsError == false).ToDictionary(k => k.Key, v => v.Value.Value);
    }
    public static ErrorOr<IDictionary<TK, IEnumerable<TB>>> SelectErrorOr<TK, TA, TB>(this IDictionary<TK, ErrorOr<IEnumerable<TA>>> ma, Func<TA, TB> f)
        where TK : notnull
    {
        var errors = ma.Where(a => a.Value.IsError).SelectMany(b => b.Value.Errors).ToList();
        if (errors.Count != 0)
        {
            return ErrorOr<IDictionary<TK, IEnumerable<TB>>>.From(errors);
        }
        return ma.Where(a => a.Value.IsError == false).ToDictionary(k => k.Key, v => v.Value.Value.Select(x => f(x)));
    }
}
