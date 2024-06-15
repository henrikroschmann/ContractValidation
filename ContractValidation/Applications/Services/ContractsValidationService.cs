using ContractValidation.Applications.Common;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ContractValidation.Applications.Services;

public class ContractsValidationService : IContractsValidationService
{
	public ValidationResult<T> Validate<T>(T model) where T : class
	{
		var validationContext = new ValidationContext(model);
		var validationResults = new List<ValidationResult>();
		var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);


		if (!isValid)
		{
			var errors = validationResults.ConvertAll(r => r.ErrorMessage);
			return ValidationResult<T>.Invalid(errors);
		}

		var nestedErrors = ValidateListProperties(model);
		if (nestedErrors.Any())
		{
			return ValidationResult<T>.Invalid(nestedErrors);
		}

		return ValidationResult<T>.Valid(model);
	}

	private List<string> ValidateListProperties<T>(T model) where T : class
	{
		var errors = new List<string>();
		var properties = typeof(T).GetProperties()
			.Where(p => typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string));

		foreach (var property in properties)
		{
			if (property.GetValue(model) is not IList list) continue;

			var elementType = property.PropertyType.GetGenericArguments().FirstOrDefault(); // Get the type of elements in the list.
			if (elementType == null) continue; // Skip if element type is not found.

			var validItems = new ArrayList(); // List to hold valid items.

			foreach (var item in list)
			{
				var validationContext = new ValidationContext(item); // Create validation context for list item.
				var validationResults = new List<ValidationResult>(); // List to hold validation results for item.
				var isValid = Validator.TryValidateObject(item, validationContext, validationResults, true); // Perform validation on list item.

				if (isValid)
				{
					validItems.Add(item); // Add valid item to validItems list.
				}
				else
				{
					errors.AddRange(validationResults.Select(r => r.ErrorMessage)); // Add error messages to errors list.
				}
			}

			var genericListType = typeof(List<>).MakeGenericType(elementType); // Create a generic list type.
			var filteredList = (IList?)Activator.CreateInstance(genericListType); // Create an instance of the generic list.

			foreach (var validItem in validItems)
			{
				filteredList?.Add(validItem); // Add valid items to the filtered list.
			}

			property.SetValue(model, filteredList); // Set the filtered list back to the property.
		}

		return errors; // Return the list of validation errors.
	}

}
