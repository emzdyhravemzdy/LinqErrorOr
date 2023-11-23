namespace ErrorOr;

public static class MakeErrorOr<TA>
{
    public static ErrorOr<TA> Success(TA value) => (ErrorOr<TA>)value;
    public static ErrorOr<TA> Failure(Error error) => (ErrorOr<TA>)error;
    public static ErrorOr<TA> Failure(List<Error> errors) => (ErrorOr<TA>)errors;
    public static ErrorOr<TA> Failure(params Error[] errors) => (ErrorOr<TA>)errors;
    public static IReadOnlyList<ErrorOr<TA>> MapErrors(params Error[] errors) 
        => errors.Select(e => Failure(e)).ToList().AsReadOnly();
    public static IReadOnlyList<ErrorOr<TA>> MapErrors(params List<Error>[] errors)
        => errors.Select(e => Failure(e)).ToList().AsReadOnly();
}
