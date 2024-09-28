using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WhoKnows_backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient
builder.Services.AddHttpClient();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Define the BearerAuth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Apply Bearer Auth to all operations
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
            Array.Empty<string>()
        }
    });
});

// Configure DbContext with Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<WhoknowsContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)) // Replace with your MySQL version
    );
});
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Optional: Use this if you want to preserve the property names as is
});

// Add JWT Authentication
var jwtSecretKey = builder.Configuration.GetValue<string>("Jwt:SecretKey");
if (string.IsNullOrEmpty(jwtSecretKey))
{
    Console.WriteLine("Jwt:SecretKey is not set.");
    throw new ArgumentNullException(nameof(jwtSecretKey), "Jwt:SecretKey cannot be null or empty.");
}
var key = Encoding.ASCII.GetBytes(jwtSecretKey);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // Set true if you have a specific issuer
        ValidateAudience = false, // Set true if you have a specific audience
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Optional: reduce token expiry time discrepancy
    };
});

builder.Services.AddDistributedMemoryCache(); // Add in-memory distributed cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure DbContext with Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<WhoknowsContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32)) // Replace with your MySQL version
    );
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Ensure Swagger is enabled in development
    app.UseSwaggerUI();
}
else {

    app.UseSwagger(); // Ensure Swagger is enabled in development
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseSession(); // Ensure session middleware is used

app.UseAuthentication(); // Add Authentication middleware
app.UseAuthorization(); // Add Authorization middleware

app.MapControllers();

app.Run();
