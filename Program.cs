using Document_Management_System.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<QueryLoader>();

/* Each request gets its own fresh service object, so temporary data,
variables, database connections, and resources from one request can't
accidentally interfere with another request.
*/
builder.Services.AddScoped<AppDb>();
builder.Services.AddScoped<FileStorageService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

/* The middleware pipeline is the list of steps every web request goes through before ASP.NET returns a response */

/* Serves static files like CSS, JavaScript, and images from the wwwroot folder */
app.UseStaticFiles();

/* Handles routing URLs to the correct Razor Pages or controllers */
app.UseRouting();

/* Handles session management, which allows you to store user data between requests */
app.UseSession();

/* Handles authorization and authentication, which allows you to restrict access to certain pages or actions */
app.UseAuthorization();

/* Maps the Razor Pages to the correct URL paths */
app.MapRazorPages();

app.Run();