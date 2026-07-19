var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithPgAdmin().AddDatabase("restaurantedb");

builder.AddProject<Projects.Api>("api").WithReference(postgres).WaitFor(postgres);

builder.Build().Run();
