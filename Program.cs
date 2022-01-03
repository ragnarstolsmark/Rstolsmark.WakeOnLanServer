using Rstolsmark.Owin.PasswordAuthentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddRazorPages()
    .AddSessionStateTempDataProvider();
    
builder.Services.AddSession();
var app = builder.Build();
var basedir = app.Environment.ContentRootPath;
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(basedir, "data"));
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
var passwordAuthenticationOptions = app.Configuration.GetSection("PasswordAuthenticationOptions").Get<PasswordAuthenticationOptions>();
if (!string.IsNullOrEmpty(passwordAuthenticationOptions?.HashedPassword))
{
    app.UseOwin(pipeline =>
    {
        pipeline.UsePasswordAuthentication(passwordAuthenticationOptions);
    });
}
app.UseStaticFiles();
app.UseSession();
app.MapRazorPages();
app.Run();