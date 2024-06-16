namespace ContractValidation.Application.Tests.Services;

public class ContractsValidationServiceTests
{
	private readonly IFixture _fixture = new Fixture();

	[Fact]
	public void Validate_Should_ReturnErrors_OnInvalidInput()
	{
		var stub = _fixture.Build<Stub>().With(x => x.Age, 10).Create();
		var validationService = new ContractsValidationService();

		var result = validationService.Validate(stub);

		result.Errors.Should().NotBeEmpty();
		result.IsValid.Should().BeFalse();
		result.Value.Should().BeNull();
		result.InvalidValue.Should().NotBeNull();
	}

	[Fact]
	public void Validate_Should_ReturnErrors_OnInvalidListInput()
	{
		List<SubStub> stubList = [
			_fixture.Create<SubStub>(),
			_fixture.Build<SubStub>().With(x => x.SomeValue, string.Empty).Create(),
			_fixture.Create<SubStub>()
			];

		var stub = _fixture.Build<Stub>()
			.With(x => x.Age, 2)
			.With(x => x.SubStubs, stubList)
			.Create();

		var validationService = new ContractsValidationService();

		var result = validationService.Validate(stub);

		result.Errors.Should().NotBeEmpty();
		result.IsValid.Should().BeFalse();
		result.Value.Should().NotBeNull();
		result.InvalidValue.Should().NotBeNull();
	}
}