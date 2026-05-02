using ForgingCalc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDrawingService, DrawingService>();
builder.Services.AddScoped<IForgingCalculationService, ForgingCalculationService>();
builder.Services.AddLogging(config => config.AddConsole());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Forging}/{action=Index}/{id?}");

app.Run();