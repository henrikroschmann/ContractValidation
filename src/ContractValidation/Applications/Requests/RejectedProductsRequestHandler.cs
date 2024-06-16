
namespace ContractValidation.Applications.Requests;

public sealed class RejectedProductsRequestHandler : IRequestHandler<RejectedProductsRequest, Unit>
{
	public Task<Unit> Handle(RejectedProductsRequest request, CancellationToken cancellationToken)
	{
        Console.WriteLine("Error");
        return Task.FromResult(new Unit());
	}
}
