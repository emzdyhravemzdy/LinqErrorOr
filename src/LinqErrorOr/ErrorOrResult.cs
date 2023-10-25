namespace ErrorOr;

public static class ErrorOrResult<TA>
{
    public static ErrorOr<TA> Success(TA value) => (ErrorOr<TA>)value;
    public static ErrorOr<TA> Failure(Error error) => (ErrorOr<TA>)error;
    public static ErrorOr<TA> Failure(List<Error> errors) => (ErrorOr<TA>)errors;
    public static ErrorOr<TA> Failure(params Error[] errors) => (ErrorOr<TA>)errors;
}
