using ContractValidation.Applications.Common;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ContractValidation.Applications.Services;

public class ContractsValidationService : IContractsValidationService
{
	public ValidationResult<T> Validate<T>(T input) where T : class, new()
	{
		var invalidModel = new T();
		var validationContext = new ValidationContext(input);
		var validationResults = new List<ValidationResult>();
		var isValid = Validator.TryValidateObject(input, validationContext, validationResults, true);

		var errors = new List<string>();
		if (!isValid)
		{
			errors.AddRange(validationResults.Select(r => r.ErrorMessage));
		}

		CopyNonListProperties(input, invalidModel);

		var nestedErrors = ValidateListProperties(input, invalidModel);
		errors.AddRange(nestedErrors);

		return ValidationResult<T>.Result(input, invalidModel, errors);
	}

	private static void CopyNonListProperties<T>(T source, T target) where T : class
	{
		var properties = typeof(T).GetProperties()
			.Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string));

		foreach (var property in properties)
		{
			var value = property.GetValue(source);
			property.SetValue(target, value);
		}
	}

	private static List<string> ValidateListProperties<T>(T model, T? invalidModel) where T : class
	{
		var errors = new List<string>();
		var properties = typeof(T).GetProperties()
			.Where(p => typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string));

		foreach (var property in properties)
		{
			if (property.GetValue(model) is not IList list) continue;

			var elementType = property.PropertyType.GetGenericArguments().FirstOrDefault();
			if (elementType == null) continue;

			var validItems = new ArrayList();
			var invalidItems = new ArrayList();

			foreach (var item in list)
			{
				var validationContext = new ValidationContext(item);
				var validationResults = new List<ValidationResult>();
				var isValid = Validator.TryValidateObject(item, validationContext, validationResults, true);

				if (isValid)
				{
					validItems.Add(item);
				}
				else
				{
					invalidItems.Add(item);
					errors.AddRange(validationResults.Select(r => r.ErrorMessage));
				}
			}

			var genericListType = typeof(List<>).MakeGenericType(elementType); // Create a generic list type.
			var filteredList = (IList?)Activator.CreateInstance(genericListType);
			var filterInvalidList = (IList?)Activator.CreateInstance(genericListType);

			foreach (var validItem in validItems)
			{
				filteredList?.Add(validItem); // Add valid items to the filtered list.
			}

			foreach (var invalidItem in invalidItems)
			{
				filterInvalidList?.Add(invalidItem);
			}

			if (filterInvalidList?.Count > 0)
			{
				property.SetValue(invalidModel, filterInvalidList);
			}
			property.SetValue(model, filteredList); 
		}

		return errors;
	}
}