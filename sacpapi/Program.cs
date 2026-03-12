using Microsoft.EntityFrameworkCore;
using sacpapi.Data.Services;
using sacpapi.Data;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data.Services;
using sacpapi.Data;
using sacpapi.Models;
//using OfficeOpenXml;

// EPPlus 8+ license (NonCommercial)
//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddHttpClient();
//builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IAuditTrail, AuditTrailService>();
//builder.Services.AddHostedService<MSMERegisterFetcher>();
//builder.Services.AddHostedService<FocusGroupDiscussionDataFetcher>();
//builder.Services.AddHostedService<GroupsDataFetcher>();
//builder.Services.AddHostedService<FieldRegisterFetcher>();
//builder.Services.AddHostedService<EOIBeneficiariesDataFetcher>();
builder.Services.AddScoped<IFieldRegisterBeneficiaries, FieldRegisterBeneficiariesService>();
builder.Services.AddScoped<IActivityAttendants, ActivityAttendantsService>();
builder.Services.AddScoped<IMSMERegisterEnterprises, MSMERegisterEnterprisesService>();
builder.Services.AddScoped<IMSMEs, MSMEsService>();
builder.Services.AddScoped<IEOIBeneficiaries, EOIBeneficiariesService>();
builder.Services.AddScoped<IBeneficiaries, BeneficiariesService>();
builder.Services.AddScoped<ParticipantActivityService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var jwtSecretKey = builder.Configuration["JwtSecretKey"];

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
