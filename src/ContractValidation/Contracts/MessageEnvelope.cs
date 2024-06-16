using ContractValidation.Contracts.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ContractValidation.Contracts;

public abstract class MessageEnvelope<T> where T : class
{
	[Required]
	[Range(1, int.MaxValue, ErrorMessage = "Customer Id cannot be negative")]
	public int CustomerId { get; set; }

	[NotDefaultGuid]
	public Guid SomeGuid { get; set; }

	public T Data { get; set; } = null!;
}
