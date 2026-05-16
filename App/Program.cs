using DAL.EF;
using BLL.Services;
using DAL.EF;
using DAL.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache(); // Required for session 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session expiration 
    options.Cookie.HttpOnly = true;                // Prevent JavaScript access 
    options.Cookie.IsEssential = true;             // GDPR compliance 
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HotelManagementContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConn")));

// Dependency Injection
builder.Services.AddScoped<RegistrationRepo>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthRepo>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AdminRepo>();
builder.Services.AddScoped<UserManagementRepo>();
builder.Services.AddScoped<UserManagementService>();

builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<EmployeeRepo>();


builder.Services.AddScoped<RoomManagementRepo>();
builder.Services.AddScoped<RoomManagementService>();

builder.Services.AddScoped<ReceptionistRepo>();
builder.Services.AddScoped<ReceptionistService>();

builder.Services.AddScoped<CustomerRepo>();
builder.Services.AddScoped<CustomerService>();





// Add this line to register AutoMapper
builder.Services.AddSingleton(BLL.MapperConfig.GetMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();

app.UseSession();
app.Run();
