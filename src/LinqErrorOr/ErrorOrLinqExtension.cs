namespace ErrorOr;
public static class ErrorOrLinqExtension
{

    // Functor map
    public static ErrorOr<TB> Select<TA, TB>(this ErrorOr<TA> ma, Func<TA, TB> f) =>
        ma.Match(
            x => f(x),
            err => ErrorOr<TB>.From(err));

    // Functor map
    public static ErrorOr<TB> Map<TA, TB>(this ErrorOr<TA> ma, Func<TA, TB> f) =>
        ma.Select(f);

    // Monadic bind
    public static ErrorOr<TB> SelectMany<TA, TB>(this ErrorOr<TA> ma, Func<TA, ErrorOr<TB>> f) =>
        ma.Match(
            x => f(x),
            err => ErrorOr<TB>.From(err));

    // Monadic bind
    public static ErrorOr<TB> Bind<TA, TB>(this ErrorOr<TA> ma, Func<TA, ErrorOr<TB>> f) =>
        SelectMany(ma, f);

    // Monadic bind + projection
    public static ErrorOr<TC> SelectMany<TA, TB, TC>(
        this ErrorOr<TA> ma,
        Func<TA, ErrorOr<TB>> bind,
        Func<TA, TB, TC> project) =>
        ma.SelectMany(a => bind(a).Select(b => project(a, b)));
}
