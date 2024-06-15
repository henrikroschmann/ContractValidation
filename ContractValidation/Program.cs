using ContractValidation.Applications.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder();
builder.Services.AddControllers();
builder.Services.AddTransient<IContractsValidationService, ContractsValidationService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
var app = builder.Build();
app.MapControllers();
app.Run();