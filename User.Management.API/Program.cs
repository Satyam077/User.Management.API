using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Management.API.Models;
using User.Management.Service.Models;
using User.Management.Service.Services;

var builder = WebApplication.CreateBuilder(args);

//Entity Framework core
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("defaultConnection"))
);

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

//Add Email Configuration
var emailconfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailconfig);

builder.Services.AddScoped<IEmailServices,EmailService>();

//Email Verification via code
builder.Services.Configure<IdentityOptions>(
    opt => opt.SignIn.RequireConfirmedEmail = true//,
   // opt => opt.SignIn.RequireConfirmedPhoneNumber = true,
    );


// Add services to the container
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
