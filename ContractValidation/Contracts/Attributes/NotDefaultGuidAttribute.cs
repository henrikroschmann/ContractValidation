namespace ContractValidation.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NotDefaultGuidAttribute : Attribute
{
	public bool IsValid(object value)
	{
		if (value is Guid guidValue)
		{
			return guidValue != Guid.Empty;
		}

		return false;
	}
}