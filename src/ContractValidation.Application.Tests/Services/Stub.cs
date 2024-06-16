

namespace ContractValidation.Application.Tests.Services;

public class Stub
{
	[Required]
	[MinLength(1, ErrorMessage = $"{nameof(Name)} Should not be empty")]
	public string Name { get; set; }

	[Required]
	[Range(1, 5, ErrorMessage = $"{nameof(Age)} Accepted Age is between 1 and 5")]
	public int Age { get; set; }

	public List<SubStub>? SubStubs { get; set; } = [];
}

public class SubStub
{
	[Required]
	public string SomeValue { get; set; }
	[Required]
	public int SomeInt { get; set; }
}