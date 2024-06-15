using ContractValidation.Applications.Requests;
using ContractValidation.Applications.Services;
using ContractValidation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContractValidation.Controller;

[ApiController]
[Route("/api/[controller]")]
public class ProductController(IContractsValidationService validationService, IMediator mediator)
{
	private readonly IContractsValidationService _validationService = validationService;
	private readonly IMediator __mediator = mediator;

	[HttpPost]
	public async Task<IActionResult> AddProducts(PutProductsRequest products)
	{
		var validationResult = _validationService.Validate(products);
		validationResult
			.OnSuccess(validRecords => __mediator.Send(new ProductIngestionsRequest(validRecords)))
			.OnFailure(errors => __mediator.Send(new RejectedProductsRequest(errors)));

		return default;
	}
}