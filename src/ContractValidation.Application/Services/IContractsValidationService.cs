using ContractValidation.Application.Common;

namespace ContractValidation.Application.Services;

public interface IContractsValidationService
{
	ValidationResult<T> Validate<T>(T input) where T : class, new();
}