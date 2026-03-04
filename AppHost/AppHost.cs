using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);
var sql = builder.AddSqlServer("AspireSqlServer");

var database = sql.AddDatabase("VideoGamesCatalogue");

var api = builder.AddProject<Projects.VideoGames_Api>("apiService")
    .WithReference(database)
    .WaitFor(database);

var web = builder.AddDockerfile("web", "../VideoGames.Web")
    .WithHttpEndpoint(port: 80, targetPort: 80, name: "http")
    .WithExternalHttpEndpoints()
    .WaitFor(api);

builder.AddNpmApp("webdevelopment", @"..\VideoGames.Web")
    .WithHttpEndpoint(name: "http", port: 4202, targetPort: 4201)
    .WithExternalHttpEndpoints()
    .WithNpmPackageInstallation()
    .WaitFor(api);

builder.Build().Run();

