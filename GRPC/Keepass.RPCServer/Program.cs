using Keepass.DBLib;
using Keepass.RPCServer.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

//Ajout de EF
builder.Services.AddDbContextFactory<KeepassDbContext>(o => o.UseSqlite("Data Source=C:\\_workdir\\keepass.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<VaultEndpointService>();


app.MapGet("/", () => "Server is up");

app.Run();
