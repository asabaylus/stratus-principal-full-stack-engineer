using Stratus.WorkOrders.Api.Domain;
using Stratus.WorkOrders.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddSingleton<IWorkOrderRepository, InMemoryWorkOrderRepository>();
builder.Services.AddSingleton<IScopeBriefGenerator, ScopeBriefGenerator>();
builder.Services.AddSingleton<IScopeBriefApprovalService, ScopeBriefApprovalService>();
builder.Services.AddSingleton<IWorkOrderScopeBriefOrchestrator, WorkOrderScopeBriefOrchestrator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

app.Run();

public partial class Program { }
