namespace ContractValidation.Applications.Common;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public class ValidationResult<T>
{
	public T Value { get; }
	public List<string> Errors { get; }
	public bool IsValid => Errors == null || !Errors.Any();

	private ValidationResult(T value, List<string> errors)
	{
		Value = value;
		Errors = errors ?? new List<string>();
	}

	public static ValidationResult<T> Valid(T value) => new ValidationResult<T>(value, null);

	public static ValidationResult<T> Invalid(List<string> errors) => new ValidationResult<T>(default, errors);

	public ValidationResult<T> OnSuccess(Action<T> action)
	{
		if (IsValid)
		{
			action(Value);
		}
		return this;
	}

	public ValidationResult<T> OnFailure(Action<List<string>> action)
	{
		if (!IsValid)
		{
			action(Errors);
		}
		return this;
	}
}