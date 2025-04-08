using System.Reflection;

namespace MyFeedbackHub.Api.Shared.Utils.Carter;

internal static class CarterModule
{
    internal static WebApplication MapCarter(this WebApplication app)
    {
        var carterModuleType = typeof(ICarterModule);

        var moduleTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => carterModuleType.IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var moduleType in moduleTypes)
        {
            if (Activator.CreateInstance(moduleType) is ICarterModule module)
            {
                module.AddRoutes(app);
            }
        }

        return app;
    }
}
