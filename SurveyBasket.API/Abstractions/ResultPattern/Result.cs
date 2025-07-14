namespace SurveyBasket.API.Abstractions.ResultPattern
{
    public class Result //لو مش هرجع فاليو يعني ترو او فالس بس 
    {
        public Result(bool isSuccess, Error error)
        {
            if (IsSuccess && Error != Error.None || !IsSuccess && Error == Error.None)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }
        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; set; } = default!;

        public static Result Success() => new Result(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    }

    public class Result<TValue> : Result //لو احتاجت ارجع فاليو
    {
        public readonly TValue? _value;
        public Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Failure Result Cannot Have Value");
    }
}
