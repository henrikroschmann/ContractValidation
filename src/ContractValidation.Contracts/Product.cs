using System.ComponentModel.DataAnnotations;

namespace ContractValidation.Contracts;

public class Product
{
	[Required]
	[MinLength(1, ErrorMessage = $"{nameof(Name)} should not be empty")]
	public string Name { get; set; }

	[Required]
	[Range(0, int.MaxValue, ErrorMessage = "Price cannot be negative")]
	public int Price { get; set; }
}