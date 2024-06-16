namespace ContractValidation.Application.Requests;

public sealed record RejectedProductsRequest(PutProductsRequest Request, List<string> Errors) : IRequest<Unit>;