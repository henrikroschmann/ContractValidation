using ContractValidation.Applications.Common;

namespace ContractValidation.Applications.Services;

public interface IContractsValidationService
{
	ValidationResult<T> Validate<T>(T input) where T : class;
}
