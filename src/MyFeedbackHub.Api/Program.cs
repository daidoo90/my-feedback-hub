using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Api.Shared.Registration;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Infrastructure.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services
    .AddBadRequestDetails()
    .AddIdentity(builder)
    .AddSwagger()
    .AddEndpointsApiExplorer()
    .AddInfrastructure(builder)
    .AddDomain();

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
    var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FeedbackHubDbContext>>();
    dbContext.CreateDbContext().Database.Migrate();
}

app.UseWebSockets();
app.UseHttpsRedirection();
app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.Run();