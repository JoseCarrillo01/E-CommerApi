using DepositoDentalAPI;
using DepositoDentalAPI.Helpers;
using DepositoDentalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IBraintreeService, BraintreeService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();


//SEGUNDO PASO AGREGAR SERVICIO AUTHENTICATION

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(y =>
{
    y.RequireHttpsMetadata = false;
    y.SaveToken = true;
    y.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:secretJWT"])),
        ClockSkew = TimeSpan.Zero
    };

    y.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                context.Token = context.Request.Cookies["X-Access-Token"];
            }

            return Task.CompletedTask;
        },
    };

});


//TERCER PASO AGREGAR SYSTEM.IDENTITYMODEL.TOKENS.JWT
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
{
    builder.SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()          
            .AllowCredentials();
}));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new String[]{}
                    }
     });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();




app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();


app.Run();
