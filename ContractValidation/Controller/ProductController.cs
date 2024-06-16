namespace ContractValidation.Controller;

[ApiController]
[Route("/api/[controller]")]
public class ProductController(
	IContractsValidationService validationService,
	IMediator mediator) : ControllerBase
{
	private readonly IContractsValidationService _validationService = validationService;
	private readonly IMediator _mediator = mediator;

	[HttpPost]
	public IActionResult AddProducts(PutProductsRequest products)
	{
		var validationResult = _validationService.Validate(products);
		var (Success, Data) = validationResult
				.OnSuccess(validRecords => _mediator.Send(new ProductIngestionsRequest(validRecords)))
				.OnFailure((invalidValue, errors) => _mediator.Send(new RejectedProductsRequest(invalidValue, errors)))
				.HandleResponse();

		return Success
			? Ok(Data)
			: BadRequest(Data);
	}
}