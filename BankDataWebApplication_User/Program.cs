using BankDataWebService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

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

DBManager.clearAccountTable();
DBManager.clearUserTable();
DBManager.clearTransactionTable();

DBManager.CreateAccountTable();
DBManager.CreateUserTable();
DBManager.CreateTransactionTable();

var users = DBManager.generateUsers(10);
var accounts = DBManager.generateAccounts(10);
var transactions = DBManager.generateTransaction(10);

DBManager.dataSeeding(users, accounts, transactions);  

app.Run();