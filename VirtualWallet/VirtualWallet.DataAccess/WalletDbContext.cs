using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

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

		public DbSet<Transaction> Transactions { get; set; }

		public DbSet<Currency> Currencies { get; set; }

		public DbSet<Balance> Balances { get; set; }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			//Вкарах ги в отделни методи за по-чисто, дано не направи проблем в бъдеще.
			ConfigureMigration(builder);

			base.OnModelCreating(builder);

			CreateSeed(builder);
		}

		protected void ConfigureMigration(ModelBuilder builder)
		{
			builder.Entity<User>()
				.HasOne(u => u.Wallet)
				.WithOne(vw => vw.User)
				.HasForeignKey<User>(u => u.WalletId)
				.OnDelete(DeleteBehavior.NoAction);

            builder.Entity<User>()
                .HasMany(u => u.Cards)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            builder.Entity<Balance>()
				.HasKey(cb => new { cb.CurrencyId, cb.WalletId });

			builder.Entity<Balance>()
				.Property(cb => cb.Amount)
				.HasPrecision(18, 2);

			builder.Entity<Transaction>()
				.Property(t => t.Amount)
				.HasPrecision(18, 2);

			builder.Entity<Transaction>()
				.HasOne(t => t.Sender)
				.WithMany(u => u.Outgoing)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Entity<Transaction>()
				.HasOne(t => t.Recipient)
				.WithMany(u => u.Incoming)
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
					Id = 1,
					Name="Blocked"
				},
				new Role
				{
					Id = 2,
					Name = "User"
				},
				new Role
				{
					Id = 3,
					Name = "Admin"
				}
			};
			IList<Currency> currencies = new List<Currency>
			{
				new Currency
				{
					Id = 1,
					Name="USD"
				},
				new Currency
				{
					Id = 2,
					Name="EUR"
				},
				new Currency
				{
					Id = 3,
					Name="BGN"
				},
				new Currency
				{
					Id = 4,
					Name="JPY"
				},
				new Currency
				{
					Id = 5,
					Name="CFH"
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
				WalletId = 1

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
				WalletId = 2

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
				WalletId = 3

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
				WalletId = 4

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
				WalletId = 5

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
				WalletId = 6


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
				WalletId = 7

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
				WalletId = 8

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
				WalletId = 9

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
                    CheckNumber = 789,
                    ExpirationDate = new DateTime(2022, 10, 31),
                    Id = 3,
                    UserId = 3
                }
            };

            builder.Entity<Role>().HasData(roles);
			builder.Entity<Currency>().HasData(currencies);
			builder.Entity<User>().HasData(users);
			builder.Entity<Wallet>().HasData(wallets);
			builder.Entity<Balance>().HasData(balances);
			builder.Entity<Card>().HasData(cards);
		}
	}
}
