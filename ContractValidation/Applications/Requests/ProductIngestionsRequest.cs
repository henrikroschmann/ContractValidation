using ContractValidation.Contracts;
using MediatR;

namespace ContractValidation.Applications.Requests;

public sealed record ProductIngestionsRequest(PutProductsRequest Request) : IRequest<Unit>;