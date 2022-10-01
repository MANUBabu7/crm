using CrmTracker.Contracts;
using CrmTracker.DapperORM;
using CrmTracker.Repository;
using CrmTracker.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//git hub
// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");


builder.Services.AddTransient<ILoggerManager, LoggerManager>();
builder.Services.AddTransient<CRMtrackerDapperContext>();
builder.Services.AddTransient<IEnquiry, EnquiriesRepo>();
builder.Services.AddTransient<IcustomerMasterEntrty, CustomerRepo>();
builder.Services.AddTransient<IRfpContract, RfpsRepo>();
builder.Services.AddTransient<IJwtTokenManager,JwtTokenManager>();

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//configure dbcontext with pgsql
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
//for identity
builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
//add authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.SaveToken = true;
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//builder.Services.AddScoped<RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
