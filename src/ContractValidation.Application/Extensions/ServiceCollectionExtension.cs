using ContractValidation.Application.Abstractions;
using ContractValidation.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ContractValidation.Application.Extensions;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddTransient<IContractsValidationService, ContractsValidationService>();
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		return services;
	}
}