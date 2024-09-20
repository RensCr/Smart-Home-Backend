using SmartHome_Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = "Server=127.0.0.1;Database=Smart-Home;Uid=root;Pwd=;";
builder.Services.AddScoped(_ => new UserRepository(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
