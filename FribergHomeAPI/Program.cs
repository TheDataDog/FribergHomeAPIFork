using FribergHomeAPI.Constants;
using FribergHomeAPI.Data;
using FribergHomeAPI.Data.Repositories;
using FribergHomeAPI.Data.Seeding;
using FribergHomeAPI.Models;
using FribergHomeAPI.Services;
using FribergHomeAPI.TestDemo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Identity
builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Injected Services 
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IRealEstateAgencyRepository, RealEstateAgencyRepository>();
builder.Services.AddScoped<IRealEstateAgentRepository, RealEstateAgentRepository>();
builder.Services.AddScoped<IMuncipalityRepository, MuncipalityRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IAgencyService, AgencyService>();
builder.Services.AddScoped<ITransactionManager, EfCoreTransactionManager>();
builder.Services.AddScoped<IDogService, DogService>(); //For TestDemo
builder.Services.AddHttpClient<IDogService, DogService>(); //For TestDemo
builder.Services.Configure<ApiServiceConfig>(builder.Configuration.GetSection("ApiServiceConfig")); //For TestDemo

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options => options.AddPolicy("AllowAll",
    x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Use Identity
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration[Settings.Issuer],
        ValidAudience = builder.Configuration[Settings.Audience],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration[Settings.Key])),
        NameClaimType = ClaimTypes.NameIdentifier
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Seeding
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();
    SeedData.SeedAsync(ctx, roleManager, userManager).Wait();
}
//Cors middleware
app.UseCors("AllowAll");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// Order is hard, keep it straight
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
