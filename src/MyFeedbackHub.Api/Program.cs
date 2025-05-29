using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Api.Shared;
using MyFeedbackHub.Api.Shared.Registration;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services
    .AddBadRequestDetails()
    .AddIdentity(builder)
    .AddSwagger()
    .AddEndpointsApiExplorer()
    .AddInfrastructure(builder)
    .AddDomain()
    .AddHttpContextAccessor()
    .AddScoped<IUserContext, UserContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Feedback Hub API");
    });
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IFeedbackHubDbContextFactory>().Create();

    dbContext.Database.Migrate();
}

app.UseWebSockets();
app.UseHttpsRedirection();
app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.Run();