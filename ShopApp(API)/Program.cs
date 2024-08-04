using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApp_API_.Apps.AdminApp.Validators.ProductValidators;
using ShopApp_API_.Data;
using ShopApp_API_.Entities;
using ShopApp_API_.Profiles;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<ProductCreateValidator>();

builder.Services.AddFluentValidationRulesToSwagger();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password = new()
    {
        RequiredLength = 8,
        RequireUppercase = true,
        RequireLowercase = true,
        RequireDigit = true,
        RequireNonAlphanumeric = true
    };
    //options.Lockout = new()
    //{
    //    MaxFailedAccessAttempts = 5,
    //    AllowedForNewUsers = true,
    //    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5)
    //};
    //options.User = new()
    //{
    //    //todo:email confirm sondurmusen
    //    RequireUniqueEmail = true,
    //};
    //options.SignIn.RequireConfirmedEmail = true;

}).AddDefaultTokenProviders().AddEntityFrameworkStores<ShopAppDbContext>();



builder.Services.AddDbContext<ShopAppDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));

});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(opt =>
      opt.AddProfile(new MapperProfile(new HttpContextAccessor()))
);
//builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly); birden chox automapper elave etmek uchun ancag bunu ishdetmek bes edir


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
