using System.ComponentModel.DataAnnotations;

namespace ContractValidation.Contracts;

public class PutProductsRequest : MessageEnvelope<List<Product>>
{
	[Required]
	public string Source { get; set; }
}