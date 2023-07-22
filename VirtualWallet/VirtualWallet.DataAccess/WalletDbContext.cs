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

        protected override void OnModelCreating(ModelBuilder builder)
		{
			//Вкарах ги в отделни методи за по-чисто, дано не направи проблем в бъдеще.
			ConfigureMigration(builder);

			base.OnModelCreating(builder);

			CreateSeed(builder);
		}

		protected void ConfigureMigration(ModelBuilder builder)
		{
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

            //modelBuilder.Entity<Blog>()
            //.HasOne(e => e.Header)
            //.WithOne(e => e.Blog)
            //.HasForeignKey<BlogHeader>(e => e.BlogId)
            //.IsRequired();
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
					Id = (int)CurrencyCode.CFH,
					Code = CurrencyCode.CFH
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
				PhoneNumber="0888888881"

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
				PhoneNumber="0888888882"

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
				PhoneNumber="0888888883"

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
				PhoneNumber="0888888884"

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
				PhoneNumber="0888888885"

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
				PhoneNumber="0888888886"


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
				PhoneNumber="0888888887"

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
				PhoneNumber="0888888888"

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
				PhoneNumber="0888888889"

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

			};

			IList<Balance> balances = new List<Balance>
			{
				new Balance()
				{
					WalletId = 1,
					CurrencyId = 1,
					Amount = 10000,
				},
				new Balance()
				{
					WalletId = 2,
					CurrencyId = 2,
					Amount = 10000,
				},
				new Balance()
				{
					WalletId = 3,
					CurrencyId = 3,
					Amount = 10000,
				},
				new Balance()
				{
					WalletId = 4,
					CurrencyId = 1,
					Amount = 10200,
				},
				new Balance()
				{
					WalletId = 5,
					CurrencyId = 2,
					Amount = 102400,
				},
				new Balance()
				{
					WalletId = 6,
					CurrencyId = 1,
					Amount = 102004,
				},
				new Balance()
				{
					WalletId = 7,
					CurrencyId = 3,
					Amount = 50200,
				},
				new Balance()
				{
					WalletId = 8,
					CurrencyId = 5,
					Amount = 1023230,
				},
				new Balance()
				{
					WalletId = 9,
					CurrencyId = 4,
					Amount = 102000000,
				},

			};

			IList<Card> cards = new List<Card>
			{
				new Card
				{
					CardHolder = "Georgi Georgiev",
					CardNumber = 1234567890123456,
					CheckNumber = 123,
					CurrencyId = 1,
					ExpirationDate = new DateTime(2023, 12, 31),
					Id = 1,
					UserId = 1
				},
				new Card
				{
					CardHolder = "Nikolai Barekov",
					CardNumber = 9876543210987654,
					CheckNumber = 456,
                    CurrencyId = 2,
                    ExpirationDate = new DateTime(2024, 12, 31),
					Id = 2,
					UserId = 2
				},
				new Card
				{
					CardHolder = "Shtilian Uzunov",
					CardNumber = 1111222233334444,
					CheckNumber = 789,
                    CurrencyId = 3,
                    ExpirationDate = new DateTime(2022, 10, 31),
					Id = 3,
					UserId = 3
				}
			};

			IList<Transfer> transfers = new List<Transfer>
			{
				new Transfer
				{
					Amount = 100.00m,
					CardId = 1,
					CurrencyId = 3,
					Id = 1,
					WalletId = 1
				},
				new Transfer
				{
					Amount = 50.00m,
					CardId = 2,
					CurrencyId = 5,
					Id = 2,
					WalletId = 2
				}
			};

            builder.Entity<Role>().HasData(roles);
			builder.Entity<Currency>().HasData(currencies);
			builder.Entity<User>().HasData(users);
			builder.Entity<Wallet>().HasData(wallets);
			builder.Entity<Balance>().HasData(balances);
			builder.Entity<Card>().HasData(cards);
            builder.Entity<Transfer>().HasData(transfers);
        }
    }
}