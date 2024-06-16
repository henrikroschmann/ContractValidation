using ContractValidation.Application.Common;

namespace ContractValidation.Application.Abstractions;

public interface IContractsValidationService
{
    ValidationResult<T> Validate<T>(T input) where T : class, new();
}