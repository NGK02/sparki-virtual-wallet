using Microsoft.EntityFrameworkCore;
using VirtualWallet.Business.AutoMapper;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.Business.AuthManager;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using VirtualWallet.Web.Helper.Contracts;
using VirtualWallet.Web.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IReferralRepository, ReferralRepository>();
builder.Services.AddScoped<IReferralService, ReferralService>();

builder.Services.AddScoped<IAuthManager,AuthManager>();
builder.Services.AddScoped<IImageManager,ImageManager>();
builder.Services.AddScoped<IAuthManagerMvc, AuthManagerMvc>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();

builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<ITransferService, TransferService>();

builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();

builder.Services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
builder.Services.AddScoped<IWalletTransactionService, WalletTransactionService>();

builder.Services.AddScoped<IExchangeService, ExchangeService>();
builder.Services.AddScoped<IExchangeRepository, ExchangeRepository>();

builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
{cfg.AddProfile(new AutoMapperProfile(provider.GetService<IUserService>()));
}).CreateMapper());

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WalletDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(2000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
