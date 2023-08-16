using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Enums;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace VirtualWallet.DataAccess
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Balance> Balances { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<Exchange> Exchanges { get; set; }

        public DbSet<Referral> Referrals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Вкарах ги в отделни методи за по-чисто, дано не направи проблем в бъдеще.
            ConfigureMigration(builder);

            base.OnModelCreating(builder);

            CreateSeed(builder);
        }

        protected void ConfigureMigration(ModelBuilder builder)
        {
            builder.Entity<Referral>()
                .HasOne(r => r.Referrer)
                .WithMany(u => u.Referrals)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Balance>()
                .HasKey(b => new { b.CurrencyId, b.WalletId });

            builder.Entity<Balance>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);

            builder.Entity<WalletTransaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            builder.Entity<WalletTransaction>()
                .HasOne(t => t.Sender)
                .WithMany(u => u.Outgoing)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<WalletTransaction>()
                .HasOne(t => t.Recipient)
                .WithMany(u => u.Incoming)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Role>()
                .Property(r => r.Name)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.Entity<Currency>()
                .Property(c => c.Code)
                .HasConversion<string>()
                .HasMaxLength(3);

            builder.Entity<Transfer>()
                .Property(t => t.Amount)
                .HasPrecision(18, 4);

            builder.Entity<Transfer>()
                .HasOne(t => t.Card)
                .WithMany(c => c.Transfers)
                .HasForeignKey(t => t.CardId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transfer>()
                .HasOne(t => t.Currency)
                .WithMany()
                .HasForeignKey(t => t.CurrencyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transfer>()
                .HasOne(t => t.Wallet)
                .WithMany(w => w.Transfers)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Exchange>()
                .HasOne(e => e.FromCurrency)
                .WithMany(c => c.Exchanges)
                .HasForeignKey(e => e.FromCurrencyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Exchange>()
                .Property(t => t.Amount)
                .HasPrecision(18, 4);

            builder.Entity<Exchange>()
                .Property(t => t.Rate)
                .HasPrecision(18, 4);

            builder.Entity<Exchange>()
                .Property(t => t.ExchangedAmout)
                .HasPrecision(18, 4);
        }

        protected void CreateSeed(ModelBuilder builder)
        {
            IList<Role> roles = new List<Role>
            {
                new Role
                {
                    Id = (int)RoleName.Blocked,
                    Name = RoleName.Blocked
                },
                new Role
                {
                    Id = (int)RoleName.User,
                    Name = RoleName.User
                },
                new Role
                {
                    Id = (int)RoleName.Admin,
                    Name = RoleName.Admin
                }
            };
            IList<Currency> currencies = new List<Currency>
            {
                new Currency
                {
                    Id = (int)CurrencyCode.USD,
                    Code = CurrencyCode.USD
                },
                new Currency
                {
                    Id = (int)CurrencyCode.EUR,
                    Code = CurrencyCode.EUR
                },
                new Currency
                {
                    Id = (int)CurrencyCode.JPY,
                    Code = CurrencyCode.JPY
                },
                new Currency
                {
                    Id = (int)CurrencyCode.CHF,
                    Code = CurrencyCode.CHF
                },
                new Currency
                {
                    Id = (int)CurrencyCode.BGN,
                    Code = CurrencyCode.BGN
                },
            };
            IList<User> users = new List<User>
            {
                new User()
            {
                Id = 1,
                FirstName = "Georgi",
                LastName = "Georgiev",
                Username = "goshoXx123",
                Email = "gosho@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 1,
                IsConfirmed = true,
                PhoneNumber="0888888881",
                ProfilePicPath="/Assets/OriginalProfilePics/1d825707-a65e-4378-8e70-91030d7443bb_c372cf2e-0cab-43a0-81e1-73ea610f9dfd_ddh0598-18d7e667-d117-4b11-8ef0-244eb60bfa45.jpg"

                //123
            },
            new User()
            {
                Id = 2,
                FirstName = "Nikolai",
                LastName = "Barekov",
                Username = "BarekaXx123",
                Email = "Barekov@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 2,
                IsConfirmed = true,
                PhoneNumber="0888888882",
                ProfilePicPath="/Assets/OriginalProfilePics/00305ad2-a944-49ef-ac46-036c00ec8387_1805f02d-058f-4396-b8e9-3286a4344754_650566858583572501.png"

                //123
            },
            new User()
            {
                Id = 3,
                FirstName = "Shtilian",
                LastName = "Uzunov",
                Username = "Uzunkata",
                Email = "Uzunkata@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 3,
                IsConfirmed = true,
                PhoneNumber="0888888883",
                ProfilePicPath="/Assets/GeneratedProfilePics/77764ebb-a43a-4bbb-a6ea-2fc5697df26b_SU.png"

                //123
            },
            new User()
            {
                Id = 4,
                FirstName = "Vladislav",
                LastName = "Cvetanov",
                Username = "Cvete123",
                Email = "Cvetan@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 4,
                IsConfirmed = true,
                PhoneNumber="0888888884",
                ProfilePicPath="/Assets/GeneratedProfilePics/aff0050a-910b-432c-8754-d3dc0e40293b_VC.png"

                //123
            },
            new User()
            {
                Id = 5,
                FirstName = "Kosta",
                LastName = "Kostev",
                Username = "BrainDamage123",
                Email = "Kostev@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 5,
                IsConfirmed = true,
                PhoneNumber="0888888885",
                ProfilePicPath="/Assets/OriginalProfilePics/3ec69106-3e2a-4392-b996-81c2627af234_kakashi.jpeg"

                //123
            },
            new User()
            {
                Id = 6,
                FirstName = "Admin",
                LastName = "Adminov",
                Username = "Admin",
                Email = "Admin@gmail.com",
                Password = "MTIz",
                RoleId = 3,
                WalletId = 6,
                IsConfirmed = true,
                PhoneNumber="0888888886",
                ProfilePicPath="/Assets/OriginalProfilePics/0fb0bfde-2f25-409c-8e9e-48f97a35ddf1_gandalfAdmin.png"


                //123
            },
            new User()
            {
                Id = 7,
                FirstName = "Andrea",
                LastName = "Paynera",
                Username = "TopAndreika",
                Email = "Andrea@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 7,
                IsConfirmed = true,
                PhoneNumber="0888888887",
                ProfilePicPath="/Assets/GeneratedProfilePics/7448d873-3696-4afb-921e-ace24f80379a_AP.png"

                //123
            },
            new User()
            {
                Id = 8,
                FirstName = "Emanuela",
                LastName = "Paynera",
                Username = "TopEmanuelka",
                Email = "Emanuela@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 8,
                IsConfirmed = true,
                PhoneNumber="0888888888",
                ProfilePicPath="/Assets/GeneratedProfilePics/6cdf3f00-c41f-46a2-a6fe-577b9901cf31_EP.png"

                //123
            },
            new User()
            {
                Id = 9,
                FirstName = "Katrin",
                LastName = "lilova",
                Username = "Katrin",
                Email = "Katrin@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 9,
                IsConfirmed = true,
                PhoneNumber="0888888889",
                ProfilePicPath="/Assets/GeneratedProfilePics/b3d6b069-1325-4793-a0ad-375cd7f23d7e_KL.png"

                //123
            },
            new User()
            {
                Id = 10,
                FirstName = "Atanas",
                LastName = "Iliev",
                Username = "Nachosa",
                Email = "Nachosa@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 10,
                IsConfirmed = true,
                PhoneNumber="0888888810",
                ProfilePicPath="/Assets/OriginalProfilePics/naskoPhoto.jpeg"

                //123
            },
            new User()
            {
                Id = 11,
                FirstName = "Nikolai",
                LastName = "Gigov",
                Username = "Niki",
                Email = "kitaeca@gmail.com",
                Password = "MTIz",
                RoleId = 2,
                WalletId = 11,
                IsConfirmed = true,
                PhoneNumber="0888888820",
                ProfilePicPath="/Assets/OriginalProfilePics/nikiPhoto.jpeg"

                //123
            },
            };

            IList<Wallet> wallets = new List<Wallet>
            {
                new Wallet()
                {
                    Id = 1,
                    UserId = 1,
                },
                new Wallet()
                {
                    Id = 2,
                    UserId = 2,
                },
                new Wallet()
                {
                    Id = 3,
                    UserId = 3,
                },
                new Wallet()
                {
                    Id = 4,
                    UserId = 4,
                },
                new Wallet()
                {
                    Id = 5,
                    UserId = 5
                },
                new Wallet()
                {
                    Id = 6,
                    UserId = 6,
                },
                new Wallet()
                {
                    Id = 7,
                    UserId = 7,
                },
                new Wallet()
                {
                    Id = 8,
                    UserId = 8,
                },
                new Wallet()
                {
                    Id = 9,
                    UserId = 9,
                },
                new Wallet()
                {
                    Id = 10,
                    UserId = 10,
                },
                new Wallet()
                {
                    Id = 11,
                    UserId = 11,
                },
            };

            IList<Balance> balances = new List<Balance>
            {
                new Balance()
                {
                    WalletId = 1,
                    CurrencyId = 1,
                    Amount = 976,
                },
                new Balance()
                {
                    WalletId = 2,
                    CurrencyId = 2,
                    Amount = 347,
                },
                new Balance()
                {
                    WalletId = 3,
                    CurrencyId = 3,
                    Amount = 5034,
                },
                new Balance()
                {
                    WalletId = 4,
                    CurrencyId = 1,
                    Amount = 1320,
                },
                new Balance()
                {
                    WalletId = 5,
                    CurrencyId = 2,
                    Amount = 124,
                },
                new Balance()
                {
                    WalletId = 5,
                    CurrencyId = 1,
                    Amount = 45,
                },
                new Balance()
                {
                    WalletId = 7,
                    CurrencyId = 3,
                    Amount = 8020,
                },
                new Balance()
                {
                    WalletId = 8,
                    CurrencyId = 5,
                    Amount = 420,
                },
                new Balance()
                {
                    WalletId = 9,
                    CurrencyId = 4,
                    Amount = 50000,
                },
                new Balance()
                {
                    WalletId = 10,
                    CurrencyId = 1,
                    Amount = 4,
                },
                new Balance
                {
                    WalletId = 10,
                    CurrencyId = 2,
                    Amount = 4,
                },
                new Balance
                {
                    WalletId = 10,
                    CurrencyId = 3,
                    Amount = 420,
                },
                new Balance
                {
                    WalletId = 10,
                    CurrencyId = 4,
                    Amount = 333666,
                },
                new Balance
                {
                    WalletId = 10,
                    CurrencyId = 5,
                    Amount = 54,
                },
                new Balance
                {
                    WalletId = 11,
                    CurrencyId = 5,
                    Amount = 999,
                },
                new Balance
                {
                    WalletId = 1,
                    CurrencyId = 2,
                    Amount = 99,
                },
                new Balance
                {
                    WalletId = 1,
                    CurrencyId = 5,
                    Amount = 59,
                },
                new Balance
                {
                    WalletId = 2,
                    CurrencyId = 1,
                    Amount = 34,
                },
                new Balance
                {
                    WalletId = 2,
                    CurrencyId = 4,
                    Amount = 5686,
                },
                new Balance
                {
                    WalletId = 3,
                    CurrencyId = 4,
                    Amount = 57856,
                },
            };

            IList<Card> cards = new List<Card>
            {
                new Card
                {
                    CardHolder = "Georgi Georgiev",
                    CardNumber = 1234567890123456,
                    CheckNumber = 123,
                    ExpirationDate = new DateTime(2023, 12, 31),
                    Id = 1,
                    UserId = 1
                },
                new Card
                {
                    CardHolder = "Nikolai Barekov",
                    CardNumber = 9876543210987654,
                    CheckNumber = 456,
                    ExpirationDate = new DateTime(2024, 12, 31),
                    Id = 2,
                    UserId = 2
                },
                new Card
                {
                    CardHolder = "Shtilian Uzunov",
                    CardNumber = 1111222233334444,
                    CheckNumber = 032,
                    ExpirationDate = new DateTime(2024, 10, 31),
                    Id = 3,
                    UserId = 3
                },
                new Card
                {
                    CardHolder = "Atanas Iliev",
                    CardNumber = 4580720568975513,
                    CheckNumber = 274,
                    ExpirationDate = new DateTime(2025, 12, 31),
                    Id = 4,
                    UserId = 10
                },
                new Card
                {
                    CardHolder = "Atanas Iliev",
                    CardNumber = 4947031911706375,
                    CheckNumber = 123,
                    ExpirationDate = new DateTime(2024, 12, 31),
                    Id = 5,
                    UserId = 10
                },
                new Card
                {
                    CardHolder = "Atanas Iliev",
                    CardNumber = 4978047843297814,
                    CheckNumber = 425,
                    ExpirationDate = new DateTime(2025, 8, 23),
                    Id = 6,
                    UserId = 10
                },
                new Card
                {
                    CardHolder = "Atanas Iliev",
                    CardNumber = 4840494157984254,
                    CheckNumber = 475,
                    ExpirationDate = new DateTime(2025, 8, 23),
                    Id = 7,
                    UserId = 10
                },

            };

            IList<WalletTransaction> walletTransactions = new List<WalletTransaction>
            {
                new WalletTransaction
                {
                    Id = 1,
                    RecipientId = 7,
                    SenderId = 10,
                    CurrencyId = 4,
                    Amount = 23004m,
                    CreatedOn = new DateTime(2023, 8, 14)
                },
                new WalletTransaction
                {
                    Id = 2,
                    RecipientId = 10,
                    SenderId = 5,
                    CurrencyId = 2,
                    Amount = 15m,
                    CreatedOn = new DateTime(2023, 8, 16)
                },
                new WalletTransaction
                {
                    Id = 3,
                    RecipientId = 11,
                    SenderId = 10,
                    CurrencyId = 1,
                    Amount = 12m,
                    CreatedOn = new DateTime(2023, 8, 18)
                },
                new WalletTransaction
                {
                    Id = 4,
                    RecipientId = 10,
                    SenderId = 1,
                    CurrencyId = 5,
                    Amount = 35m,
                    CreatedOn = new DateTime(2023, 8, 13)
                },
                new WalletTransaction
                {
                    Id = 5,
                    RecipientId = 10,
                    SenderId = 1,
                    CurrencyId = 3,
                    Amount = 17m,
                    CreatedOn = new DateTime(2023, 8, 15)
                },
                new WalletTransaction
                {
                    Id = 6,
                    RecipientId = 4,
                    SenderId = 9,
                    CurrencyId = 4,
                    Amount = 216505m,
                    CreatedOn = new DateTime(2023, 8, 17)
                },
                new WalletTransaction
                {
                    Id = 7,
                    RecipientId = 10,
                    SenderId = 2,
                    CurrencyId = 2,
                    Amount = 19m,
                    CreatedOn = new DateTime(2023, 8, 12)
                },
                new WalletTransaction
                {
                    Id = 8,
                    RecipientId = 1,
                    SenderId = 10,
                    CurrencyId = 1,
                    Amount = 11m,
                    CreatedOn = new DateTime(2023, 8, 14)
                },
                new WalletTransaction
                {
                    Id = 9,
                    RecipientId = 5,
                    SenderId =10 ,
                    CurrencyId=5 ,
                    Amount=28m ,
                    CreatedOn=new DateTime(2023 ,8 ,16 )
                },
                new WalletTransaction
                {
                    Id = 10 ,
                    RecipientId = 3 ,
                    SenderId = 10 ,
                    CurrencyId = 3 ,
                    Amount = 14m ,
                    CreatedOn = new DateTime(2023 ,8 ,18 )
                },
                new WalletTransaction
                {
                    Id = 11 ,
                    RecipientId = 10 ,
                    SenderId = 1 ,
                    CurrencyId = 4 ,
                    Amount = 15600m ,
                    CreatedOn = new DateTime(2023 ,8 ,13 )
                },
                new WalletTransaction
                {
                    Id = 12 ,
                    RecipientId = 10 ,
                    SenderId = 5 ,
                    CurrencyId = 2 ,
                    Amount = 16m ,
                    CreatedOn = new DateTime(2023 ,8 ,15 )
                },
                new WalletTransaction
                {
                    Id = 13 ,
                    RecipientId = 9 ,
                    SenderId = 10 ,
                    CurrencyId = 1 ,
                    Amount = 13m ,
                    CreatedOn = new DateTime(2023 ,8 ,17 )
                },
                new WalletTransaction
                {
                    Id = 14 ,
                    RecipientId = 10 ,
                    SenderId = 1 ,
                    CurrencyId = 5 ,
                    Amount = 36m ,
                    CreatedOn = new DateTime(2023 ,8 ,12 )
                },
                new WalletTransaction
                {
                    Id = 15 ,
                    RecipientId = 2 ,
                    SenderId = 10 ,
                    CurrencyId = 3 ,
                    Amount = 18m ,
                    CreatedOn = new DateTime(2023 ,8 ,14 )
                },
                new WalletTransaction
                {
                    Id = 16 ,
                    RecipientId = 4 ,
                    SenderId = 7 ,
                    CurrencyId = 4 ,
                    Amount = 45024m ,
                    CreatedOn = new DateTime(2023 ,8 ,16 )
                },
                new WalletTransaction
                {
                    Id = 17,
                    RecipientId = 8,
                    SenderId = 3,
                    CurrencyId = 2,
                    Amount = 20m,
                    CreatedOn = new DateTime(2023 ,8 ,18 )
                },
                new WalletTransaction
                {
                    Id = 18,
                    RecipientId = 1,
                    SenderId = 9,
                    CurrencyId = 1,
                    Amount = 10m,
                    CreatedOn = new DateTime(2023 ,8 ,13 )
                },
                new WalletTransaction
                {
                    Id = 19,
                    RecipientId = 5,
                    SenderId = 10,
                    CurrencyId = 5,
                    Amount = 29m,
                    CreatedOn = new DateTime(2023, 8, 15)
                },
                new WalletTransaction
                {
                    Id = 20,
                    RecipientId = 3,
                    SenderId = 2,
                    CurrencyId = 3,
                    Amount = 15m,
                    CreatedOn = new DateTime(2023, 8, 17)
                },
                new WalletTransaction()
                {
                    Id = 21,
                    RecipientId = 10,
                    SenderId = 9,
                    CurrencyId = 4,
                    Amount = 2535m,
                    CreatedOn = DateTime.Now.AddDays(-2)
                },
                new WalletTransaction()
                {
                    Id = 22,
                    RecipientId = 8,
                    SenderId = 10,
                    CurrencyId = 2,
                    Amount = 18m,
                    CreatedOn = DateTime.Now.AddDays(-4)
                },
                new WalletTransaction()
                {
                    Id = 23,
                    RecipientId = 5,
                    SenderId = 7,
                    CurrencyId = 1,
                    Amount = 14m,
                    CreatedOn = DateTime.Now.AddDays(-6)
                },
                new WalletTransaction()
                {
                    Id = 24,
                    RecipientId = 10,
                    SenderId = 5,
                    CurrencyId = 5,
                    Amount = 37m,
                    CreatedOn = DateTime.Now.AddDays(-1)
                },
                new WalletTransaction()
                {
                    Id = 25,
                    RecipientId = 4,
                    SenderId = 2,
                    CurrencyId = 3,
                    Amount = 19m,
                    CreatedOn = DateTime.Now.AddDays(-3)
                },
                new WalletTransaction()
                {
                    Id = 26,
                    RecipientId = 3,
                    SenderId = 10,
                    CurrencyId = 4,
                    Amount = 3458m,
                    CreatedOn=DateTime.Now.AddDays(-5)
                },
                new WalletTransaction()
                {
                    Id=27,
                    RecipientId=9,
                    SenderId=3,
                    CurrencyId=2,
                    Amount=20m,
                    CreatedOn=DateTime.Now.AddDays(-7)
                },
                new WalletTransaction ()
                {
                    Id=28,
                    RecipientId=2,
                    SenderId=8,
                    CurrencyId=1,
                    Amount=15m,
                    CreatedOn=DateTime.Now.AddDays(-2)
                },
                new WalletTransaction ()
                {
                    Id=29,
                    RecipientId=7,
                    SenderId=5,
                    CurrencyId=5,
                    Amount=30m,
                    CreatedOn=DateTime.Now.AddDays(-4)
                },
                new WalletTransaction ()
                {
                    Id=30,
                    RecipientId=10,
                    SenderId=4,
                    CurrencyId=3,
                    Amount=16m,
                    CreatedOn=DateTime.Now.AddDays(-6)
                },
                new WalletTransaction()
                {
                    Id = 31,
                    RecipientId = 2,
                    SenderId = 10,
                    CurrencyId = 4,
                    Amount = 5604m,
                    CreatedOn = DateTime.Now.AddDays(-2),
                },
                new WalletTransaction()
                {
                    Id = 32,
                    RecipientId = 3,
                    SenderId = 2,
                    CurrencyId = 2,
                    Amount = 18m,
                    CreatedOn = DateTime.Now.AddDays(-4),
                },
                new WalletTransaction()
                {
                    Id = 33,
                    RecipientId = 10,
                    SenderId = 3,
                    CurrencyId = 1,
                    Amount = 14m,
                    CreatedOn=DateTime.Now.AddDays(-6),
                },
                new WalletTransaction()
                {
                    Id = 34,
                    RecipientId = 10,
                    SenderId = 2,
                    CurrencyId = 2,
                    Amount = 18m,
                    CreatedOn = DateTime.Now.AddDays(-4),
                },
                new WalletTransaction()
                {
                    Id = 35,
                    RecipientId = 10,
                    SenderId = 3,
                    CurrencyId = 1,
                    Amount = 14m,
                    CreatedOn=DateTime.Now.AddDays(-6),
                },
                new WalletTransaction()
                {
                    Id=36,
                    RecipientId=2,
                    SenderId=10,
                    CurrencyId=5,
                    Amount=37m,
                    CreatedOn=DateTime.Now.AddDays(-1),
                },
                new WalletTransaction ()
                {
                    Id=37,
                    RecipientId=3,
                    SenderId=1,
                    CurrencyId=3,
                    Amount=19m,
                    CreatedOn=DateTime.Now.AddDays(-3),
                },
                new WalletTransaction()
                {
                    Id=38,
                    RecipientId=7,
                    SenderId=10,
                    CurrencyId=2,
                    Amount=12m,
                    CreatedOn=DateTime.Now.AddDays(-2),
                },
                new WalletTransaction()
                {
                    Id=39,
                    RecipientId=8,
                    SenderId=5,
                    CurrencyId=1,
                    Amount=45m,
                    CreatedOn=DateTime.Now.AddDays(-4),
                },
                new WalletTransaction()
                {
                    Id=40,
                    RecipientId=9,
                    SenderId=11,
                    CurrencyId=3,
                    Amount=23m,
                    CreatedOn=DateTime.Now.AddDays(-6),
                },
                new WalletTransaction()
                {
                    Id=41,
                    RecipientId=10,
                    SenderId=7,
                    CurrencyId=2,
                    Amount=34m,
                    CreatedOn=DateTime.Now.AddDays(-1),
                },
                new WalletTransaction()
                {
                    Id=42,
                    RecipientId=11,
                    SenderId=8,
                    CurrencyId=1,
                    Amount=16m,
                    CreatedOn=DateTime.Now.AddDays(-3),
                },
                new WalletTransaction()
                {
                    Id=43,
                    RecipientId=5,
                    SenderId=9,
                    CurrencyId=3,
                    Amount=27m,
                    CreatedOn=DateTime.Now.AddDays(-5),
                },
                new WalletTransaction()
                {
                    Id=44,
                    RecipientId=5,
                    SenderId=10,
                    CurrencyId=2,
                    Amount=38m,
                    CreatedOn=DateTime.Now.AddDays(-7),
                },
                new WalletTransaction()
                {
                    Id = 45,
                    RecipientId = 7,
                    SenderId = 11,
                    CurrencyId = 1,
                    Amount = 50m,
                    CreatedOn = DateTime.Now.AddDays(-2)
                },
                new WalletTransaction()
                {
                    Id = 46,
                    RecipientId = 8,
                    SenderId = 5,
                    CurrencyId = 3,
                    Amount = 15m,
                    CreatedOn = DateTime.Now.AddDays(-4)
                },
                new WalletTransaction()
                {
                    Id = 47,
                    RecipientId = 9,
                    SenderId = 6,
                    CurrencyId = 2,
                    Amount = 25m,
                    CreatedOn = DateTime.Now.AddDays(-6)
                },
                new WalletTransaction()
                {
                    Id=48,
                    RecipientId=9,
                    SenderId=11,
                    CurrencyId=2,
                    Amount=12m,
                    CreatedOn=DateTime.Now.AddDays(-2),
                },
                new WalletTransaction()
                {
                    Id=49,
                    RecipientId=10,
                    SenderId=9,
                    CurrencyId=1,
                    Amount=45m,
                    CreatedOn=DateTime.Now.AddDays(-4),
                },
                new WalletTransaction()
                {
                    Id=50,
                    RecipientId=11,
                    SenderId=10,
                    CurrencyId=3,
                    Amount=23m,
                    CreatedOn=DateTime.Now.AddDays(-6),
                },
                new WalletTransaction()
                {
                    Id=51,
                    RecipientId=9,
                    SenderId=11,
                    CurrencyId=2,
                    Amount=34m,
                    CreatedOn=DateTime.Now.AddDays(-1),
                },
                new WalletTransaction()
                {
                    Id=52,
                    RecipientId=10,
                    SenderId=9,
                    CurrencyId=1,
                    Amount=16m,
                    CreatedOn=DateTime.Now.AddDays(-3),
                },
                new WalletTransaction()
                {
                    Id = 53,
                    RecipientId = 11,
                    SenderId = 10,
                    CurrencyId = 3,
                    Amount = 27m,
                    CreatedOn = DateTime.Now.AddDays(-5)
                },
                new WalletTransaction()
                {
                    Id = 54,
                    RecipientId = 9,
                    SenderId = 11,
                    CurrencyId = 2,
                    Amount = 38m,
                    CreatedOn = DateTime.Now.AddDays(-7)
                },
                new WalletTransaction()
                {
                    Id = 55,
                    RecipientId = 10,
                    SenderId = 9,
                    CurrencyId = 1,
                    Amount = 50m,
                    CreatedOn = DateTime.Now.AddDays(-2)
                },
                new WalletTransaction()
                {
                    Id = 56,
                    RecipientId = 11,
                    SenderId = 10,
                    CurrencyId = 3,
                    Amount = 15m,
                    CreatedOn = DateTime.Now.AddDays(-4)
                },
                new WalletTransaction()
                {
                    Id = 57,
                    RecipientId = 9,
                    SenderId = 11,
                    CurrencyId = 2,
                    Amount = 25m,
                    CreatedOn = DateTime.Now.AddDays(-6)
                },
                new WalletTransaction()
                {
                    Id=58,
                    RecipientId=1,
                    SenderId=2,
                    CurrencyId=2,
                    Amount=12m,
                    CreatedOn=DateTime.Now.AddDays(-2),
                },
                new WalletTransaction()
                {
                    Id=59,
                    RecipientId=2,
                    SenderId=3,
                    CurrencyId=1,
                    Amount=45m,
                    CreatedOn=DateTime.Now.AddDays(-4),
                },
                new WalletTransaction()
                {
                    Id=60,
                    RecipientId=3,
                    SenderId=4,
                    CurrencyId=3,
                    Amount=23m,
                    CreatedOn=DateTime.Now.AddDays(-6),
                },
                new WalletTransaction()
                {
                    Id=61,
                    RecipientId=4,
                    SenderId=5,
                    CurrencyId=2,
                    Amount=34m,
                    CreatedOn=DateTime.Now.AddDays(-1),
                },
                new WalletTransaction()
                {
                    Id = 62,
                    RecipientId = 5,
                    SenderId = 1,
                    CurrencyId = 1,
                    Amount = 16m,
                    CreatedOn = DateTime.Now.AddDays(-3)
                },
                new WalletTransaction()
                {
                    Id = 63,
                    RecipientId = 1,
                    SenderId = 2,
                    CurrencyId = 3,
                    Amount = 27m,
                    CreatedOn = DateTime.Now.AddDays(-5)
                },
                new WalletTransaction()
                {
                    Id = 64,
                    RecipientId = 2,
                    SenderId = 3,
                    CurrencyId = 2,
                    Amount = 38m,
                    CreatedOn = DateTime.Now.AddDays(-7)
                },
                new WalletTransaction()
                {
                    Id = 65,
                    RecipientId = 3,
                    SenderId = 4,
                    CurrencyId = 1,
                    Amount = 50m,
                    CreatedOn = DateTime.Now.AddDays(-2)
                },
                new WalletTransaction()
                {
                    Id = 66,
                    RecipientId = 4,
                    SenderId = 5,
                    CurrencyId = 3,
                    Amount = 15m,
                    CreatedOn = DateTime.Now.AddDays(-4)
                },
                new WalletTransaction()
                {
                    Id = 67,
                    RecipientId = 5,
                    SenderId = 1,
                    CurrencyId = 2,
                    Amount = 25m,
                    CreatedOn = DateTime.Now.AddDays(-6)
                }
            };

            IList<Transfer> transfers = new List<Transfer>
            {
                new Transfer
                {
                    Amount = 100m,
                    CardId = 1,
                    CurrencyId = 3,
                    Id = 1,
                    WalletId = 1
                },
                new Transfer
                {
                    Amount = 50m,
                    CardId = 2,
                    CurrencyId = 5,
                    Id = 2,
                    WalletId = 2
                },
                new Transfer
                {
                    Amount = 15m,
                    CardId = 4,
                    CurrencyId = 2,
                    Id = 3,
                    WalletId = 10
                },
                new Transfer
                {
                    Amount = 23m,
                    CardId = 1,
                    CurrencyId = 5,
                    Id = 4,
                    WalletId = 10
                },
                new Transfer
                {
                    Amount = 12m,
                    CardId = 6,
                    CurrencyId = 3,
                    Id = 5,
                    WalletId = 10
                },
                new Transfer
                {
                    Amount = 35m,
                    CardId = 5,
                    CurrencyId = 4,
                    Id = 6,
                    WalletId = 10
                },
                new Transfer
                {
                    Amount = 17m,
                    CardId = 4,
                    CurrencyId = 1,
                    Id = 7,
                    WalletId =10
                },
                new Transfer
                {
                    Amount = 21m,
                    CardId = 1,
                    CurrencyId = 2,
                    Id=8 ,
                    WalletId=10
                },
                new Transfer ()
                {
                    Amount=19m ,
                    CardId=6 ,
                    CurrencyId=3 ,
                    Id=9 ,
                    WalletId=10
                },
                new Transfer ()
                {
                    Amount=11m ,
                    CardId=5 ,
                    CurrencyId=4 ,
                    Id=10 ,
                    WalletId=10
                },
            };

            IList<Exchange> exchanges = new List<Exchange>
            {
                new Exchange
                {
                    Id = 1,
                    WalletId = 10,
                    ToCurrencyId=1,
                    FromCurrencyId=2,
                    Amount=100m,
                    ExchangedAmout=110m,
                    Rate=1.10m

                },
                new Exchange
                {
                    Id = 2,
                    WalletId = 10,
                    ToCurrencyId=1,
                    FromCurrencyId=3,
                    Amount=100m,
                    ExchangedAmout=56m,
                    Rate=0.56m
                },
                new Exchange
                {
                    Id = 3,
                    WalletId = 10,
                    ToCurrencyId=1,
                    FromCurrencyId=4,
                    Amount=1000m,
                    ExchangedAmout=6.92m,
                    Rate=0.0069m
                },
                new Exchange
                {
                    Id = 4,
                    WalletId = 10,
                    ToCurrencyId = 2,
                    FromCurrencyId = 1,
                    Amount = 50m,
                    ExchangedAmout = 45.45m,
                    Rate = 0.908972m,
                    CreatedOn = new DateTime(2023 ,8 ,13 )
                },
                new Exchange
                {
                    Id = 5,
                    WalletId = 10,
                    ToCurrencyId = 2,
                    FromCurrencyId = 1,
                    Amount = 100m,
                    ExchangedAmout = 90.919m,
                    Rate = 0.909190m,
                    CreatedOn = new DateTime(2023 ,8 ,16 )
                },
                new Exchange
                {
                    Id = 6,
                    WalletId = 10,
                    ToCurrencyId = 2,
                    FromCurrencyId = 1,
                    Amount = 25m,
                    ExchangedAmout = 22.7297m,
                    Rate = 0.909187m,
                    CreatedOn = new DateTime(2023 ,8 ,17 )
                },
                new Exchange
                {
                    Id = 7,
                    WalletId = 10,
                    ToCurrencyId = 4,
                    FromCurrencyId = 3,
                    Amount = 25m,
                    ExchangedAmout = 2032.06m,
                    Rate = 81.2824m,
                    CreatedOn = new DateTime(2023 ,8 ,16 )
                },
                new Exchange
                {
                    Id = 8,
                    WalletId = 10,
                    ToCurrencyId = 3,
                    FromCurrencyId = 4,
                    Amount = 10000m,
                    ExchangedAmout = 123.036m,
                    Rate = 0.0123036m,
                    CreatedOn = new DateTime(2023 ,8 ,12 )
                },
                new Exchange
                {
                    Id = 9,
                    WalletId = 10,
                    ToCurrencyId = 4,
                    FromCurrencyId = 5,
                    Amount = 56m,
                    ExchangedAmout = 9252.5791m,
                    Rate = 165.225m,
                    CreatedOn = new DateTime(2023 ,8 ,14 )
                },
            };

            builder.Entity<Role>().HasData(roles);
            builder.Entity<Currency>().HasData(currencies);
            builder.Entity<User>().HasData(users);
            builder.Entity<Wallet>().HasData(wallets);
            builder.Entity<Balance>().HasData(balances);
            builder.Entity<Card>().HasData(cards);
            builder.Entity<WalletTransaction>().HasData(walletTransactions);
            builder.Entity<Transfer>().HasData(transfers);
            builder.Entity<Exchange>().HasData(exchanges);

        }
    }
}