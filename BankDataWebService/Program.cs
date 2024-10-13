using BankDataWebService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var users = DBManager.generateUsers(10);
var accounts = DBManager.generateAccounts(10, users);
var transactions = DBManager.generateTransaction(10);

foreach(var user in users)
{
    DBManager.insertUser(user);
}
foreach (var account in accounts)
{
    DBManager.insertAccount(account);
}
foreach (var transaction in transactions)
{
    DBManager.insertTransaction(transaction);
}

app.Run();
