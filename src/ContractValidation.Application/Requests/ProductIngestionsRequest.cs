namespace ContractValidation.Application.Requests;

public sealed record ProductIngestionsRequest(PutProductsRequest Request) : IRequest<Unit>;