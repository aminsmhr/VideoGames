var builder = DistributedApplication.CreateBuilder(args);
var sql = builder.AddSqlServer("sql");

var database = sql.AddDatabase("CleanArchitectureDb");

var api = builder.AddProject<Projects.VideoGames_Api>("api")
    .WithReference(database)
    .WaitFor(database);

//var web = builder.AddProject<Projects.VideoGames_Web>("web")
//    .WaitFor(api);

builder.AddNpmApp("webangular", @"..\VideoGames.Web")
    .WithHttpEndpoint(name: "http", port: 4202, targetPort: 4201)
    .WithExternalHttpEndpoints()
    .WithNpmPackageInstallation()
    .WaitFor(api);

builder.Build().Run();



//var builder = DistributedApplication.CreateBuilder(args);


//var sql = builder.AddSqlServer("sqlserver");

//var database = sql.AddDatabase("VideoGameDb");

//var apiService = builder.AddProject<Projects.VideoGame_ApiService>("apiservice")
//    .WithHttpHealthCheck("/health")
//    .WithReference(database)
//    .WaitFor(database);

//builder.AddNpmApp("webangular", "../ClientApp")
//    .WithHttpEndpoint(name: "http", port: 4202, targetPort: 4201)
//    .WithExternalHttpEndpoints()
//    .WithNpmPackageInstallation()
//    .WaitFor(apiService);

//builder.AddProject<Projects.VideoGame_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(apiService)
//    .WaitFor(apiService);

//builder.Build().Run();
