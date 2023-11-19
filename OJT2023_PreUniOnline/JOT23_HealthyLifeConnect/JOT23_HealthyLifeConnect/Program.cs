using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Hubs;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddSignalR();
builder.Services.AddSession();
builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 104857600; });
builder.Services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	options.DefaultRequestCulture = new RequestCulture("vi-VN", "Asia/Ho_Chi_Minh");
});

// Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("pre2uni")));

// Đăng ký dịch vụ Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>();
var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

// Thiết lập mối quan hệ 1:1
 // Tạo hoặc cập nhật cơ sở dữ liệu
var modelBuilder = new ModelBuilder(new ConventionSet());


// Thiết lập mối quan hệ 1:1 giữa Account và Lecturer
modelBuilder.Entity<Account>()
	.HasOne(a => a.Lecturer)
	.WithOne(l => l.Account)
	.HasForeignKey<Lecturer>(l => l.IDAccount);

// Thiết lập mối quan hệ 1:1 giữa Account và Student
modelBuilder.Entity<Account>()
	.HasOne(a => a.Student)
	.WithOne(s => s.Account)
	.HasForeignKey<Student>(s => s.IDAccount);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapDefaultControllerRoute();
});
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "NotFound",
	pattern: "Error/NotFound",
	defaults: new { controller = "Error", action = "NotFound" }
);

app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<HubMetting>("meeting");
});

app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<ChatHub>("chatHub");
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}"
    );
});

app.Run();
