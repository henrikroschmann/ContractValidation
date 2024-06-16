var builder = WebApplication.CreateBuilder();
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddTransient<IContractsValidationService, ContractsValidationService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();
app.MapControllers();
await app.RunAsync();