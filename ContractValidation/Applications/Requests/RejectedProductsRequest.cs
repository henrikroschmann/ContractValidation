using MediatR;

namespace ContractValidation.Applications.Requests;

public sealed record RejectedProductsRequest(List<string> Errors) : IRequest<Unit>; // TODO: I don't want this (List<string>) let's see