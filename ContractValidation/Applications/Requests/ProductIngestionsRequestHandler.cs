namespace ContractValidation.Applications.Requests;

public sealed class ProductIngestionsRequestHandler : IRequestHandler<ProductIngestionsRequest, Unit>
{
	public Task<Unit> Handle(ProductIngestionsRequest request, CancellationToken cancellationToken)
	{
        Console.WriteLine("Hello");
		return Task.FromResult(new Unit());
    }
}
