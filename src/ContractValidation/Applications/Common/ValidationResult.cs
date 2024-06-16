namespace ContractValidation.Applications.Common;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public sealed class ValidationResult<T>
{
	public T? Value { get; }
	public T? InvalidValue { get; }
	public List<string> Errors { get; }
	public bool IsValid => Errors == null || Errors.Count == 0;
	
	private ValidationResult(T? value, T? invalidValue, List<string> errors)
	{
		Value = value;
		InvalidValue = invalidValue;
		Errors = errors ?? [];
	}

	public static ValidationResult<T> Result(T validModel, T invalidModel, List<string> errors) => new(validModel, invalidModel, errors);

	public ValidationResult<T> OnSuccess(Action<T> action)
	{
		if (!object.Equals(Value, default(T)))
		{
			action(Value!);
		}
		
		return this;
	}

	public ValidationResult<T> OnFailure(Action<T, List<string>> action)
	{
		if (!IsValid)
		{
			action(InvalidValue, Errors);
		}
		return this;
	}

	public (bool Success, object Data) HandleResponse()
	{
		if (IsValid)
		{
			return (true, Value);
		}
		return (false, new { InvalidValue, Errors });
	}
}