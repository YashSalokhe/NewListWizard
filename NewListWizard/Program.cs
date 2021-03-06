

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NewListWizardContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnStr"));
    }
    );
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});
builder.Services.AddHttpContextAccessor();
//builder.Services.AddCookiePolicy();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<ListService>();

builder.Services.AddScoped<EncryptDecryptService>();

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
app.UseCookiePolicy();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=LoginPage}/{id?}");

app.Run();
