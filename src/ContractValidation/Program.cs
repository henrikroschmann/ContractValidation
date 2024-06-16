var builder = WebApplication.CreateBuilder();
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddApplication();

var app = builder.Build();
app.MapControllers();
await app.RunAsync();