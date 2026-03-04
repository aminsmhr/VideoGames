using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var mode = builder.Configuration["APPHOST_MODE"]
    ?? builder.Configuration["AppHost:Mode"]
    ?? "Development";

if (string.Equals(mode, "Production", StringComparison.OrdinalIgnoreCase))
{
    var productionSqlServer = builder.AddSqlServer("ProductionSqlServer");
    var productionDatabase = productionSqlServer.AddDatabase("VideoGamesCatalogue");

    var apiContainer = builder.AddDockerfile("ProductionApi", "..", "src/VideoGames.Api/Dockerfile")
        .WithReference(productionDatabase, "SqlServer")
        .WithHttpEndpoint(port: 5077, targetPort: 8080, name: "http")
        .WithExternalHttpEndpoints()
        .WaitFor(productionDatabase);

    builder.AddDockerfile("ProductionWeb", "../VideoGames.Web")
        .WithHttpEndpoint(port: 80, targetPort: 80, name: "http")
        .WithExternalHttpEndpoints()
        .WaitFor(apiContainer);
}
else if (string.Equals(mode, "Development", StringComparison.OrdinalIgnoreCase))
{
    var developmentSqlConnection =
        builder.Configuration.GetConnectionString("SqlServer")
        ?? "Server=(localdb)\\MSSQLLocalDB;Database=VideoGamesCatalogue;Trusted_Connection=True;TrustServerCertificate=True;";

    var api = builder.AddProject<Projects.VideoGames_Api>("DevelopmentApi")
        .WithEnvironment("ConnectionStrings__SqlServer", developmentSqlConnection);

    builder.AddNpmApp("DevelopmentWeb", @"..\VideoGames.Web")
        .WithHttpEndpoint(name: "http", port: 4202, targetPort: 4201)
        .WithExternalHttpEndpoints()
        .WithNpmPackageInstallation()
        .WaitFor(api);
}
else
{
    throw new InvalidOperationException(
        $"Invalid AppHost mode '{mode}'. Supported values: Development, Production.");
}

builder.Build().Run();

