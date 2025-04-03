using dotnet_project2.Data;
using dotnet_project2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using dotnet_project2.Interfaces;
using dotnet_project2.Service;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var ConnectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseMySql(ConnectionString,ServerVersion.AutoDetect(ConnectionString));
});

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddIdentity<ApplicationUser,IdentityRole>( options => {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = 
        options.DefaultChallengeScheme = 
        options.DefaultForbidScheme = 
        options.DefaultScheme = 
        options.DefaultSignInScheme = 
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(
        options => {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),
            };
        }
    );


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_project1 API V1");
        c.RoutePrefix = "swagger"; // ✅ Swagger available at http://localhost:5178/swagger
    });
}

app.UseHttpsRedirection();
app.UseCors(x=>
{
    x.AllowAnyOrigin();
    x.AllowAnyMethod();
    x.AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

