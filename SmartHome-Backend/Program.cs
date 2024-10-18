using SmartHome_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = "Server=127.0.0.1;Database=smarthome;Uid=root;Pwd=;Convert Zero Datetime=True;";
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    c.EnableAnnotations(); // Enables Swagger annotations
});

var key = Encoding.ASCII.GetBytes("dee7c79d56dc4c493b0c57c689a60202eb1eb2339d492bf9bfdfe526275eaa85465248f83489052590ac14d0972dbf504bdcd53c8d69994a06a92705c98b6388fe4a8450ff513d8e00e78045f3c2eda51bbd0e00af4417e4733ae7badac17014cb653209101af6fc7f2c02b2119b53e26d1b7fa8a9580eed23273a39bfa9fda172a1a1c5239bbcf025734246e1e30ce595f6c84a816239f1bcd8214ac8026f26caac740b5a4549a8cb7b12ef2430accb1369b8434b4886c1343e33030fb37bd9521cd8f5b3bd0916d71892ace68ffcfd40089bdbe7940e20ab2cdb3213a2658ca2ca0c3c19b9e1f7e8bf06ee223852eb5aa5a9b6c085fece2f28be0a791774bb"); // Replace with your secret key
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "Localhost",
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
