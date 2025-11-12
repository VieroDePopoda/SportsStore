using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.WebUI.Binders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie(options =>
		{
			options.LoginPath = "/Account/Login";
			options.ExpireTimeSpan = TimeSpan.FromMinutes(2880);
		});

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
	options.ModelBinderProviders.Insert(0, new CartModelBinderProvider());
});

builder.Services.AddDbContext<EFDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.Configure<EmailSettings>(
		builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IAuthProvider, CookieAuthProvider>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<IOrderProcessor, EmailOrderProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Product}/{action=List}/{id?}"
);

// Пагинация без категории
app.MapControllerRoute(
		name: "page_only",
		pattern: "Page{page:int}",
		defaults: new { controller = "Product", action = "List", category = (string)null }
);

// Категория без страницы
app.MapControllerRoute(
		name: "category_only",
		pattern: "{category}",
		defaults: new { controller = "Product", action = "List", page = 1 }
);

// Категория с пагинацией
app.MapControllerRoute(
		name: "category_with_page",
		pattern: "{category}/Page{page:int}",
		defaults: new { controller = "Product", action = "List" }
);



app.Run();